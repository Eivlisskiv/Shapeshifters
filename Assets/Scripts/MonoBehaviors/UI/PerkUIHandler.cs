using IgnitedBox.Tweening;
using IgnitedBox.Tweening.EasingFunctions;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using UnityEngine;
using UnityEngine.UI;

public class PerkUIHandler : MonoBehaviour
{
    public Sprite sprite;
    public Color textColor = Color.white;

    private bool inited;

    Image icon;
    Text level;
    Text charge;

    Vector3 ogPos;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        UpdateSprite();
    }

    private void Init()
    {
        if (inited) return;
        inited = true;

        var tBase = transform.GetChild(0);
        icon = tBase.GetComponent<Image>();
        level = tBase.GetChild(0).GetComponent<Text>();
        charge = tBase.GetChild(1).GetComponent<Text>();

        RectTransform rt = icon.GetComponent<RectTransform>();
        ogPos = rt.localPosition;
        rt.localPosition -= new Vector3(0, 40, 0);

        rt.Tween<Transform, Vector3, PositionTween>(ogPos,
            1.5f, easing: BackEasing.Out);
    }

    public void UpdateSprite(Sprite sprite = null)
    {
        if (sprite != null) this.sprite = sprite;
        if (this.sprite == null) return;

        if(icon) icon.sprite = this.sprite;
    }

    public void Init(int level, int buff, float charge)
    {
        Init();
        if(buff > 0) SetBuff(level + buff, charge);
        else SetLevel(level);
    }

    public void SetLevel(int level)
    {
        this.level.text = level.ToString();
        charge.text = "";
    }

    public void SetBuff(int intensity, float charge)
    {
        level.text = $"+{intensity}";
        SetCharge(charge);
    }

    public void SetCharge(float charge)
        => this.charge.text = Mathf.Ceil(charge).ToString();

    public void Remove()
    {
        icon.transform.Tween<Transform, Vector3, PositionTween>(
            icon.transform.localPosition - new Vector3(0, 30, 0), 1f, 
            callback: () => Destroy(gameObject));
    }

    public void Bounce()
    {
        icon.transform.Tween<Transform, Vector3, PositionTween>(
            ogPos - new Vector3(0, 30, 0), 0.8f,
            easing: ExponentEasing.Out,
            callback: () => icon.transform.Tween
            <Transform, Vector3, PositionTween>(ogPos, 0.8f));
    }
}
