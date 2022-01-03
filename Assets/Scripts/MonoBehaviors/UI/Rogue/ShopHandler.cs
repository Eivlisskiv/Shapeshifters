using Scripts.OOP.Game_Modes.Rogue;
using Scripts.OOP.Game_Modes;
using Scripts.OOP.Perks;
using Scripts.OOP.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using IgnitedBox.Tweening.Tweeners.ColorTweeners;
using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners;

public class ShopHandler : MonoBehaviour
{
    public RectTransform perkList;
    public GameObject perkPrefab;
    public Text pointsDisplay;

    public Text perkTitle;
    public GeneralButton purchase;
    public Text info;

    Text btnTitle;

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

        btnTitle = purchase.transform.GetChild(1).GetComponent<Text>();
        purchase.Enabled = false;
    }

    private void SelectPerk()
    {
        if (!selected.HasValue) return;

        (Perk perk, _, _) = selected.Value;

        perkTitle.text = $"{perk.Name} Level {perk.Level}";

        purchase.Enabled = false;

        if (player.Level < perk.Level)
        {
            InfoTextRed($"You must be level {perk.Level} to upgrade " +
                $"{perk.Name} to level {perk.Level}");
            return;
        }

        int cost = perk.Level * perk.Level;
        if (mode.points < cost)
        {
            InfoTextRed($"Missing {mode.points}/{cost} ({cost - mode.points}) points" +
                $" for level {perk.Level} {perk.Name}");
            return;
        }

        purchase.Enabled = true;

        if (perk.Level == 1)
        {
            btnTitle.text = "Purchase";
            purchase.ChangeFocus(false);
            InfoTextGreen($"Purchase {perk.Name} for {cost} points");
        }
        else
        {
            btnTitle.text = "Upgrade";
            purchase.ChangeFocus(false);
            InfoTextGreen($"Upgrade {perk.Name} to level {perk.Level} for {cost} points");
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
        player.AddPerk(add);

        perk.LevelUp();
        title.text = $"{perk.Name} lvl. {perk.Level}";
        desc.text = perk.Description;
    }

    public void Continue()
    {
        mode.MenuClosed();
        gameObject.SetActive(false);
    }

    private void InfoTextGreen(string text)
    {
        TweenTitleColor(Color.green);
        info.text = text;
        var tween = info.Tween<Graphic, Color, GraphicColorTween>
            (Color.green, 1f);
        tween.scaledTime = false;
    }

    private void InfoTextRed(string text)
    {
        TweenTitleColor(Color.red);
        info.color = Color.clear;
        info.text = text;
        var tween = info.Tween<Graphic, Color, GraphicColorTween>
            (Color.red, 0.5f, 1.5f, easing: (t) => 4*Math.Pow(t - 0.5, 2));
        tween.scaledTime = false;
        tween.loop = TweenerBase.LoopType.ResetLoop;
    }

    private void TweenTitleColor(Color color)
    {
        if (perkTitle.color == color) return;
        perkTitle.Tween<Graphic, Color, GraphicColorTween>(color, 0.5f)
            .scaledTime = false;
    }
}
