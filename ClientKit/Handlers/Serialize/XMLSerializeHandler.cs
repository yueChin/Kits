// using System;
// using ClientKit.Utilities;
// using DevlpKit.Helpers.SerializeHelpers;
// using UnityEngine;
//
// namespace ClientKit.Handlers.DateTransform
// {
//     public class XMLSerializeHandler
//     {
//         public static System.Object XmlDeserialize(string path, Type type)
//         {
//             object obj = XMLSerializeHelper.XmlDeserialize(path, type);
//             if (obj == null)
//             {
//                 Debuger.LogError("此xml无法转成二进制: " + path );
//             }
//
//             return obj;
//         }
//
//         public static T XmlDeserialize<T>(string path) where T : class
//         {
//             T t = XMLSerializeHelper.XmlDeserialize<T>(path);
//             if (t == null)
//             {
//                 Debuger.LogError("此xml无法转成二进制: " + path);
//             }
//
//             return t;
//         }
//
//         /// <summary>
//         /// 类序列化成xml
//         /// </summary>
//         /// <param name="path"></param>
//         /// <param name="obj"></param>
//         /// <returns></returns>
//         public static bool XmlSerialize(string path, System.Object obj)
//         {
//             bool isSuccess =  XMLSerializeHelper.XmlSerialize(path, obj);
//             if (!isSuccess)
//             {
//                 Debuger.LogError("此类无法转换成xml " + obj.GetType() );
//             }
//             return isSuccess;
//         }
//
//         private static bool ClassToXml(string name)
//         {
//             bool isSuccess = XMLSerializeHelper.ClassToXml(name,ResPath.Instance.XMLPath);
//             if (isSuccess)
//             {
//                 Debug.Log($"{name}类转xml成功，xml路径为:{ResPath.Instance.XMLPath}{name}.xml");
//             }
//             else
//             {
//                 Debug.LogError(name + "类转xml失败！");
//             }
//
//             return isSuccess;
//         }
//
//         /// <summary>
//         /// xml转protobuf
//         /// </summary>
//         /// <param name="name"></param>
//         public static bool XmlToProtoBuf(string name)
//         {
//             bool isSuccess = XMLSerializeHelper.XmlToProtoBuf(name, ResPath.Instance.XMLPath, ResPath.Instance.ProtoBufPath);
//             if (isSuccess)
//             {
//                 Debuger.Log($"{name}xml转ProtoBuf成功，ProtoBuf路径为:{ResPath.Instance.ProtoBufPath}{name}.bytes");
//
//             }
//             else
//             {
//                 Debuger.LogError(name + "xml转二进制失败！");
//
//             }
//             return isSuccess;
//         }
//
//         /// <summary>
//         /// xml转二进制
//         /// </summary>
//         /// <param name="name"></param>
//         public static bool XmlToBinary(string name)
//         {
//             bool isSuccess = XMLSerializeHelper.XmlToBinary(name, ResPath.Instance.XMLPath, ResPath.Instance.BinaryPath);
//             if (isSuccess)
//             {
//                 Debuger.Log($"{name}xml转二进制成功，二进制路径为:{ResPath.Instance.BinaryPath}{name}.bytes");
//
//             }
//             else
//             {
//                 Debuger.LogError(name + "xml转二进制失败！");
//
//             }
//             return isSuccess;
//         }
//     }
// }
