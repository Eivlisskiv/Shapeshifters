using Scripts.OOP.ShapeController;
using Scripts.OOP.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BodyPhysicsHandler : MonoBehaviour
{
    public GameObject deathPrefab;
    public GameObject hitPrefab;

    public bool jelly;

    public int Radius
    {
        get => _radius;
        set 
        {
            _radius = value;
            Reshape(corners);
        }
    }
    private int _radius = 1;

    public float flexibility = 0.1f;
    public float elasticity = 2f;
    public int corners = 3;

    public PolygonCollider2D Collider
    { get => pcollider; }

    private readonly Color[] _colors = new Color[2];
    public void SetColor(int i, Color color)
    {
        _colors[i] = color;
        SetMatColor();
    }

    Rigidbody2D body;

    public Rigidbody2D Body
    { get => body; }

    SpriteShapeController shapeController;
    SpriteShapeRenderer shapeRenderer;
    PolygonCollider2D pcollider;
    SplinePoint[] points;

    public float Rotation
    { get => body.rotation; }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        shapeRenderer = GetComponent<SpriteShapeRenderer>();
        SetMatColor();
        shapeController = GetComponent<SpriteShapeController>();
        pcollider = GetComponent<PolygonCollider2D>();
        Reshape(corners);
    }

    void SetMatColor()
    {
        if (!shapeRenderer) return;

        for (int i = 0; i < shapeRenderer.materials.Length; i++)
            shapeRenderer.materials[i].color = _colors[i];
    }

    public void Reshape(int count)
    {
        Spline spline = shapeController.spline;
        spline.Clear();

        points = new SplinePoint[count];

        float angle = (360 / count) * Mathf.Deg2Rad;

        Vector2 getVect(int index) =>
            new Vector2(Mathf.Cos(angle * index) * Radius,
                        Mathf.Sin(angle * index) * Radius);

        Vector2 prev = getVect(count - 1);
        Vector2 current;
        Vector2 next = getVect(0);

        AffectShape((i, point, _) =>
        {
            current = next;
            next = getVect(i + 1);

            InitPoint(spline, i, current, next, prev);

            prev = current;
        });
        RefreshShape();
    }

    private void RefreshShape()
    {
        shapeController.RefreshSpriteShape();
        //pcollider.SetPath(0, points.Select(p => p.Position).ToArray());
    }

    void InitPoint(Spline spline, int i, Vector2 current, Vector2 next, Vector2 prev)
    {
        points[i] = new SplinePoint(i, current, prev, next);
        spline.InsertPointAt(i, current);

        spline.SetTangentMode(i, ShapeTangentMode.Broken);

        spline.SetLeftTangent(i, points[i].left.Origin);
        spline.SetRightTangent(i, points[i].right.Origin);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(corners != points.Length)
        {
            corners = Mathf.Clamp(corners, 3, 10);
            if(corners != points.Length)
            {
                Reshape(corners);
                return;
            }
        }

        if(jelly) UpdateVelocities();
    }

    private void UpdateVelocities()
    {
        AffectShape((index, point, spline) =>
        {
            point.left.UpdateVelocity(index, spline, elasticity);
            point.UpdateVelocity(spline, elasticity);
            point.right.UpdateVelocity(index, spline, elasticity);
        });
        RefreshShape();
    }

    public void AddForce(float angle, float strength, float push)
    {
        if (jelly)
        {
            Vector2 hit = Vectors2.FromDegAngle(angle + transform.rotation.z, Radius + 1);
            AffectShape((i, point, spline) =>
            {
                strength -= CalculateForceToTangent(hit, i, strength, point.left, spline);
                strength -= CalculateForceToPoint(hit, i, strength, point, spline);
                strength -= CalculateForceToTangent(hit, i, strength, point.right, spline);
            });
        }
        else strength /= 2;

        if (push != 0 || !jelly)
        {
            Vector2 force = -Vectors2.FromDegAngle(angle, push + strength);
            body.velocity = Vector2.ClampMagnitude(body.velocity + force, 40);
            body.angularVelocity = Mathf.Clamp(body.angularVelocity + ((points.Length - Radius) * strength * -force.x), -360, 360);
        }
    }

    private float CalculateForceToTangent(Vector2 hit, int i, float strength, SplineTangent tangent, Spline spline)
    {
        Vector2 pos = tangent.GetPosition(spline, i);
        Vector2 force = GetForce(pos, hit, strength, out float intensity);
        tangent.AddVelocity(force);

        return intensity;
    }

    private float CalculateForceToPoint(Vector2 hit, int index, float strength, SplinePoint point, Spline spline)
    {
        Vector2 pos = spline.GetPosition(index);

        Vector2 force = GetForce(pos, hit, strength, out float intensity);
        point.AddVelocity(force);

        return intensity;
    }

    private Vector2 GetForce(Vector2 pos, Vector2 hit, float strength, out float intensity)
    {
        Vector2 force = pos - hit;
        intensity = (Radius * 2 - force.magnitude) * flexibility * strength;
        return force * intensity;
    }

    void AffectShape(Action<int, SplinePoint, Spline> action)
    {
        Spline spline = shapeController.spline;
        for (int i = 0; i < points.Length; i++)
            action(i, points[i], spline);
    }

    public Vector2 ShotVector(float angle)
        => Vectors2.FromDegAngle(angle, Radius);

    public (Vector2, bool)[] GetPointsIn(float from, float range)
    {
        List<(Vector2, bool)> points = new List<(Vector2, bool)>();
        Vector2 hit = ShotVector(from);
        AffectShape((index, point, spline) =>
        {
            Vector2 p = point.Position.Rotate(Rotation);
            if (Vector2.Angle(hit, p) <= range) points.Add((p, true));

            Vector2 l = point.left.Origin.Rotate(Rotation);
            if (Vector2.Angle(hit, l) <= range) points.Add((l, false));

            Vector2 r = point.right.Origin.Rotate(Rotation);
            if (Vector2.Angle(hit, r) <= range) points.Add((r, false));
        });

        return points.ToArray();
    }
}
