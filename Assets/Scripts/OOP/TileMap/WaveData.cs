using System.Collections.Generic;
using UnityEngine;

namespace Scripts.OOP.TileMaps
{
    public class WaveData
    {
        public static WaveData Wave
        {
            get => instance;
        }

        static WaveData instance;

        public Transform contentParent;
        public MapTileType[,] mapContent;
        public int level;

        int spawned;
        float spawnCooldown = 5;
        readonly List<AIController> mobs;

        public WaveData(MapTileType[,] mapContent, Transform contentparent, int level)
        {
            contentParent = contentparent;
            this.mapContent = mapContent;
            this.level = level;

            mobs = new List<AIController>();

            if (instance != null) instance.Clear();
            instance = this;
        }

        public void Clear()
        {
            int mobsL = mobs.Count;
            for (int i = 0; i < mobsL; i++)
                if(mobs[i]) Object.Destroy(mobs[i].gameObject);
            mobs.Clear();
            Object.Destroy(contentParent.gameObject);

            instance = null;
        }

        public bool CheckEnemySpawns(out Vector2Int pos)
        {
            pos = Vector2Int.zero;
            if(spawnCooldown > 0)
            {
                spawnCooldown = System.Math.Max(spawnCooldown - Time.deltaTime, 0);
                return false;
            }
            
            if (mobs.Count < 3)
            {
                pos = new Vector2Int(
                    Random.Range(0, mapContent.GetLength(0)), 
                    Random.Range(0, mapContent.GetLength(1)));

                MapTileType type = mapContent[pos.x, pos.y];

                if(type == MapTileType.Empty)
                    return true;
            }

            return false;
        }

        public AIController SpawnMob(GameObject mob)
        {
            var ai = AIController.Spawn(mob, $"Enemy {spawned}", level);
            mobs.Add(ai);
            spawned++;
            return ai;
        }

        public void RemoveMob(AIController mob)
            => mobs.Remove(mob);
    }
}
