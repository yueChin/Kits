using UnityEngine;

namespace Kits.ClientKit.Handlers.Graphic
{
    /// <summary>
    /// Unity Rendering 工具箱
    /// </summary>
    public static class RenderingHandler
    {
        private static Texture2D s_WhiteTexture;
        private static Texture2D s_BlackTexture;
        private static Cubemap s_WhiteCubemap;
        private static Cubemap s_BlackCubemap;
        private static Mesh s_QuadMesh;


        public static Texture2D whiteTexture
        {
            get
            {
                if (s_WhiteTexture == null)
                {
                    s_WhiteTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    s_WhiteTexture.SetPixel(0, 0, Color.white);
                    s_WhiteTexture.Apply();
                }

                return s_WhiteTexture;
            }
        }


        public static Texture2D blackTexture
        {
            get
            {
                if (s_BlackTexture == null)
                {
                    s_BlackTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    s_BlackTexture.SetPixel(0, 0, Color.black);
                    s_BlackTexture.Apply();
                }

                return s_BlackTexture;
            }
        }


        public static Cubemap whiteCubemap
        {
            get
            {
                if (s_WhiteCubemap == null)
                {
                    s_WhiteCubemap = new Cubemap(1, TextureFormat.ARGB32, false);
                    s_WhiteCubemap.SetPixel(CubemapFace.NegativeX, 0, 0, Color.white);
                    s_WhiteCubemap.SetPixel(CubemapFace.NegativeY, 0, 0, Color.white);
                    s_WhiteCubemap.SetPixel(CubemapFace.NegativeZ, 0, 0, Color.white);
                    s_WhiteCubemap.SetPixel(CubemapFace.PositiveX, 0, 0, Color.white);
                    s_WhiteCubemap.SetPixel(CubemapFace.PositiveY, 0, 0, Color.white);
                    s_WhiteCubemap.SetPixel(CubemapFace.PositiveZ, 0, 0, Color.white);
                    s_WhiteCubemap.Apply();
                }
                return s_WhiteCubemap;
            }
        }


        public static Cubemap blackCubemap
        {
            get
            {
                if (s_BlackCubemap == null)
                {
                    s_BlackCubemap = new Cubemap(1, TextureFormat.ARGB32, false);
                    s_BlackCubemap.SetPixel(CubemapFace.NegativeX, 0, 0, Color.black);
                    s_BlackCubemap.SetPixel(CubemapFace.NegativeY, 0, 0, Color.black);
                    s_BlackCubemap.SetPixel(CubemapFace.NegativeZ, 0, 0, Color.black);
                    s_BlackCubemap.SetPixel(CubemapFace.PositiveX, 0, 0, Color.black);
                    s_BlackCubemap.SetPixel(CubemapFace.PositiveY, 0, 0, Color.black);
                    s_BlackCubemap.SetPixel(CubemapFace.PositiveZ, 0, 0, Color.black);
                    s_BlackCubemap.Apply();
                }
                return s_BlackCubemap;
            }
        }


        public static Mesh quadMesh
        {
            get
            {
                if (!s_QuadMesh)
                {
                    Vector3[] vertices = new[]
                    {
                        new Vector3(-0.5f, -0.5f, 0f),
                        new Vector3(0.5f,  0.5f, 0f),
                        new Vector3(0.5f, -0.5f, 0f),
                        new Vector3(-0.5f,  0.5f, 0f)
                    };

                    Vector2[] uvs = new[]
                    {
                        new Vector2(0f, 0f),
                        new Vector2(1f, 1f),
                        new Vector2(1f, 0f),
                        new Vector2(0f, 1f)
                    };

                    int[] indices = new[] { 0, 1, 2, 1, 0, 3 };

                    s_QuadMesh = new Mesh
                    {
                        vertices = vertices,
                        uv = uvs,
                        triangles = indices
                    };

                    s_QuadMesh.RecalculateNormals();
                    s_QuadMesh.RecalculateBounds();
                }

                return s_QuadMesh;
            }
        }

    } // struct RenderingKit

} // namespace UnityExtensions