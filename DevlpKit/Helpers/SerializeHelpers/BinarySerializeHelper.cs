using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kits.DevlpKit.Helpers.SerializeHelpers
{
    public class BinarySerializeHelper
    {
        /// <summary>
        /// 类转换成二进制
        /// </summary>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool BinarySerialize(string path, System.Object obj)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, obj);
                }

                return true;
            }
            catch (Exception e)
            {
                //Debug.LogError("此类无法转换成二进制 " + obj.GetType() + "," + e);
            }

            return false;
        }

        /// <summary>
        /// 读取二进制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T BinaryDeserialize<T>(string path) where T : class
        {
            T t = default(T);
            //TextAsset textAsset = ResourceManager.Instance.LoadResource<TextAsset>(path);

            //if (textAsset == null)
            //{
            //    UnityEngine.Debug.LogError("cant load TextAsset: " + path);
            //    return null;
            //}

            //try
            //{
            //    using (MemoryStream stream = new MemoryStream(textAsset.bytes))
            //    {
            //        BinaryFormatter bf = new BinaryFormatter();
            //        t = (T)bf.Deserialize(stream);
            //    }
            //    ResourceManager.Instance.ReleaseResouce(path, true);
            //}
            //catch (Exception e)
            //{
            //    Debug.LogError("load TextAsset exception: " + path + "," + e);
            //}
            return t;
        }
    }
}