using IgnitedBox.Random.DropTables;
using Scripts.OOP.MongoRealm;
using UnityEngine;

namespace Scripts.OOP.Game_Modes
{
    public abstract class ArcadeMode : AGameMode 
    {
        public ArcadeMode(MainMenuHandler menu, MapHandler map, params Color[] teamColors)
            : base(menu, map, teamColors) { }

        protected override void ScoreChanged()
        {
            switch (Score)
            {
                case 1: AddSpawns("Regular/Tier1/", "Sniper"); break;
                case 2: AddSpawns("Regular/Tier1/", "Bomber"); break;
                case 3: AddSpawns("Regular/Tier1/", "Tank"); break;
                case 4: AddSpawns("Regular/Tier2/", "Flamer"); break;
                case 5: AddSpawns("Regular/Tier2/", "Gunner"); break;
                case 6: AddSpawns("Regular/Tier2/", "Pirate"); break;
                case 8: AddSpawns("Special/", "Eye Holder"); break;
                case 10: AddSpawns("Special/", "Cloner"); break;
            }
        }

        protected abstract void AddSpawns(string category, params string[] name);

        public override (string, int, int, float, float) LoadProgress()
        {
            ArcadeProgress progress = ArcadeProgress.LoadOne<ArcadeProgress>(Name);
            return ("Game Over", progress.TopScore, Score, -1, -1);
        }

        public override void SaveProgress()
        {
            ArcadeProgress progress = ArcadeProgress.LoadOne<ArcadeProgress>(Name);
            progress.LastScore = Score;
            if (Score > progress.TopScore)
                progress.TopScore = Score;
            progress.Save();
        }

        public override void UpdateMenu(MainMenuHandler menu)
        {
            ArcadeProgress progress = ArcadeProgress.LoadOne<ArcadeProgress>(Name);

            if(Score > progress.TopScore)
                menu.Modes[GetType()].SetScore(Score);
        }
    }

    public abstract class ArcadeMode<TTable> : ArcadeMode
        where TTable : Table
    {
        protected readonly TTable spawnTable;

        public ArcadeMode(MainMenuHandler menu, MapHandler map,
            TTable spawntable, params Color[] teamColors)
            : base(menu, map, teamColors)
        {
            spawnTable = spawntable;
        }

        protected GameObject GetRandomEnemy()
        {
            string name = spawnTable.DropCore<string>();
            if (string.IsNullOrEmpty(name))
            {
                Debug.Log("the drop core returned an invalid string");
                name = "Regular/Tier1/Regular";
            }

            return enemies.Instantiate(name);
        }
    }
}
