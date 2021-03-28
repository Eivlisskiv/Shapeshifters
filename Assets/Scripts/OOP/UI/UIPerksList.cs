using Scripts.OOP.Perks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.OOP.UI
{
    class UIPerksList
    {
        Perk[] perks;

        readonly RectTransform container;
        readonly GameObject prefab;

        public UIPerksList(RectTransform container, GameObject prefab)
        {
            this.container = container;
            this.prefab = prefab;
        }

        public void LoadPerks(Action<Perk, Text, Text> onClick)
        {
            container.DetachChildren();
            perks = new Perk[PerksHandler.perksTypes.Count];

            int i = 0;
            foreach (var pair in PerksHandler.perksTypes)
            {
                Perk perk = (Perk)Activator.CreateInstance(pair.Value);
                perk.LevelUp();
                AddPerkDesc(perk, onClick);
                perks[i] = perk;
                i++;
            }
        }

        private void AddPerkDesc(Perk perk, Action<Perk, Text, Text> onClick)
        {
            GameObject desc = UnityEngine.Object.Instantiate(prefab, container);
            Transform imgT = desc.transform.GetChild(0);
            Image img = imgT.GetComponent<Image>();
            if (img) img.sprite = Resources.Load<Sprite>($"Sprites/Perks/{perk.Name}");

            Transform texts = desc.transform.GetChild(1);
            Text title = texts.GetChild(0).GetComponent<Text>();
            if (title) title.text = $"{perk.Name} lvl {perk.Level}";
            Text description = texts.GetChild(1).GetComponent<Text>();
            if (description) description.text = perk.Description;

            RectTransform rect = desc.GetComponent<RectTransform>();
            container.sizeDelta += new Vector2(0, rect.sizeDelta.y);

            Button button = desc.GetComponent<Button>();
            if (button && onClick != null)
            {
                button.onClick.AddListener(() =>
                    onClick(perk, title, description));
            }
        }
    }
}
