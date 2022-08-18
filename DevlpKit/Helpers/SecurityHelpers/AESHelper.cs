using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Kits.DevlpKit.Helpers.SecurityHelpers
{
    public static class AesHelper
    {
        private static string s_AesHead = "AESEncrypt";

        /// <summary>
        /// 文件加密，传入文件路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encrptyKey"></param>
        public static void AesFileEncrypt(this string filePath, string encrptyKey)
        {
            if (!File.Exists(filePath))
                return;

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    if (fs != null)
                    {
                        //读取字节头，判断是否已经加密过了
                        byte[] headBuff = new byte[10];
                        fs.Read(headBuff, 0, headBuff.Length);
                        string headTag = Encoding.UTF8.GetString(headBuff);
                        if (headTag == s_AesHead)
                        {
#if UNITY_EDITOR
                            Debug.Log(filePath + "已经加密过了！");
#endif
                            return;
                        }

                        //加密并且写入字节头
                        fs.Seek(0, SeekOrigin.Begin);
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, Convert.ToInt32(fs.Length));
                        fs.Seek(0, SeekOrigin.Begin);
                        fs.SetLength(0);
                        byte[] headBuffer = Encoding.UTF8.GetBytes(s_AesHead);
                        fs.Write(headBuffer, 0, headBuffer.Length);
                        byte[] encBuffer = AesEncrypt(buffer, encrptyKey);
                        fs.Write(encBuffer, 0, encBuffer.Length);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 文件解密，传入文件路径（会改动加密文件，不适合运行时）
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encrptyKey"></param>
        public static void AesFileDecrypt(this string filePath, string encrptyKey)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    if (fs != null)
                    {
                        byte[] headBuff = new byte[10];
                        fs.Read(headBuff, 0, headBuff.Length);
                        string headTag = Encoding.UTF8.GetString(headBuff);
                        if (headTag == s_AesHead)
                        {
                            byte[] buffer = new byte[fs.Length - headBuff.Length];
                            fs.Read(buffer, 0, Convert.ToInt32(fs.Length - headBuff.Length));
                            fs.Seek(0, SeekOrigin.Begin);
                            fs.SetLength(0);
                            byte[] decBuffer = AesDecrypt(buffer, encrptyKey);
                            fs.Write(decBuffer, 0, decBuffer.Length);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 文件界面，传入文件路径，返回字节
        /// </summary>
        /// <returns></returns>
        public static byte[] AesFileByteDecrypt(this string filePath, string encrptyKey)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            byte[] decBuffer = null;
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (fs != null)
                    {
                        byte[] headBuff = new byte[10];
                        fs.Read(headBuff, 0, headBuff.Length);
                        string headTag = Encoding.UTF8.GetString(headBuff);
                        if (headTag == s_AesHead)
                        {
                            byte[] buffer = new byte[fs.Length - headBuff.Length];
                            fs.Read(buffer, 0, Convert.ToInt32(fs.Length - headBuff.Length));
                            decBuffer = AesDecrypt(buffer, encrptyKey);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return decBuffer;
        }

        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="encryptString">待加密密文</param>
        /// <param name="encryptKey">加密密钥</param>
        public static string AesEncrypt(this string encryptString, string encryptKey)
        {
            return Convert.ToBase64String(AesEncrypt(Encoding.Default.GetBytes(encryptString), encryptKey));
        }

        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="EncryptString">待加密密文</param>
        /// <param name="encryptKey">加密密钥</param>
        public static byte[] AesEncrypt(this byte[] encryptByte, string encryptKey)
        {
            if (encryptByte.Length == 0)
            {
                throw (new Exception("明文不得为空"));
            }

            if (string.IsNullOrEmpty(encryptKey))
            {
                throw (new Exception("密钥不得为空"));
            }

            byte[] strEncrypt;
            byte[] btIv = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
            byte[] salt = Convert.FromBase64String("gsf4jvkyhye5/d7k8OrLgM==");
            Rijndael aesProvider = Rijndael.Create();
            try
            {
                MemoryStream stream = new MemoryStream();
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(encryptKey, salt);
                ICryptoTransform transform = aesProvider.CreateEncryptor(pdb.GetBytes(32), btIv);
                CryptoStream csStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                csStream.Write(encryptByte, 0, encryptByte.Length);
                csStream.FlushFinalBlock();
                strEncrypt = stream.ToArray();
                stream.Close();
                stream.Dispose();
                csStream.Close();
                csStream.Dispose();
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (CryptographicException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                aesProvider.Clear();
            }

            return strEncrypt;
        }


        /// <summary>
        /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="decryptString">待解密密文</param>
        /// <param name="decryptKey">解密密钥</param>
        public static string AesDecrypt(this string decryptString, string decryptKey)
        {
            return Convert.ToBase64String(AesDecrypt(Encoding.Default.GetBytes(decryptString), decryptKey));
        }

        /// <summary>
        /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="decryptKey">解密密钥</param>
        public static byte[] AesDecrypt(this byte[] decryptByte, string decryptKey)
        {
            if (decryptByte.Length == 0)
            {
                throw (new Exception("密文不得为空"));
            }

            if (string.IsNullOrEmpty(decryptKey))
            {
                throw (new Exception("密钥不得为空"));
            }

            byte[] strDecrypt;
            byte[] btIv = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
            byte[] salt = Convert.FromBase64String("gsf4jvkyhye5/d7k8OrLgM==");
            Rijndael aesProvider = Rijndael.Create();
            try
            {
                MemoryStream stream = new MemoryStream();
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(decryptKey, salt);
                ICryptoTransform transform = aesProvider.CreateDecryptor(pdb.GetBytes(32), btIv);
                CryptoStream csStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                csStream.Write(decryptByte, 0, decryptByte.Length);
                csStream.FlushFinalBlock();
                strDecrypt = stream.ToArray();
                stream.Close();
                stream.Dispose();
                csStream.Close();
                csStream.Dispose();
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (CryptographicException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                aesProvider.Clear();
            }

            return strDecrypt;
        }


    }

}