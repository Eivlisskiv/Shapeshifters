using IgnitedBox.Tweening.TweenPresets;
using Scripts.OOP.Utils;
using UnityEngine;

namespace Scripts.Orbiters.Eye.Types
{
    public class Eye_Beam : EyeOrbiterType
    {
        private SpriteRenderer[] motifs;

        public override void Start<TOrbiter>(TOrbiter orbiter)
        {
            if (!(orbiter is EyeOrbiter eye)) return;

            Pupil = eye.Pupil;

            if (motifs != null) return;

            motifs = new SpriteRenderer[5];

            for (int i = 0; i < 5; i++)
            {
                var sr = new GameObject("Ring").AddComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>("Sprites/Basics/Ring");
                sr.color = orbiter.Color;
                sr.material = StaticStuff.UnlitSpriteMaterial; ;
                sr.transform.SetParent(Pupil);

                float f = 0.2f * (1 + i);
                sr.transform.localScale = new Vector3(f, f, 1);
                sr.transform.localPosition = Vector3.zero;

                sr.sortingOrder = 2;

                sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                motifs[i] = sr;
            }

            base.Start(orbiter);
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

        protected override void WhileCharge(Orbiter orbiter, float progress)
        {

        }

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

            for (int i = 0; i < motifs.Length; i++)
            {
                SpriteRenderer sr = motifs[i];

                float f = 0.02f * (1 + i);
                sr.transform.TweenScale(
                    new Vector3(f, f, 1), time);

                Shake(sr.transform);
            }
        }

        protected override void WhileFire(Orbiter orbiter, float progress)
        {
            
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

            for (int i = 0; i < motifs.Length; i++)
            {
                SpriteRenderer sr = motifs[i];
                sr.color = color;
            }
        }
    }
}
