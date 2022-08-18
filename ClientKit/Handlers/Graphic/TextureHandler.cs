using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Graphic
{
    public static class TextureHandler
    {
        public static Texture2D CreateCheckerTex(Color c0, Color c1)
        {
            Texture2D tex = new Texture2D(16, 16);
            tex.name = "[Generated] Checker Texture";
            tex.hideFlags = HideFlags.DontSave;

            for (int y = 0; y < 8; ++y)
            {
                for (int x = 0; x < 8; ++x)
                {
                    tex.SetPixel(x, y, c1);
                }
            }

            for (int y = 8; y < 16; ++y)
            {
                for (int x = 0; x < 8; ++x)
                {
                    tex.SetPixel(x, y, c0);
                }
            }

            for (int y = 0; y < 8; ++y)
            {
                for (int x = 8; x < 16; ++x)
                {
                    tex.SetPixel(x, y, c0);
                }
            }

            for (int y = 8; y < 16; ++y)
            {
                for (int x = 8; x < 16; ++x)
                {
                    tex.SetPixel(x, y, c1);
                }
            }

            tex.Apply();
            tex.filterMode = FilterMode.Point;
            return tex;
        }

        public static bool SaveTextureToPNG(Texture inputTex, string saveFileName)
        {
            RenderTexture temp =
                RenderTexture.GetTemporary(inputTex.width, inputTex.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(inputTex, temp);
            bool ret = SaveRenderTextureToPNG(temp, saveFileName);
            RenderTexture.ReleaseTemporary(temp);
            return ret;

        }

        //将RenderTexture保存成一张png图片  
        public static bool SaveRenderTextureToPNG(RenderTexture rt, string saveFileName)
        {
            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
            png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            byte[] bytes = png.EncodeToPNG();
            string directory = Path.GetDirectoryName(saveFileName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            FileStream file = File.Open(saveFileName, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(file);
            writer.Write(bytes);
            file.Close();
            Texture2D.DestroyImmediate(png);
            png = null;
            RenderTexture.active = prev;
            return true;

        }

        public static Texture2D LoadTextureInLocal(string filePath)
        {
            //创建文件读取流
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            //创建文件长度缓冲区
            byte[] bytes = new byte[fileStream.Length];
            //读取文件
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            //释放文件读取流
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;

            //创建Texture
            int width = 300;
            int height = 372;
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(bytes);
            return texture;
        }

        /// <summary>
        /// Load an image from disk
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Texture2D LoadImage(string filePath)
        {
            Texture2D texture = null;
            if (File.Exists(filePath))
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                texture = new Texture2D(2, 2);
                // Loading will auto resize the texture dimensions
                texture.LoadImage(fileData);
            }

            return texture;
        }

        /// <summary>
        /// Save an image in PNG format
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="filePath"></param>
        public static void SaveAsPNG(Texture2D texture, string filePath)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// Save an image in JPG format
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="filePath"></param>
        /// <param name="quality"></param>
        public static void SaveAsJPG(Texture2D texture, string filePath, int quality = 100)
        {
            byte[] bytes = texture.EncodeToJPG(quality);
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// Take a screenshot using the designated camera in 24-bit color depth
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Texture2D TakeScreenshot(UnityEngine.Camera camera, int width, int height)
        {
            RenderTexture renderTexture = new RenderTexture(width, height, 24);
            camera.targetTexture = renderTexture;
            Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = renderTexture;
            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(renderTexture);

            return screenshot;
        }

        /// <summary>
        /// Create a texture by assigning a color to an array of pixels. This can be used
        /// to give EditorGUI elements a foreground color or background color
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Texture2D CreateTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Create a texture with a border by assigning a color to pixel arrays.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="textureColor"></param>
        /// <param name="border"></param>
        /// <param name="borderColor"></param>
        /// <returns></returns>
        public static Texture2D CreateTexture(int width, int height, Color textureColor, RectOffset border,
            Color borderColor)
        {
            int innerWidth = width;
            width += border.left;
            width += border.right;

            Color[] pixels = new Color[width * (height + border.top + border.bottom)];

            for (int i = 0; i < pixels.Length; i++)
            {
                if (i < (border.bottom * width))
                    pixels[i] = borderColor;
                else if (i >= ((border.bottom * width) + (height * width)))  //Border Top
                    pixels[i] = borderColor;
                else
                { //Center of Texture

                    if ((i % width) < border.left) // Border left
                        pixels[i] = borderColor;
                    else if ((i % width) >= (border.left + innerWidth)) //Border right
                        pixels[i] = borderColor;
                    else
                        pixels[i] = textureColor;    //Color texture
                }
            }

            Texture2D texture = new Texture2D(width, height + border.top + border.bottom);
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }
        
        public static Texture2D GetBGTexture(Color backgroundColor, List<Texture2D> tempTextureList)
        {
            int res = 1;

            Color[] colors = new Color[res * res];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = backgroundColor;
            }

            Texture2D tex = new Texture2D(res, res);
            tex.SetPixels(colors);
            tex.Apply(true);
            tempTextureList.Add(tex);

            return tex;
        }


        public static Texture2D GetBGTexture(Color backgroundColor, Color borderColor, List<Texture2D> tempTextureList)
        {
            int res = 6;

            Color[] colors = new Color[res * res];

            for (int x = 0; x < res; x++)
            {
                for (int y = 0; y < res; y++)
                {
                    int i = x * res + y;

                    if (x == 0 || x == res - 1 || y == 0 || y == res - 1)
                    {
                        // Apply the border color
                        colors[i] = borderColor;
                    }
                    else
                    {
                        // Apply the background color
                        colors[i] = backgroundColor;
                    }
                }
            }

            Texture2D tex = new Texture2D(res, res);
            tex.SetPixels(colors);
            tex.Apply(true);
            tempTextureList.Add(tex);

            return tex;
        }
        
        /// <summary>
        /// Converts a Render Texture to texture 2D by reading the pixels from it.
        /// </summary>
        /// <param name="renderTexture"></param>
        /// <param name="texture"></param>
        public static Texture2D ConvertRenderTextureToTexture2D(RenderTexture renderTexture)
        {
            Texture2D output = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.R16, false);
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;
            output.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            output.Apply();
            RenderTexture.active = currentRT;
            return output;
        }
        
        /// <summary>
        /// Convert the supplied texture to an array based on grayscale value
        /// </summary>
        /// <param name="texture">Input texture - must be read enabled</param>
        /// <returns>Texture as grayscale array</returns>
        public static float[,] ConvertTextureToArray(Texture2D texture)
        {
            float[,] array = new float[texture.width, texture.height];
            for (int x = 0; x < texture.width; x++)
            {
                for (int z = 0; z < texture.height; z++)
                {
                    array[x, z] = texture.GetPixel(x, z).grayscale;
                }
            }
            return array;
        }
        


    }
}
