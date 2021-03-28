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
    public Text errors;

    internal PlayerController player;

    UIPerksList perks;

    Rogue mode;

    // Start is called before the first frame update
    void Start()
    {
        perks = new UIPerksList(perkList, perkPrefab);
        perks.LoadPerks((perk, title, description)
            => PurchasePerk(perk, title, description));

        mode = ((Rogue)GameModes.GameMode);
        pointsDisplay.text = mode.points.ToString();
    }

    private void PurchasePerk(Perk perk, Text title, Text desc)
    {
        if(player.Level < perk.Level)
        {
            errors.text = $"You must be level {perk.Level} to upgrade " +
                $"{perk.Name} to level {perk.Level}";
            return;
        }

        int cost = perk.Level * perk.Level;
        if (mode.points < cost)
        {
            errors.text = $"Missing {mode.points}/{cost} ({cost - mode.points}) points" +
                $" for level {perk.Level} {perk.Name}";
            return;
        }

        errors.text = "";

        mode.points -= cost;
        pointsDisplay.text = mode.points.ToString();
        Purchased(perk, title, desc);
    }

    private void Purchased(Perk perk, Text title, Text desc)
    {
        var add = (Perk)Activator.CreateInstance(perk.GetType());
        add.LevelUp(); //Set to level 1;
        player.perks.Add(add, player.UI);

        perk.LevelUp();
        title.text = $"{perk.Name} lvl {perk.Level}";
        desc.text = perk.Description;
    }

    public void Continue()
    {
        mode.MenuClosed();
        gameObject.SetActive(false);
    }
}
