using System;
using System.Text;
using Kits.DevlpKit.Helpers;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Transform
{
    public static class TransformHandler
    {

        /// <summary>
        /// ?? Transform ? localPosition, localRotation ? localScale
        /// </summary>
        public static void ResetLocal(this UnityEngine.Transform transform)
        {
            if (transform == null)
            {
                return;
            }

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }


        /// <summary>
        /// ?? Transform ?????????, ????????????????
        /// </summary>
        /// <param name="root"> ??????? Transform ?? </param>
        /// <param name="operate"> ??????????????? </param>
        /// <param name="depthLimit"> ??????, ???????, 0 ????? root ?????????, ????????????? </param>
        public static void TraverseHierarchy(this UnityEngine.Transform root, Action<UnityEngine.Transform> operate, int depthLimit = -1)
        {
            operate(root);

            if (depthLimit != 0)
            {
                int count = root.childCount;
                for (int i = 0; i < count; i++)
                {
                    TraverseHierarchy(root.GetChild(i), operate, depthLimit - 1);
                }
            }
        }


        /// <summary>
        /// ???? Transform ??????????, ????????????????
        /// </summary>
        /// <param name="root"> ???? Transform ?? </param>
        /// <param name="operate"> ??????????????? </param>
        /// <param name="depthLimit"> ??????, ???????, 0 ????? root ?????????, ????????????? </param>
        public static void InverseTraverseHierarchy(this UnityEngine.Transform root, Action<UnityEngine.Transform> operate, int depthLimit = -1)
        {
            if (depthLimit != 0)
            {
                int count = root.childCount;
                for (int i = 0; i < count; i++)
                {
                    InverseTraverseHierarchy(root.GetChild(i), operate, depthLimit - 1);
                }
            }

            operate(root);
        }


        /// <summary>
        /// ?? Transform ?????????, ??????????????, ?????????????
        /// </summary>
        /// <param name="root"> ??????? Transform ?? </param>
        /// <param name="match"> ????????????? </param>
        /// <param name="depthLimit"> ??????, ???????, 0 ????? root ?????????, ????????????? </param>
        /// <returns> ?????????????; ???? null </returns>
        public static UnityEngine.Transform SearchHierarchy(this UnityEngine.Transform root, Predicate<UnityEngine.Transform> match, int depthLimit = -1)
        {
            if (match(root)) return root;
            if (depthLimit == 0) return null;

            int count = root.childCount;
            UnityEngine.Transform result = null;

            for (int i = 0; i < count; i++)
            {
                result = SearchHierarchy(root.GetChild(i), match, depthLimit - 1);
                if (result) break;
            }

            return result;
        }

        public static void SetPositionX(this UnityEngine.Transform transform, float x)
        {
            Vector3 v3 = transform.position;
            v3.x = x;
            transform.position = v3;
        }


        public static void SetPositionY(this UnityEngine.Transform transform, float y)
        {
            Vector3 v3 = transform.position;
            v3.y = y;
            transform.position = v3;
        }


        public static void SetPositionZ(this UnityEngine.Transform transform, float z)
        {
            Vector3 v3 = transform.position;
            v3.z = z;
            transform.position = v3;
        }


        public static void SetPositionXY(this UnityEngine.Transform transform, float x, float y)
        {
            transform.position = new Vector3(x, y, transform.position.z);
        }


        public static void SetPositionXY(this UnityEngine.Transform transform, in Vector2 v2)
        {
            transform.position = new Vector3(v2.x, v2.y, transform.position.z);
        }


        public static void SetPositionXY(this UnityEngine.Transform transform, float value)
        {
            transform.position = new Vector3(value, value, transform.position.z);
        }


        public static void SetPositionXY(this UnityEngine.Transform transform, UnityEngine.Transform transform2)
        {
            Vector3 v3 = transform2.position;
            v3.z = transform.position.z;
            transform.position = v3;
        }


        public static void SetPosition(this UnityEngine.Transform transform, float value)
        {
            transform.position = new Vector3(value, value, value);
        }


        public static void SetRelativePositionX(this UnityEngine.Transform transform, float x)
        {
            Vector3 v3 = transform.position;
            v3.x += x;
            transform.position = v3;
        }


        public static void SetRelativePositionY(this UnityEngine.Transform transform, float y)
        {
            Vector3 v3 = transform.position;
            v3.y += y;
            transform.position = v3;
        }


        public static void SetRelativePositionZ(this UnityEngine.Transform transform, float z)
        {
            Vector3 v3 = transform.position;
            v3.z += z;
            transform.position = v3;
        }


        public static void SetRelativePositionXY(this UnityEngine.Transform transform, float x, float y)
        {
            Vector3 v3 = transform.position;
            v3.x += x;
            v3.y += y;
            transform.position = v3;
        }


        public static void SetRelativePositionXY(this UnityEngine.Transform transform, in Vector2 v2)
        {
            Vector3 v3 = transform.position;
            v3.x += v2.x;
            v3.y += v2.y;
            transform.position = v3;
        }


        public static void SetRelativePositionXY(this UnityEngine.Transform transform, float value)
        {
            Vector3 v3 = transform.position;
            v3.x += value;
            v3.y += value;
            transform.position = v3;
        }


        public static void SetRelativePositionXY(this UnityEngine.Transform transform, UnityEngine.Transform transform2)
        {
            Vector3 v3 = transform.position;
            Vector3 v23 = transform2.position;
            v3.x += v23.x;
            v3.y += v23.y;
            transform.position = v3;
        }


        public static void SetRelativePosition(this UnityEngine.Transform transform, float value)
        {
            transform.position += new Vector3(value, value, value);
        }


        public static void SetLocalPositionX(this UnityEngine.Transform transform, float x)
        {
            Vector3 v3 = transform.localPosition;
            v3.x = x;
            transform.localPosition = v3;
        }


        public static void SetLocalPositionY(this UnityEngine.Transform transform, float y)
        {
            Vector3 v3 = transform.localPosition;
            v3.y = y;
            transform.localPosition = v3;
        }


        public static void SetLocalPositionZ(this UnityEngine.Transform transform, float z)
        {
            Vector3 v3 = transform.localPosition;
            v3.z = z;
            transform.localPosition = v3;
        }


        public static void SetLocalPositionXY(this UnityEngine.Transform transform, float x, float y)
        {
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
        }


        public static void SetLocalPositionXY(this UnityEngine.Transform transform, in Vector2 v2)
        {
            transform.localPosition = new Vector3(v2.x, v2.y, transform.localPosition.z);
        }


        public static void SetLocalPositionXY(this UnityEngine.Transform transform, float value)
        {
            transform.localPosition = new Vector3(value, value, transform.localPosition.z);
        }


        public static void SetLocalPositionXY(this UnityEngine.Transform transform, UnityEngine.Transform transform2)
        {
            Vector3 v3 = transform2.localPosition;
            v3.z = transform.localPosition.z;
            transform.localPosition = v3;
        }


        public static void SetLocalPosition(this UnityEngine.Transform transform, float value)
        {
            transform.localPosition = new Vector3(value, value, value);
        }


        public static void SetRelativeLocalPositionX(this UnityEngine.Transform transform, float x)
        {
            Vector3 v3 = transform.localPosition;
            v3.x += x;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionY(this UnityEngine.Transform transform, float y)
        {
            Vector3 v3 = transform.localPosition;
            v3.y += y;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionZ(this UnityEngine.Transform transform, float z)
        {
            Vector3 v3 = transform.localPosition;
            v3.z += z;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionXY(this UnityEngine.Transform transform, float x, float y)
        {
            Vector3 v3 = transform.localPosition;
            v3.x += x;
            v3.y += y;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionXY(this UnityEngine.Transform transform, in Vector2 v2)
        {
            Vector3 v3 = transform.localPosition;
            v3.x += v2.x;
            v3.y += v2.y;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionXY(this UnityEngine.Transform transform, float value)
        {
            Vector3 v3 = transform.localPosition;
            v3.x += value;
            v3.y += value;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPositionXY(this UnityEngine.Transform transform, UnityEngine.Transform transform2)
        {
            Vector3 v3 = transform.localPosition;
            Vector3 v23 = transform2.localPosition;
            v3.x += v23.x;
            v3.y += v23.y;
            transform.localPosition = v3;
        }


        public static void SetRelativeLocalPosition(this UnityEngine.Transform transform, float value)
        {
            transform.localPosition += new Vector3(value, value, value);
        }


        public static void SetScaleX(this UnityEngine.Transform transform, float x)
        {
            Vector3 v3 = transform.localScale;
            v3.x = x;
            transform.localScale = v3;
        }


        public static void SetScaleY(this UnityEngine.Transform transform, float y)
        {
            Vector3 v3 = transform.localScale;
            v3.y = y;
            transform.localScale = v3;
        }


        public static void SetScaleZ(this UnityEngine.Transform transform, float z)
        {
            Vector3 v3 = transform.localScale;
            v3.z = z;
            transform.localScale = v3;
        }


        public static void SetScaleXY(this UnityEngine.Transform transform, float x, float y)
        {
            transform.localScale = new Vector3(x, y, transform.localScale.z);
        }


        public static void SetScaleXY(this UnityEngine.Transform transform, in Vector2 v2)
        {
            transform.localScale = new Vector3(v2.x, v2.y, transform.localScale.z);
        }


        public static void SetScaleXY(this UnityEngine.Transform transform, float value)
        {
            transform.localScale = new Vector3(value, value, transform.localScale.z);
        }


        public static void SetScaleXY(this UnityEngine.Transform transform, UnityEngine.Transform transform2)
        {
            Vector3 v3 = transform2.localScale;
            v3.z = transform.localScale.z;
            transform.localScale = v3;
        }


        public static void SetScale(this UnityEngine.Transform transform, float value)
        {
            transform.localScale = new Vector3(value, value, value);
        }


        public static void SetRelativeScaleX(this UnityEngine.Transform transform, float x)
        {
            Vector3 v3 = transform.localScale;
            v3.x += x;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleY(this UnityEngine.Transform transform, float y)
        {
            Vector3 v3 = transform.localScale;
            v3.y += y;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleZ(this UnityEngine.Transform transform, float z)
        {
            Vector3 v3 = transform.localScale;
            v3.z += z;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleXY(this UnityEngine.Transform transform, float x, float y)
        {
            Vector3 v3 = transform.localScale;
            v3.x += x;
            v3.y += y;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleXY(this UnityEngine.Transform transform, in Vector2 v2)
        {
            Vector3 v3 = transform.localScale;
            v3.x += v2.x;
            v3.y += v2.y;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleXY(this UnityEngine.Transform transform, float value)
        {
            Vector3 v3 = transform.localScale;
            v3.x += value;
            v3.y += value;
            transform.localScale = v3;
        }


        public static void SetRelativeScaleXY(this UnityEngine.Transform transform, UnityEngine.Transform transform2)
        {
            Vector3 v3 = transform.localScale;
            Vector3 v23 = transform2.localScale;
            v3.x += v23.x;
            v3.y += v23.y;
            transform.localScale = v3;
        }


        public static void SetRelativeScale(this UnityEngine.Transform transform, float value)
        {
            transform.localScale += new Vector3(value, value, value);
        }


        public static void SetRotationX(this UnityEngine.Transform transform, float x)
        {
            Vector3 v3 = transform.eulerAngles;
            v3.x = x;
            transform.eulerAngles = v3;
        }


        public static void SetRotationY(this UnityEngine.Transform transform, float y)
        {
            Vector3 v3 = transform.eulerAngles;
            v3.y = y;
            transform.eulerAngles = v3;
        }


        public static void SetRotationZ(this UnityEngine.Transform transform, float z)
        {
            Vector3 v3 = transform.eulerAngles;
            v3.z = z;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationX(this UnityEngine.Transform transform, float x)
        {
            Vector3 v3 = transform.eulerAngles;
            v3.x += x;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationY(this UnityEngine.Transform transform, float y)
        {
            Vector3 v3 = transform.eulerAngles;
            v3.y += y;
            transform.eulerAngles = v3;
        }


        public static void SetRelativeRotationZ(this UnityEngine.Transform transform, float z)
        {
            Vector3 v3 = transform.eulerAngles;
            v3.z += z;
            transform.eulerAngles = v3;
        }


        public static void SetLocalRotationX(this UnityEngine.Transform transform, float x)
        {
            Vector3 v3 = transform.localEulerAngles;
            v3.x = x;
            transform.localEulerAngles = v3;
        }


        public static void SetLocalRotationY(this UnityEngine.Transform transform, float y)
        {
            Vector3 v3 = transform.localEulerAngles;
            v3.y = y;
            transform.localEulerAngles = v3;
        }


        public static void SetLocalRotationZ(this UnityEngine.Transform transform, float z)
        {
            Vector3 v3 = transform.localEulerAngles;
            v3.z = z;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationX(this UnityEngine.Transform transform, float x)
        {
            Vector3 v3 = transform.localEulerAngles;
            v3.x += x;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationY(this UnityEngine.Transform transform, float y)
        {
            Vector3 v3 = transform.localEulerAngles;
            v3.y += y;
            transform.localEulerAngles = v3;
        }


        public static void SetRelativeLocalRotationZ(this UnityEngine.Transform transform, float z)
        {
            Vector3 v3 = transform.localEulerAngles;
            v3.z += z;
            transform.localEulerAngles = v3;
        }
        
        public static string GetRelativePath(UnityEngine.Transform parent, UnityEngine.Transform child)
        {
            StringBuilder buf = StringBuilderHelper.GetBuilder();
            UnityEngine.Transform tmp = child;
            while(tmp != null && tmp != parent)
            {
                if (buf.Length > 0)
                    buf.Insert(0, '/');
                buf.Insert(0, tmp.name);
                tmp = tmp.parent;
            }
            return StringBuilderHelper.ReleaseBuilder(buf);
        }
    }
}