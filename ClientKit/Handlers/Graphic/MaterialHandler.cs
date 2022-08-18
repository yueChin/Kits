using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Graphic
{
    public static class MaterialHandler
    {
        private static readonly List<MatEntry> s_Entries = new List<MatEntry>();

        public static Material Add(Material baseMat, Texture texture, int id)
        {
            MatEntry e;
            for (int i = 0; i < s_Entries.Count; ++i)
            {
                e = s_Entries[i];
                if (e.baseMat != baseMat || e.texture != texture || e.id != id) continue;
                ++e.count;
                return e.customMat;
            }

            e = new MatEntry
            {
                count = 1,
                baseMat = baseMat,
                texture = texture,
                id = id,
                customMat = new Material(baseMat)
                {
                    hideFlags = HideFlags.HideAndDontSave
                }
            };
            if (texture)
                e.customMat.mainTexture = texture;
            s_Entries.Add(e);
            // Debug.LogFormat(">>>> ModifiedMaterial.Add -> count = {0} {1} {2} {3}", s_Entries.Count, baseMat, texture, id);
            return e.customMat;
        }

        public static void Remove(Material customMat)
        {
            if (!customMat) return;

            for (int i = 0; i < s_Entries.Count; ++i)
            {
                MatEntry e = s_Entries[i];
                if (e.customMat != customMat)
                    continue;
                if (--e.count == 0)
                {
                    // Debug.LogFormat(">>>> ModifiedMaterial.Add -> count = {0} {1} {2} {3}", s_Entries.Count - 1, e.customMat, e.texture, e.id);
                    DestroyImmediate(e.customMat);
                    e.baseMat = null;
                    e.texture = null;
                    s_Entries.RemoveAt(i);
                }

                break;
            }
        }

        private static void DestroyImmediate(Object obj)
        {
            if (!obj) return;
            if (Application.isEditor)
                Object.DestroyImmediate(obj);
            else
                Object.Destroy(obj);
        }

        private class MatEntry
        {
            public Material baseMat;
            public Material customMat;
            public int count;
            public Texture texture;
            public int id;
        }
        
        /// <summary>
        /// Returns a list of materials using that shader
        /// </summary>
        /// <param name="targetShader"></param>
        /// <returns></returns>
        public static List<Material> FindMaterialsByShader(Shader targetShader)
        {

            List<Material> collectedMaterials = new List<Material>();
#if UNITY_EDITOR
            //We got the shader, now get all material GUIDs in the project
            string[] materialGUIDs = AssetDatabase.FindAssets("t:Material");
            //Iterate through the guids, load the material, if it uses our shader we collect it for the array

            foreach (string guid in materialGUIDs)
            {
                Material mat = (Material)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Material));
                if (mat.shader == targetShader)
                {
                    collectedMaterials.Add(mat);
                }
            }
#endif
            return collectedMaterials;
        }
    }
}
