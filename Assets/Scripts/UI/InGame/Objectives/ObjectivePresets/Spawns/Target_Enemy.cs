using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.CustomLevels;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.InGame.Objectives.ObjectivePresets.Spawns
{
    public class Target_Enemy : ObjectivePreset, IControllerElimenated
    {
        private EnemyController target;

        public Target_Enemy(GameObject element, ObjectiveData data = null) : base(element, data) { }

        protected override Text Initialize(ObjectiveData data)
        {
            Text title = base.Initialize(data);

            SpawnTarget(data);
            SetIcon();
            SetHealthBar();

            return title;
        }

        private void SpawnTarget(ObjectiveData data)
        {
            string target_path = LoadParam(data, 1, "Regular/Tier1/Regular");
            int team = LoadParam(data, 2, 1);
            int level = LoadParam(data, 3, 0);
            GameObject obj = Resources.Load<GameObject>("Enemies/" + target_path);
            if (!obj) obj = Resources.Load<GameObject>("Enemies/Regular/Tier1/Regular");

            target = GameModes.GameMode.SpawnEnemy(obj, team, level);
        }

        private void SetIcon()
        {
            Get<Image>("Skull", img =>
            {
                img.sprite = Resources.Load<Sprite>($"Sprites/Bosses/{target.Name}/Icon");
                if (!img.sprite) img.sprite = Resources.Load<Sprite>("Sprites/Bosses/Skull");

                img.transform.localScale = new Vector3(0.08f, 0.5f);
            });
        }

        private void SetHealthBar()
        {
            GameObject bar = Resources.Load<GameObject>("UI/ShieldHealthBar");
            if (!bar) return;

            bar = Object.Instantiate(bar);
            bar.name = $"{target.Name} Health";
            bar.GetComponent<AspectRatioFitter>().enabled = false;

            Add(bar);

            bar.transform.localScale = new Vector3(20, 0.9f, 1);
            target.SetHealthBar(bar.transform);
        }

        public void Progress(AGameMode game, BaseController elimenated)
        {
            if (elimenated != target) return;

            Handler.Remove(this);
        }
    }
}
