using IgnitedBox.UnityUtilities;
using Scripts.OOP.Utils;
using System.Linq;
using UnityEngine;

namespace Scripts.Orbiters.Eye
{
    public class EyeOrbiter : Orbiter
    {
        public Transform Pupil { get; private set; }

        protected override void OnStart()
        {
            if (Started) return;

            base.OnStart();

            BuildEye();

            Archetype?.Start(this);
        }

        private void BuildEye()
        {
            var unlit = StaticStuff.UnlitSpriteMaterial;

            gameObject.AddComponent<SpriteRenderer>(eye =>
            {
                eye.sprite = LoadResource<Sprite>("EyeShape");
                eye.color = Color.black;
                if (unlit) eye.material = unlit;
                eye.sortingOrder = 3;
                eye.transform.localScale =
                new Vector3(0.3f, 0.3f, 1);
            });

            Components.CreateGameObject<SpriteRenderer>(
                "Inner", gameObject.transform, inner =>
            {
                inner.sprite = LoadResource<Sprite>("EyeInner");
                if (unlit) inner.material = unlit;
                inner.transform.localPosition = Vector3.zero;
                inner.transform.localScale = Vector3.one;

                inner.sortingOrder = 1;
            });

            Components.CreateGameObject<SpriteMask>(
                "Mask", gameObject.transform, mask =>
            {
                mask.sprite = LoadResource<Sprite>("EyeInner");
                mask.transform.localPosition = Vector3.zero;
                mask.transform.localScale = Vector3.one;
            });

            Pupil = Components.CreateGameObject<SpriteRenderer>(
                "Pupil", gameObject.transform, pup =>
            {
                pup.sprite = LoadResource<Sprite>("Pupil");
                pup.color = Color.black;
                if (unlit) pup.material = unlit;

                pup.sortingOrder = 1;

                pup.transform.localPosition = Vector3.zero;

                pup.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            }).transform;

            Pupil.localScale = new Vector3(0.2f, 0.2f, 1);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            LookAtTarget();
        }

        private void LookAtTarget()
        {
            if (Target && Pupil)
            {
                Vector3 dir = Target.transform.position
                    - transform.position;

                Pupil.localPosition = Vector3.ClampMagnitude(dir, 4) * 0.25f;
            }
        }

        protected override void OnColorChange()
        {
            base.OnColorChange();
        }

        protected override System.Type RandomArchetype()
        {
            System.Type[] array = EyeOrbiterType.types.Values.ToArray();
            return Randomf.Element(array);
        }

        protected override void OnOrbiterDestroy()
            => CheckOrbiterSpawner<EyeOrbiter>();
    }
}
