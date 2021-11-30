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
            : base(previous, new Vector2Int(preset.tiles.GetLength(0),
                                 preset.tiles.GetLength(1)), propsContainer)
        {
            tileBaseIndex = preset.tileBaseIndex;
            mapContent = preset.tiles;

            SpawnProps(preset.props);
        }

        private void SpawnProps(MapProp[] props)
        {
            this.props = new Dictionary<string, GameObject>();

            for(int i = 0; i < props.Length; i++)
            {
                MapProp mp = props[i];
                if (this.props.TryGetValue(mp.id, out _)) continue;
                //instantiate the prop 
                GameObject prefab = Resources.Load<GameObject>(mp.prefabPath);
                if (!prefab) continue;
                GameObject prop = GameObject.Instantiate(prefab, propsContainer);
                prop.name = mp.id;
                prop.transform.localPosition = (Vector2)mp.position;
                prop.SetActive(false);
                this.props.Add(mp.id, prop);
            }
        }

        public override void Initialize()
        {
            //Nothing to initialize
        }

        public override bool DrawOne(Tilemap map, TileBase tilebase, bool center, out MapTileType tile)
        {
            tile = mapContent[current.x, current.y];
            HandleTileDraw(map, tilebase, tile);
            return Next();
        }
    }
}
