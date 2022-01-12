using IgnitedBox.UnityUtilities;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.CustomLevels;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Spawns
{
    public class Target_Enemy : ObjectivePreset, IControllerElimenated
    {
        public EnemyController Target { get; private set; }

        public Target_Enemy(GameObject element, ObjectiveData data = null) : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            Text title = base.Initialize(data);

            ChangeWidth("Title", 0.4f);

            SpawnTarget(data);
            SetIcon();
            SetHealthBar();

            Game?.ObjectiveEvents.Subscribe<CustomLevel, BaseController>
                (typeof(IControllerElimenated), Progress);

            return title;
        }

        private void SpawnTarget(ObjectiveData data)
        {
            string target_path = LoadParam(data, 1, "Regular/Tier1/Regular");
            int team = LoadParam(data, 2, 1);
            int level = LoadParam(data, 3, 0);
            GameObject obj = Resources.Load<GameObject>("Enemies/" + target_path);
            if (!obj) obj = Resources.Load<GameObject>("Enemies/Regular/Tier1/Regular");

            Target = GameModes.GameMode.SpawnEnemy(obj, team, level);
        }

        private void SetIcon()
        {
            Components.CreateGameObject<Image>(out GameObject skullObj, 
                "Skull", null, img => 
                {
                    img.sprite = Resources.Load<Sprite>($"Sprites/Bosses/{Target.Name}/Icon");
                    if (!img.sprite) img.sprite = Resources.Load<Sprite>("Sprites/Bosses/Skull");
                });

            Add(skullObj, 0.1f);
        }

        private void SetHealthBar()
        {
            GameObject bar = Resources.Load<GameObject>("UI/ShieldHealthBar");
            if (!bar) return;

            bar = Object.Instantiate(bar);
            bar.name = $"{Target.Name} Health";
            //bar.GetComponent<AspectRatioFitter>().enabled = false;

            Add(bar, 0.6f);

            Target.SetHealthBar(bar.transform);
        }

        public void Progress(AGameMode game, BaseController elimenated)
        {
            if (elimenated != Target) return;

            Handler.Remove(this);
        }
    }
}
