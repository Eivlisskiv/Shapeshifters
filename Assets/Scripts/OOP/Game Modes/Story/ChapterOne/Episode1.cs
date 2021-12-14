using IgnitedBox.UnityUtilities.Vectors;
using Scripts.OOP.TileMaps;

namespace Scripts.OOP.Game_Modes.Story.ChapterOne
{
    public class Episode1 : Chapter1
    {
        public Episode1(StoryLevel set, MainMenuHandler menu, MapHandler map)
            : base(set, menu, map) 
        
        {
            levelSettings = new StoryLevel()
            {
                name = "Chapter 1, Act 1",
                playerSpawn = new SerializableInt2(0, 0),
                maps = new MapPreset[]
                {
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new MapTileType[,]
                        {
                            { MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, },
                            { MapTileType.Wall, MapTileType.Empty, MapTileType.Empty, MapTileType.Empty, MapTileType.Wall, },
                            { MapTileType.Wall, MapTileType.Empty, MapTileType.Empty, MapTileType.Empty, MapTileType.Empty, },
                            { MapTileType.Wall, MapTileType.Empty, MapTileType.Empty, MapTileType.Empty, MapTileType.Empty, },
                            { MapTileType.Wall, MapTileType.Empty, MapTileType.Empty, MapTileType.Empty, MapTileType.Wall, },
                            { MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, }
                        }
                    },
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new MapTileType[,]
                        {
                            { MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, },
                            { MapTileType.Wall, MapTileType.Empty, MapTileType.Empty, MapTileType.Empty, MapTileType.Wall, },
                            { MapTileType.Gate, MapTileType.Empty, MapTileType.Empty, MapTileType.Empty, MapTileType.Wall, },
                            { MapTileType.Gate, MapTileType.Empty, MapTileType.Empty, MapTileType.Empty, MapTileType.Wall, },
                            { MapTileType.Wall, MapTileType.Empty, MapTileType.Empty, MapTileType.Empty, MapTileType.Wall, },
                            { MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, MapTileType.Wall, }
                        }
                    }
                },
                main = new ObjectiveData[]
                {
                    new ObjectiveData()
                    {
                        id = "",
                        color = new SerializableFloat4(0, 0, 1, 1),
                    }
                }
            };
        
        }
    }
}
