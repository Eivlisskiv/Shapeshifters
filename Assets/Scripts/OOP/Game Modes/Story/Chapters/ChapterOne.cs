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
                PlayerSpawn = new SerializableInt2(5, 6),
                Maps = new MapPreset[]
                {
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        yOffset = -16,
                        tiles = new int[,]
                        {
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 2, 2, 2, 2, },
                            { 1, 1, 1, 1, 0, 0, 0, 2, 2, 2, 2, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, },
                        },
                    },
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new int[,]
                        {
                            { 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                            { 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1},
                            { 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1},
                            { 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2},
                            { 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2},
                            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 2},
                            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1},
                            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1},
                            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1},
                            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                        },
                        props = new MapProp[]
                        {
                            new MapProp("Module1", "Props/Perks/PerkModule", -4.53f, 13.21f),
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
                        },
                        props = new MapProp[]
                        {
                            new MapProp("ShieldModule", "Props/Perks/PerkModule", 3f, 0f, "Shield", 5, 0, 0),
                            new MapProp("Target1", "Props/Targets/TargetPractice", -6.78f, 8.39f, 100, 0),
                            new MapProp("Target2", "Props/Targets/TargetPractice", 11.81f, 8.59f, 100, 1),
                            new MapProp("Target3", "Props/Targets/TargetPractice", 11.31f, -11f, 100, 2),
                            new MapProp("Target4", "Props/Targets/TargetPractice", -6.06f, -9.95f, 100, 3),
                        }
                    },
                    new MapPreset()
                    {
                        tileBaseIndex = 0,
                        tiles = new int[,]
                        {
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,},
                            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 0, 3, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 3, 0, 1, 1, 1,},
                            { 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1,},
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1,},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,},
                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,}
                        },
                        props = new MapProp[]
                        {
                            new MapProp("Target1", "Props/Targets/TargetPractice", -15.86f, 15.89f, 100, 32),
                            new MapProp("Target2", "Props/Targets/TargetPractice", 15.46f, 10.53f, 100, 32),
                            new MapProp("Target3", "Props/Targets/TargetPractice", 9.12f, -3f, 100, 32),
                            new MapProp("Target4", "Props/Targets/TargetPractice", -3.26f, -16.2f, 100, 32),

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
                        parameters = new object[] { "reach the exit" }
                    },
                    new ObjectiveData()
                    {
                        id = "Reach Map",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "PROCEED \\_ NEXT ST4GE" }
                    },
                    new ObjectiveData()
                    {
                        id = "Prop Activation",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "Destroy the first target", "Target1", }
                    },
                    new ObjectiveData()
                    {
                        id = "Prop Activation",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "Destroy the second target", "Target2", }
                    },
                    new ObjectiveData()
                    {
                        id = "Prop Activation",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "Destroy the third target", "Target3", }
                    },
                    new ObjectiveData()
                    {
                        id = "Prop Activation",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "Destroy the last target", "Target4", }
                    },
                    new ObjectiveData()
                    {
                        id = "Reach Map",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "CONTINUE \\_ STAGE" }
                    },
                    new ObjectiveData()
                    {
                        id = "Prop Activation",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "Destroy the first target", "Target1", }
                    },
                    new ObjectiveData()
                    {
                        id = "Prop Activation",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "Destroy the second target", "Target2", }
                    },
                    new ObjectiveData()
                    {
                        id = "Prop Activation",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "Destroy the third target", "Target3", }
                    },
                    new ObjectiveData()
                    {
                        id = "Prop Activation",
                        color = new SerializableFloat4(0, 0, 1, 1),
                        parameters = new object[] { "Destroy the last target", "Target4", }
                    },
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
