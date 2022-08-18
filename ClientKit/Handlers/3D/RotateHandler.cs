using UnityEngine;

namespace Kits.ClientKit.Handlers._3D
{
    public class RotateHandler
    {
        /// <summary>
        /// Rotate the point around the pivot - used to handle rotation
        /// </summary>
        /// <param name="point">Point to move</param>
        /// <param name="pivot">Pivot</param>
        /// <param name="angle">Angle to pivot</param>
        /// <returns>New location</returns>
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle)
        {
            Vector3 dir = point - pivot;
            dir = Quaternion.Euler(angle) * dir;
            point = dir + pivot;
            return point;
        }

    }
}
