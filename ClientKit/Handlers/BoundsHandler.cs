using System;
using UnityEngine;

namespace Kits.ClientKit.Handlers
{

    public static class BoundsHandler
    {
        /// <summary>
        /// Return the bounds of both the object and any colliders it has
        /// </summary>
        /// <param name="go">Game object to check</param>
        public static Bounds GetBounds(GameObject go)
        {
            Bounds bounds = new Bounds(go.transform.position, Vector3.zero);
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(r.bounds);
            }
            foreach (Collider c in go.GetComponentsInChildren<Collider>())
            {
                bounds.Encapsulate(c.bounds);
            }
            return bounds;
        }
        
        public static float GetBoundsForTaggedObject(string tag)
        {
            try
            {
                GameObject[] allGOsWithTag = GameObject.FindGameObjectsWithTag(tag);
                Bounds bounds = GetBounds(allGOsWithTag[0]);
                return (float)Math.Round(Mathf.Max(bounds.extents.x, bounds.extents.z), 2);
            }
            catch { }

            return 5f;
        }
        
    }
}