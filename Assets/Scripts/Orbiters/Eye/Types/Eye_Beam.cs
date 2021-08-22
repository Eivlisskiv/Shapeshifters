using Assets.IgnitedBox.UnityUtilities;
using IgnitedBox.Tweening.TweenPresets;
using IgnitedBox.UnityUtilities;
using Scripts.OOP.EnemyBehaviors.Targetting;
using Scripts.OOP.Utils;
using UnityEngine;

namespace Scripts.Orbiters.Eye.Types
{
    public class Eye_Beam : EyeOrbiterType
    {
        const float growthSpeed = 8;
        const float aimSpeed = 20;

        protected override float FireDuration => 10;

        private ParticleSystem chargeParticles;

        private float beamAngle;
        private float beamLength;
        private ParticleSystem fireParticles;
        private LineRenderer fireBeam;

        private Collision2DHandler beamHandler;

        private SpriteRenderer[] motifs;

        private float Strength { get; set; }

        public Eye_Beam() : base(new SingleTarget()) { }

        public float QuadCurve(float x)
            => (-Mathf.Pow(x - 0.5f, 2) + 0.25f) / 0.25f;

        public override void Start<TOrbiter>(TOrbiter orbiter)
        {
#pragma warning disable IDE0083 //Unity has no "is not", what a loser...
            if (!(orbiter is EyeOrbiter eye)) return;
#pragma warning restore IDE0083 

            Pupil = eye.Pupil;

            if (motifs != null) return;

            InitializeMotifs(orbiter);

            InitializeChargeParticles(eye.Color);

            InitializeFireParticles(eye.Color);
            InitializeFireLine(eye.Color);

            base.Start(orbiter);
        }

        private void InitializeChargeParticles(Color color)
        {
            chargeParticles = Components.CreateGameObject<ParticleSystem>(
                out GameObject charge, "OnCharge", Pupil.transform);
            charge.transform.localPosition = Vector3.zero;
            charge.transform.localScale = Vector3.one;

            chargeParticles.SetMaterials(StaticStuff.UnlitSpriteMaterial, StaticStuff.UnlitSpriteMaterial);

            var shape = chargeParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 3;
            shape.radiusThickness = 0;

            var settings = chargeParticles.main;
            settings.playOnAwake = false;
            settings.startColor = Color.clear;
            settings.startSpeed = -20;
            settings.startLifetime = 0.15f;

            var emission = chargeParticles.emission;
            emission.rateOverTime = 10;

            var trail = chargeParticles.trails;
            trail.enabled = true;

            trail.sizeAffectsWidth = false;
            trail.sizeAffectsLifetime = false;

            trail.inheritParticleColor = false;
            trail.dieWithParticles = false;
            trail.colorOverTrail = color;
            trail.widthOverTrail = new ParticleSystem.MinMaxCurve(0.2f,
                Curves.ToCurve(QuadCurve, 11));
        }

        private void InitializeFireParticles(Color color)
        {
            fireParticles = Components.CreateGameObject<ParticleSystem>(
                out GameObject fire, "OnFireParticles", Pupil.transform);

            fire.transform.localPosition = Vector3.zero;
            fire.transform.localScale = Vector3.one;

            fireParticles.SetMaterials(StaticStuff.UnlitSpriteMaterial, StaticStuff.UnlitSpriteMaterial);

            var shape = fireParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            //shape.arc = 6;
            shape.radius = 0;
            shape.radiusThickness = 0;

            var emission = fireParticles.emission;
            emission.rateOverTime = 10;

            var settings = fireParticles.main;
            settings.playOnAwake = false;
            settings.startColor = color;

            settings.startSpeed = 5;
            settings.startSize = 0.3f;

            settings.startLifetime = 0.4f;
            //settings.startSpeed.constant / (20 * 10);

            settings.gravityModifier = 1;
            settings.simulationSpace = ParticleSystemSimulationSpace.World;

            /*
            var trail = fireParticles.trails;
            trail.enabled = true;
            trail.mode = ParticleSystemTrailMode.Ribbon;
            trail.sizeAffectsWidth = false;
            trail.sizeAffectsLifetime = false;

            trail.inheritParticleColor = false;
            trail.colorOverTrail = color;

            trail.widthOverTrail = 0.2f;
            */
        }

        private void InitializeFireLine(Color color)
        {
            fireBeam = Components.CreateGameObject<LineRenderer>(
                out GameObject fire, "Beam", Pupil.transform);

            fire.transform.localPosition = Vector3.zero;

            fireBeam.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
            fireBeam.useWorldSpace = false;
            fireBeam.startColor = color;
            fireBeam.endColor = color;

            fireBeam.textureMode = LineTextureMode.Tile;

            fireBeam.material = Resources.Load<Material>(
                "Materials/Effects/LaserBeam");

            fireBeam.material.SetVector("Anim_Speed", new Vector4(growthSpeed, 0, 0, 0));
            fireBeam.material.SetColor("Beam_Color", color);

            beamHandler = fire.AddCollisionHandler<PolygonCollider2D>(0.25f, HitTarget);
        }

        private void InitializeMotifs<TOrbiter>(TOrbiter orbiter) where TOrbiter : Orbiter
        {
            motifs = new SpriteRenderer[5];

            for (int i = 0; i < 5; i++)
            {
                var sr = new GameObject("Ring").AddComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>("Sprites/Basics/Ring");
                sr.color = orbiter.Color;
                sr.material = StaticStuff.UnlitSpriteMaterial;
                sr.transform.SetParent(Pupil);

                float f = 0.2f * (1 + i);
                sr.transform.localScale = new Vector3(f, f, 1);
                sr.transform.localPosition = Vector3.zero;

                sr.sortingOrder = 2;

                sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                motifs[i] = sr;
            }
        }

        protected override void OnStateChange(OrbiterState _)
        {
            bool firing = State == OrbiterState.Firing;
            if(fireBeam.enabled != firing)
                fireBeam.enabled = firing;

            ParticleState(chargeParticles, OrbiterState.Charging);
            ParticleState(fireParticles, OrbiterState.Firing);
        }

        private void ParticleState(ParticleSystem ps, OrbiterState playState)
        {
            bool plays = State == playState;
            if (plays && !ps.isPlaying) ps.Play();
            else if (!plays && ps.isPlaying) ps.Stop();
        }

        protected override void OnCharge(float time)
        {
            if (motifs == null) return;

            for (int i = 0; i < motifs.Length; i++)
            {
                SpriteRenderer sr = motifs[i];
                float f = 0.6f * (1 + i);
                sr.transform.TweenScale(
                    new Vector3(f, f, 1), time).loop 
                    = IgnitedBox.Tweening.Tweeners
                    .TweenerBase.LoopType.ReverseLoop;
            }
        }

        protected override void WhileCharge(float progress) { }

        protected override void OnCooldown()
        {
            if (motifs == null) return;

            for (int i = 0; i < motifs.Length; i++)
            {
                SpriteRenderer sr = motifs[i];
                float f = 0.2f * (1 + i);

                sr.transform.TweenPosition(Vector3.zero, 0.1f);

                sr.transform.TweenScale(
                    new Vector3(f, f, 1), 0.2f, callback: () =>
                    sr.transform.TweenScale(
                        new Vector3(f, f, 1) * 2, 1).loop
                        = IgnitedBox.Tweening.Tweeners
                        .TweenerBase.LoopType.ReverseLoop
                    );
            }
        }

        protected override void OnFire(float time)
        {
            if (motifs == null) return;

            beamLength = 0;

            Vector2 toTarget = SelfOrbiter.Target.transform.position
                - Pupil.position;

            beamAngle = toTarget.TrueAngle();

            for (int i = 0; i < motifs.Length; i++)
            {
                SpriteRenderer sr = motifs[i];

                float f = 0.02f * (1 + i);
                sr.transform.TweenScale(
                    new Vector3(f, f, 1), time);

                Shake(sr.transform);
            }
        }

        protected override void WhileFire(float progress)
        {
            Vector2 toTarget = SelfOrbiter.Target.transform.position
                - Pupil.position;

            UpdateFireBeam(toTarget, QuadCurve(progress) / 2);
        }

        private void HitTarget(Collider2D collider)
        {
            if (State != OrbiterState.Firing) return;

            var obj = collider.gameObject;
            if (!obj.TryGetComponent(out BaseController controller)) return;
            if (controller == SelfOrbiter.Owner) return;

            bool enemy = controller.team != SelfOrbiter.Owner.team;

            float damage = SelfOrbiter.damage * 5 * Strength;

            Vector2 point = GetHitPoint(controller);

            controller.ApplyCollisionForce(point, damage,
                damage * (enemy ? growthSpeed : growthSpeed / 2));

            if (!enemy) return;

            controller.TakeDamage(damage, SelfOrbiter.Owner, point);
        }

        private Vector2 GetHitPoint(BaseController target)
        {
            Vector2 a = fireBeam.GetPosition(1);
            Vector2 c = target.transform.position;

            Vector2 v = (Vector2)fireBeam.GetPosition(0) - a;
            Vector2 u = c - a;

            Vector2 d = v.normalized * (Mathf.Cos(
                Mathf.Deg2Rad * Vector2.Angle(v, u))
                * u.magnitude);

            return c - ((c - d).normalized * target.body.Radius);
        }

        private void HitParticles(RaycastHit2D hit2d, float width)
        {
            var shape = fireParticles.shape;

            Vector2 pos = Pupil.transform.position;

            Vector2 diff = (hit2d.point - pos);

            float angle = diff.TrueAngle();
            float mid = (shape.arc / 2);
            shape.rotation = new Vector3(0, 0, -angle + (
                angle < 0 ? mid : -mid));

            shape.randomPositionAmount = width / 2;

            float distance = (hit2d.point - pos).magnitude;

            var settings = fireParticles.main;
            settings.startLifetime =
                (distance - 2) / settings.startSpeed.constant;
        }

        private void UpdateFireBeam(Vector2 toTarget, float width)
        {
            Vector2 pupil = Pupil.position;
            Strength = 0.8f + width;
            Vector2 beam = Vectors2.FromDegAngle(beamAngle, beamLength);

            float angle = Vector2.SignedAngle(beam, toTarget);

            if (angle != 0)
            {
                System.Func<float, float, float> minMax = angle > 0 ?
#pragma warning disable IDE0004 //Unity needs this because Unity is a little bitch
                    (System.Func<float, float, float>)Mathf.Min :
                    (System.Func<float, float, float>)Mathf.Max;
#pragma warning restore IDE0004
                float change = minMax(angle, aimSpeed *
                    Time.deltaTime * (angle/(Mathf.Abs(angle))));

                beamAngle += change;
            }

            if (Raycast.TryRaycast2D(pupil, beam, 
                8, out RaycastHit2D hit2d))
            {
                beam = hit2d.point - pupil;
                if (beamLength > beam.magnitude) beamLength = beam.magnitude;

                //HitParticles(hit2d, width);
            }
            else
            {
                beamLength += Time.deltaTime * growthSpeed;
                Vector2.ClampMagnitude(beam, beamLength);
            }

            fireBeam.startWidth = width * .5f;
            fireBeam.endWidth = width * 1.3f;

            SetBeamPosition(beam);
        }

        private void SetBeamPosition(Vector2 hit)
        {
            fireBeam.SetPositions(new Vector3[]
                { hit, Vector2.zero });

            PolygonCollider2D collider = (PolygonCollider2D)beamHandler.Collider;
            Vector2 perp = Vector2.Perpendicular(-hit).normalized;

            Vector2 end = perp * (fireBeam.endWidth / 2);
            Vector2 start = perp * (fireBeam.startWidth / 2);

            collider.points = new Vector2[]
            {
                end, -end,

                hit - start,
                hit + start,

            };
        }

        private void Shake(Transform ring)
        {
            float x = Random.Range(0.2f, 0.8f);
            float y = Random.Range(0.2f, 0.8f);

            ring.TweenPosition(new Vector3(x, y, 0),
                0.1f, callback: () => Shake(ring));
        }

        protected override void OnIdle()
        {
            if (motifs == null) return;

            for (int i = 0; i < motifs.Length; i++)
            {
                SpriteRenderer sr = motifs[i];
                float f = 0.2f * (1 + i);

                sr.transform.TweenScale(
                    new Vector3(f, f, 1), 0.2f);
                sr.transform.TweenPosition(
                    Vector3.zero, 0.2f);
            }
        }

        public override void OnRemove()
        {
            Object.Destroy(chargeParticles.gameObject);
            Object.Destroy(fireParticles.gameObject);
            Object.Destroy(fireBeam.gameObject);

            if (motifs == null) return;

            for (int i = 0; i < motifs.Length; i++)
            {
                SpriteRenderer sr = motifs[i];
                Object.Destroy(sr.gameObject);
            }
        }

        public override void SetColor(Color color)
        {
            if (motifs == null) return;

            var trail = chargeParticles.trails;
            trail.colorOverTrail = color;

            var fireTrail = fireParticles.trails;
            fireTrail.colorOverTrail = color;

            fireBeam.startColor = color;
            fireBeam.endColor = color;

            for (int i = 0; i < motifs.Length; i++)
            {
                SpriteRenderer sr = motifs[i];
                sr.color = color;
            }
        }
    }
}
