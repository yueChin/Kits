using System;
using System.IO;
using System.Reflection;
using Kits.ClientKit.Handlers.ValueType;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Graphic
{
    public static class SpriteHandler
    {
#if UNITY_EDITOR
        private static Type m_SpriteEditorExtension =
            Type.GetType("UnityEditor.Experimental.U2D.SpriteEditorExtension, UnityEditor")
            ?? Type.GetType("UnityEditor.U2D.SpriteEditorExtension, UnityEditor");

        private static MethodInfo m_GetActiveAtlasTexture = m_SpriteEditorExtension
            .GetMethod("GetActiveAtlasTexture", BindingFlags.Static | BindingFlags.NonPublic);

        public static Texture2D GetActualTexture(this Sprite self)
        {
            if (!self) return 
                null;

            if (Application.isPlaying) 
                return self.texture;
            Texture2D ret = m_GetActiveAtlasTexture.Invoke(null, new[] {self}) as Texture2D;
            return ret ? ret : self.texture;
        }
#else
        internal static Texture2D GetActualTexture(this Sprite self)
        {
            return self ? self.texture : null;
        }
#endif
        //加载外部资源为Sprite
        public static Sprite LoadSpriteInLocal(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.Log("LoadSpriteInLocal() cannot find sprite file : " + filePath);
                return null;
            }
            Texture2D texture = TextureHandler.LoadTextureInLocal(filePath);
            //创建Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2Handler.Half);
            return sprite;
        }

        /// <summary>
        /// Get ratio size by max width and height with rect scale.
        /// </summary>
        public static Vector2 GetRatioSizeByMax(this Sprite sprite, float maxWidth, float maxHeight, float scale = 1.0f)
        {
            Rect rect = sprite.rect;
            float width = rect.width * scale;
            float height = rect.height * scale;

            if (width >= height)
            {
                if (width > maxWidth)
                {
                    // height becomes small
                    height *= maxWidth / width;
                    // width becoms small
                    width = maxWidth;
                }

                if (height > maxHeight)
                {
                    // width also becoms smaller
                    width *= maxHeight / height;
                    // height becomes smalller
                    height = maxHeight;
                }
            }
            else
            {
                if (height > maxHeight)
                {
                    // width becoms small
                    width *= maxHeight / height;
                    // height becomes small
                    height = maxHeight;
                }

                if (width > maxWidth)
                {
                    // height becomes smaller
                    height *= maxWidth / width;
                    // width becomes smaller
                    width = maxWidth;
                }
            }

            return new Vector2(width, height);
        }

        /// <summary>
        /// Convert a Texture2D to a Sprite object
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Sprite TextureToSprite(Texture2D texture)
        {
            if (texture == null) { return null; }
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// Convert an older Texture format to a Sprite object
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Sprite TextureToSprite(Texture texture)
        {
            return TextureToSprite(texture as Texture2D);
        }
    }
}