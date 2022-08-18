using UnityEngine;

namespace Kits.ClientKit.Handlers.LoopSystems
{
    public static class PointerHandler
    {
        public static bool IsPointInRect(Vector3 mouseAbsPos, RectTransform trans)
        {
            if (trans != null)
            {
                float l_t_x = trans.position.x;
                float l_t_y = trans.position.y;
                float r_b_x = l_t_x + trans.sizeDelta.x;
                float r_b_y = l_t_y - trans.sizeDelta.y;
                if (mouseAbsPos.x >= l_t_x && mouseAbsPos.y <= l_t_y && mouseAbsPos.x <= r_b_x && mouseAbsPos.y >= r_b_y)
                {
                    return true;
                }
            }
            return false;
        }

    }
}