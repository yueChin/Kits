// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Profiling;
// using UnityEngine.UI;
//
// namespace ClientKit.Handlers.Graphic
// {
//     internal static class MeshHandler
//     {
//         public static List<bool> activeMeshIndices { get; private set; }
//         private static readonly List<CombineInstanceEx> s_CachedInstance;
//         private static int count;
//
//         public static void Init()
//         {
//             activeMeshIndices = new List<bool>();
//         }
//
//         static MeshHandler()
//         {
//             s_CachedInstance = new List<CombineInstanceEx>(8);
//             for (int i = 0; i < 8; i++)
//             {
//                 s_CachedInstance.Add(new CombineInstanceEx());
//             }
//         }
//
//         private static CombineInstanceEx Get(int index, long hash)
//         {
//             if (0 < count && s_CachedInstance[count - 1].hash == hash)
//                 return s_CachedInstance[count - 1];
//
//             if (s_CachedInstance.Count <= count)
//             {
//                 CombineInstanceEx newInst = new CombineInstanceEx();
//                 s_CachedInstance.Add(newInst);
//             }
//
//             CombineInstanceEx inst = s_CachedInstance[count];
//             inst.hash = hash;
//             if (inst.index != -1) return inst;
//
//             inst.index = index;
//             count++;
//             return inst;
//         }
//
//         public static Mesh GetTemporaryMesh()
//         {
//             return MeshPool.Rent();
//         }
//
//         public static void Push(int index, long hash, Mesh mesh, Matrix4x4 transform)
//         {
//             if (mesh.vertexCount <= 0)
//             {
//                 DiscardTemporaryMesh(mesh);
//                 return;
//             }
//
//             Profiler.BeginSample("[UIParticle] MeshHelper > Get CombineInstanceEx");
//             CombineInstanceEx inst = Get(index, hash);
//             Profiler.EndSample();
//
//             Profiler.BeginSample("[UIParticle] MeshHelper > Push To Mesh Helper");
//             inst.Push(mesh, transform);
//             Profiler.EndSample();
//
//             activeMeshIndices[inst.index] = true;
//         }
//
//         public static void Clear()
//         {
//             count = 0;
//             activeMeshIndices.Clear();
//             foreach (CombineInstanceEx inst in s_CachedInstance)
//             {
//                 inst.Clear();
//             }
//         }
//
//         public static void CombineMesh(Mesh result)
//         {
//             if (count == 0) return;
//
//             for (int i = 0; i < count; i++)
//             {
//                 Profiler.BeginSample("[UIParticle] MeshHelper > Combine Mesh Internal");
//                 s_CachedInstance[i].Combine();
//                 Profiler.EndSample();
//             }
//
//             Profiler.BeginSample("[UIParticle] MeshHelper > Combine Mesh");
//             CombineInstance[] cis = CombineInstanceArrayPool.Get(s_CachedInstance, count);
//             result.CombineMeshes(cis, false, true);
//             cis.Clear();
//             Profiler.EndSample();
//
//             result.RecalculateBounds();
//         }
//
//         public static void DiscardTemporaryMesh(Mesh mesh)
//         {
//             MeshPool.Return(mesh);
//         }
//
//         public static void Clear(this CombineInstance[] self)
//         {
//             for (int i = 0; i < self.Length; i++)
//             {
//                 MeshPool.Return(self[i].mesh);
//                 self[i].mesh = null;
//             }
//         }
//         
//         /// <summary>
//         /// Create a mesh for the heightmap
//         /// </summary>
//         /// <param name="heightmap"></param>
//         /// <param name="targetSize"></param>
//         /// <param name="resolution"></param>
//         /// <returns></returns>
//         public static Mesh CreateMesh(float[,] heightmap, Vector3 targetSize)
//         {
//             //Need to sample these to not blow unity mesh sizes
//             int width = heightmap.GetLength(0);
//             int height = heightmap.GetLength(1);
//             int targetRes = 1;
//
//             Vector3 targetOffset = Vector3.zero - (targetSize / 2f);
//             Vector2 uvScale = new Vector2(1.0f / (width - 1), 1.0f / (height - 1));
//
//             //Choose best possible target res
//             for (targetRes = 1; targetRes < 100; targetRes++)
//             {
//                 if (((width / targetRes) * (height / targetRes)) < 65000)
//                 {
//                     break;
//                 }
//             }
//
//             targetSize = new Vector3(targetSize.x / (width - 1) * targetRes, targetSize.y, targetSize.z / (height - 1) * targetRes);
//             width = (width - 1) / targetRes + 1;
//             height = (height - 1) / targetRes + 1;
//
//             Vector3[] vertices = new Vector3[width * height];
//             Vector2[] uvs = new Vector2[width * height];
//             Vector3[] normals = new Vector3[width * height];
//             Color[] colors = new Color[width * height];
//             int[] triangles = new int[(width - 1) * (height - 1) * 6];
//
//             // Build vertices and UVs
//             for (int y = 0; y < height; y++)
//             {
//                 for (int x = 0; x < width; x++)
//                 {
//                     colors[y * width + x] = Color.black;
//                     normals[y * width + x] = Vector3.up;
//                     //vertices[y * w + x] = Vector3.Scale(targetSize, new Vector3(-y, heightmap[x * tRes, y * tRes], x)) + targetOffset;
//                     vertices[y * width + x] = Vector3.Scale(targetSize, new Vector3(x, heightmap[x * targetRes, y * targetRes], y)) + targetOffset;
//                     uvs[y * width + x] = Vector2.Scale(new Vector2(x * targetRes, y * targetRes), uvScale);
//                 }
//             }
//
//             // Build triangle indices: 3 indices into vertex array for each triangle
//             int index = 0;
//             for (int y = 0; y < height - 1; y++)
//             {
//                 for (int x = 0; x < width - 1; x++)
//                 {
//                     triangles[index++] = (y * width) + x;
//                     triangles[index++] = ((y + 1) * width) + x;
//                     triangles[index++] = (y * width) + x + 1;
//                     triangles[index++] = ((y + 1) * width) + x;
//                     triangles[index++] = ((y + 1) * width) + x + 1;
//                     triangles[index++] = (y * width) + x + 1;
//                 }
//             }
//
//             Mesh mesh = new Mesh();
//
//             mesh.vertices = vertices;
//             mesh.colors = colors;
//             mesh.normals = normals;
//             mesh.uv = uvs;
//             mesh.triangles = triangles;
//             mesh.RecalculateBounds();
//             mesh.RecalculateNormals();
//             return mesh;
//         }
//     }
// }
