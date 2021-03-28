using Scripts.OOP.Game_Modes;
using Scripts.OOP.Game_Modes.Rogue;
using Scripts.OOP.UI;
using Scripts.Tweening.Tweeners;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    private enum MenuAction { None, GameOver }

    private static MainMenuHandler instance;

    public static void GameOver(int level)
    {
        //Play a game over animation
        instance.GameOverUI(level);
    }

    public GameOverHandler gameOver;
    public GameObject container;
    public MapHandler map;

    public RectTransform perkList;
    public GameObject perkDescPrefab;

    public RectTransform gameModeContainer;
    public GameObject gameModePrefab;

    private Vector3 iposition;
    private MenuAction action;
    private float cooldown;

    private Button start;
    private Text buttonText;
    private Image background;

    private SoundHandler sounds;

    private UIPerksList perks;
    private GameModeUI[] modes;

    private RectTransform openTab;
    private UITween tweener;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponent<SoundHandler>();
        tweener = GetComponent<UITween>();

        instance = this;

        perks = new UIPerksList(perkList, perkDescPrefab);
        perks.LoadPerks((perk, title, desc) =>
        {
            perk.LevelUp();
            title.text = $"{perk.Name} lvl {perk.Level}";
            desc.text = perk.Description;
        });

        iposition = container.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (action != MenuAction.None)
        {
            switch (action)
            {
                case MenuAction.GameOver: EndGame(); break;
            }

            cooldown = Math.Max(0, cooldown - Time.deltaTime);
            if (cooldown == 0) action = MenuAction.None;
        }
    }

    private void SwitchTab(RectTransform tab)
    {
        const float speed = 0.4f;

        if (openTab == tab) return;
        
        if(openTab != null)//Close existing tab
        {
            tweener.Tween<RectSizeTween>(openTab,
                new Vector2(0, openTab.sizeDelta.y), speed);
        }

        if (tab == null) //If closing tab
        {
            tweener.Tween<PositionTween>(container.GetComponent<RectTransform>(),
            container.transform.localPosition + new Vector3(590 / 2, 0), speed);
        }
        else
        {
            if(openTab == null) //tab was closed
            {
                tweener.Tween<PositionTween>(container.GetComponent<RectTransform>(),
                container.transform.localPosition - new Vector3(590 / 2, 0), speed);
            }

            tweener.Tween<RectSizeTween>(tab,
                new Vector2(590, tab.sizeDelta.y), speed,
                openTab == null ? 0 : 05f);
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
    
    public void StartGame(Type mode)
    {
        AGameMode gamemode = (AGameMode)Activator.CreateInstance(mode, this, map);
        gamemode.StartMap();

        SwitchTab(null);

        SetStartButton(false);
    }

    public void SetStartButton(bool active)
    {
        buttonText.text = active ? "Start" : "Loading...";
        if (!background) background = container.GetComponent<Image>();
        start.interactable = active;
    }

    public void OnClick_Quit() => Application.Quit();

    private void GameOverUI(int newScore)
    {
        if(sounds) sounds.PlayRandom("Game Over");
        gameOver.gameObject.SetActive(true);
        gameOver.SetScores(GameModes.LoadScore(), newScore);
        cooldown = 5;
        action = MenuAction.GameOver;
    }

    private void EndGame()
    {
        action = MenuAction.None;
        gameOver.gameObject.SetActive(false);
        int newScore = gameOver.score;
        int topScore = GameModes.LoadScore();
        container.SetActive(true);
        if (newScore > topScore)
        {
            Text score = modes.First(m => m.mode == GameModes.GameMode.GetType()).score;
            score.text = newScore.ToString();
            GameModes.SaveScore(newScore);
        }

        GameModes.GameMode?.EndGame();
    }
}
