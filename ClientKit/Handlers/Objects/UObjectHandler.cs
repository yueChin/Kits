using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Objects
{
    public static class UObjectHandler
    {
        public static string ObjectToGUID(UnityEngine.Object obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            return (!string.IsNullOrEmpty(path)) ? AssetDatabase.AssetPathToGUID(path) : null;
        }

        private static MethodInfo s_GetInstanceIDFromGUID;
        public static UnityEngine.Object GUIDToObject(string guid)
        {
            if (string.IsNullOrEmpty(guid)) return null;

            if (s_GetInstanceIDFromGUID == null)
                s_GetInstanceIDFromGUID = typeof(AssetDatabase).GetMethod("GetInstanceIDFromGUID", BindingFlags.Static | BindingFlags.NonPublic);

            int id = (int)s_GetInstanceIDFromGUID.Invoke(null, new object[] { guid });
            if (id != 0) return EditorUtility.InstanceIDToObject(id);
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) return null;
            return AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
        }

        public static T GUIDToObject<T>(string guid) where T : UnityEngine.Object
        {
            UnityEngine.Object obj = GUIDToObject(guid);
            if (obj == null) return null;

            System.Type objType = obj.GetType();
            if (objType == typeof(T) || objType.IsSubclassOf(typeof(T))) return obj as T;

            if (objType == typeof(UnityEngine.GameObject) && typeof(T).IsSubclassOf(typeof(Component)))
            {
                UnityEngine.GameObject go = obj as UnityEngine.GameObject;
                return go.GetComponent(typeof(T)) as T;
            }
            return null;
        }
    }
}