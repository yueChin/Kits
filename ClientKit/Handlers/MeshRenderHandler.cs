using System.Collections.Generic;
using UnityEngine;

namespace Kits.ClientKit.Handlers
{
    public static class MeshRenderHandler
    {
        /// <summary>
        /// Gets the cloud materials from the scene
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static List<Material> GetCloudLayerMaterials(string objectName, string ignoreContainName)
        {
            List<Material> materials = new List<Material>();
            if (objectName.Length > 0)
            {
                GameObject cloudObject = GameObject.Find(objectName);
                if (cloudObject != null)
                {
                    MeshRenderer[] meshRenderers = cloudObject.GetComponentsInChildren<MeshRenderer>();
                    if (meshRenderers.Length > 0)
                    {
                        foreach (MeshRenderer renderer in meshRenderers)
                        {
                            if (ignoreContainName.Length > 0)
                            {
                                if (!renderer.sharedMaterial.name.Contains(ignoreContainName))
                                {
                                    materials.Add(renderer.sharedMaterial);
                                }
                            }
                            else
                            {
                                materials.Add(renderer.sharedMaterial);
                            }
                        }
                    }
                }
            }
            return materials;
        }
    }
}