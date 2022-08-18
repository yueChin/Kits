using UnityEngine;

namespace Kits.ClientKit.Handlers.Camera
{
    public static class CameraHandler
    {

        /// <summary>
        /// 将世界尺寸转化为屏幕尺寸
        /// </summary>
        public static float WorldToScreenSize(this UnityEngine.Camera camera, float worldSize, float clipPlane)
        {
            if (camera.orthographic)
            {
                return worldSize * camera.pixelHeight * 0.5f / camera.orthographicSize;
            }
            else
            {
                return worldSize * camera.pixelHeight * 0.5f / (clipPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
            }
        }


        /// <summary>
        /// 将世界平面转化为相机裁剪面
        /// </summary>
        public static Vector4 GetClipPlane(this UnityEngine.Camera camera, Vector3 point, Vector3 normal)
        {
            Matrix4x4 wtoc = camera.worldToCameraMatrix;
            point = wtoc.MultiplyPoint(point);
            normal = wtoc.MultiplyVector(normal).normalized;

            return new Vector4(normal.x, normal.y, normal.z, -Vector3.Dot(point, normal));
        }

        /// <summary>
        /// 计算 ZBufferParams, 可用于 compute shader 
        /// </summary>
        public static Vector4 GetZBufferParams(this UnityEngine.Camera camera)
        {
            double f = camera.farClipPlane;
            double n = camera.nearClipPlane;

            double rn = 1f / n;
            double rf = 1f / f;
            double fpn = f / n;

            return SystemInfo.usesReversedZBuffer
                ? new Vector4((float)(fpn - 1.0), 1f, (float)(rn - rf), (float)rf)
                : new Vector4((float)(1.0 - fpn), (float)fpn, (float)(rf - rn), (float)rn);
        }
    }
}