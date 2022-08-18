using System;
using System.Security.Cryptography;

namespace Kits.DevlpKit.Helpers.SecurityHelpers
{
    public static class HashHelper
    {

        //使用指定算法Hash
        public static byte[] Hash(this byte[] data, string hashName)
        {
            HashAlgorithm algorithm;
            if (string.IsNullOrEmpty(hashName)) algorithm = HashAlgorithm.Create();
            else algorithm = HashAlgorithm.Create(hashName);
            return algorithm.ComputeHash(data);
        }
        //使用默认算法Hash
        public static byte[] Hash(this byte[] data)
        {
            return Hash(data, null);
        }
        
        public static string ToString(byte[] hashBytes)
        {
            string result = BitConverter.ToString(hashBytes);
            result = result.Replace("-", "");
            return result.ToLower();
        }
    }

}