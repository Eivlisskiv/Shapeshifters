using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using Scripts.Explosion;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public FireworkExplosionHandler explosion;

    public SpriteRenderer teamColor;
    public SpriteRenderer activeLight;

    public Collider2D BodyCollider =>
        bodyCollider ? bodyCollider :
        (bodyCollider = GetComponent<Collider2D>());

    Collider2D bodyCollider;
    BaseController owner;
    float damage;
    float force;

    bool active;

    private void Start()
    {
        var handler = explosion.GetComponent<FireworkExplosionHandler>();
        handler.Angle = 180;
        handler.Speed = 15;
        handler.Intensity = 1;
        handler.Range = 10;
    }

    public void Activate(float damage, float force, BaseController owner)
    {
        if(owner)
            teamColor.color = owner.GetColor(0);

        this.damage = damage;
        this.force = force;
        this.owner = owner;
        active = true;

        if(owner) Physics2D.IgnoreCollision(BodyCollider, owner.Body.Collider);

        var tween = activeLight.Tween<SpriteRenderer, Color, SpriteRendererColorTween>
            (new Color(1, 66f/255, 66f/255, 1), 2, 0.2f);

        tween.loop = IgnitedBox.Tweening.Tweeners.TweenerBase.LoopType.ResetLoop;

        AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0),
            new Keyframe(0.10f, 1), new Keyframe(0.20f, 0), new Keyframe(0.30f, 1),
            new Keyframe(1, 0));
        for(int i = 0; i < curve.length; i++)
            curve.SmoothTangents(i, 0);
        tween.Curve = curve;

        Destroy(gameObject, 300f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!active) return;

        BaseController victim = collision.gameObject.GetComponent<BaseController>();
        if (!victim || owner.IsTeammate(victim)) return;

        victim.TakeDamage(damage, owner, transform.position);
        victim.ApplyCollisionForce(transform.position, force, force);

        active = false;

        BodyCollider.enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<SpriteRenderer>().enabled = false;

        Destroy(teamColor.gameObject);
        Destroy(activeLight.gameObject);

        explosion.Play();

        Destroy(gameObject, explosion.Duration + 0.5f);
    }
}
