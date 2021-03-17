using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    public Transform topTransform;
    public Transform scoreTransform;

    UITextHandler top;
    UITextHandler current;

    public int score;

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

    public void SetScores(int top, int score)
    {
        Init();
        this.score = score;
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
            number.text = score.ToString();
            background.text = score.ToString();
        }
    }
}