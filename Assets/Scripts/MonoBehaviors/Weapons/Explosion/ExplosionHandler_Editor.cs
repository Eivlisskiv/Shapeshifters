#if UNITY_EDITOR
using UnityEditor;

namespace Scripts.Explosion
{
    [CustomEditor(typeof(ExplosionHandler), true)]
    public class ExplosionHandler_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            ExplosionHandler handler = (ExplosionHandler)target;

            base.OnInspectorGUI();

            if (Intensity(handler) ||
                Speed(handler) || Range(handler))
                handler.Restart();

            if(Rotation(handler) || Angle(handler))
                    handler.Play();
        }

        private bool Intensity(ExplosionHandler handler)
        {
            float value = EditorGUILayout.FloatField("Intensity", handler.Intensity);
            if (value == handler.Intensity) return false;
            handler.Intensity = value;
            return true;
        }

        private bool Speed(ExplosionHandler handler)
        {
            float value = EditorGUILayout.FloatField("Speed", handler.Speed);
            if (value == handler.Speed) return false;
            handler.Speed = value;
            return true;
        }

        private bool Range(ExplosionHandler handler)
        {
            float value = EditorGUILayout.FloatField("Range", handler.Range);
            if (value == handler.Range) return false;
            handler.Range = value;
            return true;
        }

        private bool Rotation(ExplosionHandler handler)
        {
            float value = EditorGUILayout.Slider("Rotation", handler.Rotation, 0 , 360);
            if (value == handler.Rotation) return false;
            handler.Rotation = value;
            return true;
        }

        private bool Angle(ExplosionHandler handler)
        {
            float value = EditorGUILayout.Slider("Angle", handler.Angle, 0, 360);
            if (value == handler.Angle) return false;
            handler.Angle = value;
            return true;
        }
    }
}
#endif
