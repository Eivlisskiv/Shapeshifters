using IgnitedBox.Tweening.EasingFunctions;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.UI;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using IgnitedBox.Tweening;

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

    public RectTransform perkList;
    public GameObject perkDescPrefab;

    public RectTransform gameModeContainer;
    public GameObject gameModePrefab;

    public GameObject instructions;
    public GameObject objectivePrefab;

    private MenuAction action;
    private float cooldown;

    private Button start;
    private Text buttonText;
    private Image background;

    private SoundHandler sounds;

    private UIPerksList perks;
    private GameModeUI[] modes;

    private RectTransform openTab;
    private RectTransform containerRect;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponent<SoundHandler>();
        containerRect = container.GetComponent<RectTransform>();

        instance = this;

        perks = new UIPerksList(perkList, perkDescPrefab);
        perks.LoadPerks((perk, title, desc) =>
        {
            perk.LevelUp();
            title.text = $"{perk.Name} lvl {perk.Level}";
            desc.text = perk.Description;
        });
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
        
        if(openTab)//Close existing tab
        {
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

        if (tab == null) //If closing tab
        {
            containerRect.Tween<Transform, Vector3, PositionTween>(
            containerRect.localPosition + new Vector3(width / 2, 0), speed / 2.5f);
        }
        else if(openTab == null) //tab was closed
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

    private void CheckButtons(Button button)
    {
        if (!start)
        {
            start = button;
            buttonText = start.GetComponentInChildren<Text>();
        }
    }

    public void OnClick_TabButton(RectTransform tab)
        => SwitchTab(tab);

    public void OnClick_Start(Button button)
    {
        if(modes == null)
        {
            modes = new GameModeUI[GameModes.modes.Count];
            int i = 0;
            foreach(var mode in GameModes.modes)
            {
                modes[i] = new GameModeUI(mode.Key, mode.Value,
                    Instantiate(gameModePrefab, gameModeContainer), this);
                i++;
            }
        }
        SwitchTab(gameModeContainer);

        CheckButtons(button);
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

    public void SetStartButton(bool active)
    {
        buttonText.text = active ? "Select Game Mode" : "Loading...";
        if (!background) background = container.GetComponent<Image>();
        start.interactable = active;
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
