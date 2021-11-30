using UnityEngine;

namespace Scripts.OOP.Game_Modes.Story
{
    public abstract class StoryMode : AGameMode
    {
        private float game_timer;

        protected MapPreset[] mapsPresets;

        public StoryMode(MainMenuHandler menu, MapHandler map)
            : base(menu, map, Color.green, Color.red)
        {
            //2. Populate the map with the props
            //3. Load the first objective and spawn the player
            //4. Start checking when there is no objectives: if so, story completed
        }

        public override void OnUpdate()
        {
            game_timer += Time.deltaTime;
        }

        protected override void OnMapStarted()
        {
            //Load the first map
            map.QueuRoom(mapsPresets[0]);
        }

        public override void OnLoaded()
        {
            //base only after loading props
            base.OnLoaded();
        }
    }
}
