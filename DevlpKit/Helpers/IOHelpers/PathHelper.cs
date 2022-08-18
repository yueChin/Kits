using System;
using System.IO;

namespace Kits.DevlpKit.Helpers.IOHelpers
{

    /// <summary>路径相关的实用函数。</summary>
    public static class PathHelper
    {
        // /// <summary>
        // /// 持久化根目录
        // /// </summary>
        // public static string PersistentPath
        // {
        //     get
        //     {
        //         return ResPath.Instance.PersistentPath;
        //     }
        // }

        /// <summary>获取规范的路径。</summary>
        /// <param name="path">要规范的路径。</param>
        /// <returns>规范的路径。</returns>
        public static string RegularPath(string path)
        {
            return path?.Replace('\\', '/');
        }

        /// <summary>获取连接后的路径。</summary>
        /// <param name="path">路径片段。</param>
        /// <returns>连接后的路径。</returns>
        public static string CombinePath(params string[] path)
        {
            if (path == null || path.Length < 1)
                return (string) null;
            string str = path[0];
            for (int index = 1; index < path.Length; ++index)
                str = System.IO.Path.Combine(str, path[index]);
            return PathHelper.RegularPath(str);
        }

        public static string CombinePath(int startIndex, int endIndex, params string[] path)
        {
            if (path == null || path.Length < 1)
                return (string) null;
            if (startIndex < 0 || startIndex >= path.Length - 1 || endIndex < 0 || endIndex > path.Length - 1 ||
                startIndex > endIndex)
            {
                return (string) null;
            }

            string str = path[startIndex];
            for (int index = startIndex + 1; index <= endIndex; ++index)
                str = System.IO.Path.Combine(str, path[index]);
            return PathHelper.RegularPath(str);
        }

        /// <summary>获取远程格式的路径（带有file:// 或 http:// 前缀）。</summary>
        /// <param name="path">原始路径。</param>
        /// <returns>远程格式路径。</returns>
        public static string RemotePath(params string[] path)
        {
            string combinePath = PathHelper.CombinePath(path);
            if (combinePath == null)
                return (string) null;
            if (!combinePath.Contains("://"))
                return ("file:///" + combinePath).Replace("file:////", "file:///");
            return combinePath;
        }

        /// <summary>获取带有后缀的资源名。</summary>
        /// <param name="resourceName">原始资源名。</param>
        /// <returns>带有后缀的资源名。</returns>
        public static string ResourceNameWithSuffix(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new Exception("Resource name is invalid.");
            return $"{(object) resourceName}.dat";
        }

        /// <summary>获取带有 CRC32 和后缀的资源名。</summary>
        /// <param name="resourceName">原始资源名。</param>
        /// <param name="hashCode">CRC32 哈希值。</param>
        /// <returns>带有 CRC32 和后缀的资源名。</returns>
        public static string ResourceNameWithCrc32AndSuffix(string resourceName, int hashCode)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new Exception("Resource name is invalid.");
            return $"{(object) resourceName}.{(object) hashCode:x8}.dat";
        }

        public static string[] ProgressiveAssetFolderPath(string assetFolderPath)
        {
            if (assetFolderPath.IndexOf(Path.DirectorySeparatorChar) != -1)
            {
                string[] folderName = assetFolderPath.Split(Path.DirectorySeparatorChar);
                string[] result = new string[folderName.Length];
                for (int i = 0; i < folderName.Length; i++)
                {
                    result[i] = CombinePath(0, i, folderName);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// 格式化路径为标准格式
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FormatPath(string path)
        {
            string result = path.Replace('\\', '/');
            return result.TrimEnd('/');
        }
        
        /// <summary>
        /// 获取路径名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileNameByPath(string path)
        {
            path = path.Replace("\\", "/");
            int lastGangIndex = path.LastIndexOf("/", StringComparison.Ordinal);
            if (lastGangIndex == -1)
                return "";
            lastGangIndex++;
            string name = path.Substring(lastGangIndex, path.Length - lastGangIndex);
            int lastDotIndex = name.LastIndexOf('.');
            if (lastDotIndex == -1)
                return "";
            name = name.Substring(0, lastDotIndex);
            return name;
        }

        /// <summary>
        /// 获取路径后缀
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileTypeSuffixByPath(string path)
        {
            path = path.Replace("\\", "/");
            int lastDotIndex = path.LastIndexOf('.');
            if (lastDotIndex == -1)
                return "";
            lastDotIndex++;
            string typeStr = path.Substring(lastDotIndex, path.Length - lastDotIndex);
            return typeStr;
        }
        
        
    }
}