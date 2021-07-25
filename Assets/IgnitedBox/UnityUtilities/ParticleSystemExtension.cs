using UnityEngine;

namespace Assets.IgnitedBox.UnityUtilities
{
    public static class ParticleSystemExtension
    {
        public static void SetMaterials(this ParticleSystem ps, Material main, Material trail = null)
        {
            var render = ps.GetComponent<ParticleSystemRenderer>();
            if (render)
            {
                render.material = main;
                if(trail != null) render.trailMaterial = trail;
            }
        }
    }
}
