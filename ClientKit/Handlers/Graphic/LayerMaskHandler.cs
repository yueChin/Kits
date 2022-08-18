using System.Collections.Generic;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Graphic
{

    public static class LayerMaskHandler
    {
                /// <summary>
        /// Converts a Layer Mask into a string Array of Layer Names. (See StringToLayerMask)
        /// </summary>
        /// <param name="layerMask">The Layer Mask to convert into a string array.</param>
        /// <returns></returns>
        public static string[] LayerMaskToString(LayerMask layerMask)
        {
            List<int> allIndices = GetIndicesfromLayerMask(layerMask);
            string[] resultStrings = new string[allIndices.Count];
            for (int i = 0; i < allIndices.Count; i++)
            {
                int layerIndex = allIndices[i];
                resultStrings[i] = LayerMask.LayerToName(layerIndex);
            }
            return resultStrings;
        }

        /// <summary>
        /// Converts a string array with layer names back into a layermask. (See LayerMaskToString)
        /// </summary>
        /// <param name="layerNames">The string of layer mask names separated with a |.</param>
        /// <param name="targetLayerMask">The target layer mask that will contain the result if the conversion was successful.</param>
        /// <returns></returns>
        public static bool StringToLayerMask(string[] layerNames, ref LayerMask targetLayerMask)
        {
            for (int i = 0; i < layerNames.Length; i++)
            {
                string layerName = layerNames[i];
                if (LayerMask.NameToLayer(layerName) == -1)
                {
                    //Abort Mission, we could not find the layer contained in this string
                    return false;
                }
            }
            targetLayerMask = LayerMask.GetMask(layerNames);
            return true; 
        }

        /// <summary>
        /// Translates the layer mask bitmask into a list of indices
        /// </summary>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static List<int> GetIndicesfromLayerMask(LayerMask layerMask)
        {
            List<int> layers = new List<int>();
            int bitmask = layerMask.value;
            for (int i = 0; i < 32; i++)
            {
                if (((1 << i) & bitmask) != 0)
                {
                    layers.Add(i);
                }
            }
            return layers;
        }

    }

}