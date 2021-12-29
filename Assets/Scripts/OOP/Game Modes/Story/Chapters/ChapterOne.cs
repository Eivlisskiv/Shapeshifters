using IgnitedBox.UnityUtilities.Vectors;
using Scripts.OOP.Game_Modes.CustomLevels;

namespace Scripts.OOP.Game_Modes.Story.Chapters
{
    public class ChapterOne
    {
        public static StorySettings[] episodes = new StorySettings[]
        {
            new StorySettings(1, 1, "System Check")
            {
                Description = "ERROR \\\\\\\\ INCOMING INSTRUCTIONS \\\\\\\\",
                PlayerSpawn = new SerializableInt2(2, 2),
                Maps = new MapPreset[]
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
                            { 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2,},
                            { 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2,},
                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 2,},
                            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1,},
                            { 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1,},
                            { 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1,},
                            { 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1,}
                        },
                        props = new MapProp[]
                        {
                            new MapProp("Module1", "Props/Perks/PerkModule", -4.53f, 11.89f, "Shield", 5, 0, 0),
                        }
                    },
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new int[,]
                        {
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 1, 1, 1, },
                            { 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, },
                            { 1, 1, 0, 3, 3, 3, 3, 1, 3, 3, 3, 3, 1, },
                            { 1, 1, 0, 0, 3, 3, 1, 1, 1, 3, 3, 3, 1, },
                            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 1, },
                            { 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 1, },
                            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 1, },
                            { 1, 1, 0, 0, 0, 0, 1, 1, 1, 3, 3, 3, 1, },
                            { 1, 1, 0, 0, 0, 3, 3, 1, 3, 3, 3, 3, 1, },
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 0, 1, },
                            { 1, 1, 1, 1, 0, 3, 3, 3, 3, 3, 0, 2, 2, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, }
                        }
                    },
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new int[,]
                        {
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 1, 3, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 3, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 0, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, }
                        },
                        props = new MapProp[]
                        {
                            new MapProp("LandMine1", "Projectiles/Landmine", -10.27f, 8.52f, 0, 100),
                            new MapProp("LandMine2", "Projectiles/Landmine", 2.39f, 8.52f, 0, 100),
                            new MapProp("LandMine3", "Projectiles/Landmine", -10.27f, 0, 0, 100),
                            new MapProp("LandMine4", "Projectiles/Landmine", 2.39f, 0, 0, 100),
                            new MapProp("LandMine5", "Projectiles/Landmine", -10.27f, -10f, 0, 100),
                            new MapProp("LandMine6", "Projectiles/Landmine", 4.34f, -10f, 0, 100),
                        }
                    }

                },
                Main = new ObjectiveData[]
                {
                    new ObjectiveData()
                    {
                        id = "Reach Map",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "PROCEED \\_ NEXT ST4GE" }
                    },
                    new ObjectiveData()
                    {
                        id = "Waves",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[]
                        {
                            "HOSTILES \\ CLEAR THREAT",
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
                        parameters = new object[] { "CONTINUE \\_ STAAAAAAGE" }
                    },
                    new ObjectiveData()
                    {
                        id = "Waves",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[]
                        {
                            "HOSTILES \\ CLEAR THREAT",
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
                    }
                }
            },
            new StorySettings(1, 2, "Modular Equipment")
            {
                Description = "Systems Functional. Proceed",
                PlayerSpawn = new SerializableInt2(10, 14),
                Maps = new MapPreset[]
                {
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new int[,]
                        {
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 1, 3, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 3, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 0, 0, 0, 1, 1, 1, },
                            { 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 2, 2, 2, },
                            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, },
                            { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, }
                        },
                    },
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new int[,]
                        {
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, }
                        },
                        props = new MapProp[]
                        {
                            new MapProp("Module1", "Props/Perks/PerkModule", -16.7f, -14.03f, "Shield", 5, 0, 0),
                            new MapProp("Module2", "Props/Perks/PerkModule", 0, -14.03f, "Health Regen", 5, 0, 0),
                            new MapProp("Module3", "Props/Perks/PerkModule", 16.7f, -14.03f, "Shield", 5, 0, 0),
                        }
                    },
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new int[,]
                        {
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 2, 2, 2,},
                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2,},
                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,},
                            { 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 1, 1, 1, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 1, 1, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 1, 1, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 1, 1, 1, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1,},
                            { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,}
                        },
                    }
                },
                Main = new ObjectiveData[]
                {
                    new ObjectiveData()
                    {
                        id = "Reach Map",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "PROCEED" }
                    },
                    new ObjectiveData()
                    {
                        id = "Prop Activation",
                        color = new SerializableFloat4(0, 1, 0, 1),
                        parameters = new object[] { "COLLECT MODULE/PERK", "Module2" }
                    },
                    new ObjectiveData()
                    {
                        id = "Reach Map",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "PROCEED" }
                    },
                    new ObjectiveData()
                    {
                        id = "Waves",
                        color = new SerializableFloat4(1, 0, 0, 1),
                        parameters = new object[]
                        {
                            "SYSTEM INTRUSION \\ CLEAR THREAT",
                            1,
                            new (string, int)[][]
                            {
                                new (string, int)[] {
                                    ("Regular/Tier1/Regular", 0),
                                    ("Regular/Tier1/Regular", 0),
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
                        id = "Timer",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "Prepare for next wave...", 30f }
                    },
                    new ObjectiveData()
                    {
                        id = "Waves",
                        color = new SerializableFloat4(1, 0, 0, 1),
                        parameters = new object[]
                        {
                            "SYSTEM INTRUSION \\ CLEAR THREAT",
                            1,
                            new (string, int)[][]
                            {
                                new (string, int)[] {
                                    ("Regular/Tier1/Regular", 0),
                                    ("Regular/Tier1/Regular", 0),
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
                }
            },
        };
    }
}
