using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using IgnitedBox.Utilities;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.CustomLevels;
using Scripts.OOP.Perks;
using Scripts.UI.InGame.Objectives.ObjectivePresets.Other;
using UnityEngine;

namespace Assets.Scripts.Items.InGame.Props.Perks
{
    public class PerksModule : MonoBehaviour, ILevelProp
    {
        public string PerkName
        {
            get => perkName;
            set
            {
                perkName = value;
                Sprite img = Resources.Load<Sprite>("Sprites/Perks/" + perkName);
                var sprite = GetRenderer();
                sprite.sprite = img ? img : defSprite;
            }
        }

        [SerializeField]
        private string perkName;

        public int perkLevel;
        public int perkBuff;
        public float perkCharge;

        private Sprite defSprite;

        private SpriteRenderer GetRenderer()
        {
            if (_sprite) return _sprite;
            _sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
            defSprite = _sprite.sprite;
            return _sprite;
        }

        private SpriteRenderer _sprite;

        public bool Consumed => consumed;

        private bool consumed;

        private void Start()
        {
            transform.Tween<Transform, Vector3, VectorRotationTween>
                (new Vector3(0, 360, 0), 1.2f)
                .loop = TweenerBase.LoopType.ResetLoop;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject go = other.gameObject;
            if (!go.TryGetComponent(out BaseController controller)) return;

            consumed = true;

            Perk perk = PerksHandler.Load(perkName);
            if (perk == null) PerksHandler.Random(0);

            if (perkLevel > 0) perk.LevelUp(perkLevel);
            if (perkBuff > 0) perk.ChargeBuff(perkBuff, perkCharge);

            controller.AddPerk(perk);

            if (GameModes.GameMode is CustomLevel level)
                level.ObjectiveEvents.Invoke<ILevelProp, string>
                    (typeof(Prop_Activation), this, gameObject.name);

            Destroy();
        }

        private void Destroy()
        {
            Collider2D collider = GetComponent<Collider2D>();
            if(collider) Destroy(collider);

            transform.Tween<Transform, Vector3, VectorRotationTween>
                (new Vector3(0, 180, 0), 0.4f)
                .loop = TweenerBase.LoopType.ReverseLoop;

            transform.Tween<Transform, Vector3, ScaleTween>
                (Vector3.zero, 1f, easing: BackEasing.Out);

            Destroy(gameObject, 1);
        }

        public void LoadParameters(object[] param)
        {
            PerkName = param.ParamAs<string>(0);
            perkLevel = param.ParamAs(1, 0);
            perkBuff = param.ParamAs(2, 5);
            perkCharge = param.ParamAs(3, 20);
        }
    }
}
