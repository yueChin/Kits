using System.Collections.Generic;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Curve
{
    public static class BezierHandler
    {
        #region Vector2

        //获取二阶贝塞尔曲线路径数组
        public static Vector2[] Bezier2Path(this RectTransform rect, Vector2 controlPos, Vector2 endPos, int cnt)
        {
            Vector2[] path = new Vector2[cnt];
            for (int i = 1; i <= cnt; i++)
            {
                float t = i / cnt;
                path[i - 1] = Bezier2(rect.anchoredPosition, controlPos, endPos, t);
            }
            return path;
        }


        //获取二阶贝塞尔曲线路径数组
        public static Vector2[] Bezier2Path(Vector2 startPos, Vector2 controlPos, Vector2 endPos, int cnt)
        {
            Vector2[] path = new Vector2[cnt];
            for (int i = 1; i <= cnt; i++)
            {
                float t = i / cnt;
                path[i - 1] = Bezier2(startPos, controlPos, endPos, t);
            }
            return path;
        }
        // 2阶贝塞尔曲线
        private static Vector2 Bezier2(Vector2 startPos, Vector2 controlPos, Vector2 endPos, float t)
        {
            return (1 - t) * (1 - t) * startPos + 2 * t * (1 - t) * controlPos + t * t * endPos;
        }

        // 3阶贝塞尔曲线
        private static Vector2 Bezier3(Vector2 startPos, Vector2 controlPos1, Vector2 controlPos2, Vector2 endPos, float t)
        {
            float t2 = 1 - t;
            return t2 * t2 * t2 * startPos
                   + 3 * t * t2 * t2 * controlPos1
                   + 3 * t * t * t2 * controlPos2
                   + t * t * t * endPos;
        }

        #endregion

        #region Vector3
        //获取二阶贝塞尔曲线路径数组
        public static Vector3[] Bezier2Path(Vector3 startPos, Vector3 controlPos, Vector3 endPos, int cnt)
        {
            Vector3[] path = new Vector3[cnt];
            for (int i = 1; i <= cnt; i++)
            {
                float t = i / cnt;
                path[i - 1] = Bezier2(startPos, controlPos, endPos, t);
            }
            return path;
        }
        // 2阶贝塞尔曲线
        private static Vector3 Bezier2(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float t)
        {
            return (1 - t) * (1 - t) * startPos + 2 * t * (1 - t) * controlPos + t * t * endPos;
        }

        // 3阶贝塞尔曲线
        private static Vector3 Bezier3(Vector3 startPos, Vector3 controlPos1, Vector3 controlPos2, Vector3 endPos, float t)
        {
            float t2 = 1 - t;
            return t2 * t2 * t2 * startPos
                   + 3 * t * t2 * t2 * controlPos1
                   + 3 * t * t * t2 * controlPos2
                   + t * t * t * endPos;
        }

        public static Vector3[] GetBezierList(IList<Vector3> PiList, int number)
        {
            Vector3[] bezierPath = new Vector3[number];
            for (int i = 1; i <= number; i++)
            {
                float t = i / number;
                Vector3 v3 = GetBezierPoint(PiList, t);
                bezierPath[i - 1] = v3;
            }
            return bezierPath;
        }

        public static Vector3 GetBezierPoint(IList<Vector3> PiList, float t)
        {

            Stack<IList<Vector3>> stack = new Stack<IList<Vector3>>();
            stack.Push(PiList);
            Vector3 v3 = Vector3.zero;
            do
            {
                IList<Vector3> pList = stack.Pop();
                if (pList.Count > 1)
                {
                    for (int i = 0; i < pList.Count - 1; i++)
                    {
                        Vector3 pp = (1 - t) * pList[i] + t * pList[i + 1];
                        pList.Add(pp);
                    }
                    stack.Push(pList);
                }
                else
                {
                    v3 = pList[0];
                }
            } while (stack.Count > 0);

            return v3;
            //if (PiList.Count <= 1)
            //    return PiList[0];
            //IList<Vector3> plist = new List<Vector3>();
            //for (var i = 0; i < PiList.Count - 1; i++)
            //{
            //    Vector3 pp = (1 - t) * PiList[i] + t * PiList[i + 1];
            //    plist.Add(pp);
            //}

            //return GetBezierPoint(plist,t);
        }

        #endregion
    }
}

