//#define OOTII_DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Physics
{
    /// <summary>
    /// Provides functions for specialized raycast solutions
    /// </summary>
    public static class RaycastHandler
    {
        /// <summary>
        /// Total number of hits that can happen with our pre-allocated arrays
        /// </summary>
        public const int MAX_HITS = 40;

        /// <summary>
        /// Used to compare hits based on distance
        /// </summary>
        public static RaycastHitDistanceComparer HitDistanceComparer = new RaycastHitDistanceComparer();

        /// <summary>
        /// Used when we need to return an empty raycast
        /// </summary>
        public static RaycastHit EmptyHitInfo = new RaycastHit();

        /// <summary>
        /// We use this if we don't want to re-allocate arrays. This is simple, but
        /// won't work with multi-threading and the contents need to be used immediately,
        /// as they are not persistant across alls.
        /// </summary>
        public static readonly RaycastHit[] SharedHitArray = new RaycastHit[MAX_HITS];

        /// <summary>
        /// We use this if we don't want to re-allocate arrays. This is simple, but
        /// won't work with multi-threading and the contents need to be used immediately,
        /// as they are not persistant across alls.
        /// </summary>
        public static readonly Collider[] SharedColliderArray = new Collider[MAX_HITS];

        // ***********************************************************************************
        // Newer non-allocating versions of the call
        // ***********************************************************************************

        /// <summary>
        /// Use the non-alloc version of raycast to see if the ray hits anything. Here we are
        /// not particular about what we hit. We just test for a hit
        /// </summary>
        /// <param name="rayStart">Starting point of the ray</param>
        /// <param name="rayDirection">Direction of the ray</param>
        /// <param name="distance">Max distance f the ray</param>
        /// <param name="layerMask">Layer mask to determine what we'll hit</param>
        /// <param name="ignore">Single transform we'll test if we should ignore</param>
        /// <param name="ignoreList">List of transforms we should ignore collisions with</param>
        /// <param name="ignoreTriggers">Determines if we'll ignore triggers or collide with them</param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rayStart, Vector3 rayDirection, float distance = 1000f, int layerMask = -1, UnityEngine.Transform ignore = null, List<UnityEngine.Transform> ignoreList = null, bool ignoreTriggers = true)
        {
#if (UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
            
            int hitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (hitCount < 5 && distance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = false;
                RaycastHit tempHitInfo;

                if (layerMask != -1)
                {
                    lHit = UnityEngine.Physics.Raycast(rayStart, rayDirection, out tempHitInfo, distance, layerMask);
                }
                else
                {
                    lHit = UnityEngine.Physics.Raycast(rayStart, rayDirection, out tempHitInfo, distance);
                }

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (ignoreTriggers && tempHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // Test if we're ignoring the collider
                Transform lColliderTransform = tempHitInfo.collider.transform;
                if (lIsValidHit && ignore != null && IsDescendant(ignore, lColliderTransform)) { lIsValidHit = false; }

                if (lIsValidHit && ignoreList != null)
                {
                    for (int i = 0; i < ignoreList.Count; i++)
                    {
                        if (IsDescendant(ignoreList[i], lColliderTransform))
                        {
                            lIsValidHit = false;
                            break;
                        }
                    }
                }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += tempHitInfo.distance + 0.05f;
                    rayStart = tempHitInfo.point + (rayDirection * 0.05f);

                    distance -= (tempHitInfo.distance + 0.05f);

                    hitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                tempHitInfo.distance += lDistanceOffset;

                return true;
            }

            // If we got here, we exceeded our attempts and we should drop out
            return false;

#else

            // In this specific case, we can get out early since there is a way to ignore triggers
            if (ignore == null && ignoreList == null && layerMask != -1)
            {
                return UnityEngine.Physics.Raycast(rayStart, rayDirection, distance, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }

            // Perform the more expensive raycast
            int hits = 0;

            int lLength = SharedHitArray.Length;
            for (int i = 0; i < lLength; i++) { SharedHitArray[i] = RaycastHandler.EmptyHitInfo; }

            if (layerMask != -1)
            {
                hits = UnityEngine.Physics.RaycastNonAlloc(rayStart, rayDirection, SharedHitArray, distance, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }
            else
            {
                hits = UnityEngine.Physics.RaycastNonAlloc(rayStart, rayDirection, SharedHitArray, distance);
            }

            // With no hits, this is easy
            if (hits == 0)
            {
                return false;
            }
            // One hit is also easy
            else if (hits == 1)
            {
                if (ignoreTriggers && SharedHitArray[0].collider.isTrigger) { return false; }

                UnityEngine.Transform lColliderTransform = SharedHitArray[0].collider.transform;

                if (ignore != null && IsDescendant(ignore, lColliderTransform)) { return false; }

                if (ignoreList != null)
                {
                    for (int i = 0; i < ignoreList.Count; i++)
                    {
                        if (IsDescendant(ignoreList[i], lColliderTransform)) { return false; }
                    }
                }

                return true;
            }
            // Go through all the hits and see if any hit
            else
            {
                for (int i = 0; i < hits; i++)
                {
                    if (ignoreTriggers && SharedHitArray[i].collider.isTrigger) { continue; }

                    UnityEngine.Transform lColliderTransform = SharedHitArray[i].collider.transform;

                    if (ignore != null && IsDescendant(ignore, lColliderTransform)) { continue; }

                    if (ignoreList != null)
                    {
                        bool lIgnoreFound = false;
                        for (int j = 0; j < ignoreList.Count; j++)
                        {
                            if (IsDescendant(ignoreList[j], lColliderTransform))
                            {
                                lIgnoreFound = true;
                                break;
                            }
                        }

                        if (lIgnoreFound) { continue; }
                    }

                    return true;
                }
            }

            return false;

#endif
        }

        /// <summary>
        /// Use the non-alloc version of raycast to see if the ray hits anything. Here we are
        /// not particular about what we hit. We just test for a hit
        /// </summary>
        /// <param name="rayStart">Starting point of the ray</param>
        /// <param name="rayDirection">Direction of the ray</param>
        /// <param name="hitInfo">First RaycastHit value that the ray hits</param>
        /// <param name="distance">Max distance f the ray</param>
        /// <param name="layerMask">Layer mask to determine what we'll hit</param>
        /// <param name="ignore">Single transform we'll test if we should ignore</param>
        /// <param name="ignoreList">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeRaycast(Vector3 rayStart, Vector3 rayDirection, out RaycastHit hitInfo, float distance = 1000f, int layerMask = -1, UnityEngine.Transform ignore = null, List<UnityEngine.Transform> ignoreList = null, bool ignoreTriggers = true, bool debug = false)
        {
#if (UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)

            hitInfo = RaycastHandler.S_EmptyHitInfo;

            int hitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (hitCount < 5 && distance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = false;
                RaycastHit tempHitInfo;

                if (layerMask != -1)
                {
                    lHit = UnityEngine.Physics.Raycast(rayStart, rayDirection, out tempHitInfo, distance, layerMask);
                }
                else
                {
                    lHit = UnityEngine.Physics.Raycast(rayStart, rayDirection, out tempHitInfo, distance);
                }

                // Easy, just return no hit
                if (!lHit) { return false; }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (ignoreTriggers && tempHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // Test if we're ignoring the collider
                Transform lColliderTransform = tempHitInfo.collider.transform;
                if (lIsValidHit && ignore != null && IsDescendant(ignore, lColliderTransform)) { lIsValidHit = false; }

                if (lIsValidHit && ignoreList != null)
                {
                    for (int i = 0; i < ignoreList.Count; i++)
                    {
                        if (IsDescendant(ignoreList[i], lColliderTransform))
                        {
                            lIsValidHit = false;
                            break;
                        }
                    }
                }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += tempHitInfo.distance + 0.05f;
                    rayStart = tempHitInfo.point + (rayDirection * 0.05f);

                    distance -= (tempHitInfo.distance + 0.05f);

                    hitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                tempHitInfo.distance += lDistanceOffset;

                // return the hit info
                hitInfo = tempHitInfo;

                return true;
            }

            // If we got here, we exceeded our attempts and we should drop out
            return false;

#else

#if UNITY_EDITOR || OOTII_DEBUG
            if (debug)
            {
                Debug.DrawLine(rayStart, rayStart + (rayDirection * distance), Color.blue, 0.02f);
            }
#endif

            // In this specific case, we can get out early since there is a way to ignore triggers
            if (ignore == null && ignoreList == null && layerMask != -1)
            {
                return UnityEngine.Physics.Raycast(rayStart, rayDirection, out hitInfo, distance, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }

            // Perform the more expensive raycast
            hitInfo = EmptyHitInfo;

            // Use the non allocating version
            int hits = 0;

            int lLength = SharedHitArray.Length;
            for (int i = 0; i < lLength; i++) { SharedHitArray[i] = RaycastHandler.EmptyHitInfo; }

            if (layerMask != -1)
            {
                hits = UnityEngine.Physics.RaycastNonAlloc(rayStart, rayDirection, SharedHitArray, distance, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }
            else
            {
                hits = UnityEngine.Physics.RaycastNonAlloc(rayStart, rayDirection, SharedHitArray, distance);
            }

            // With no hits, this is easy
            if (hits == 0)
            {
                return false;
            }
            // One hit is also easy
            else if (hits == 1)
            {
                if (ignoreTriggers && SharedHitArray[0].collider.isTrigger) { return false; }

                UnityEngine.Transform lColliderTransform = SharedHitArray[0].collider.transform;

                if (ignore != null && IsDescendant(ignore, lColliderTransform)) { return false; }

                if (ignoreList != null)
                {
                    for (int i = 0; i < ignoreList.Count; i++)
                    {
                        if (IsDescendant(ignoreList[i], lColliderTransform)) { return false; }
                    }
                }

                hitInfo = SharedHitArray[0];
                return true;
            }
            // Find the closest hit and test it
            else
            {
                for (int i = 0; i < hits; i++)
                {
                    if (ignoreTriggers && SharedHitArray[i].collider.isTrigger) { continue; }

                    UnityEngine.Transform lColliderTransform = SharedHitArray[i].collider.transform;

                    if (ignore != null && IsDescendant(ignore, lColliderTransform)) { continue; }

                    if (ignoreList != null)
                    {
                        bool lIgnoreFound = false;
                        for (int j = 0; j < ignoreList.Count; j++)
                        {
                            if (IsDescendant(ignoreList[j], lColliderTransform))
                            {
                                lIgnoreFound = true;
                                break;
                            }
                        }

                        if (lIgnoreFound) { continue; }
                    }

                    // If we got here, we have a valid it. See if it's closer
                    if (hitInfo.collider == null || SharedHitArray[i].distance < hitInfo.distance)
                    {
                        hitInfo = SharedHitArray[i];
                    }
                }

                if (hitInfo.collider != null)
                {
                    return true;
                }
            }

            return false;

#endif
        }

        /// <summary>
        /// Use the non-alloc version of raycast to see if the ray hits anything. Here we are
        /// not particular about what we hit. We just test for a hit
        /// </summary>
        /// <param name="rayStart">Starting point of the ray</param>
        /// <param name="rayDirection">Direction of the ray</param>
        /// <param name="hitArray">Array of RaycastHit objects that were hit</param>
        /// <param name="distance">Max distance f the ray</param>
        /// <param name="layerMask">Layer mask to determine what we'll hit</param>
        /// <param name="ignore">Single transform we'll test if we should ignore</param>
        /// <param name="ignoreList">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static int SafeRaycastAll(Vector3 rayStart, Vector3 rayDirection, out RaycastHit[] hitArray, float distance = 1000f, int layerMask = -1, UnityEngine.Transform ignore = null, List<UnityEngine.Transform> ignoreList = null, bool ignoreTriggers = true)
        {
#if (UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)

            hitArray = UnityEngine.Physics.RaycastAll(rayStart, rayDirection, distance);

            // With no hits, this is easy
            if (hitArray.Length == 0)
            {
            }
            // With one hit, this is easy too
            else if (hitArray.Length == 1)
            {
                if (ignoreTriggers && hitArray[0].collider.isTrigger)
                {
                    hitArray = new RaycastHit[0];
                }
            }
            // Find the closest hit
            else
            {
                // Order the array by distance and get rid of items that don't pass
                hitArray.Sort(delegate (RaycastHit rLeft, RaycastHit rRight) { return (rLeft.distance < rRight.distance ? -1 : (rLeft.distance > rRight.distance ? 1 : 0)); });
                for (int i = hitArray.Length - 1; i >= 0; i--)
                {
                    if (ignoreTriggers && hitArray[i].collider.isTrigger) { hitArray = ArrayExt.RemoveAt(hitArray, i); }
                }
            }

            return (hitArray != null ? hitArray.Length : 0);

#else
            hitArray = null;

            // Use the non allocating version
            int hits = 0;

            int lLength = SharedHitArray.Length;
            for (int i = 0; i < lLength; i++) { SharedHitArray[i] = RaycastHandler.EmptyHitInfo; }

            if (layerMask != -1)
            {
                hits = UnityEngine.Physics.RaycastNonAlloc(rayStart, rayDirection, SharedHitArray, distance, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }
            else
            {
                hits = UnityEngine.Physics.RaycastNonAlloc(rayStart, rayDirection, SharedHitArray, distance);
            }

            // With no hits, this is easy
            if (hits == 0)
            {
                return 0;
            }
            // One hit is also easy
            else if (hits == 1)
            {
                if (ignoreTriggers && SharedHitArray[0].collider.isTrigger) { return 0; }

                UnityEngine.Transform lColliderTransform = SharedHitArray[0].collider.transform;

                if (ignore != null && IsDescendant(ignore, lColliderTransform)) { return 0; }

                if (ignoreList != null)
                {
                    for (int i = 0; i < ignoreList.Count; i++)
                    {
                        if (IsDescendant(ignoreList[i], lColliderTransform)) { return 0; }
                    }
                }

                hitArray = SharedHitArray;
                return 1;
            }
            // Go through all the hits and see if any hit
            else
            {
                int validHits = 0;
                for (int i = 0; i < hits; i++)
                {
                    bool lShift = false;
                    UnityEngine.Transform lColliderTransform = SharedHitArray[i].collider.transform;

                    if (ignoreTriggers && SharedHitArray[i].collider.isTrigger) { lShift = true; }

                    if (ignore != null && IsDescendant(ignore, lColliderTransform)) { lShift = true; }

                    if (ignoreList != null)
                    {
                        for (int j = 0; j < ignoreList.Count; j++)
                        {
                            if (IsDescendant(ignoreList[j], lColliderTransform))
                            {
                                lShift = true;
                                break;
                            }
                        }
                    }

                    if (lShift)
                    {
                        // Since we are shifting left, out count is reduced
                        hits--;

                        // Shift the contents, but we care about the old count (hence the + 1)
                        for (int j = i; j < hits; j++)
                        {
                            SharedHitArray[j] = SharedHitArray[j + 1];
                        }

                        // Move our index so when the for-loop iterates us forward, we stay put
                        i--;
                    }
                    else
                    {
                        validHits++;
                    }

                    // With the valid hits gathered, we now need to sort the array
                }

                hitArray = SharedHitArray;
                return validHits;
            }

#endif
        }

        /// <summary>
        /// Use the non-alloc version of raycast to see if the ray hits anything. Here we are
        /// not particular about what we hit. We just test for a hit
        /// </summary>
        /// <param name="rayStart">Starting point of the ray</param>
        /// <param name="rayDirection">Direction of the ray</param>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="distance">Max distance f the ray</param>
        /// <param name="layerMask">Layer mask to determine what we'll hit</param>
        /// <param name="ignore">Single transform we'll test if we should ignore</param>
        /// <param name="ignoreList">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeSphereCast(Vector3 rayStart, Vector3 rayDirection, float radius, float distance = 1000f, int layerMask = -1, UnityEngine.Transform ignore = null, List<UnityEngine.Transform> ignoreList = null, bool ignoreTriggers = true)
        {
#if (UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)

            int hitCount = 0;
            RaycastHit tempHitInfo;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (hitCount < 5 && distance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = false;

                if (layerMask != -1)
                {
                    lHit = UnityEngine.Physics.SphereCast(rayStart, radius, rayDirection, out tempHitInfo, distance, layerMask);
                }
                else
                {
                    lHit = UnityEngine.Physics.SphereCast(rayStart, radius, rayDirection, out tempHitInfo, distance);
                }

                // Easy, just return no hit
                if (!lHit) { return false; }

                // Turns out we can't actually trust the sphere cast as it sometimes returns incorrect point and normal values.
                RaycastHit lRayHitInfo;
                if (UnityEngine.Physics.Raycast(rayStart, tempHitInfo.point - rayStart, out lRayHitInfo, distance))
                {
                    tempHitInfo = lRayHitInfo;
                }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (ignoreTriggers && tempHitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we hit a transform to ignore
                if (lIsValidHit && ignore != null)
                {
                    Transform lCurrentTransform = tempHitInfo.collider.transform;
                    while (lCurrentTransform != null)
                    {
                        if (lCurrentTransform == ignore)
                        {
                            lIsValidHit = false;
                            break;
                        }

                        lCurrentTransform = lCurrentTransform.parent;
                    }
                }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += tempHitInfo.distance + 0.05f;
                    rayStart = tempHitInfo.point + (rayDirection * 0.05f);

                    distance -= (tempHitInfo.distance + 0.05f);

                    hitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                tempHitInfo.distance += lDistanceOffset;

                return true;
            }

            // If we got here, we exceeded our attempts and we should drop out
            return false;

#else

            // In this specific case, we can get out early since there is a way to ignore triggers
            if (ignore == null && ignoreList == null && layerMask != -1)
            {
                RaycastHit tempHitInfo;
                return UnityEngine.Physics.SphereCast(rayStart, radius, rayDirection, out tempHitInfo, distance, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }

            // Perform the more expensive raycast
            int hits = 0;

            int lLength = SharedHitArray.Length;
            for (int i = 0; i < lLength; i++) { SharedHitArray[i] = RaycastHandler.EmptyHitInfo; }

            if (layerMask != -1)
            {
                hits = UnityEngine.Physics.SphereCastNonAlloc(rayStart, radius, rayDirection, SharedHitArray, distance, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }
            else
            {
                hits = UnityEngine.Physics.SphereCastNonAlloc(rayStart, radius, rayDirection, SharedHitArray, distance);
            }

            // With no hits, this is easy
            if (hits == 0)
            {
                return false;
            }
            // One hit is also easy
            else if (hits == 1)
            {
                if (ignoreTriggers && SharedHitArray[0].collider.isTrigger) { return false; }

                UnityEngine.Transform lColliderTransform = SharedHitArray[0].collider.transform;

                if (ignore != null && IsDescendant(ignore, lColliderTransform)) { return false; }

                if (ignoreList != null)
                {
                    for (int i = 0; i < ignoreList.Count; i++)
                    {
                        if (IsDescendant(ignoreList[i], lColliderTransform)) { return false; }
                    }
                }

                return true;
            }
            // Go through all the hits and see if any hit
            else
            {
                for (int i = 0; i < hits; i++)
                {
                    if (ignoreTriggers && SharedHitArray[i].collider.isTrigger) { continue; }

                    UnityEngine.Transform lColliderTransform = SharedHitArray[i].collider.transform;

                    if (ignore != null && IsDescendant(ignore, lColliderTransform)) { continue; }

                    if (ignoreList != null)
                    {
                        bool lIgnoreFound = false;
                        for (int j = 0; j < ignoreList.Count; j++)
                        {
                            if (IsDescendant(ignoreList[j], lColliderTransform))
                            {
                                lIgnoreFound = true;
                                break;
                            }
                        }

                        if (lIgnoreFound) { continue; }
                    }

                    return true;
                }
            }

            return false;

#endif
        }

        /// <summary>
        /// Use the non-alloc version of raycast to see if the ray hits anything. Here we are
        /// not particular about what we hit. We just test for a hit
        /// </summary>
        /// <param name="rayStart">Starting point of the ray</param>
        /// <param name="rayDirection">Direction of the ray</param>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="hitInfo">First RaycastHit value that the ray hits</param>
        /// <param name="distance">Max distance f the ray</param>
        /// <param name="layerMask">Layer mask to determine what we'll hit</param>
        /// <param name="ignore">Single transform we'll test if we should ignore</param>
        /// <param name="ignoreList">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static bool SafeSphereCast(Vector3 rayStart, Vector3 rayDirection, float radius, out RaycastHit hitInfo, float distance = 1000f, int layerMask = -1, UnityEngine.Transform ignore = null, List<UnityEngine.Transform> ignoreList = null, bool ignoreTriggers = true)
        {
#if (UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)

            int hitCount = 0;
            float lDistanceOffset = 0f;

            // Since some objects (like triggers) are invalid for collisions, we want
            // to go through them. That means we may continue a ray cast even if a hit occured
            while (hitCount < 5 && distance > 0f)
            {
                // Assume the next hit to be valid
                bool lIsValidHit = true;

                // Test from the current start
                bool lHit = false;

                if (layerMask != -1)
                {
                    lHit = UnityEngine.Physics.SphereCast(rayStart, radius, rayDirection, out hitInfo, distance, layerMask);
                }
                else
                {
                    lHit = UnityEngine.Physics.SphereCast(rayStart, radius, rayDirection, out hitInfo, distance);
                }

                // Easy, just return no hit
                if (!lHit) { return false; }

                // Turns out we can't actually trust the sphere cast as it sometimes returns incorrect point and normal values.
                RaycastHit lRayHitInfo;
                if (UnityEngine.Physics.Raycast(rayStart, hitInfo.point - rayStart, out lRayHitInfo, distance))
                {
                    hitInfo = lRayHitInfo;
                }

                // If we hit a trigger, we'll continue testing just a tad bit beyond.
                if (ignoreTriggers && hitInfo.collider.isTrigger) { lIsValidHit = false; }

                // If we hit a transform to ignore
                if (lIsValidHit && ignore != null)
                {
                    Transform lCurrentTransform = hitInfo.collider.transform;
                    while (lCurrentTransform != null)
                    {
                        if (lCurrentTransform == ignore)
                        {
                            lIsValidHit = false;
                            break;
                        }

                        lCurrentTransform = lCurrentTransform.parent;
                    }
                }

                // If we have an invalid hit, we'll continue testing by using the hit point
                // (plus a little extra) as the new start
                if (!lIsValidHit)
                {
                    lDistanceOffset += hitInfo.distance + 0.05f;
                    rayStart = hitInfo.point + (rayDirection * 0.05f);

                    distance -= (hitInfo.distance + 0.05f);

                    hitCount++;
                    continue;
                }

                // If we got here, we must have a valid hit. Update
                // the distance info incase we had to cycle through invalid hits
                hitInfo.distance += lDistanceOffset;

                return true;
            }

            // We have to initialize the out param
            hitInfo = S_EmptyHitInfo;

            // If we got here, we exceeded our attempts and we should drop out
            return false;

#else

            // In this specific case, we can get out early since there is a way to ignore triggers
            if (ignore == null && ignoreList == null && layerMask != -1)
            {
                return UnityEngine.Physics.SphereCast(rayStart, radius, rayDirection, out hitInfo, distance, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }

            // Perform the more expensive raycast
            hitInfo = EmptyHitInfo;

            // Use the non allocating version
            int hits = 0;

            int lLength = SharedHitArray.Length;
            for (int i = 0; i < lLength; i++) { SharedHitArray[i] = RaycastHandler.EmptyHitInfo; }

            if (layerMask != -1)
            {
                hits = UnityEngine.Physics.SphereCastNonAlloc(rayStart, radius, rayDirection, SharedHitArray, distance, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }
            else
            {
                hits = UnityEngine.Physics.SphereCastNonAlloc(rayStart, radius, rayDirection, SharedHitArray, distance);
            }

            // With no hits, this is easy
            if (hits == 0)
            {
                return false;
            }
            // One hit is also easy
            else if (hits == 1)
            {
                if (ignoreTriggers && SharedHitArray[0].collider.isTrigger) { return false; }

                UnityEngine.Transform lColliderTransform = SharedHitArray[0].collider.transform;

                if (ignore != null && IsDescendant(ignore, lColliderTransform)) { return false; }

                if (ignoreList != null)
                {
                    for (int i = 0; i < ignoreList.Count; i++)
                    {
                        if (IsDescendant(ignoreList[i], lColliderTransform)) { return false; }
                    }
                }

                hitInfo = SharedHitArray[0];
                return true;
            }
            // Find the closest hit and test it
            else
            {
                for (int i = 0; i < hits; i++)
                {
                    if (ignoreTriggers && SharedHitArray[i].collider.isTrigger) { continue; }

                    UnityEngine.Transform lColliderTransform = SharedHitArray[i].collider.transform;

                    if (ignore != null && IsDescendant(ignore, lColliderTransform)) { continue; }

                    if (ignoreList != null)
                    {
                        bool lIgnoreFound = false;
                        for (int j = 0; j < ignoreList.Count; j++)
                        {
                            if (IsDescendant(ignoreList[j], lColliderTransform))
                            {
                                lIgnoreFound = true;
                                break;
                            }
                        }

                        if (lIgnoreFound) { continue; }
                    }

                    // If we got here, we have a valid it. See if it's closer
                    if (hitInfo.collider == null || SharedHitArray[i].distance < hitInfo.distance)
                    {
                        hitInfo = SharedHitArray[i];
                    }
                }

                if (hitInfo.collider != null)
                {
                    return true;
                }
            }

            return false;

#endif
        }

        /// <summary>
        /// Use the non-alloc version of raycast to see if the ray hits anything. Here we are
        /// not particular about what we hit. We just test for a hit
        /// </summary>
        /// <param name="rayStart">Starting point of the ray</param>
        /// <param name="rayDirection">Direction of the ray</param>
        /// <param name="hitArray">Array of RaycastHit objects that were hit</param>
        /// <param name="distance">Max distance f the ray</param>
        /// <param name="layerMask">Layer mask to determine what we'll hit</param>
        /// <param name="ignore">Single transform we'll test if we should ignore</param>
        /// <param name="ignoreList">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static int SafeSphereCastAll(Vector3 rayStart, Vector3 rayDirection, float radius, out RaycastHit[] hitArray, float distance = 1000f, int layerMask = -1, UnityEngine.Transform ignore = null, List<UnityEngine.Transform> ignoreList = null, bool ignoreTriggers = true)
        {
#if (UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)

            hitArray = null;

            if (layerMask != -1)
            {
                hitArray = UnityEngine.Physics.SphereCastAll(rayStart, radius, rayDirection, distance, layerMask);
            }
            else
            {
                hitArray = UnityEngine.Physics.SphereCastAll(rayStart, radius, rayDirection, distance);
            }

            // With no hits, this is easy
            if (hitArray.Length == 0)
            {
            }
            // With one hit, this is easy too
            else if (hitArray.Length == 1)
            {
                if ((ignoreTriggers && hitArray[0].collider.isTrigger) ||
                    (ignore != null && ignore == hitArray[0].collider.transform)
                   )
                {
                    hitArray = new RaycastHit[0];
                }
            }
            // Find the closest hit
            else
            {
                // Order the array by distance and get rid of items that don't pass
                hitArray.Sort(delegate (RaycastHit rLeft, RaycastHit rRight) { return (rLeft.distance < rRight.distance ? -1 : (rLeft.distance > rRight.distance ? 1 : 0)); });
                for (int i = hitArray.Length - 1; i >= 0; i--)
                {
                    if (ignoreTriggers && hitArray[i].collider.isTrigger) { hitArray = ArrayExt.RemoveAt(hitArray, i); }

                    if (ignore != null)
                    {
                        bool lIsValidHit = true;
                        Transform lCurrentTransform = hitArray[i].collider.transform;
                        while (lCurrentTransform != null)
                        {
                            if (lCurrentTransform == ignore)
                            {
                                lIsValidHit = false;
                                break;
                            }

                            lCurrentTransform = lCurrentTransform.parent;
                        }

                        if (!lIsValidHit) { hitArray = ArrayExt.RemoveAt(hitArray, i); }
                    }
                }
            }

            return (hitArray != null ? hitArray.Length : 0);

#else
            hitArray = null;

            // Use the non allocating version
            int hits = 0;

            int lLength = SharedHitArray.Length;
            for (int i = 0; i < lLength; i++) { SharedHitArray[i] = RaycastHandler.EmptyHitInfo; }

            if (layerMask != -1)
            {
                hits = UnityEngine.Physics.SphereCastNonAlloc(rayStart, radius, rayDirection, SharedHitArray, distance, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }
            else
            {
                hits = UnityEngine.Physics.SphereCastNonAlloc(rayStart, radius, rayDirection, SharedHitArray, distance);
            }

            // With no hits, this is easy
            if (hits == 0)
            {
                return 0;
            }
            // One hit is also easy
            else if (hits == 1)
            {
                if (ignoreTriggers && SharedHitArray[0].collider.isTrigger)
                {
                    return 0;
                }

                UnityEngine.Transform lColliderTransform = SharedHitArray[0].collider.transform;

                if (ignore != null && IsDescendant(ignore, lColliderTransform))
                {
                    return 0;
                }

                if (ignoreList != null)
                {
                    for (int i = 0; i < ignoreList.Count; i++)
                    {
                        if (IsDescendant(ignoreList[i], lColliderTransform))
                        {
                            return 0;
                        }
                    }
                }

                hitArray = SharedHitArray;
                return 1;
            }
            // Go through all the hits and see if any hit
            else
            {
                int validHits = 0;
                for (int i = 0; i < hits; i++)
                {
                    bool lShift = false;
                    UnityEngine.Transform lColliderTransform = SharedHitArray[i].collider.transform;

                    if (ignoreTriggers && SharedHitArray[i].collider.isTrigger)
                    {
                        lShift = true;
                    }

                    if (ignore != null && IsDescendant(ignore, lColliderTransform))
                    {
                        lShift = true;
                    }

                    if (ignoreList != null)
                    {
                        for (int j = 0; j < ignoreList.Count; j++)
                        {
                            if (IsDescendant(ignoreList[j], lColliderTransform))
                            {
                                lShift = true;
                                break;
                            }
                        }
                    }

                    if (lShift)
                    {
                        // Since we are shifting left, out count is reduced
                        hits--;

                        // Shift the contents, but we care about the old count (hence the + 1)
                        for (int j = i; j < hits; j++)
                        {
                            SharedHitArray[j] = SharedHitArray[j + 1];
                        }

                        // Move our index so when the for-loop iterates us forward, we stay put
                        i--;
                    }
                    else
                    {
                        validHits++;
                    }
                }

                hitArray = SharedHitArray;
                return validHits;
            }

#endif
        }

        /// <summary>
        /// Use the non-alloc version of raycast to see if the ray hits anything. Here we are
        /// not particular about what we hit. We just test for a hit
        /// </summary>
        /// <param name="position">Position of the sphere</param>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="colliderArray">Array of collision objects that were hit</param>
        /// <param name="layerMask">Layer mask to determine what we'll hit</param>
        /// <param name="ignore">Single transform we'll test if we should ignore</param>
        /// <param name="ignoreList">List of transforms we should ignore collisions with</param>
        /// <returns></returns>
        public static int SafeOverlapSphere(Vector3 position, float radius, out Collider[] colliderArray, int layerMask = -1, UnityEngine.Transform ignore = null, List<UnityEngine.Transform> ignoreList = null, bool ignoreTriggers = true)
        {
#if (UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)

            // This causes 28 B of GC.
            colliderArray = null;

            if (layerMask != -1)
            {
                colliderArray = UnityEngine.Physics.OverlapSphere(position, radius, layerMask);
            }
            else
            {
                colliderArray = UnityEngine.Physics.OverlapSphere(position, radius, layerMask);
            }

            // Get rid of elements we don't need
            if (ignoreTriggers)
            {
                for (int i = colliderArray.Length - 1; i >= 0; i--)
                {
                    if (colliderArray[i].isTrigger)
                    {
                        colliderArray = ArrayExt.RemoveAt<Collider>(colliderArray, i);
                    }
                }
            }

            if (ignore != null)
            {
                for (int i = colliderArray.Length - 1; i >= 0; i--)
                {
                    if (ignore == colliderArray[i].transform)
                    {
                        colliderArray = ArrayExt.RemoveAt<Collider>(colliderArray, i);
                    }
                }
            }

            if (ignoreList != null)
            {
                for (int i = colliderArray.Length - 1; i >= 0; i--)
                {
                    if (ignoreList.Contains(colliderArray[i].transform))
                    {
                        colliderArray = ArrayExt.RemoveAt<Collider>(colliderArray, i);
                    }
                }
            }

            return (colliderArray != null ? colliderArray.Length : 0);

#else
            colliderArray = null;

            // Use the non allocating version
            int hits = 0;

            if (layerMask != -1)
            {
                hits = UnityEngine.Physics.OverlapSphereNonAlloc(position, radius, SharedColliderArray, layerMask, (ignoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide));
            }
            else
            {
                hits = UnityEngine.Physics.OverlapSphereNonAlloc(position, radius, SharedColliderArray);
            }

            // With no hits, this is easy
            if (hits == 0)
            {
                return 0;
            }
            // One hit is also easy
            else if (hits == 1)
            {
                if (ignoreTriggers && SharedColliderArray[0].isTrigger)
                {
                    return 0;
                }

                UnityEngine.Transform lColliderTransform = SharedColliderArray[0].transform;

                if (ignore != null && IsDescendant(ignore, lColliderTransform))
                {
                    return 0;
                }

                if (ignoreList != null)
                {
                    for (int i = 0; i < ignoreList.Count; i++)
                    {
                        if (IsDescendant(ignoreList[i], lColliderTransform))
                        {
                            return 0;
                        }
                    }
                }

                colliderArray = SharedColliderArray;
                return 1;
            }
            // Go through all the hits and see if any hit
            else
            {
                int validHits = 0;
                for (int i = 0; i < hits; i++)
                {
                    bool lShift = false;
                    UnityEngine.Transform lColliderTransform = SharedColliderArray[i].transform;

                    if (ignoreTriggers && SharedColliderArray[i].isTrigger) { lShift = true; }

                    if (ignore != null && IsDescendant(ignore, lColliderTransform)) { lShift = true; }

                    if (ignoreList != null)
                    {
                        for (int j = 0; j < ignoreList.Count; j++)
                        {
                            if (IsDescendant(ignoreList[j], lColliderTransform))
                            {
                                lShift = true;
                                break;
                            }
                        }
                    }

                    if (lShift)
                    {
                        // Move our index so when the for-loop iterates us forward, we stay put
                        hits--;

                        // Shift the contents left, but we care about the old count (hence the + 1)
                        for (int j = i; j < hits; j++)
                        {
                            SharedColliderArray[j] = SharedColliderArray[j + 1];
                        }

                        // Move our index so when the for-loop iterates us forward, we stay put
                        i--;
                    }
                    else
                    {
                        validHits++;
                    }
                }

                colliderArray = SharedColliderArray;
                return validHits;
            }

#endif
        }

        /// <summary>
        /// Use the non-alloc version of raycast to see if the ray hits anything. Here we are
        /// not particular about what we hit. We just test for a hit
        /// </summary>
        /// <param name="rootTransform">Transform whose position and radius will be used</param>
        /// <param name="hitInfo">Object that is hit</param>
        /// <param name="radius">Final radius of the spiral cone</param>
        /// <param name="distance">Final distance of the cast</param>
        /// <param name="degreesPerStep">Degrees of spacing between each casting step</param>
        /// <param name="layerMask">Layer mask to determine what we'll hit</param>
        /// <param name="tag">Tag the hit object must have</param>
        /// <param name="ignore">Single transform we'll test if we should ignore</param>
        /// <param name="ignoreList">List of transforms we should ignore collisions with</param>
        /// <param name="ignoreTriggers">Determines if we ignore triggers</param>
        /// <param name="debug">Determines if we draw debug info</param>
        /// <returns></returns>
        public static bool SafeSpiralCast(UnityEngine.Transform rootTransform, out RaycastHit hitInfo, float radius = 8f, float distance = 1000f, float degreesPerStep = 27f, int layerMask = -1, string tag = null, UnityEngine.Transform ignore = null, List<UnityEngine.Transform> ignoreList = null, bool ignoreTriggers = true, bool debug = false)
        {
            hitInfo = RaycastHandler.EmptyHitInfo;

            //float lMaxRadius = radius;
            //float lMaxDistance = 20f;
            float revolutions = 2f;
            //float lDegreesPerStep = 27f;
            float steps = revolutions * (360f / degreesPerStep);
            float radiusPerStep = radius / steps;

            float angle = 0f;
            float tempRadius = 0f;
            Vector3 position = Vector3.zero;

            float colorPerStep = 1f / steps;
            Color color = Color.white;

            // We want our final revolution to be max radius. So, increase the steps
            steps = steps + (360f / degreesPerStep) - 1f;

            // Start at the center and spiral out
            int lCount = 0;
            for (lCount = 0; lCount < steps; lCount++)
            {
                position.x = tempRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
                position.y = tempRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
                position.z = distance;

                //if (debug)
                //{
                //    Graphics.GraphicsManager.DrawLine(rootTransform.position, rootTransform.TransformPoint(position), (lCount == 0 ? Color.red : color));
                //}

                RaycastHit tempHitInfo;
                Vector3 direction = (rootTransform.TransformPoint(position) - rootTransform.position).normalized;
                if (SafeRaycast(rootTransform.position, direction, out tempHitInfo, distance, layerMask, ignore, ignoreList, ignoreTriggers, debug))
                {
                    // Grab the gameobject this collider belongs to
                    UnityEngine.GameObject gameObject = tempHitInfo.collider.gameObject;

                    // Don't count the ignore
                    if (gameObject.transform == rootTransform)
                    {
                        continue;
                    }

                    if (tempHitInfo.collider is TerrainCollider)
                    {
                        continue;
                    }

                    // Determine if the object has the appropriate tag
                    if (!string.IsNullOrEmpty(tag))
                    {
                        if (!gameObject.CompareTag(tag))
                        {
                            continue;
                        }
                    }

                    // We can do a catch-all if a combatant isn't required
                    hitInfo = tempHitInfo;

                    return true;
                }

                // Increment the spiral
                angle += degreesPerStep;
                tempRadius = Mathf.Min(tempRadius + radiusPerStep, radius);

                if (debug)
                {
                    color.r = color.r - colorPerStep;
                    color.g = color.g - colorPerStep;
                }
            }

            // No hit found
            return false;
        }

        /// <summary>
        /// Sends raycasts out in a circular pattern around the start position.
        /// </summary>
        /// <param name="rayStart">Point that the ray starts from</param>
        /// <param name="rayDirection">Direction that the ray starts from</param>
        /// <param name="hitInfo">Object that is hit</param>
        /// <param name="distance">Final distance of the cast</param>
        /// <param name="degreesPerStep">Degrees of spacing between each casting step</param>
        /// <param name="layerMask">Layer mask to determine what we'll hit</param>
        /// <param name="tag">Tag the hit object must have</param>
        /// <param name="ignore">Single transform we'll test if we should ignore</param>
        /// <param name="ignoreList">List of transforms we should ignore collisions with</param>
        /// <param name="ignoreTriggers">Determines if we ignore triggers</param>
        /// <param name="debug">Determines if we draw debug info</param>
        /// <returns></returns>
        public static bool SafeCircularCast(Vector3 rayStart, Vector3 rayDirection, Vector3 rayUp, out RaycastHit hitInfo, float distance = 1000f, float degreesPerStep = 30f, int layerMask = -1, string tag = null, UnityEngine.Transform ignore = null, List<UnityEngine.Transform> ignoreList = null, bool ignoreTriggers = true, bool debug = false)
        {
            for (float angle = 0f; angle <= 360f; angle += degreesPerStep)
            {
                RaycastHit tempHitInfo;
                Vector3 direction = Quaternion.AngleAxis(angle, rayUp) * rayDirection;

                //if (debug)
                //{
                //    Graphics.GraphicsManager.DrawLine(rayStart, rayStart + (direction * distance), Color.cyan, null, 5f);
                //}

                if (SafeRaycast(rayStart, direction, out tempHitInfo, distance, layerMask, ignore, ignoreList, ignoreTriggers, debug))
                {
                    // Grab the gameobject this collider belongs to
                    UnityEngine.GameObject gameObject = tempHitInfo.collider.gameObject;

                    // Determine if the object has the appropriate tag
                    if (!string.IsNullOrEmpty(tag))
                    {
                        if (!gameObject.CompareTag(tag)) { continue; }
                    }

                    // We can do a catch-all if a combatant isn't required
                    hitInfo = tempHitInfo;
                    return true;
                }
            }

            hitInfo = RaycastHandler.EmptyHitInfo;
            return false;
        }

        /// <summary>
        /// This function will help to find a forward edge. 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="maxDistance"></param>
        /// <param name="maxHeight"></param>
        /// <param name="collisionLayers"></param>
        /// <param name="edgeHitInfo"></param>
        /// <returns></returns>
        public static bool GetForwardEdge(UnityEngine.Transform transform, float maxDistance, float maxHeight, int collisionLayers, out RaycastHit edgeHitInfo)
        {
            edgeHitInfo = RaycastHandler.EmptyHitInfo;

            // Shoot above the expected height to make sure that it's open. We don't want to hit anything
            Vector3 rayStart = transform.position + (transform.up * (maxHeight + 0.001f));
            Vector3 rayDirection = transform.forward;
            float rayDistance = maxDistance * 1.5f;

            if (SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform))
            {
                return false;
            }

            // Shoot down to see if we hit a ledge. We want to hit the top of the ledge.
            rayStart = rayStart + (transform.forward * maxDistance);
            rayDirection = -transform.up;
            rayDistance = maxHeight;

            if (!SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform))
            {
                return false;
            }

            // This is the height of our edge
            float edgeHeight = (maxHeight + 0.001f) - edgeHitInfo.distance;

            // Shoot a ray forward to find the actual edge. We want to hit the front of the ledge.
            rayStart = transform.position + (transform.up * (edgeHeight - 0.001f));
            rayDirection = transform.forward;
            rayDistance = maxDistance;

            if (!SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform))
            {
                return false;
            }

#if UNITY_EDITOR || OOTII_DEBUG
            //Utilities.Debug.DebugDraw.DrawSphereMesh(edgeHitInfo.point, 0.02f, Color.red, 1f);
#endif

            // If we get here, there was a valid hit
            return true;
        }

        /// <summary>
        /// This function will help to find a forward edge. 
        /// </summary>
        /// <returns></returns>
        public static bool GetForwardEdge(UnityEngine.Transform transform, Vector3 position, float minHeight, float maxHeight, float maxDepth, int collisionLayers, out RaycastHit edgeHitInfo)
        {
            edgeHitInfo = RaycastHandler.EmptyHitInfo;

            // Shoot above the expected height to make sure that it's open. We don't want to hit anything
            Vector3 rayStart = position + (transform.up * (maxHeight + 0.001f));
            Vector3 rayDirection = transform.forward;
            float rayDistance = maxDepth;

            if (SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform, null, false, true))
            {
                return false;
            }

            // Shoot down to see if we hit a ledge. We want to hit the top of the ledge.
            rayStart = rayStart + (transform.forward * maxDepth);
            rayDirection = -transform.up;
            rayDistance = maxHeight - minHeight;

            if (!SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform, null, false, true))
            {
                return false;
            }

            // This is the height of our edge
            float edgeHeight = (maxHeight + 0.001f) - edgeHitInfo.distance;

            // Shoot a ray forward to find the actual edge. We want to hit the front of the ledge.
            rayStart = position + (transform.up * (edgeHeight - 0.001f));
            rayDirection = transform.forward;
            rayDistance = maxDepth;

            if (!SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform, null, false, true))
            {
                return false;
            }

#if UNITY_EDITOR || OOTII_DEBUG
            //Utilities.Debug.DebugDraw.DrawSphereMesh(edgeHitInfo.point, 0.02f, Color.red, 1f);
#endif

            // If we get here, there was a valid hit
            return true;
        }

        /// <summary>
        /// This function will help to find a forward edge. 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="maxDistance"></param>
        /// <param name="maxHeight"></param>
        /// <param name="collisionLayers"></param>
        /// <param name="edgeHitInfo"></param>
        /// <returns></returns>
        public static bool GetForwardEdge(UnityEngine.Transform transform, float maxDistance, float maxHeight, float minHeight, int collisionLayers, out RaycastHit edgeHitInfo)
        {
            edgeHitInfo = RaycastHandler.EmptyHitInfo;

            // Shoot above the expected min height to make sure that it's blocked. We want to hit something
            Vector3 rayStart = transform.position + (transform.up * (minHeight + 0.001f));
            Vector3 rayDirection = transform.forward;
            float rayDistance = maxDistance;

            if (!SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform))
            {
                return false;
            }

            float lHitDepth = edgeHitInfo.distance;

            // Shoot above the expected height to make sure that it's open. We don't want to hit anything
            rayStart = transform.position + (transform.up * (maxHeight + 0.001f));
            rayDirection = transform.forward;
            rayDistance = maxDistance;

            if (SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform))
            {
                // If there is no ledge, we need to stop
                if (edgeHitInfo.distance < lHitDepth + 0.1f)
                {
                    return false;
                }
            }

            // Shoot down to see if we hit a ledge. We want to hit the top of the ledge.
            rayStart = rayStart + (transform.forward * (lHitDepth + 0.001f));
            rayDirection = -transform.up;
            rayDistance = maxHeight;

            if (!SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform))
            {
                return false;
            }

            // This is the height of our edge
            float edgeHeight = (maxHeight + 0.001f) - edgeHitInfo.distance;

            // Shoot a ray forward to find the actual edge. We want to hit the front of the ledge.
            rayStart = transform.position + (transform.up * (edgeHeight - 0.001f));
            rayDirection = transform.forward;
            rayDistance = maxDistance;

            if (!SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform))
            {
                return false;
            }

#if UNITY_EDITOR || OOTII_DEBUG
            //Utilities.Debug.DebugDraw.DrawSphereMesh(edgeHitInfo.point, 0.02f, Color.red, 1f);
#endif

            // If we get here, there was a valid hit
            return true;
        }

        /// <summary>
        /// This function will help to find a forward edge. 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="minHeight"></param>
        /// <param name="maxHeight"></param>
        /// <param name="collisionLayers"></param>
        /// <param name="edgeHitInfo"></param>
        /// <returns></returns>
        public static bool GetForwardEdge2(UnityEngine.Transform transform, float minHeight, float maxHeight, float edgeDepth, float maxDepth, int collisionLayers, out RaycastHit edgeHitInfo, bool debug = false)
        {
            return GetForwardEdge2(transform, transform.position, transform.forward, transform.up, minHeight, maxHeight, edgeDepth, maxDepth, collisionLayers, out edgeHitInfo, debug);
        }

        /// <summary>
        /// This function will help to find a forward edge. 
        /// </summary>
        /// <returns></returns>
        public static bool GetForwardEdge2(UnityEngine.Transform transform, Vector3 position, float minHeight, float maxHeight, float edgeDepth, float maxDepth, int collisionLayers, out RaycastHit edgeHitInfo, bool debug = false)
        {
            return GetForwardEdge2(transform, position, transform.forward, transform.up, minHeight, maxHeight, edgeDepth, maxDepth, collisionLayers, out edgeHitInfo, debug);
        }

        /// <summary>
        /// This function will help to find a forward edge. 
        /// </summary>
        /// <param name="edgeDepth">This is the step distance that we use when having vertical rays step forward horizontally for an edge. We do this if our initial horizontal rays don't hit anything.</param>
        /// <returns></returns>
        public static bool GetForwardEdge2(UnityEngine.Transform transform, Vector3 position, Vector3 forward, Vector3 up, float minHeight, float maxHeight, float edgeDepth, float maxDepth, int collisionLayers, out RaycastHit edgeHitInfo, bool debug = false)
        {
            edgeHitInfo = RaycastHandler.EmptyHitInfo;

            float edgeHeight = 0f;
            float lEdgeDepth = float.MaxValue;
            float lAboveDepth = float.MaxValue;

            Vector3 rayStart = position + (up * (maxHeight - 0.001f));
            Vector3 rayDirection = forward;
            float rayDistance = maxDepth;

#if UNITY_EDITOR || OOTII_DEBUG
            if (debug)
            {
                Vector3 bottomRayStart = position + (up * (minHeight + 0.001f));
                Debug.DrawLine(bottomRayStart, bottomRayStart + (rayDirection * rayDistance), Color.blue, 0.02f);
            }
#endif

            // Shoot just below the expected max height to get the depth above the edge (if we hit anything)
            if (SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform, null, false, debug))
            {
                lEdgeDepth = edgeHitInfo.distance;
                lAboveDepth = lEdgeDepth;
            }
            // Shoot one more ray forward to see if we can get the depth and avoid having to step. This ray is halfway
            // between the min and max heights.
            else
            {
                rayStart = position + (up * (minHeight + ((maxHeight - minHeight) * 0.5f)));
                if (SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform, null, false, debug))
                {
                    lEdgeDepth = edgeHitInfo.distance;
                }
            }

            // Since have a depth hit, find the height
            if (lEdgeDepth < float.MaxValue)
            {
                // Shoot down to see if we hit a ledge. We want to hit the top of the ledge.
                rayStart = position + (forward * (lEdgeDepth + 0.001f)) + (up * (maxHeight + 0.001f));
                rayDirection = -up;
                rayDistance = maxHeight - minHeight;

                if (!SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform, null, debug))
                {
                    return false;
                }

                // This is the height of our edge
                edgeHeight = maxHeight - (edgeHitInfo.distance + 0.001f);
            }
            // Since we didn't have a depth hit, we'll step forward until we get a hit going down. We do this in case we're dealing with a 
            // think wall we're trying to get up.
            else
            {
                rayDirection = -up;
                rayDistance = maxHeight - minHeight;

                float lMinDepth = (edgeDepth > 0f ? edgeDepth : 0.05f);
                for (float lDepth = lMinDepth; lDepth <= maxDepth; lDepth += (lMinDepth * 0.5f))
                {
                    rayStart = position + (forward * lDepth) + (up * (maxHeight + 0.001f));
                    if (SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform, null, false, debug))
                    {
                        edgeHeight = maxHeight - (edgeHitInfo.distance + 0.001f);
                        break;
                    }
                }
            }

            // If we don't have an edge height, we don't have an edge
            if (edgeHeight == 0f) { return false; }

            // Shoot a ray forward to find the actual edge. We want to hit the front of the ledge.
            rayStart = position + (up * edgeHeight);
            rayDirection = forward;
            rayDistance = maxDepth;

            if (!SafeRaycast(rayStart, rayDirection, out edgeHitInfo, rayDistance, collisionLayers, transform, null, false, debug))
            {
                return false;
            }

            // If the edge isn't deep enough, stop.
            if (lAboveDepth - edgeHitInfo.distance < edgeDepth)
            {
                return false;
            }

#if UNITY_EDITOR || OOTII_DEBUG
            //Utilities.Debug.DebugDraw.DrawSphereMesh(edgeHitInfo.point, 0.02f, Color.red, 1f);
#endif

            // If we get here, there was a valid hit
            return true;
        }

        /// <summary>
        /// Insertion sort for an array of RaycastHit items. Insertion sort works
        /// great for small lists.
        /// </summary>
        /// <param name="hitArray">Array to sort</param>
        /// <param name="count">Item count to sort</param>
        public static void Sort(RaycastHit[] hitArray, int count)
        {
            if (hitArray == null)
            {
                return;
            }

            if (hitArray.Length <= 1)
            {
                return;
            }

            if (count > hitArray.Length)
            {
                count = hitArray.Length;
            }

            int savedIndex = 0;
            RaycastHit temp;

            for (int lIndex = 1; lIndex < count; lIndex++)
            {
                savedIndex = lIndex;
                temp = hitArray[lIndex];

                while ((savedIndex > 0) && (hitArray[savedIndex - 1].distance > temp.distance))
                {
                    hitArray[savedIndex] = hitArray[savedIndex - 1];
                    savedIndex = savedIndex - 1;
                }

                hitArray[savedIndex] = temp;
            }
        }

        /// <summary>
        /// Determines if the "descendant" transform is a child (or grand child)
        /// of the "parent" transform.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="descendant"></param>
        /// <returns></returns>
        private static bool IsDescendant(UnityEngine.Transform parent, UnityEngine.Transform descendant)
        {
            if (parent == null)
            {
                return false;
            }

            UnityEngine.Transform lDescendantParent = descendant;
            while (lDescendantParent != null)
            {
                if (lDescendantParent == parent)
                {
                    return true;
                }
                lDescendantParent = lDescendantParent.parent;
            }

            return false;
        }
    }

    /// <summary>
    /// Comparerer for distance
    /// </summary>
    public class RaycastHitDistanceComparer : IComparer
    {
        int IComparer.Compare(object rCompare1, object rCompare2)
        {
            RaycastHit lCompare1 = (RaycastHit)rCompare1;
            RaycastHit lCompare2 = (RaycastHit)rCompare2;

            if (lCompare1.distance > lCompare2.distance)
            {
                return 1;
            }

            if (lCompare1.distance < lCompare2.distance)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// Comparerer for distance (furthest to closest)
    /// </summary>
    public class RaycastHitInvDistanceComparer : IComparer
    {
        int IComparer.Compare(object rCompare1, object rCompare2)
        {
            RaycastHit lCompare1 = (RaycastHit)rCompare2;
            RaycastHit lCompare2 = (RaycastHit)rCompare1;

            if (lCompare1.distance > lCompare2.distance)
            {
                return 1;
            }

            if (lCompare1.distance < lCompare2.distance)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}