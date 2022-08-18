using UnityEngine;

namespace Kits.ClientKit.Handlers.Graphic
{
    public static class ShaderHandler
    {
        /// <summary>
        /// Validates that the shader property exists on the material
        /// </summary>
        /// <param name="material"></param>
        /// <param name="shaderID"></param>
        /// <returns></returns>
        public static bool ValidateShaderProperty(Material material, string shaderID)
        {
            if (material == null)
            {
                return false;
            }

            if (material.HasProperty(shaderID))
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Validates that the shader property exists on the material
        /// </summary>
        /// <param name="material"></param>
        /// <param name="shaderID"></param>
        /// <returns></returns>
        public static bool ValidateShaderProperty(Material material, int shaderID)
        {
            if (material == null)
            {
                return false;
            }

            if (material.HasProperty(shaderID))
            {
                return true;
            }

            return false;
        }
    }
}
