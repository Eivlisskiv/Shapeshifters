using IgnitedBox.Random.DropTables;
using UnityEngine;

namespace Scripts.OOP.Game_Modes
{
    public abstract class ArcadeMode<TTable> : AGameMode
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
                case 8: AddSpawns("Special/", "Cloner"); break;
                case 10: AddSpawns("Special/", "Eye Holder"); break;
            }
        }

        protected abstract void AddSpawns(string category, params string[] name);
    }
}
