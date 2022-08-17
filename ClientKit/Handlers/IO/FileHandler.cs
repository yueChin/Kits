// using System.IO;
// using System.Text;
// using ClientKit.Utilities;
// using DevlpKit.Helpers;
// using DevlpKit.Helpers.IOHelpers;
// using UnityEngine;
// using Debuger = AFForUnity.EditorForUnity.Extension.Debugger.Debuger;
//
// namespace ClientKit.Handlers
// {
//     public class FileHandler
//     {
//         /// <summary>
//         ///  写入文本
//         /// </summary>
//         /// <param name="relativePath">相对路径</param>
//         /// <param name="text">文本</param>
//         public static void WriteText(string relativePath, string text)
//         {
//             if (string.IsNullOrEmpty(relativePath) || string.IsNullOrEmpty(text))
//                 return;
//
//             string allPath = string.Concat(PathHandler.PersistentRootPath, relativePath);
//             byte[] byteArr = Encoding.UTF8.GetBytes(text);
//             FileHelper.Write(allPath, byteArr);
//         }
//
//         /// <summary>
//         /// 读取文本
//         /// </summary>
//         /// <param name="relativePath">文本所在工程的相对路径</param>
//         /// <returns></returns>
//         public static string ReadText(string relativePath)
//         {
//             if (string.IsNullOrEmpty(relativePath)) return null;
//
//             string allPath = string.Concat(PathHandler.PersistentRootPath, relativePath);
//             if (!File.Exists(allPath))
//             {
//                 Debuger.LogWarning("<<FileGameUtil , Read>> Cant read file ! Path is " + allPath);
//                 return null;
//             }
//
//             return File.ReadAllText(allPath);
//         }
//
//     }
// }
