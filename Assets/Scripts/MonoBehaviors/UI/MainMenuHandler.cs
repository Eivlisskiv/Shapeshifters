using Scripts.OOP.Perks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    private static int topScore = 0;
    private static MainMenuHandler instance;

    public static void GameOver(int level)
    {
        //Play a game over animation
        instance.GameOverUI(level);
    }

    public GameOverHandler gameOver;
    public GameObject container;
    public MapHandler map;

    public Text score;

    public RectTransform perkList;
    public GameObject perkDescPrefab;

    float? goTime;

    private Button start;
    private Text buttonText;
    private Image background;

    private SoundHandler sounds;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponent<SoundHandler>();

        instance = this;
        if (PlayerPrefs.HasKey("Score"))
        {
            topScore = PlayerPrefs.GetInt("Score");
            score.text = topScore.ToString();
        }

        LoadPerks();
    }

    // Update is called once per frame
    void Update()
    {
        if (goTime.HasValue)
        {
            goTime = Math.Max(0, goTime.Value - Time.deltaTime);
            if (goTime == 0) EndGame();
        }
    }

    public void OnClick_Start(Button button)
    {
        map.enabled = true;
        map.StartMap(this);

        if (!start)
        {
            start = button;
            buttonText = start.GetComponentInChildren<Text>();
        }

        SetStartButton(false);
    }

    public void SetStartButton(bool active)
    {
        buttonText.text = active ? "Start" : "Loading...";
        if (!background) background = container.GetComponent<Image>();
        background.color = new Color(background.color.r, background.color.g, background.color.b, active ? 1 : 0.3f);
        start.interactable = active;
    }

    public void OnClick_Quit() => Application.Quit();

    private void LoadPerks()
    {
        perkList.transform.DetachChildren();

        foreach (var pair in PerksHandler.perksTypes)
        {
            Perk perk = (Perk)Activator.CreateInstance(pair.Value);
            perk.LevelUp();
            AddPerkDesc(perk);
        }
    }

    private void AddPerkDesc(Perk perk)
    {
        GameObject desc = Instantiate(perkDescPrefab, perkList.transform);
        Transform imgT = desc.transform.GetChild(0);
        Image img = imgT.GetComponent<Image>();
        if(img) img.sprite = Resources.Load<Sprite>($"Sprites/Perks/{perk.Name}");

        Transform texts = desc.transform.GetChild(1);
        Text title = texts.GetChild(0)?.GetComponent<Text>();
        if (title) title.text = perk.Name;
        Text description = texts.GetChild(1)?.GetComponent<Text>();
        if (description) description.text = perk.Description;

        RectTransform rect = desc.GetComponent<RectTransform>();
        perkList.sizeDelta += new Vector2(0, rect.sizeDelta.y);
    }
       

    private void GameOverUI(int newScore)
    {
        if(sounds) sounds.PlayRandom("Game Over");
        gameOver.gameObject.SetActive(true);
        gameOver.SetScores(topScore, newScore);
        goTime = 5;
    }

    private void EndGame()
    {
        gameOver.gameObject.SetActive(false);
        int newScore = gameOver.score;
        container.SetActive(true);
        if (newScore > topScore)
        {
            topScore = newScore;
            score.text = newScore.ToString();
            PlayerPrefs.SetInt("Score", topScore);
        }
        map.Clear();
    }
}
