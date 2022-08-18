using UnityEngine;

namespace Kits.ClientKit.Handlers.Transform
{
    public static class RectTransformHandler
    {

        /// <summary>
        /// Reset a RectTransform's anchor and local position, rotation, and scale. This is usually required when
        /// adding UI elements as children of a RectTransform at runtime.
        /// </summary>
        /// <param name="rTransform"></param>
        public static void ResetRectTransform(this RectTransform rectTransform)
        {
            if (rectTransform == null)
            {
                return;
            }

            rectTransform.localPosition = Vector3.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// ?? UI ????? RectTransform ??
        /// </summary>
        public static RectTransform RectTransform(this Component target)
        {
            return target.transform as RectTransform;
        }


        /// <summary>
        /// ?? UI ????? RectTransform ??
        /// </summary>
        public static RectTransform RectTransform(this UnityEngine.GameObject target)
        {
            return target.transform as RectTransform;
        }

        public static Rect GetWorldRect(this RectTransform rectTransform)
        {
            Rect rect = rectTransform.rect;
            rect.min = rectTransform.TransformPoint(rect.min);
            rect.max = rectTransform.TransformPoint(rect.max);
            return rect;
        }

        public static void SetAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            Vector2 v2 = rectTransform.anchoredPosition;
            v2.x = x;
            rectTransform.anchoredPosition = v2;
        }


        public static void SetAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            Vector2 v2 = rectTransform.anchoredPosition;
            v2.y = y;
            rectTransform.anchoredPosition = v2;
        }


        public static void SetAnchoredPositionZ(this RectTransform rectTransform, float z)
        {
            Vector3 v3 = rectTransform.anchoredPosition3D;
            v3.z = z;
            rectTransform.anchoredPosition3D = v3;
        }


        public static void SetAnchoredPositionXY(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.anchoredPosition = new Vector2(x, y);
        }


        public static void SetAnchoredPositionXY(this RectTransform rectTransform, float value)
        {
            rectTransform.anchoredPosition = new Vector2(value, value);
        }


        public static void SetAnchoredPosition3D(this RectTransform rectTransform, float value)
        {
            rectTransform.anchoredPosition3D = new Vector3(value, value, value);
        }


        public static void SetRelativeAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            Vector2 v2 = rectTransform.anchoredPosition;
            v2.x += x;
            rectTransform.anchoredPosition = v2;
        }


        public static void SetRelativeAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            Vector2 v2 = rectTransform.anchoredPosition;
            v2.y += y;
            rectTransform.anchoredPosition = v2;
        }


        public static void SetRelativeAnchoredPositionZ(this RectTransform rectTransform, float z)
        {
            Vector3 v3 = rectTransform.anchoredPosition3D;
            v3.z += z;
            rectTransform.anchoredPosition3D = v3;
        }


        public static void SetRelativeAnchoredPositionXY(this RectTransform rectTransform, float x, float y)
        {
            Vector2 v2 = rectTransform.anchoredPosition;
            v2.x += x;
            v2.y += y;
            rectTransform.anchoredPosition = v2;
        }


        public static void SetRelativeAnchoredPositionXY(this RectTransform rectTransform, in Vector2 v2)
        {
            Vector2 pos = rectTransform.anchoredPosition;
            pos.x += v2.x;
            pos.y += v2.y;
            rectTransform.anchoredPosition = pos;
        }


        public static void SetRelativeAnchoredPositionXY(this RectTransform rectTransform, float value)
        {
            Vector2 v2 = rectTransform.anchoredPosition;
            v2.x += value;
            v2.y += value;
            rectTransform.anchoredPosition = v2;
        }


        public static void SetRelativeAnchoredPositionXY(this RectTransform rectTransform, RectTransform rectTransform2)
        {
            Vector2 pos = rectTransform.anchoredPosition;
            Vector2 v2 = rectTransform2.anchoredPosition;
            pos.x += v2.x;
            pos.y += v2.y;
            rectTransform.anchoredPosition = pos;
        }


        public static void SetRelativeAnchoredPosition3D(this RectTransform rectTransform, float value)
        {
            rectTransform.anchoredPosition3D += new Vector3(value, value, value);
        }


        public static void SetOffsetMaxX(this RectTransform rectTransform, float x)
        {
            Vector2 offsetMax = rectTransform.offsetMax;
            offsetMax.x = x;
            rectTransform.offsetMax = offsetMax;
        }


        public static void SetRelativeOffsetMaxX(this RectTransform rectTransform, float x)
        {
            Vector2 offsetMax = rectTransform.offsetMax;
            offsetMax.x += x;
            rectTransform.offsetMax = offsetMax;
        }


        public static void SetOffsetMaxY(this RectTransform rectTransform, float y)
        {
            Vector2 offsetMax = rectTransform.offsetMax;
            offsetMax.y = y;
            rectTransform.offsetMax = offsetMax;
        }


        public static void SetRelativeOffsetMaxY(this RectTransform rectTransform, float y)
        {
            Vector2 offsetMax = rectTransform.offsetMax;
            offsetMax.y += y;
            rectTransform.offsetMax = offsetMax;
        }


        public static void SetRelativeOffsetMax(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.offsetMax += new Vector2(x, y);
        }


        public static void SetOffsetMinX(this RectTransform rectTransform, float x)
        {
            Vector2 offsetMin = rectTransform.offsetMin;
            offsetMin.x = x;
            rectTransform.offsetMin = offsetMin;
        }


        public static void SetRelativeOffsetMinX(this RectTransform rectTransform, float x)
        {
            Vector2 offsetMin = rectTransform.offsetMin;
            offsetMin.x += x;
            rectTransform.offsetMin = offsetMin;
        }


        public static void SetOffsetMinY(this RectTransform rectTransform, float y)
        {
            Vector2 offsetMin = rectTransform.offsetMin;
            offsetMin.y = y;
            rectTransform.offsetMin = offsetMin;
        }


        public static void SetSetRelativeOffsetMinY(this RectTransform rectTransform, float y)
        {
            Vector2 offsetMin = rectTransform.offsetMin;
            offsetMin.y += y;
            rectTransform.offsetMin = offsetMin;
        }


        public static void SetSetRelativeOffsetMin(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.offsetMin += new Vector2(x, y);
        }
        
        public static void SetSizeDeltaWidth(this RectTransform thisObj, float width)
        {
            Vector2 size = thisObj.sizeDelta;
            size.x = width;
            thisObj.sizeDelta = size;
        }
        public static void SetSizeDeltaHeight(this RectTransform thisObj, float height)
        {
            Vector2 size = thisObj.sizeDelta;
            size.y = height;
            thisObj.sizeDelta = size;
        }
        
        private static readonly Vector3[] s_WorldCornersCache = new Vector3[4];

		public static void SetLeft(RectTransform transform, float left)
		{
			transform.offsetMin = new Vector2(left, transform.offsetMin.y);
		}

		public static float GetLeft(RectTransform transform)
		{
			return transform.offsetMin.x;
		}

		public static void SetRight(RectTransform transform, float right)
		{
			transform.offsetMax = new Vector2(-right, transform.offsetMax.y);
		}

		public static float GetRight(RectTransform transform)
		{
			return -transform.offsetMax.x;
		}

		public static void SetTop(RectTransform transform, float top)
		{
			transform.offsetMax = new Vector2(transform.offsetMax.x, -top);
		}

		public static float GetTop(RectTransform transform)
		{
			return -transform.offsetMax.y;
		}

		public static void SetBottom(RectTransform transform, float bottom)
		{
			transform.offsetMin = new Vector2(transform.offsetMin.x, bottom);
		}

		public static float GetBottom(RectTransform transform)
		{
			return transform.offsetMin.y;
		}

		public static void SetWidth(RectTransform transform, float width)
		{
			transform.sizeDelta = new Vector2(width, transform.sizeDelta.y);
		}

		public static float GetWidth(RectTransform transform)
		{
			return transform.sizeDelta.x;
		}

		public static void SetHeight(RectTransform transform, float height)
		{
			transform.sizeDelta = new Vector2(transform.sizeDelta.x, height);
		}

		public static float GetHeight(RectTransform transform)
		{
			return transform.sizeDelta.y;
		}

		public static void SetX(RectTransform transform, float x)
		{
			transform.anchoredPosition = new Vector2(x, transform.anchoredPosition.y);
		}

		public static float GetX(RectTransform transform)
		{
			return transform.anchoredPosition.x;
		}

		public static void SetY(RectTransform transform, float y)
		{
			transform.anchoredPosition = new Vector2(transform.anchoredPosition.x, y);
		}

		public static float GetY(RectTransform transform)
		{
			return transform.anchoredPosition.y;
		}

		public static Vector3 GetWorldPos(RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(s_WorldCornersCache);
			return s_WorldCornersCache[0] + ((s_WorldCornersCache[2] - s_WorldCornersCache[0]) * 0.5f);
		}

		public static void SetPivotKeepPosition(RectTransform rectTransform, Vector2 pivot)
		{
			Vector3 deltaPosition = rectTransform.pivot - pivot;
			deltaPosition.Scale(rectTransform.rect.size);
			deltaPosition.Scale(rectTransform.localScale);
			deltaPosition = rectTransform.rotation * deltaPosition;

			rectTransform.pivot = pivot;
			rectTransform.localPosition -= deltaPosition; 
		}
    }
}