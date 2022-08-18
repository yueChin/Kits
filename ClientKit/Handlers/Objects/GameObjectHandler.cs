using System;
using System.Reflection;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Objects
{
    public static partial class GameObjectHandler
    {
        /// <summary>
        /// 安全获取组件. 如果物体上没有组件则自动添加
        /// </summary>
        public static T TryGetComponent<T>(this UnityEngine.GameObject target) where T : Component
        {
            T component = target.GetComponent<T>();
            if (!component)
            {
                component = target.AddComponent<T>();
            }
            return component;
        }

        public static T TryGetComponent<T>(this UnityEngine.GameObject go, out bool isNew ) where T : Component 
        {
            T ret = go.GetComponent<T>();
            if ( ret == null ) {
                isNew = true;
                ret = go.AddComponent<T>();
            } else {
                isNew = false;
            }
            return ret;
        }
        
        public static Bounds GetBounds(UnityEngine.GameObject obj)
        {
            Vector3 Min = new Vector3(99999, 99999, 99999);
            Vector3 Max = new Vector3(-99999, -99999, -99999);
            MeshRenderer[] renders = obj.GetComponentsInChildren<MeshRenderer>();
            if (renders.Length > 0)
            {
                for (int i = 0; i < renders.Length; i++)
                {
                    if (renders[i].bounds.min.x < Min.x)
                        Min.x = renders[i].bounds.min.x;
                    if (renders[i].bounds.min.y < Min.y)
                        Min.y = renders[i].bounds.min.y;
                    if (renders[i].bounds.min.z < Min.z)
                        Min.z = renders[i].bounds.min.z;

                    if (renders[i].bounds.max.x > Max.x)
                        Max.x = renders[i].bounds.max.x;
                    if (renders[i].bounds.max.y > Max.y)
                        Max.y = renders[i].bounds.max.y;
                    if (renders[i].bounds.max.z > Max.z)
                        Max.z = renders[i].bounds.max.z;
                }
            }
            else
            {
                RectTransform[] rectTrans = obj.GetComponentsInChildren<RectTransform>();
                Vector3[] corner = new Vector3[4];
                for (int i = 0; i < rectTrans.Length; i++)
                {
                    //获取节点的四个角的世界坐标，分别按顺序为左下左上，右上右下
                    rectTrans[i].GetWorldCorners(corner);
                    if (corner[0].x < Min.x)
                        Min.x = corner[0].x;
                    if (corner[0].y < Min.y)
                        Min.y = corner[0].y;
                    if (corner[0].z < Min.z)
                        Min.z = corner[0].z;

                    if (corner[2].x > Max.x)
                        Max.x = corner[2].x;
                    if (corner[2].y > Max.y)
                        Max.y = corner[2].y;
                    if (corner[2].z > Max.z)
                        Max.z = corner[2].z;
                }
            }

            Vector3 center = (Min + Max) / 2;
            Vector3 size = new Vector3(Max.x - Min.x, Max.y - Min.y, Max.z - Min.z);
            return new Bounds(center, size);
        }

        /// <summary>
        /// Parent add child from new GameObject and add T Component
        /// [isWorldStays]: Whether keep world space position, rotation, scale as before.
        /// </summary>
        public static T AddChild<T>(this UnityEngine.GameObject parent, bool isWorldStays = false) where T : Component
        {
            UnityEngine.GameObject child = new UnityEngine.GameObject();
            child.transform.SetParent(parent.transform, isWorldStays);

            return child.AddComponent<T>();
        }

        /// <summary>
        /// Determines if this game object is in a chain where one of its parents
        /// is the specified parent.
        /// </summary>
        /// <param name="rThis">Child game object whose relationship we're testing.</param>
        /// <param name="rParent">Parent game object we want to see if the child belongs to.</param>
        /// <returns>True if transform is a child (or in the child chain) of the parent.</returns>
        public static bool IsChildOf(this UnityEngine.GameObject rThis, UnityEngine.GameObject rParent)
        {
            if (rThis == null) { return false; }
            if (rParent == null) { return false; }

            UnityEngine.Transform lSearchParent = rParent.transform;

            UnityEngine.Transform parent = rThis.transform;
            while (parent != null)
            {
                if (parent == lSearchParent) { return true; }

                parent = parent.parent;
            }

            return false;
        }

        /// <summary>
        /// Looks for the specified component in the heirarchy chain
        /// </summary>
        /// <param name="rThis">GameObject we're starting with</param>
        public static object GetComponentInParents(this UnityEngine.GameObject go, System.Type type)
        {
            if (go == null)
            {
                return null;
            }

            UnityEngine.Transform parent = go.transform;
            while (parent != null)
            {
                Component lComponent = parent.gameObject.GetComponent(type);
                if (lComponent != null)
                {
                    return lComponent;
                }

                parent = parent.parent;
            }

            return null;
        }

        /// <summary>
        /// Looks for the specified component in the heirarchy chain
        /// </summary>
        /// <param name="rThis">GameObject we're starting with</param>
        public static T GetComponentInParents<T>(this UnityEngine.GameObject go) where T : Component
        {
            if (go == null)
            {
                return null;
            }

            UnityEngine.Transform parent = go.transform;
            while (parent != null)
            {
                T lComponent = parent.GetComponent<T>();
                if (lComponent != null)
                {
                    return lComponent;
                }

                parent = parent.parent;
            }

            return null;
        }

        /// <summary>
        /// Creates a copy of a component and returns it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comp"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static T GetCopyOf<T>(this Component rThis, T rOther) where T : Component
        {
            Type lType = rThis.GetType();
            if (lType != rOther.GetType()) return null; // type mis-match

#if !UNITY_EDITOR && (NETFX_CORE || WINDOWS_UWP || UNITY_WP8 || UNITY_WP_8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0)
            BindingFlags lFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
#else
            BindingFlags lFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
#endif

            PropertyInfo[] lPropertyInfos = lType.GetProperties(lFlags);
            foreach (PropertyInfo lPropertyInfo in lPropertyInfos)
            {
                if (lPropertyInfo.CanWrite)
                {
                    try
                    {
                        lPropertyInfo.SetValue(rThis, lPropertyInfo.GetValue(rOther, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }

            FieldInfo[] lFieldInfos = lType.GetFields(lFlags);
            foreach (FieldInfo lFieldInfo in lFieldInfos)
            {
                lFieldInfo.SetValue(rThis, lFieldInfo.GetValue(rOther));
            }

            return rThis as T;
        }
        
#if UNITY_2018_3_OR_NEWER && UNITY_EDITOR
        //Returns whether the gameobject can safely be deleted in regards to the prefab system
        public static bool IsSafePrefabDelete(this UnityEngine.GameObject go)
        {
            bool isPrefab = UnityEditor.PrefabUtility.IsPartOfAnyPrefab(go);
            bool isOveride = UnityEditor.PrefabUtility.IsAddedGameObjectOverride(go);
            return !isPrefab || isOveride;
        }
#endif
    }
}