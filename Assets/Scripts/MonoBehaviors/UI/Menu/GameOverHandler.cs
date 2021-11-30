using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using Scripts.OOP.Game_Modes;
using Tween.Source.Tweeners.FloatTweeners;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    public Transform topTransform;
    public Transform scoreTransform;

    UITextHandler top;
    UITextHandler current;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (top == null || current == null)
        {
            top = new UITextHandler(topTransform);
            current = new UITextHandler(scoreTransform);
        }
    }

    public void SetScores(int top)
    {
        Init();
        int score = GameModes.GameMode.Score;
        this.top.SetScore(top);
        current.SetScore(score);
        current.number.color = score > top ? Color.green : Color.red;
    }

    internal class UITextHandler
    {
        internal Text number;
        internal Text background;

        internal UITextHandler(Transform parent)
        {
            var textParent = parent.GetChild(1);
            number = textParent.GetChild(1).GetComponent<Text>();
            background = textParent.GetChild(0).GetComponent<Text>();
        }

        internal void SetScore(int score)
        {
            number.text = "0";
            background.text = "0";

            number.Tween<Text, float, TextIntegerTweener>(score, 0.3f, easing: ExponentEasing.Out);
            background.Tween<Text, float, TextIntegerTweener>(score, 0.3f, easing: ExponentEasing.Out);
        }
    }
}