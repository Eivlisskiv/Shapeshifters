using IgnitedBox.UnityUtilities.Vectors;
using Scripts.OOP.Game_Modes.Story;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.OOP.TileMaps
{
    public class FixedMapRoom : MapRoom
    {
        public readonly int tileBaseIndex;

        protected Dictionary<string, GameObject> props;

        public FixedMapRoom(MapPreset preset, RoomHandler previous, Transform propsContainer)
            : base(previous, preset.tiles, propsContainer)
        {
            tileBaseIndex = preset.tileBaseIndex;

            SpawnProps(preset.props);
        }

        private void SpawnProps(MapProp[] props)
        {
            this.props = new Dictionary<string, GameObject>();

            if (props == null || props.Length == 0) return;

            for(int i = 0; i < props.Length; i++)
            {
                MapProp mp = props[i];
                if (this.props.TryGetValue(mp.id, out _)) continue;
                //instantiate the prop 
                GameObject prefab = Resources.Load<GameObject>(mp.prefabPath);
                if (!prefab) continue;
                GameObject prop = Object.Instantiate(prefab, propsContainer);
                prop.name = mp.id;

                prop.transform.SetParent(propsContainer, true);

                prop.transform.localPosition = (Vector2)mp.position;
                prop.SetActive(false);
                this.props.Add(mp.id, prop);
            }
        }

        public override void Initialize() { }

        public override bool DrawOne(Tilemap map, TileBase tilebase, bool center, out MapTileType tile)
        {
            tile = GetTile(current);
            HandleTileDraw(map, tilebase, tile);
            return Next();
        }

        public override void LoadFinished()
        {
            ForEachProp(prop => prop.SetActive(true));
        }

        protected void ForEachProp(System.Action<GameObject> action)
        {
            foreach (KeyValuePair<string, GameObject> prop in props)
                if(prop.Value) action(prop.Value);
        }
    }
}
