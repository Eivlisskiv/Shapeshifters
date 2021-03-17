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

    // Start is called before the first frame update
    void Start()
    {
        Init();
        UpdateSprite();
    }

    private void Init()
    {
        if (inited) return;
        var tBase = transform.GetChild(0);
        icon = tBase.GetComponent<Image>();
        level = tBase.GetChild(0).GetComponent<Text>();
        charge = tBase.GetChild(1).GetComponent<Text>();

        inited = true;
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
}
