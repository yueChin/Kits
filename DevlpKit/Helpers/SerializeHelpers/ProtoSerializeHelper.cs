// using System;
// using System.IO;
// using ProtoBuf;
//
// namespace DevlpKit.Helpers.SerializeHelpers
// {
//     public static class ProtoSerializeHelper
//     {
//         public static bool ProtoSerialize(this string path, System.Object obj)
//         {
//             try
//             {
//                 using (Stream file = File.Create(path))
//                 {
//                     Serializer.Serialize(file, obj);
//                     return true;
//                 }
//             }
//             catch (Exception e)
//             {
//                 //Debug.LogError(e);
//                 return false;
//             }
//         }
//
//         public static T ProtoDeSerialize<T>(this string path) where T : class
//         {
//             try
//             {
//                 using (Stream file = File.OpenRead(path))
//                 {
//                     return Serializer.Deserialize<T>(file);
//                 }
//             }
//             catch (Exception e)
//             {
//                 //Debug.LogError(e);
//                 return null;
//             }
//         }
//
//         public static byte[] ProtoSerialize(this System.Object obj)
//         {
//             try
//             {
//                 using (MemoryStream ms = new MemoryStream())
//                 {
//                     Serializer.Serialize(ms, obj);
//                     byte[] result = new byte[ms.Length];
//                     ms.Position = 0;
//                     ms.Read(result, 0, result.Length);
//                     return result;
//                 }
//             }
//             catch (Exception e)
//             {
//                 //Debug.LogError("????? ? " + e.ToString());
//                 return null;
//             }
//         }
//
//
//         public static T ProtoDeSerialize<T>(this byte[] msg) where T : class
//         {
//             try
//             {
//                 using MemoryStream ms = new MemoryStream();
//                 ms.Write(msg, 0, msg.Length);
//                 ms.Position = 0;
//                 return Serializer.Deserialize<T>(ms);
//             }
//             catch (Exception e)
//             {
//                 //Debug.LogError(e);
//                 return null;
//             }
//         }
//     }
// }