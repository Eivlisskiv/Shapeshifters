using UnityEngine;

namespace Scripts.OOP.Utils
{
    public static class StaticStuff
    {
        public static Material UnlitSpriteMaterial
        {
            get
            {
                if (!_unlitSpriteMaterial) _unlitSpriteMaterial = Resources.Load<Material>("Materials/Sprite-Unlit");
                return _unlitSpriteMaterial;
            }
        }

        static Material _unlitSpriteMaterial;
    }
}
