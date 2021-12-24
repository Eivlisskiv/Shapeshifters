﻿using IgnitedBox.UnityUtilities;
using Scripts.OOP.Database;
using Scripts.OOP.Game_Modes.Story;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Menu.Story
{
    public class StoryMenu : MonoBehaviour
    {
        public GameObject buttonPrefab;

        public Transform chaptersContent;
        public Transform episodeContent;

        public GameObject details;
        public Text details_Title; 
        public Text details_Score; 
        public Text details_Time; 
        public Text details_Level; 
        public Text details_Weapon;

        public Transform perks;
        public Transform upgrades;

        [NonSerialized]
        public MainMenuHandler mainMenu;

        private GeneralButton[] _chapters;

        private GeneralButton[][] episodes;
        private Transform[] episodeWindows;

        private int selectedChapter = -1;
        private int selectedEpisode = -1;

        private GameObject selectedWindow;

        private void SetText(GeneralButton button, string text)
        {
            Transform child = button.transform.GetChild(0);
            if (!child) return;

            Text ttext = child.GetComponent<Text>();
            if (!ttext) return;

            ttext.text = text;
        }

        private GeneralButton GetButton(GameObject button, string group)
        {
            Transform child = button.transform.GetChild(1);
            if (!child) return null;

            var btn = child.GetComponent<GeneralButton>();
            if (btn)
            {
                btn.selectBeforePress = false;
                btn.group = $"StoryMenu{group}";
            }
            return btn;
        }

        public void InitializeChapters()
        {
            if (_chapters != null) return;

            _chapters = new GeneralButton[Chapter.Episodes.Length];
            episodeWindows = new Transform[_chapters.Length];

            for (int i = 0; i < _chapters.Length; i++)
            {
                GameObject button = Instantiate(buttonPrefab);
                GeneralButton chapter = GetButton(button, "Chapter");

                button.transform.SetParent(chaptersContent, false);

                SetText(chapter, (i + 1).ToString());

                RectTransform rect = button.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(175, 100);

                int index = i;
                chapter.OnPress.AddListener(() => SelectChapter(index));

                _chapters[i] = chapter;
            }


            episodes = new GeneralButton[_chapters.Length][];
        }

        private void SelectChapter(int index)
        {
            if (index == selectedChapter) return;
            if(selectedChapter >= 0) _chapters[selectedEpisode].ChangeFocus(false);
            selectedChapter = index;

            GeneralButton.GroupOffStatus("StoryMenuEpisode");
            details.SetActive(false);
            selectedEpisode = -1;

            if (episodes[index] == null) CreateEpisodes(index);

            if (selectedWindow) selectedWindow.SetActive(false);
            selectedWindow = episodeWindows[index].gameObject;
            selectedWindow.SetActive(true);
        }

        private void CreateEpisodes(int index)
        {
            Transform window = episodeWindows[index];
            if (!window)
            {
                VerticalLayoutGroup layout = Components.CreateGameObject<VerticalLayoutGroup>("Chapter " + index);
                window = layout.transform;
                window.SetParent(episodeContent, false);
                RectTransform rect = window.GetComponent<RectTransform>();
                rect.Center();

                layout.padding = new RectOffset(0, 0, 25, 0);
                layout.spacing = 50;
                layout.childAlignment = TextAnchor.UpperCenter;
                layout.childControlHeight = false;
                layout.childControlWidth = false;

                episodeWindows[index] = window; 
            }

            int length = Chapter.Episodes[index].Length;
            episodes[index] = new GeneralButton[length];

            for (int i = 0; i < length; i++)
            {
                GameObject button = Instantiate(buttonPrefab);
                GeneralButton ep = GetButton(button, "Episode");

                button.transform.SetParent(window, false);

                SetText(ep, $"{index + 1}.{i + 1}");

                RectTransform rect = button.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(250, 75);

                int epIndex = i;
                ep.OnPress.AddListener(() => SelectEpisode(index, epIndex));

                episodes[index][i] = ep;
            }
        }

        private void SelectEpisode(int chapter, int ep)
        {
            if (ep == selectedEpisode) return;

            selectedEpisode = ep;

            if (!details.activeSelf) details.SetActive(true);

            details_Title.text = Chapter.Episodes[chapter][ep].Name ?? $"Episode {ep + 1}";

            StoryProgress data = StoryProgress.Load(chapter + 1, ep + 1, false);
            WriteDetails(data);
        }

        private void WriteDetails(StoryProgress data)
        {
            details_Score.text = data?.TopScore.ToString() ?? "-";
            details_Time.text = data?.BestTime() ?? "-";
            details_Level.text = data?.LevelReached.ToString() ?? "-";
            details_Weapon.text = data == null || data?.Weapon == "Weapon" ? "Basic" : data.Weapon;

            perks.DestroyChildren();
            if(data != null)
            {
                int c = data.Perks.Length;
                for (int i = 0; i < c; i++)
                {
                    OOP.Perks.SerializedPerk perk = data.Perks[i];
                    Image img = Components.CreateGameObject<Image>(perk.Name, perks);
                    img.sprite = Resources.Load<Sprite>("Sprites/Perks/" + perk.Name);
                    img.rectTransform.sizeDelta = new Vector2(75, 75);

                    Components.CreateGameObject<Text>("Level", img.transform, level =>
                    {
                        level.text = perk.Buff == 0 || perk.Charge == 0
                            ? perk.Level.ToString()
                            : $"+{perk.Buff + perk.Level}";

                        level.rectTransform.Center();
                        level.alignment = TextAnchor.MiddleCenter;
                        level.resizeTextForBestFit = true;
                        level.resizeTextMaxSize = 50;
                        level.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

                        level.color = new Color(1, 1, 1, 0.4f);
                    });
                }
            }

        }

        public void PlaySelectedEpisode()
        {
            if (selectedChapter < 0 || selectedEpisode < 0) return;

            GeneralButton.GroupOffStatus("StoryMenuEpisode");
            GeneralButton.GroupOffStatus("StoryMenuChapter");

            details.SetActive(false);
            selectedWindow.SetActive(false);

            mainMenu.StartStory(selectedChapter, selectedEpisode);

            selectedChapter = -1;
            selectedEpisode = -1;
        }
    }
}