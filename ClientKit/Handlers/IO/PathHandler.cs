// using UnityEngine;
//
// namespace ClientKit.Handlers
// {
//     public static class PathHandler
//     {
//         /// <summary>
//         /// 各平台的持久化根目录
//         /// </summary>
//         public static string PersistentRootPath
//         {
//             get
//             {
// #if UNITY_STANDALONE || UNITY_EDITOR
//                 return Application.dataPath + ResPath.Instance.ClientPersistentRootPath;
// #elif UNITY_ANDROID || UNITY_IPHONE
//                 return ResPath.Instance.ClientPersistentRootPath;
// #endif
//                 return Application.dataPath + ResPath.Instance.ClientPersistentRootPath;
//             }
//         }
//
//         /// <summary>
//         /// 获取Assets下的相对目录
//         /// </summary>
//         /// <param name="path">完整目录</param>
//         /// <param name="applicationDataPath">applicationDataPath</param>
//         /// <returns></returns>
//         public static string GetAssetsRelativePath(string path, string applicationDataPath)
//         {
//             if (path.StartsWith(applicationDataPath))
//             {
//                 return path.Replace(applicationDataPath, "Assets");
//             }
//
//             return null;
//         }
//
//         public static string GetAssetFolderFullPath(string assetPath)
//         {
//             return Application.dataPath + assetPath.Substring("Assets".Length);
//         }
//     }
// }
