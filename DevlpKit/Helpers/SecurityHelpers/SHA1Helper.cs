using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Kits.DevlpKit.Helpers.SecurityHelpers
{
    public static class SHA1Helper
    {

        /// <summary>
        /// 获取字符串的Hash值
        /// </summary>
        public static string StringSHA1(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            return BytesSHA1(buffer);
        }

        /// <summary>
        /// 获取文件的Hash值
        /// </summary>
        public static string FileSHA1(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return StreamSHA1(fs);
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// 获取数据流的Hash值
        /// </summary>
        public static string StreamSHA1(Stream stream)
        {
            // 说明：创建的是SHA1类的实例，生成的是160位的散列码
            HashAlgorithm hash = HashAlgorithm.Create();
            byte[] hashBytes = hash.ComputeHash(stream);
            return HashHelper.ToString(hashBytes);
        }

        /// <summary>
        /// 获取字节数组的Hash值
        /// </summary>
        public static string BytesSHA1(byte[] buffer)
        {
            // 说明：创建的是SHA1类的实例，生成的是160位的散列码
            HashAlgorithm hash = HashAlgorithm.Create();
            byte[] hashBytes = hash.ComputeHash(buffer);
            return HashHelper.ToString(hashBytes);
        }
    }

}