using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Kits.DevlpKit.Helpers.ValueTypeHelpers;
using UnityEngine;

namespace Kits.DevlpKit.Helpers.SecurityHelpers
{
    public static class MD5Helper
    {
        /// <summary>
        /// 获取字符串的MD5
        /// </summary>
        public static string FormatMD5(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            return FormatMD5(buffer);
        }

        /// <summary>
        /// 获取数据流的MD5
        /// </summary>
        public static string FormatMD5(Stream stream)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] hashBytes = provider.ComputeHash(stream);
            return HashHelper.ToString(hashBytes);
        }

        /// <summary>
        /// 获取字节数组的MD5
        /// </summary>
        public static string FormatMD5(byte[] buffer)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] hashBytes = provider.ComputeHash(buffer);
            return HashHelper.ToString(hashBytes);
        }

        //储存Md5码，filePath为文件路径，md5SavePath为储存md5码路径
        public static void SaveMd5(this string filePath, string md5SavePath)
        {
            string md5 = BuildFileMd5(filePath);
            string name = filePath + "_md5.dat";
            if (File.Exists(name))
            {
                File.Delete(name);
            }

            StreamWriter sw = new StreamWriter(name, false, Encoding.UTF8);
            if (sw != null)
            {
                sw.Write(md5);
                sw.Flush();
                sw.Close();
            }
        }

        //储存Md5码，filePath为文件路径
        public static void SaveMd5(this string filePath)
        {
            string md5 = BuildFileMd5(filePath);
            string name = filePath + "_md5.dat";
            if (File.Exists(name))
            {
                File.Delete(name);
            }

            StreamWriter sw = new StreamWriter(name, false, Encoding.UTF8);
            if (sw != null)
            {
                sw.Write(md5);
                sw.Flush();
                sw.Close();
            }
        }

        //获取之前储存的Md5码
        public static string GetMd5(this string path)
        {
            string name = path + "_md5.dat";
            try
            {
                StreamReader sr = new StreamReader(name, Encoding.UTF8);
                string content = sr.ReadToEnd();
                sr.Close();
                return content;
            }
            catch
            {
                return "";
            }
        }

        public static string BuildFileMd5(this string filePath)
        {
            string fileMD5 = null;
            try
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    MD5 md5 = MD5.Create();
                    byte[] fileMD5Bytes = md5.ComputeHash(fileStream); //计算指定Stream 对象的哈希值                                     
                    fileMD5 = FormatMD5(fileMD5Bytes);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }

            return fileMD5;
        }

        public static string FileMD5(string filePath)
        {
            byte[] retVal;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                retVal = md5.ComputeHash(file);
            }
            return retVal.ToHex("x2");
        }
        
        /// <summary>
        /// 对指定路径的文件进行生成MD5码
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FileMD5Hash(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            return FileMD5Hash(File.ReadAllBytes(filePath));
        }

        /// <summary>
        /// 二进制数据进行生成MD5码
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string FileMD5Hash(byte[] buffer)
        {
            if (buffer == null)
                return null;

            MD5 md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", "").ToLower();
        }

        #region MD5查找相同文件

        // 文件检索目录
        private const string c_DirPath = "";
        // 重复文件的存放目录
        private const string c_SameFilesPath = "";

        private static void FindSameFiles(string dirPath = default(String))
        {
            if(string.IsNullOrEmpty(dirPath))
                dirPath = c_DirPath; 
            DirectoryInfo rootDirInfo = new DirectoryInfo(dirPath);
            List<FileInfo> fileInfoList = new List<FileInfo>();
            // 找到所有文件
            FileInfo[] fileInfos = rootDirInfo.GetFiles("*.*");

            int total = fileInfos.Length;
            int index = 0;

            for (int i = 0; i < total; ++i)
            {
                FileInfo fileInfo = fileInfos[i];
                string time = fileInfo.CreationTime.ToString("yyyy-MM-dd-HHmmss");
                // 存储文件，以备后用
                fileInfoList.Add(fileInfo);
                // 根据文件创建日期重命名文件
                // 这是一个递归函数，如果日期名称相同，则增加名称中的计数器
                TryNaming(fileInfo, time, 0);
                // 显示重命名进度
                UnityEditor.EditorUtility.DisplayCancelableProgressBar("Renaming Files", $"Progress {++index} / {total}", index / (float)total);
                //DisplayProgressBar
                //(
                //    "Renaming Files",
                //    $"Progress {++index} / {total}",
                //    index / (float)total
                //);
            }

            // 存储每个文件的md5字符串
            string[] md5Strs = new string[total];
            // 存储相同的文件
            List<FileInfo> sameFileInfos = new List<FileInfo>();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            for (int j = 0; j < total; ++j)
            {
                if (md5Strs[j] == null)
                {
                    // 对应位置的文件没有md5则生成
                    md5Strs[j] = GetMd5Str(md5, fileInfoList[j]);
                }

                // 记录当前相同文件个数，以备后续判断，是否增加了相同文件
                int curSameCount = sameFileInfos.Count;

                // 对比j之后的每一个文件，与之前的文件的对比，在之前文件的循环中对比过
                for (int k = j + 1; k < total; ++k)
                {
                    // 只检测大小相同文件的md5
                    if (fileInfoList[k].Length == fileInfoList[j].Length)
                    {
                        if (md5Strs[k] == null)
                        {
                            // 对应位置的文件没有md5则生成
                            md5Strs[k] = GetMd5Str(md5, fileInfoList[k]);
                        }

                        if (md5Strs[k] == md5Strs[j])
                        {
                            // 找到相同文件
                            sameFileInfos.Add(fileInfoList[k]);
                        }
                    }
                }

                if (sameFileInfos.Count > curSameCount)
                {
                    // 如果有相同文件，则把对比文件放入 
                    sameFileInfos.Add(fileInfoList[j]);
                }

                // 显示对比进度
                UnityEditor.EditorUtility.DisplayCancelableProgressBar("Comparing Files", $"Progress {j} / {total}", j / (float)total);
                //DisplayProgressBar
                //(
                //    $"Comparing Files",
                //    $"Progress {j} / {total}",
                //    j / (float)total
                //);
            }

            // 显示相同文件数量
            //Debug.LogError( $"same files count = {sameFileInfos.Count}");

            // 移动相同文件到指定目录
            foreach (FileInfo fileInfo in sameFileInfos)
            {
                fileInfo.MoveTo(c_SameFilesPath + "/" + fileInfo.Name);
            }
        }


        private static string GetMd5Str(MD5CryptoServiceProvider md5, FileInfo fileInfo)
        {
            FileStream stream = fileInfo.OpenRead();
            byte[] bytes = md5.ComputeHash(stream);
            stream.Close();

            return System.BitConverter.ToString(bytes).Replace("-", "");
        }


        private static void TryNaming(FileInfo fileInfo, string time, int sameCount)
        {
            string timeName = time + "-" + sameCount + fileInfo.Extension;

            if (fileInfo.Name == timeName)
            {
                return;
            }

            try
            {
                fileInfo.MoveTo(fileInfo.DirectoryName + "/" + timeName);
            }
            catch (IOException e)
            {
                // have same names
                if (e.Message == "Error 183")
                {
                    TryNaming(fileInfo, time, ++sameCount);
                }
                else
                {
                    throw e;
                }
            }
        }

        #endregion 
    }
}