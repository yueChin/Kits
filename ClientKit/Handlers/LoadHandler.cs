using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Kits.ClientKit.Handlers
{
    public static class LoadHandler
    {
        /// <summary>
        /// Allows to loads an asset from unitys hidden builtin-extra resources
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aAssetPath"></param>
        /// <returns></returns>
        public static T LoadAssetFromUniqueAssetPath<T>(string aAssetPath) where T : UnityEngine.Object
        {
            if (aAssetPath.Contains("::"))
            {
                string[] parts = aAssetPath.Split(new string[] { "::" }, System.StringSplitOptions.RemoveEmptyEntries);
                aAssetPath = parts[0];
                if (parts.Length > 1)
                {
                    string assetName = parts[1];
                    System.Type t = typeof(T);
                    IEnumerable<T> assets = AssetDatabase.LoadAllAssetsAtPath(aAssetPath)
                        .Where(i => t.IsInstanceOfType(i)).Cast<T>();
                    T obj = assets.FirstOrDefault(i => i.name == assetName);
                    if (obj == null)
                    {
                        int id;
                        if (int.TryParse(parts[1], out id))
                            obj = assets.FirstOrDefault(i => i.GetInstanceID() == id);
                    }
                    if (obj != null)
                        return obj;
                }
            }
            return AssetDatabase.LoadAssetAtPath<T>(aAssetPath);
        }
        
        public static string GetUniqueAssetPath(UnityEngine.Object aObj)
        {
            string path = AssetDatabase.GetAssetPath(aObj);
            if (!string.IsNullOrEmpty(aObj.name))
                path += "::" + aObj.name;
            else
                path += "::" + aObj.GetInstanceID();
            return path;
        }
    }
}
