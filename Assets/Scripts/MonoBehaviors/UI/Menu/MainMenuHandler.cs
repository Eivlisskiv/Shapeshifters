using IgnitedBox.Tweening.EasingFunctions;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.UI;
using System;
using UnityEngine;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using IgnitedBox.Tweening;
using Scripts.MonoBehaviors.UI.Menu;
using Scripts.UI.InGame.Objectives;
using System.Collections.Generic;
using UnityEngine.UI;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using Scripts.OOP.Game_Modes.Story;
using Scripts.UI.Menu.Story;

public class MainMenuHandler : MonoBehaviour
{
    private enum MenuAction { None, Loading, GameOver }

    private static MainMenuHandler instance;

    public static void GameOver()
    {
        //Play a game over animation
        instance.GameOverUI();
    }

    public GameOverHandler gameOver;
    public GameObject container;
    public MapHandler map;

    public GameObject perkDescPrefab;

    public GameObject gameModePrefab;

    public GameObject instructions;
    public GameObject objectivePrefab;

    private Image background;

    private MenuAction action;

    private float cooldown;

    private SoundHandler sounds;

    private UIPerksList perks;

    public Dictionary<Type, ArcadeModeUI> Modes { get; private set; }

    private RectTransform openTab;
    private RectTransform containerRect;

    private GeneralButton arcade;
    private GeneralButton story;

    public StoryMenu StoryMenu { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponent<SoundHandler>();
        containerRect = container.GetComponent<RectTransform>();

        background = container.transform.GetChild(0).GetComponent<Image>();
        background.transform.SetParent(transform, true);
        background.transform.SetSiblingIndex(0);

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (action == MenuAction.None || action == MenuAction.Loading
            || cooldown == 0) return;

        cooldown = Math.Max(0, cooldown - Time.deltaTime);
        if (cooldown == 0)
        {
            if (action == MenuAction.GameOver) EndGame();
            action = MenuAction.None;
        }
    }

    private void SwitchTab(RectTransform tab)
    {
        const float speed = 1f;
        const float width = 650f;

        if (openTab == tab) return;

        if (openTab)//Close existing tab
        {
            GeneralButton.GroupOffStatus(openTab.name);

            openTab.Tween<RectTransform, Vector3, RectSizeTween>(
                new Vector2(-1, openTab.sizeDelta.y), speed,
                easing: ExponentEasing.Out, callback: () =>
                {
                    if (tab)
                    {
                        tab.Tween<RectTransform, Vector3, RectSizeTween>(
                        new Vector2(width, tab.sizeDelta.y), speed,
                        easing: ElasticEasing.Out);
                    }
                });
        }

        if (tab == null) //If closing current tab
        {
            containerRect.Tween<Transform, Vector3, PositionTween>(
            containerRect.localPosition + new Vector3(width / 2, 0), speed / 2.5f);
        }
        else if (openTab == null) //opening from no tab
        {
            containerRect.Tween<Transform, Vector3, PositionTween>(
            containerRect.localPosition - new Vector3(width / 2, 0), speed / 2.5f);

            if (tab)
                tab.Tween<RectTransform, Vector3, RectSizeTween>(
                new Vector2(width, tab.sizeDelta.y), speed,
                easing: ElasticEasing.Out);
        }

        openTab = tab;
    }

    public void OnClick_TabButton(RectTransform tab)
        => SwitchTab(tab);

    public void OnClick_Arcade(TabButton button)
    {
        if (!arcade) arcade = button;

        if (action == MenuAction.Loading)
        {
            SetStartButton(false);
            return;
        }

        if (!button.tab) return;

        if (Modes == null)
        {
            Modes = new Dictionary<Type, ArcadeModeUI>();
            foreach (var mode in GameModes.modes)
            {
                ArcadeModeUI ui = new ArcadeModeUI(mode.Key, mode.Value);
                ui.InitializeUI(Instantiate(gameModePrefab, button.tab), this);
                Modes.Add(mode.Key, ui);
            }
        }

        SwitchTab(button.tab);
    }
    
    public void StartGame(Type mode, string description)
    {
        if (action == MenuAction.Loading) return;

        LoadingStarting();
        SwitchTab(null);
        SetStartButton(false);

        AGameMode gamemode = (AGameMode)Activator.CreateInstance(mode, this, map);
        gamemode.description = description;
        gamemode.StartMap();
    }

    public void OnClick_Story(TabButton button)
    {
        if (!story) story = button;

        if (action == MenuAction.Loading)
        {
            SetStartButton(false);
            return;
        }

        if (button.tab)
        {
            if (!StoryMenu)
            {
                StoryMenu = button.tab.GetComponent<StoryMenu>();
                StoryMenu.mainMenu = this;
                StoryMenu.InitializeChapters();
            }

            SwitchTab(button.tab);
        }
    }

    public void StartStory(int chapterIndex, int episodeIndex)
    {
        LoadingStarting();
        SwitchTab(null);
        SetStartButton(false);

        StoryLevel game = new StoryLevel(Chapter.Episodes[chapterIndex][episodeIndex], this, map);
        game.StartMap();
    }

    public void OnClick_Perks(RectTransform tab)
    {
        RectTransform c = tab.GetChild(0)
            .GetChild(0).GetChild(0)
            .GetComponent<RectTransform>();

        perks = new UIPerksList(c, perkDescPrefab);
        perks.LoadPerks((perk, title, desc) =>
        {
            perk.LevelUp();
            title.text = $"{perk.Name} lvl {perk.Level}";
            desc.text = perk.Description;
        });

        SwitchTab(tab);
    }

    public void SetStartButton(bool active)
    {
        if (arcade) arcade.Enabled = active;
        if (story) story.Enabled = active;
    }

    public void OnClick_Quit() => Application.Quit();

    private void LoadingStarting()
    {
        action = MenuAction.Loading;
        background.Tween<Graphic, Color, GraphicColorTween>(Color.clear, 2);
    }

    private void GameOverUI()
    {
        if(sounds) sounds.PlayRandom("Game Over");
        gameOver.gameObject.SetActive(true);
        gameOver.SetScores(GameModes.GameMode.LoadProgress());
        cooldown = 5;
        action = MenuAction.GameOver;
    }

    private void EndGame()
    {
        gameOver.gameObject.SetActive(false);
        container.SetActive(true);
        GameModes.GameMode.UpdateMenu(this);
        GameModes.GameMode.SaveProgress();
        GameModes.GameMode.EndGame();

        background.Tween<Graphic, Color, GraphicColorTween>(Color.white, 2);
    }

    internal ObjectiveHandler SpawnGameUI(string gamemodeDesc, Action onReady)
    {
        var o = Instantiate(instructions);
        var i = o.GetComponent<OnStartInstructions>();
        i.onReady = onReady;
        i.SetObjective(gamemodeDesc);

        o.transform.SetParent(transform.parent);
        o.transform.localPosition = new Vector3(0, 0, 0);

        var objectiveContainer = Instantiate(objectivePrefab);
        objectiveContainer.transform.SetParent(transform.parent);
        return objectiveContainer.GetComponent<ObjectiveHandler>();
    }
}
