using IgnitedBox.Tweening.EasingFunctions;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.UI;
using System;
using UnityEngine;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using IgnitedBox.Tweening;
using Scripts.UI.InGame.Objectives;
using System.Collections.Generic;
using UnityEngine.UI;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using Scripts.OOP.Game_Modes.Story;
using Scripts.UI.Menu.Story;
using Scripts.OOP.Database;

public class MainMenuHandler : MonoBehaviour
{
    private enum MenuAction { None, Loading, GameOver }

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

    public StoryMenu storyMenu;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponent<SoundHandler>();
        containerRect = container.GetComponent<RectTransform>();

        background = container.transform.GetChild(0).GetComponent<Image>();
        background.transform.SetParent(transform, true);
        background.transform.SetSiblingIndex(0);

        storyMenu.mainMenu = this;
        storyMenu.InitializeChapters();
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

        if (openTab != null)//Close existing tab
        {
            GeneralButton.GroupOffStatus(openTab.name);

            if (openTab)
            {
                openTab.Tween<RectTransform, Vector3, RectSizeTween>(
                    new Vector2(-1, openTab.sizeDelta.y), speed,
                    easing: ExponentEasing.Out);
            }
            if (tab) //Switching Tab
            {
                tab.Tween<RectTransform, Vector3, RectSizeTween>(
                new Vector2(width, tab.sizeDelta.y), speed, speed,
                ElasticEasing.Out);
            }
            else  //Closing tab
            {
                containerRect.Tween<Transform, Vector3, PositionTween>(
                containerRect.localPosition + new Vector3(width / 2, 0), speed / 2.5f);
            }
        }
        else if (tab) //opening from no tab
        {
            containerRect.Tween<Transform, Vector3, PositionTween>(
            containerRect.localPosition - new Vector3(width / 2, 0), speed / 2.5f);

            if (tab) 
            {
                tab.Tween<RectTransform, Vector3, RectSizeTween>(
                new Vector2(width, tab.sizeDelta.y), speed,
                easing: ElasticEasing.Out);
            }
        }

        openTab = tab ? tab : null;
    }

    public void OnClick_TabButton(RectTransform tab)
        => SwitchTab(tab);

    public void OnClick_Arcade(Scripts.MonoBehaviors.UI.Menu.TabButton button)
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
            foreach (KeyValuePair<Type, GameModes.ModeData> mode in GameModes.arcadeModes)
            {
                ArcadeModeUI ui = new ArcadeModeUI(mode.Key, mode.Value.description);
                ui.InitializeUI(Instantiate(gameModePrefab, button.tab), this);
                VerifyUnlocked(ui);
                Modes.Add(mode.Key, ui);
            }
        }
        else
        {
            foreach (ArcadeModeUI mode in Modes.Values)
                VerifyUnlocked(mode);
        }

        SwitchTab(button.tab);
    }
    
    private void VerifyUnlocked(ArcadeModeUI ui)
    {
        GameModes.ModeData data = GameModes.arcadeModes[ui.mode];
        (int chap, int ep) = data.storyRequirement;
        ui.Button.Locked = !StoryProgress.Completed(chap, ep);
    }

    public void StartGame(ArcadeModeUI ui)
    {
        if (action == MenuAction.Loading) return;

        if (ui.Button.Locked)
        {
            GameModes.ModeData data = GameModes.arcadeModes[ui.mode];
            (int chap, int ep) = data.storyRequirement;
            //if(!StoryProgress.Completed(chap, ep)) { }
            if(story) story.ChangeSelect(true); else arcade.ChangeSelect(false);
            SwitchTab(storyMenu.transform.GetComponent<RectTransform>());
            storyMenu.GoToUnlocked(chap - 1, ep - 1);

            return;
        }

        LoadingStarting();
        SwitchTab(null);
        SetStartButton(false);

        AGameMode gamemode = (AGameMode)Activator.CreateInstance(ui.mode, this, map);
        gamemode.description = ui.desc;
        gamemode.StartMap();
    }

    public void OnClick_Story(Scripts.MonoBehaviors.UI.Menu.TabButton button)
    {
        if (!story) story = button;

        if (action == MenuAction.Loading)
        {
            SetStartButton(false);
            return;
        }

        if (button.tab)
        {
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
        if (perks == null)
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
        }

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

    public void GameOver()
    {
        //Play a game over animation
        GameOverUI();
    }

    private void GameOverUI()
    {
        if(sounds) sounds.PlayRandom("Game Over");
        gameOver.gameObject.SetActive(true);

        (int tscore, int lscore, float ttime, float ltime) = GameModes.GameMode.LoadProgress();

        gameOver.SetScores(tscore, lscore, ttime, ltime);
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
