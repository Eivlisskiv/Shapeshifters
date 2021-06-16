using IgnitedBox.Tweening.Components;
using IgnitedBox.Tweening.Tweeners;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace IgnitedBox.Tweening.Editor
{
    [CustomEditor(typeof(Tweener))]
    public class TweenerInspector : UnityEditor.Editor
    {
        private static Type[] _tweenTypes;
        private static string[] _tweenNames;
        private static string[] TweenNames
            => _tweenNames ?? LoadTweenTypes();

        private bool DrawOne(TweenerBase tween, int i = -1)
        {
            EditorGUILayout.BeginHorizontal();

            tween.editorOpen = EditorGUILayout.Foldout(
                tween.editorOpen, tween.GetType().Name, true);

            if (i > -1 && GUILayout.Button("-", GUILayout.Width(25)))
            {
                tweener.Remove(i);
                return false;
            }

            EditorGUILayout.EndHorizontal();

            if (!tween.editorOpen) return true;

            tween.EditorObjectField();
            tween.EditorValueFields();
            Element(tween);

            return true;
        }

        private static void Element(TweenerBase tween)
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

        private static void Easing(TweenerBase tween)
        {
            tween.drawEasing = EditorGUILayout.Toggle("Draw Easing", tween.drawEasing);
            if (tween.drawEasing)
            {
                AnimationCurve curve = EditorGUILayout.CurveField("Easing",
                tween.Curve ?? new AnimationCurve());
                tween.Curve = curve;
            }

            tween.Curve = null;
        }

        public class Callback : UnityEngine.Events.UnityEvent { }

        public Callback callback;

        private Tweener tweener;

        private static string[] LoadTweenTypes()
        {
            Type tweeners = typeof(TweenerBase);
            var assembly = Assembly.GetAssembly(tweeners);
            Type[] types = assembly.GetTypes();

            _tweenTypes = types.Where(t => !t.IsAbstract && !t.IsInterface
                && t.IsSubclassOf(tweeners)).ToArray();

            return (_tweenNames = _tweenTypes.Select(t => t.Name).ToArray());
        }

        private bool showTweenList = true;

        private int tweenTypeIndex = 0;

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
            while (i < tw.Count)
            {
                EditorGUI.indentLevel++;

                if (DrawOne(tw.Get(i), i)) i++;

                EditorGUI.indentLevel--;
            }
        }

        private void DrawAdd(Tweener tweener)
        {
            if (TweenNames.Length == 0) return;

            EditorGUILayout.BeginHorizontal();

            tweenTypeIndex = EditorGUILayout.Popup(tweenTypeIndex, TweenNames);

            if (tweener.ContainsType(_tweenTypes[tweenTypeIndex]))
                EditorGUILayout.LabelField(TweenNames[tweenTypeIndex] +
                    $" already exists.");
            else if (GUILayout.Button("Add"))
                tweener.Add(_tweenTypes[tweenTypeIndex]);

            EditorGUILayout.EndHorizontal();
        }
    }
}
