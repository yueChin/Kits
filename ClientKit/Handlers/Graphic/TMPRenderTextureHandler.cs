using UnityEngine;

namespace Kits.ClientKit.Handlers.Graphic
{
    public static class TMPRenderTextureHandler
    {
        /// <summary>
        /// Workaround to release all temporary render textures. Even when Releasing temporary render textures in code, it can still
        /// happen that additional temp render textures stay in memory, this goes over all temporary render textures and releases them.
        /// </summary>
        public static void ReleaseAllTempRenderTextures()
        {
            RenderTexture[] rendTex = (RenderTexture[])Resources.FindObjectsOfTypeAll(typeof(RenderTexture));
            for (int i = rendTex.Length - 1; i >= 0; i--)
            {
                if (rendTex[i].name.StartsWith("TempBuffer"))
                {
                    RenderTexture.ReleaseTemporary(rendTex[i]);
                }
            }
            //GC.Collect();
        }
    }
}
