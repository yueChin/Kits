using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Kits.DevlpKit.Helpers.IOHelpers
{
    public static class DirectoryHelper
    {
        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectoryExist(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public static string CurrentDirectory
        {
            get { return System.Environment.CurrentDirectory; }
        }

        public static void CleanDirectory(string dir)
        {
            foreach (string subDir in System.IO.Directory.GetDirectories(dir))
            {
                System.IO.Directory.Delete(subDir, true);
            }

            foreach (string subFile in System.IO.Directory.GetFiles(dir))
            {
                File.Delete(subFile);
            }
        }

        public static void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
            }
        }

        // 删除delPath目录中 不存在于srcPath中的文件
        public static void RidOfUnNecessaryFile(string srcPath, string checkPath)
        {
            Debug.LogFormat("RidOfUnNecessaryFile, srcPath:{0}, checkPath:{1}", srcPath, checkPath);
            if (!System.IO.Directory.Exists(checkPath))
            {
                //Debuger.Log("目标路径不存在");
                return;
            }
            if (!System.IO.Directory.Exists(srcPath))
            {
                //Debuger.Log("参考路径不存在，删除目标路径整个目录");
                DeleteDirectory(checkPath);
                CreateDirectory(checkPath);
                return;
            }


            string[] allSrcFiles = System.IO.Directory.GetFiles(srcPath);
            string[] allCheckFiles = System.IO.Directory.GetFiles(checkPath);
            Dictionary<string, bool> dicSrcFiles = new Dictionary<string, bool>();
            for (int i = 0; i < allSrcFiles.Length; i++)
            {
                string strFile = Path.GetFileName(allSrcFiles[i]);
                dicSrcFiles.Add(strFile, true);
            }
            for (int i = 0; i < allCheckFiles.Length; i++)
            {
                string strFile = allCheckFiles[i];
                string filename = Path.GetFileName(strFile);
                if (!dicSrcFiles.ContainsKey(filename))
                {
                    Debug.LogFormat("删除多余文件：{0}", strFile);
                    FileHelper.DeleteFile(strFile);
                }
            }
            //Debuger.Log("清除完成！");
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param> 路径
        /// <param name="isOverride"></param> 是否覆盖原有同名目录
        public static void CreateDirectory(string path, bool isOverride = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else if (Directory.Exists(path) && isOverride)
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static void CopyDir(string srcPath, string destPath)
        {
            if (string.IsNullOrEmpty(srcPath) || string.IsNullOrEmpty(destPath))
            {
                return;
            }
            CreateDirectory(destPath);
            DirectoryInfo sDir = new DirectoryInfo(srcPath);
            FileInfo[] fileArray = sDir.GetFiles();
            foreach (FileInfo file in fileArray)
            {
                if (file.Extension != ".meta")
                    file.CopyTo(destPath + "/" + file.Name, true);
            }
            //递归复制子文件夹
            DirectoryInfo[] subDirArray = sDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirArray)
            {
                if (subDir.Name != ".idea")
                {
                    CopyDir(subDir.FullName, destPath + "/" + subDir.Name);
                }
            }
        }

        /// <summary>
        /// 清空目录下内容
        /// </summary>
        /// <param name="path"></param>
        public static void ClearDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            DirectoryInfo sDir = new DirectoryInfo(path);
            FileInfo[] fileArray = sDir.GetFiles();
            foreach (FileInfo file in fileArray)
            {
                file.Delete();
            }
            DirectoryInfo[] subDirArray = sDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirArray)
            {
                subDir.Delete(true);
            }
        }

        /// <summary>移除空文件夹。</summary>
        /// <param name="directoryName">要处理的文件夹名称。</param>
        /// <returns>是否移除空文件夹成功。</returns>
        public static bool RemoveEmptyDirectory(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName))
                throw new Exception("Directory name is invalid.");
            try
            {
                if (!Directory.Exists(directoryName))
                    return false;
                string[] directories = Directory.GetDirectories(directoryName, "*");
                int length = directories.Length;
                foreach (string directoryName1 in directories)
                {
                    if (RemoveEmptyDirectory(directoryName1))
                        --length;
                }

                if (length > 0 || Directory.GetFiles(directoryName, "*").Length != 0)
                    return false;
                Directory.Delete(directoryName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void EnsureParentDirExist(string path)
        {
            string dir = Path.GetDirectoryName(path);
            Queue<string> parents = new Queue<string>();
            while (!Directory.Exists(dir) && !string.IsNullOrEmpty(dir))
            {
                parents.Enqueue(dir);
                dir = Path.GetDirectoryName(dir);
            }
            while (parents.Count > 0)
            {
                Directory.CreateDirectory(parents.Dequeue());
            }
        }

        public static string AssetDirectoryName(string assetPath)
        {
            return System.IO.Path.GetDirectoryName(assetPath);
        }
    }
}
