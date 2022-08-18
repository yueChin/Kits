using UnityEngine;

namespace Kits.ClientKit.Handlers
{
    /// <summary>
    /// Unity 内置类型扩展
    /// </summary>
    public static partial class GradientHandler
    {
        
        
        /// <summary>
        /// 复制 Gradient 实例
        /// </summary>
        public static Gradient Clone(this Gradient target)
        {
            Gradient newGradient = new Gradient
            {
                alphaKeys = target.alphaKeys,
                colorKeys = target.colorKeys,
                mode = target.mode
            };

            return newGradient;
        }

    } 

} 