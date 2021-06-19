using Scripts.OOP.Game_Modes.Rogue;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks;
using Scripts.OOP.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    public RectTransform perkList;
    public GameObject perkPrefab;
    public Text pointsDisplay;

    public Text perkTitle;
    public Button purchase;
    public Text info;

    private (Perk perk, Text title, Text desc)? selected;

    internal PlayerController player;

    UIPerksList perks;

    Rogue mode;

    // Start is called before the first frame update
    void Start()
    {
        perks = new UIPerksList(perkList, perkPrefab);
        perks.LoadPerks((perk, title, description) =>
        {
            selected = (perk, title, description);
            SelectPerk();
        });

        mode = ((Rogue)GameModes.GameMode);
        pointsDisplay.text = mode.points.ToString();
    }

    private void SelectPerk()
    {
        if (!selected.HasValue) return;

        (Perk perk, _, _) = selected.Value;

        perkTitle.text = $"{perk.Name} Level {perk.Level}";

        purchase.enabled = false;

        if (player.Level < perk.Level)
        {
            info.text = $"You must be level {perk.Level} to upgrade " +
                $"{perk.Name} to level {perk.Level}";
            return;
        }

        int cost = perk.Level * perk.Level;
        if (mode.points < cost)
        {
            info.text = $"Missing {mode.points}/{cost} ({cost - mode.points}) points" +
                $" for level {perk.Level} {perk.Name}";
            return;
        }

        purchase.enabled = true;

        Text btnTitle = purchase.transform.GetChild(0).GetComponent<Text>();

        if (perk.Level == 1)
        {
            btnTitle.text = "Purchase";
            info.text = $"Purchase {perk.Name} for {cost} points";
        }
        else
        {
            btnTitle.text = "Upgrade";
            info.text = $"Upgrade {perk.Name} to level {perk.Level} for {cost} points";
        }
    }

    public void PurchasePerk()
    {
        if(!selected.HasValue) return;

        (Perk perk, Text title, Text desc) = selected.Value;

        int cost = perk.Level * perk.Level;

        mode.points -= cost;
        pointsDisplay.text = mode.points.ToString();
        Purchased(perk, title, desc);
        SelectPerk();
    }

    private void Purchased(Perk perk, Text title, Text desc)
    {
        var add = (Perk)Activator.CreateInstance(perk.GetType());
        add.LevelUp(); //Set to level 1;
        player.perks.Add(add, player.UI);

        perk.LevelUp();
        title.text = $"{perk.Name} lvl. {perk.Level}";
        desc.text = perk.Description;
    }

    public void Continue()
    {
        mode.MenuClosed();
        gameObject.SetActive(false);
    }
}
