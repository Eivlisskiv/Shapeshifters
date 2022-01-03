using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using Scripts.OOP.Database;
using Tween.Source.Tweeners.FloatTweeners;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    public Transform topScoreTransform;
    public Transform topTimeTransform;
    public Transform lastScoreTransform;
    public Transform lastTimeTransform;

    UITextHandler topScore;
    UITextHandler lastScore;
    UITextHandler topTime;
    UITextHandler lastTime;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (topScore == null) topScore = new UITextHandler(topScoreTransform);
        if (topTime == null) topTime = new UITextHandler(topTimeTransform);
        if (lastScore == null) lastScore = new UITextHandler(lastScoreTransform);
        if (lastTime == null) lastTime = new UITextHandler(lastTimeTransform);

    }

    public void SetScores(int tscore, int lscore, float ttime, float ltime)
    {
        Init();

        if (tscore < 0) topScore.SetString("-");
        else topScore.SetScore(tscore);

        if (lscore < 0)
        {
            lastScore.SetString("-");
            lastScore.number.color = Color.white;
        }
        else
        {
            lastScore.SetScore(lscore);
            lastScore.number.color = lscore > tscore ? Color.green : Color.red;
        }

        
        topTime.SetString(ttime < 0 ? "-" : LevelProgress.BestTime(ttime));
        lastTime.SetString(ltime < 0 ? "-" : LevelProgress.BestTime(ltime));

        lastTime.number.color = ltime < 0 ? Color.white : 
            (ltime < ttime ? Color.green : Color.red);
    }

    internal class UITextHandler
    {
        internal Text number;
        internal Text background;

        internal UITextHandler(Transform parent)
        {
            number = parent.GetChild(1).GetComponent<Text>();
            background = parent.GetChild(0).GetComponent<Text>();
        }

        internal void SetString(string value)
        {
            number.text = value;
            background.text = value;
        }

        internal void SetScore(int score)
        {
            number.text = "0";
            background.text = "0";

            number.Tween<Text, float, TextIntegerTweener>(score, 2, 1, easing: ExponentEasing.Out);
            background.Tween<Text, float, TextIntegerTweener>(score, 2, 1, easing: ExponentEasing.Out);
        }
    }
}