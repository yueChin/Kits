//using IOCompression;

using System.IO;
using System.IO.Compression;

namespace Kits.DevlpKit.Helpers.CompressHelpers
{
    /// <summary>
    /// 压缩功能
    /// </summary>
    public class ZIPCompressHelper
    {
        //public static byte[] encodeObj(object value)
        //{
        //    byte[] numArray;
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        new BinaryFormatter().Serialize((Stream)memoryStream, value);
        //        numArray = new byte[memoryStream.Length];
        //        Buffer.BlockCopy((Array)memoryStream.GetBuffer(), 0, (Array)numArray, 0, (int)memoryStream.Length);
        //    }
        //    return numArray;
        //}

        //public static object decodeObj(byte[] value)
        //{
        //    object obj = null;
        //    using (MemoryStream memoryStream = new MemoryStream(value))
        //    {
        //        obj = new BinaryFormatter().Deserialize((Stream)memoryStream);
        //    }
        //    return obj;
        //}
        //public static void bb(byte[] bytes)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    GZipOutputStream gzip = new GZipOutputStream(ms);
        //    gzip.Write(bytes, 0, bytes.Length);
        //    gzip.Close();
        //    byte[] press = ms.ToArray();
        //    Debug.Log(Convert.ToBase64String(press) + "  " + press.Length);


        //    GZipInputStream gzi = new GZipInputStream(new MemoryStream(press));

        //    MemoryStream re = new MemoryStream();
        //    int count = 0;
        //    byte[] data = new byte[4096];
        //    while ((count = gzi.Read(data, 0, data.Length)) != 0)
        //    {
        //        re.Write(data, 0, count);
        //    }
        //    byte[] depress = re.ToArray();
        //}

        ///// <summary>
        ///// 压缩
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="dest"></param>
        //private static void Compress(Stream source, GZipStream dest)
        //{
        //    source.Position = 0L;
        //    byte[] buffer = new byte[1024];
        //    int count;
        //    while ((count = source.Read(buffer, 0, 1024)) > 0)
        //    {
        //        dest.Write(buffer, 0, count);
        //    }
        //}
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] value)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("压缩前" + value.Length);
#endif
            //byte[] array = null;
            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(value, 0, value.Length);
            compressedzipStream.Close();
#if UNITY_EDITOR
            UnityEngine.Debug.Log("压缩后" + ms.ToArray().Length);
#endif
            return ms.ToArray();

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true))
            //    {
            //        gzip.Write(value, 0, value.Length);
            //        array = ms.ToArray();
            //    }
            //}

            //using (MemoryStream memoryStream1 = new MemoryStream(value, false))
            //using (MemoryStream memoryStream2 = new MemoryStream())
            //using (GZipStream GzipStream = new GZipStream(memoryStream2, CompressionMode.Compress, true))
            //{
            //    ////序列化流一
            //    //new BinaryFormatter().Serialize((Stream)memoryStream1, value);
            //    //流一压缩到流二
            //    //CompressUtil.Compress(memoryStream1, memoryStream2);
            //    memoryStream1.Position = 0L;
            //    byte[] buffer = new byte[1024];
            //    int count;
            //    while ((count = memoryStream1.Read(buffer, 0, 1024)) > 0)
            //    {
            //        GzipStream.Write(buffer, 0, count);
            //    }

            //    array = memoryStream2.ToArray();
            //}

        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        private static void Decompress(Stream source, Stream dest)
        {
            using (GZipStream gzipStream = new GZipStream(source, CompressionMode.Decompress, true))
            {
                int count1 = 1024;
                byte[] buffer = new byte[count1];
                int count2;
                while ((count2 = gzipStream.Read(buffer, 0, count1)) > 0)
                    dest.Write(buffer, 0, count2);
                dest.Position = 0L;
            }
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] value)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("解压缩前" + value.Length);
#endif
            byte[] bytes = null;
            using (MemoryStream memoryStream1 = new MemoryStream(value, false))
            using (MemoryStream memoryStream2 = new MemoryStream())
            {
                //流一解压缩到流二
                Decompress((Stream)memoryStream1, (Stream)memoryStream2);
                //流二序列化到obj
                //obj = new BinaryFormatter().Deserialize((Stream)memoryStream2);

                bytes = new byte[memoryStream2.Length];
                memoryStream2.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始
                memoryStream2.Seek(0, SeekOrigin.Begin);
            }
#if UNITY_EDITOR
            UnityEngine.Debug.Log("解压缩后" + bytes.Length);
#endif
            return bytes;
        }


        public static byte[] Zip(byte[] data)
        {
            MemoryStream memstream = new MemoryStream(data);
            MemoryStream outstream = new MemoryStream();

            using (GZipStream encoder = new GZipStream(outstream, CompressionMode.Compress))
            {
                memstream.WriteTo(encoder);
            }
#if UNITY_EDITOR
            //Debug.Log(outstream.ToArray().Length + "压缩后");
#endif
            return outstream.ToArray();
        }

        public static byte[] Unzip(byte[] data)
        {
            GZipStream decoder = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
            MemoryStream outstream = new MemoryStream();
            CopyStream(decoder, outstream);
            return outstream.ToArray();
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            //long TempPos = input.Position;
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0) 
                    break;
                output.Write(buffer, 0, read);
            }

            //input.Position = TempPos;// or you make Position = 0 to set it at the start 
        }
    }

    //using SevenZip;
    //using System;
    //using System.Collections.Generic;
    //using System.IO;
    //using System.Linq;
    //using System.Text;
    //using System.Threading.Tasks;
    //using UnityEngine;

    //public class SevenZipUtility
    //{
    //    /// <summary>
    //    /// 压缩字符串
    //    /// </summary>
    //    /// <param name="input">源字符串</param>
    //    /// <returns>压缩后字节数组</returns>
    //    public static byte[] Compress(byte[] input)
    //    {
    //        byte[] compressed = null;
    //        SevenZipBase.SetLibraryPath(@"C:\Program Files\7-Zip\7za.dll");
    //        SevenZipCompressor compressor = new SevenZipCompressor();
    //        compressor.CompressionMethod = CompressionMethod.Ppmd;
    //        compressor.CompressionLevel = CompressionLevel.High;
    //        using (MemoryStream msin = new MemoryStream(input))
    //        {
    //            using (MemoryStream msout = new MemoryStream())
    //            {
    //                // 若手动安装，需要指定路径
    //                compressor.CompressStream(msin, msout);

    //                msout.Position = 0;
    //                compressed = new byte[msout.Length];
    //                msout.Read(compressed, 0, compressed.Length);
    //                /*
    //                Console.WriteLine("compressed: ");
    //                foreach (byte b in compressed)
    //                {
    //                    Console.Write(b);
    //                    Console.Write(" ");
    //                }
    //                Console.WriteLine();
    //                 */
    //            }
    //        }
    //        return compressed;
    //    }

    //    ///// <summary>
    //    ///// 获取输入字符串的UTF8编码
    //    ///// </summary>
    //    ///// <param name="input">源字符串</param>
    //    ///// <returns>内存数据流</returns>
    //    //private static MemoryStream GetUTF8MemorySteam(string input)
    //    //{
    //    //    MemoryStream ms = new MemoryStream();
    //    //    byte[] bytes = Encoding.UTF8.GetBytes(input);
    //    //    ms.Write(bytes, 0, bytes.Length);
    //    //    return ms;
    //    //}

    //    /// <summary>
    //    /// 解压字节数组
    //    /// </summary>
    //    /// <param name="input">源字节数组</param>
    //    /// <returns>解压后字符串</returns>
    //    public static byte[] Decompress(byte[] input)
    //    {
    //        /*
    //        Console.WriteLine("input:");
    //        foreach (byte b in input)
    //        {
    //            Console.Write(b);
    //            Console.Write(" ");
    //        }
    //        Console.WriteLine();
    //        */
    //        byte[] uncompressedbuffer = null;
    //        SevenZipBase.SetLibraryPath(@"C:\Program Files\7-Zip\7za.dll");
    //        using (MemoryStream msin = new MemoryStream())
    //        {
    //            msin.Write(input, 0, input.Length);
    //            uncompressedbuffer = new byte[input.Length];
    //            msin.Position = 0;
    //            // 若手动安装，需要指定路径
    //            using (SevenZipExtractor extractor = new SevenZipExtractor(msin))
    //            {
    //                using (MemoryStream msout = new MemoryStream())
    //                {
    //                    extractor.ExtractFile(0, msout);
    //                    msout.Position = 0;
    //                    uncompressedbuffer = new byte[msout.Length];
    //                    msout.Read(uncompressedbuffer, 0, uncompressedbuffer.Length);
    //                }
    //            }
    //        }
    //        return uncompressedbuffer;
    //    }

    //}
}


