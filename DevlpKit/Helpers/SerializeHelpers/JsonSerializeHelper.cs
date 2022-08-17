// using System;
// using LitJson;
//
// namespace DevlpKit.Helpers.SerializeHelpers
// {
//     public static class JsonSerializeHelper
//     {
// #if SERVER
//         private static readonly MongoDB.Bson.IO.JsonWriterSettings logDefineSettings = new MongoDB.Bson.IO.JsonWriterSettings() { OutputMode = MongoDB.Bson.IO.JsonOutputMode.RelaxedExtendedJson };
// #endif
//         
//         public static string ToJson(object message)
//         {
// #if SERVER
//             return MongoDB.Bson.BsonExtensionMethods.ToJson(message, logDefineSettings);
// #else
//             return JsonMapper.ToJson(message);
// #endif
//         }
//         
//         public static object FromJson(Type type, string json)
//         {
// #if SERVER
//             return MongoDB.Bson.Serialization.BsonSerializer.Deserialize(json, type);
// #else
//             return JsonMapper.ToObject(json, type);
// #endif
//             
//         }
//         
//         public static T FromJson<T>(string json)
//         {
// #if SERVER
//             return MongoDB.Bson.Serialization.BsonSerializer.Deserialize<T>(json);
// #else
//             return JsonMapper.ToObject<T>(json);
// #endif
//         }
//     }
// }