using UnityEditor.SceneManagement;
using UnityEngine;

namespace Kits.ClientKit.Handlers
{
    public static class PathHandler
    {
        /// <summary>
        /// 获取预制体资源路径
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static string GetPrefabAssetPath(GameObject gameObject)
        {
#if UNITY_EDITOR
            // Project中的Prefab是Asset不是Instance
            if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
            {
                // 预制体资源就是自身
                return UnityEditor.AssetDatabase.GetAssetPath(gameObject);
            }

// Scene中的Prefab Instance是Instance不是Asset
            if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(gameObject))
            {
                // 获取预制体资源
                GameObject prefabAsset = UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);
                return UnityEditor.AssetDatabase.GetAssetPath(prefabAsset);
            }

// PrefabMode中的GameObject既不是Instance也不是Asset
            PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
            if (prefabStage != null)
            {
                // 预制体资源：prefabAsset = prefabStage.prefabContentsRoot
                return prefabStage.prefabAssetPath;
            }
#endif
// 不是预制体
            return null;
        }
    }
}