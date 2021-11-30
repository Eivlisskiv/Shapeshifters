using IgnitedBox.Tweening.EasingFunctions;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.UI;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using IgnitedBox.Tweening;
using Scripts.MonoBehaviors.UI.Menu;

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

    private MenuAction action;
    private float cooldown;

    private SoundHandler sounds;

    private UIPerksList perks;
    private GameModeUI[] modes;

    private RectTransform openTab;
    private RectTransform containerRect;

    private GeneralButton arcade;
    private GeneralButton story;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponent<SoundHandler>();
        containerRect = container.GetComponent<RectTransform>();

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (action == MenuAction.None) return;

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
            GeneralButton.DeselectGroup(openTab.name);

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
        else if(openTab == null) //opening from no tab
        {
            containerRect.Tween<Transform, Vector3, PositionTween>(
            containerRect.localPosition - new Vector3(width / 2, 0), speed / 2.5f);

            if(tab)
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
        if (!button.tab) return;
        if (modes == null)
        {
            modes = new GameModeUI[GameModes.modes.Count];
            int i = 0;
            foreach(var mode in GameModes.modes)
            {
                modes[i] = new GameModeUI(mode.Key, mode.Value,
                    Instantiate(gameModePrefab, button.tab), this);
                i++;
            }
        }
        SwitchTab(button.tab);
    }
    
    public void StartGame(Type mode, string description)
    {
        if (action == MenuAction.Loading) return;
        action = MenuAction.Loading;

        AGameMode gamemode = (AGameMode)Activator.CreateInstance(mode, this, map);
        gamemode.description = description;
        gamemode.StartMap();

        SwitchTab(null);

        SetStartButton(false);
    }

    public void OnClick_Story(TabButton button)
    {
        if (!story) story = button;
        if (button.tab) SwitchTab(button.tab);
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

    private void GameOverUI()
    {
        if(sounds) sounds.PlayRandom("Game Over");
        gameOver.gameObject.SetActive(true);
        gameOver.SetScores(GameModes.LoadScore());
        cooldown = 5;
        action = MenuAction.GameOver;
    }

    private void EndGame()
    {
        gameOver.gameObject.SetActive(false);
        int newScore = GameModes.GameMode.Score;
        int topScore = GameModes.LoadScore();
        container.SetActive(true);
        if (newScore > topScore)
        {
            Text score = modes.First(m => 
                m.mode == GameModes.GameMode.GetType()).score;
            score.text = newScore.ToString();
            GameModes.SaveScore(newScore);
        }

        GameModes.GameMode?.EndGame();
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
