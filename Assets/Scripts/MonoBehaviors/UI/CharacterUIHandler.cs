using Scripts.OOP.Perks;
using Scripts.OOP.UI;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIHandler : MonoBehaviour
{
    public GameObject perkUIPrefab;

    public RectTransform healthContainer;
    public RectTransform xpContainer;
    public RectTransform perks;

    public Text level;

    StatBar health;
    public float healthp;
    StatBar xp;
    public float xpp;

    // Start is called before the first frame update
    void Start() 
    {
        health = new StatBar(healthContainer.transform);
        xp = new StatBar(xpContainer.transform);
    }

    void LateUpdate() 
    {
        health.Update(healthp);
        xp.Update(xpp);
    }

    public void UpdateHealth(float percent)
        => healthp = percent;
    public void UpdateXP(float percent)
        => xpp = percent;

    public void LevelUp(int level)
        => this.level.text = level.ToString();

    public void AddPerk(Perk perk)
    {
        GameObject ui = Instantiate(perkUIPrefab, perks);
        PerkUIHandler pui = ui.GetComponent<PerkUIHandler>();

        pui.UpdateSprite(Resources.Load<Sprite>($"Sprites/Perks/{perk.Name}"));

        pui.Init(perk.Level, perk.Buff, perk.Charge);
        perk.ui = pui;
    }
}
