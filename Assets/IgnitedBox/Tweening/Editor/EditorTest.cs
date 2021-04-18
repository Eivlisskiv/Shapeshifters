using IgnitedBox.Tweening.Conponents;
using IgnitedBox.Tweening.Tweeners;
using IgnitedBox.Tweening.Tweeners.VectorTweeners;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IgnitedBox.Tweening.Editor
{
    [CustomEditor(typeof(Tweener))]
    public class EditorTest : UnityEditor.Editor
    {
        private static Type[] _tweenTypes;
        private static string[] _tweenNames;
        private static string[] TweenNames
            => _tweenNames ?? LoadTweenTypes();

        public class Callback : UnityEngine.Events.UnityEvent { }

        public Callback callback;

        private Tweener tweener;

        private static string[] LoadTweenTypes()
        {
            _tweenTypes = new Type[]
            {
                typeof(RectSizeTween),
                typeof(PositionTween)
            };

            return (_tweenNames = _tweenTypes.Select(t => t.Name).ToArray());
        }

        private bool showTweenList = true;

        private int tweenTypeIndex = 0;

        private bool drawEasing;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            tweener = (Tweener)target;

            showTweenList = EditorGUILayout.Foldout(
                showTweenList, "Tweens", true);
            if (!showTweenList) return;

            DrawEachGUI(tweener);
            DrawAdd(tweener);
        }

        private void DrawEachGUI(Tweener tw)
        {
            int i = 0;
            while (i < tw.tweens.Count)
            {
                EditorGUI.indentLevel++;

                var pair = tw.tweens.ElementAt(i);
                if (DrawOne(pair.Key, pair.Value)) i++;

                EditorGUI.indentLevel--;
            }
        }

        private bool DrawOne(Type key, TweenerBase tween)
        {
            EditorGUILayout.BeginHorizontal();

            tween.editorOpen = EditorGUILayout.Foldout(
                tween.editorOpen, key.Name, true);

            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                tweener.Remove(key);
                return false;
            }

            EditorGUILayout.EndHorizontal();

            if (!tween.editorOpen) return true;

            tween.EditorObjectField();
            tween.EditorValueFields();
            Element(tween);

            return true;
        }

        private void Element(TweenerBase tween)
        {
            tween.Delay = EditorGUILayout.FloatField(
                "Delay", tween.Delay);

            tween.Duration = EditorGUILayout.FloatField(
                "Duration", tween.Duration);

            tween.Time = EditorGUILayout.FloatField(
                "Current Time", tween.Time);

            tween.loop = (TweenerBase.LoopType)EditorGUILayout.EnumPopup("Loop", tween.loop);

            EditorGUILayout.BeginHorizontal();
            Easing(tween);
            EditorGUILayout.EndHorizontal();
        }

        private void Easing(TweenerBase tween)
        {
            drawEasing = EditorGUILayout.Toggle("Draw Easing", drawEasing);
            if (drawEasing)
            {
                AnimationCurve curve = EditorGUILayout.CurveField("Easing",
                tween.Curve ?? new AnimationCurve());
                tween.Curve = curve;
                return;
            }

            tween.Curve = null;
        }

        private void DrawAdd(Tweener tweener)
        {
            if (TweenNames.Length == 0) return;

            EditorGUILayout.BeginHorizontal();

            tweenTypeIndex = EditorGUILayout.Popup(tweenTypeIndex, TweenNames);

            if (tweener.tweens.ContainsKey(_tweenTypes[tweenTypeIndex]))
                EditorGUILayout.LabelField(TweenNames[tweenTypeIndex] +
                    $" already exists.");
            else if (GUILayout.Button("Add"))
                tweener.Add(_tweenTypes[tweenTypeIndex]);

            EditorGUILayout.EndHorizontal();
        }
    }
}
