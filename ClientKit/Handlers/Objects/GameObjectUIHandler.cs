using UnityEngine;
using UnityEngine.UI;

namespace Kits.ClientKit.Handlers.Objects
{
    public static partial class GameObjectHandler
    {
        /// <summary>
        /// GameObject add image and set sprite.
        /// </summary>
        public static Image AddImage(this UnityEngine.GameObject go, Sprite sprite)
        {
            Image image = go.AddComponent<Image>();
            image.sprite = sprite;

            return image;
        }
    }
}