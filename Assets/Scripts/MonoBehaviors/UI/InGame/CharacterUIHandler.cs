using Scripts.OOP.Perks;
using Scripts.OOP.UI.StatsBar;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIHandler : MonoBehaviour
{
    public GameObject perkUIPrefab;

    public RectTransform healthContainer;
    public RectTransform xpContainer;
    public RectTransform perks;

    public Text level;

    StatusBar xp;

    // Start is called before the first frame update
    void Start() 
    {
        xp = new StatusBar(xpContainer.transform);
    }

    public void UpdateXP(float percent)
        => xp.SetHealth(percent);

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
