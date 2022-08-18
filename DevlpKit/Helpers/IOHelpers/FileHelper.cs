using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kits.DevlpKit.Helpers.IOHelpers
{
    public static partial class FileHelper
    {

        /// <summary>
        /// 删除指定文件目录下的所有文件
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static void DeleteAllFile(this string fullPath,string lastStr = ".meta")
        {
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo directory = new DirectoryInfo(fullPath);
                FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(lastStr))
                    {
                        continue;
                    }
                    File.Delete(files[i].FullName);
                }
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bytes"></param>
        public static void CreateFile(string filePath, byte[] bytes)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            FileInfo file = new FileInfo(filePath);
            Stream stream = file.Create();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            stream.Dispose();
        }

        public static void GetAllFiles(List<string> files, string dir)
        {
            string[] fls = Directory.GetFiles(dir);
            foreach (string fl in fls)
            {
                files.Add(fl);
            }

            string[] subDirs = Directory.GetDirectories(dir);
            foreach (string subDir in subDirs)
            {
                GetAllFiles(files, subDir);
            }
        }

        /// <summary>
        /// UTF8编码格式
        /// </summary>
        private static readonly UTF8Encoding UTF8Encode = new UTF8Encoding(false);

        private const int VERSION_LENGTH = 4;

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileExist(string path)
        {
            return File.Exists(path);
        }

        public static long GetFileSize(string path)
        {
            System.IO.FileInfo info = new System.IO.FileInfo(path);
            return info.Length;
        }

        public static float GetFileSizeKB(string path)
        {
            return GetFileSize(path) / 1024f;
        }

        /// <summary>
        /// 获取目录下所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="suffix"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string[] GetAllChildFiles(string path, string suffix = "", SearchOption option = SearchOption.AllDirectories)
        {
            string strPattner = "*";
            if (suffix.Length > 0 && suffix[0] != '.')
            {
                strPattner += "." + suffix;
            }
            else
            {
                strPattner += suffix;
            }

            string[] files = Directory.GetFiles(path, strPattner, option);

            return files;
        }

        /// <summary>
        /// 遍历目录及其子目录，并将结果填充到files和paths中
        /// </summary>
        public static void Recursive(string path, List<string> files, List<string> paths)
        {
            if (string.IsNullOrEmpty(path) || null == files || null == paths)
            {
                //Debuger.LogError("Recursive 传入的参数错误!");
                return;
            }
            string[] names = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string filename in names)
            {
                string ext = Path.GetExtension(filename);
                if (ext.Equals(".meta")) continue;
                files.Add(filename.Replace('\\', '/'));
            }
            foreach (string dir in dirs)
            {
                if (dir != ".idea")
                {
                    paths.Add(dir.Replace('\\', '/'));
                    Recursive(dir, files, paths);
                }
            }
        }

        public static void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                //Debuger.LogError(e.Message);
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="orginPath"></param>
        /// <param name="destPath"></param>
        /// <param name="isOverwrite"></param>
        public static void CopyFile(string orginPath, string destPath, bool isOverwrite)
        {
            DirectoryHelper.EnsureParentDirExist(destPath);
            File.Copy(orginPath, destPath, isOverwrite);
        }

        /// <summary>
        /// 读取对应路径的文件到字节数组
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// 读取对应路径的文件到string
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadString(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// 将字节数组写到对应路径的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void WriteBytes(string filePath, byte[] content)
        {
            DirectoryHelper.EnsureParentDirExist(filePath);
            File.WriteAllBytes(filePath, content);
        }

        /// <summary>
        /// 将string写到对应路径的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void WriteString(string filePath, string content)
        {
            DirectoryHelper.EnsureParentDirExist(filePath);
            File.WriteAllText(filePath, content.Replace(Environment.NewLine, "\n"), UTF8Encode);
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="fileBytes">文件的字节数组</param>
        public static void Write(string path, byte[] fileBytes)
        {
            DirectoryHelper.CreateDirectory(path);
            FileStream stream = new FileStream(path, FileMode.Create);
            byte[] buf = new byte[4096];
            using (MemoryStream ms = new MemoryStream(fileBytes))
            {
                int count = 0;
                while ((count = ms.Read(buf, 0, buf.Length)) > 0)
                {
                    stream.Write(buf, 0, count);
                }
                stream.Flush();
            }
            stream.Close();
        }



        public static byte[] Read(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                //Debug.LogWarning("<<FileGameUtil , Read>> Cant read file ! Path is " + path);
                return null;
            }

            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] buf = new byte[4096];
            using (MemoryStream ms = new MemoryStream())
            {
                int count = 0;
                while ((count = fs.Read(buf, 0, buf.Length)) > 0)
                {
                    ms.Write(buf, 0, count);
                }
                ms.Flush();
                buf = ms.ToArray();
            }
            fs.Close();
            return buf;
        }
    }
}
