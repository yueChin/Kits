using System;
using System.Collections.Generic;

namespace Kits.ClientKit.Handlers.Transform
{
    public static class BoneTransformHandler
    {
        /// <summary>
        /// Recursively searches for a bone given the name and returns it if found
        /// </summary>
        /// <param name="transform">Parent to search through</param>
        /// <param name="boneName">Bone to find</param>
        /// <returns>Transform of the bone or null</returns>
        public static UnityEngine.Transform FindTransform(this UnityEngine.Transform transform, string boneName)
        {
            return FindChildTransform(transform, boneName);
        }

        /// <summary>
        /// Recursively search for a bone that matches the specifie name
        /// </summary>
        /// <param name="transform">Parent to search through</param>
        /// <param name="boneName">Bone to find</param>
        /// <returns></returns>
        public static UnityEngine.Transform FindChildTransform(UnityEngine.Transform transform, string boneName)
        {
            string parentName = transform.name;

            // We found it. Get out fast
            if (String.Compare(parentName, boneName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return transform;
            }

            // Handle the case where the bone name is nested in a namespace
            int lIndex = parentName.IndexOf(':');
            if (lIndex >= 0)
            {
                parentName = parentName.Substring(lIndex + 1);
                if (String.Compare(parentName, boneName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return transform;
                }
            }

            // Since we didn't find it, check the children
            for (int i = 0; i < transform.transform.childCount; i++)
            {
                UnityEngine.Transform childTrans = FindChildTransform(transform.transform.GetChild(i), boneName);
                if (childTrans != null)
                {
                    return childTrans;
                }
            }

            // Return nothing
            return null;
        }

        /// <summary>
        /// Retrieves the chain of transforms that start at the name and goes down the first child
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="boneName"></param>
        /// <param name="refList"></param>
        public static void FindTransformChain(UnityEngine.Transform transform, string boneName, ref List<UnityEngine.Transform> refList)
        {
            UnityEngine.Transform parentTrans = transform.FindTransform(boneName);

            refList.Clear();
            while (parentTrans != null)
            {
                refList.Add(parentTrans);
                if (parentTrans.childCount > 0)
                {
                    parentTrans = parentTrans.GetChild(0);
                }
                else
                {
                    parentTrans = null;
                }
            }
        }
    }
}