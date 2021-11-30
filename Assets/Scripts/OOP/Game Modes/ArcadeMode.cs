using Scripts.OOP.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.Game_Modes
{
    public abstract class ArcadeMode : AGameMode
    {
        protected readonly Dictionary<string, (float rate, string[] names)> enemyTable;

        public ArcadeMode(MainMenuHandler menu, MapHandler map,
            Dictionary<string, (float, string[])> enemyTable, params Color[] teamColors)
            : base(menu, map, teamColors)
        {
            this.enemyTable = enemyTable;
        }

        protected GameObject GetRandomEnemy()
        {
            if (enemyTable != null && enemyTable.Count > 0)
            {
                float total = 0;
                float r = Random.Range(0, 101);
                foreach (var v in enemyTable)
                {
                    total += v.Value.rate;
                    if (r <= total)
                        return enemies.Instantiate($"{v.Key}/{Randomf.Element(v.Value.names)}");
                }
            }

            return enemies.Instantiate("Regular/Tier1/Regular");
        }
    }
}
