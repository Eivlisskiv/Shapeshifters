using IgnitedBox.UnityUtilities.Vectors;

namespace Scripts.OOP.Game_Modes.Story.ChapterOne
{
    public class Episode1 : StoryLevel
    {
        public Episode1(MainMenuHandler menu, MapHandler map)
            : base(new LevelSettings()
            {
                name = "Chapter 1, Act 1",
                description = "ERROR \\\\\\\\ INCOMING INSTRUCTIONS \\\\\\\\",
                playerSpawn = new SerializableInt2(1, 2),
                maps = new MapPreset[]
                {
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new int[,]
                        {
                            { 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,},
                            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,},
                            { 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1,},
                            { 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1,},
                            { 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 2,},
                            { 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 2,},
                            { 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0, 2,},
                            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 1,},
                            { 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1,},
                            { 1, 0, 1, 1, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1,},
                            { 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1,}
                        },
                        props = new MapProp[]
                        {
                            new MapProp()
                            {
                                id = "PerkSelector1",
                                prefabPath = "Props",
                                position = new SerializableFloat2(-2, 4),
                            }
                        }
                    },
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new int[,]
                        {
                            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, },
                            { 0, 0, 1, 3, 3, 3, 3, 3, 3, 1, 0, 0, },
                            { 0, 1, 3, 3, 3, 3, 3, 3, 3, 3, 1, 0, },
                            { 1, 0, 3, 3, 3, 3, 1, 3, 3, 3, 3, 1, },
                            { 1, 0, 0, 3, 3, 1, 1, 1, 3, 3, 3, 1, },
                            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 1, },
                            { 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 1, },
                            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 1, },
                            { 1, 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 1, },
                            { 1, 0, 0, 0, 3, 3, 1, 3, 3, 3, 3, 1, },
                            { 0, 1, 0, 3, 3, 3, 3, 3, 3, 3, 0, 1, },
                            { 0, 0, 1, 0, 3, 3, 3, 3, 3, 0, 2, 2, },
                            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, }
                        }
                    },
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new int[,]
                        {
                            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 0, 0, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 0, 0, 1, },
                            { 0, 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 1, },
                            { 0, 0, 1, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 1, },
                            { 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1, },
                            { 0, 0, 1, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 1, 3, 0, 0, 1, },
                            { 0, 0, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 3, 0, 1, },
                            { 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, },
                            { 0, 0, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 0, 0, 1, },
                            { 0, 0, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 0, 0, 0, 1, },
                            { 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, }
                        }
                    }

                },
                main = new ObjectiveData[]
                {
                    new ObjectiveData()
                    {
                        id = "Reach Map",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "\\\\\\ PROCEED \\_ NEXT ST4GE 0\\0\\0\\0\\0" }
                    },
                    new ObjectiveData()
                    {
                        id = "Waves",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] 
                        { 
                            "HOSTILE \\ CLEAR THREAT",
                            1,
                            new (string, int)[][]
                            {
                                new (string, int)[] { 
                                    ("Regular/Tier1/Regular", 0),
                                },
                                new (string, int)[] {
                                    ("Regular/Tier1/Regular", 0),
                                    ("Regular/Tier1/Regular", 3),
                                },
                                new (string, int)[] {
                                    ("Regular/Tier1/Bomber", 5),
                                    ("Regular/Tier1/Regular", 0),
                                },
                                new (string, int)[] {
                                    ("Regular/Tier1/Bomber", 5),
                                    ("Regular/Tier1/Regular", 0),
                                    ("Regular/Tier1/Regular", 0),
                                }
                            }
                        }
                    },
                    new ObjectiveData()
                    {
                        id = "Reach Map",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "\\00000\\ CONTINUE \\_ ST4GE \\0[u10]\\0" }
                    }
                }
            }, menu, map) 
        
        { }
    }
}
