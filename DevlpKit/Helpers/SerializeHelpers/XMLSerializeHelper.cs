// using System;
// using System.IO;
// using System.Reflection;
// using System.Xml.Serialization;
// using DevlpKit.Tools;
//
// namespace DevlpKit.Helpers.SerializeHelpers
// {
//     public class XMLSerializeHelper
//     {
//         /// <summary>
//         /// xml转二进制
//         /// </summary>
//         /// <param name="name"></param>
//         public static bool XmlToBinary(string name, string XmlPath = default(string), string BinaryPath = default(string))
//         {
//             if (string.IsNullOrEmpty(name))
//                 return false;
//
//             try
//             {
//                 Type type = null;
//                 foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
//                 {
//                     Type tempType = asm.GetType(name);
//                     if (tempType != null)
//                     {
//                         type = tempType;
//                         break;
//                     }
//                 }
//                 if (type != null)
//                 {
//                     string xmlPath = $"{XmlPath}{name}.xml" ;
//                     string binaryPath = $"{BinaryPath}{name}.bytes" ;
//                     object obj = XmlDeserialize(xmlPath, type);
//                     BinarySerializeHelper.BinarySerialize(binaryPath,obj);
//                     return true;
//                     //Debug.Log(name + "xml转二进制成功，二进制路径为:" + binaryPath);
//                 }
//             }
//             catch(Exceptions exceptions)
//             {
//                 //Debug.LogError(name + "xml转二进制失败！");
//             }
//
//             return false;
//         }
//
//         /// <summary>
//         /// xml转protobuf
//         /// </summary>
//         /// <param name="name"></param>
//         public static bool XmlToProtoBuf(string name, string XmlPath = default(string), string ProtoBufPath = default(string))
//         {
//             if (string.IsNullOrEmpty(name))
//                 return false;
//
//             try
//             {
//                 Type type = null;
//                 foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
//                 {
//                     Type tempType = asm.GetType(name);
//                     if (tempType != null)
//                     {
//                         type = tempType;
//                         break;
//                     }
//                 }
//                 if (type != null)
//                 {
//                     string xmlPath = $"{XmlPath}{name}.xml" ;
//                     string protobufPath = $"{ProtoBufPath}{name}.bytes" ;
//                     object obj = XmlDeserialize(xmlPath, type);
//                     protobufPath.ProtoSerialize( obj);
//                     return true;
//                     //Debug.Log(name + "xml转Protobuf成功，Protobuf路径为:" + protobufPath);
//                 }
//             }
//             catch
//             {
//                 //Debug.LogError(name + "xml转二进制失败！");
//             }
//
//             return false;
//         }
//
//         /// <summary>
//         /// 实际的类转XML
//         /// </summary>
//         /// <param name="name"></param>
//         public static bool ClassToXml(string name,string XmlPath = default(string))
//         {
//             if (string.IsNullOrEmpty(name))
//                 return false;
//
//             try
//             {
//                 Type type = null;
//                 foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
//                 {
//                     Type tempType = asm.GetType(name);
//                     if (tempType != null)
//                     {
//                         type = tempType;
//                         break;
//                     }
//                 }
//                 if (type != null)
//                 {
//                     object temp = Activator.CreateInstance(type);
//                     string xmlPath = $"{XmlPath}{name}.xml";
//                     XmlSerialize(xmlPath, temp);
//                     //Debug.Log(name + "类转xml成功，xml路径为:" + xmlPath);
//                     return true;
//                 }
//             }
//             catch
//             {
//                 //Debug.LogError(name + "类转xml失败！");
//             }
//
//             return false;
//         }
//
//
//
//         /// <summary>
//         /// 类序列化成xml
//         /// </summary>
//         /// <param name="path"></param>
//         /// <param name="obj"></param>
//         /// <returns></returns>
//         public static bool XmlSerialize(string path, System.Object obj)
//         {
//             try
//             {
//                 using FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
//                 using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
//                 {
//                     XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
//                     namespaces.Add(string.Empty, string.Empty);
//                     XmlSerializer xs = new XmlSerializer(obj.GetType());
//                     xs.Serialize(sw, obj);
//                 }
//
//                 return true;
//             }
//             catch (Exception e)
//             {
//                 //Debug.LogError("此类无法转换成xml " + obj.GetType() + "," + e);
//             }
//
//             return false;
//         }
//
//         /// <summary>
//         /// 编辑器使读取xml
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         /// <param name="path"></param>
//         /// <returns></returns>
//         public static T XmlDeserialize<T>(string path) where T : class
//         {
//             T t = default(T);
//             try
//             {
//                 using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
//                 {
//                     XmlSerializer xs = new XmlSerializer(typeof(T));
//                     t = (T) xs.Deserialize(fs);
//                 }
//             }
//             catch (Exception e)
//             {
//                 //Debug.LogError("此xml无法转成二进制: " + path + "," + e);
//             }
//
//             return t;
//         }
//
//         /// <summary>
//         /// Xml的反序列化
//         /// </summary>
//         /// <param name="path"></param>
//         /// <param name="type"></param>
//         /// <returns></returns>
//         public static System.Object XmlDeserialize(string path, Type type)
//         {
//             System.Object obj = null;
//             try
//             {
//                 using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
//                 {
//                     XmlSerializer xs = new XmlSerializer(type);
//                     obj = xs.Deserialize(fs);
//                 }
//             }
//             catch (Exception e)
//             {
//                 //Debug.LogError("此xml无法转成二进制: " + path + "," + e);
//             }
//
//             return obj;
//         }
//
//
//         /// <summary>
//         /// 运行时使读取xml
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         /// <param name="path"></param>
//         /// <returns></returns>
//         public static T XmlDeserializeRun<T>(string path) where T : class
//         {
//             T t = default(T);
//             //TextAsset textAsset = ResourceManager.Instance.LoadResource<TextAsset>(path);
//
//             //if (textAsset == null)
//             //{
//             //    UnityEngine.Debug.LogError("cant load TextAsset: " + path);
//             //    return null;
//             //}
//
//             //try
//             //{
//             //    using (MemoryStream stream = new MemoryStream(textAsset.bytes))
//             //    {
//             //        XmlSerializer xs = new XmlSerializer(typeof(T));
//             //        t = (T)xs.Deserialize(stream);
//             //    }
//             //    ResourceManager.Instance.ReleaseResouce(path, true);
//             //}
//             //catch (Exception e)
//             //{
//             //    Debug.LogError("load TextAsset exception: " + path + "," + e);
//             //}
//             return t;
//         }
//     }
// }