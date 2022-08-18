using Kits.DevlpKit.Helpers;
using UnityEngine;

namespace Kits.ClientKit.Handlers
{
	public static class MathHandler
	{
		/// <summary>
		/// Return true if the values are approximately equal
		/// </summary>
		/// <param name="a">Parameter A</param>
		/// <param name="b">Parameter B</param>
		/// <param name="threshold">Threshold to test for</param>
		/// <returns>True if approximately equal</returns>
		public static bool ApproximatelyEqual(float a, float b, float threshold)
		{

			if (a == b || Mathf.Abs(a - b) < threshold)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Return true if the values are approximately equal
		/// </summary>
		/// <param name="a">Parameter A</param>
		/// <param name="b">Parameter B</param>
		/// <returns>True if approximately equal</returns>
		public static bool ApproximatelyEqual(float a, float b)
		{
			return ApproximatelyEqual(a, b, float.Epsilon);
		}

		/// <summary>
		/// Calculate the distance between two points
		/// </summary>
		/// <param name="x1">X1</param>
		/// <param name="y1">Y1</param>
		/// <param name="x2">X2</param>
		/// <param name="y2">Y2</param>
		/// <returns></returns>
		public static float Distance(float x1, float y1, float x2, float y2)
		{
			return Mathf.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2)));
		}

		public static bool AlmostZero(Vector3 vec)
		{
			return Mathf.Approximately(vec.x, 0.0f) && Mathf.Approximately(vec.y, 0.0f) && Mathf.Approximately(vec.z, 0.0f);
		}
		
		public static float Clamped01(float value, float min, float max)
		{
			return Mathf.Clamp01((value - min) / (max - min));
		}
		
		public static float Clamped(float value, float min, float max, float outputMin, float outputMax)
		{
			return Mathf.Clamp01((value - min) / (max - min)) * (outputMax - outputMin) + outputMin;
		}
		
		
		public static float AngleBetween(Vector2 a, Vector2 b)
		{
			float ang = Vector2.Angle(a, b);
			Vector3 cross = Vector3.Cross(a, b);

			if (cross.z > 0)
				ang = 360 - ang;

			return MathHelper.DegreesTo180Range(ang);
		}

        public static Vector2 CardinalSpline(
            float t,
            Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3,
            float tension = 0.5f)
        {
            Vector2 a = tension * (p2 - p0);
            Vector2 b = p2 - p1;
            Vector2 c = tension * (p3 - p1) + a - b - b;

            return p1 + t * a - t * t * (a + c - b) + t * t * t * c;
        }

        public static Vector3 CardinalSpline(
            float t,
            Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3,
            float tension = 0.5f)
        {
            Vector3 a = tension * (p2 - p0);
            Vector3 b = p2 - p1;
            Vector3 c = tension * (p3 - p1) + a - b - b;

            return p1 + t * a - t * t * (a + c - b) + t * t * t * c;
        }

        public static float Cross(Vector2 lhs, Vector2 rhs)
        {
            return lhs.x * rhs.y - lhs.y * rhs.x;
        }

        public static Vector3 ProjectOnPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
        {
            float normalSqrMagnitude = planeNormal.sqrMagnitude;
            if (normalSqrMagnitude == 0) return point;
            return Vector3.Dot(planePoint - point, planeNormal) / normalSqrMagnitude * planeNormal + point;
        }

        public static int RayIntersectPlane(Vector3 rayOrigin, Vector3 rayDirection, Vector3 planePoint, Vector3 planeNormal, out Vector3 result)
        {
            float cos = Vector3.Dot(planeNormal, rayDirection);
            float distance = Vector3.Dot(rayOrigin - planePoint, planeNormal);

            if (cos < 0f)
            {
                if (distance >= 0f)
                {
                    result = rayOrigin + distance / cos * rayDirection;
                    return 1;
                }
            }
            else if (cos > 0f)
            {
                if (distance <= 0f)
                {
                    result = rayOrigin - distance / cos * rayDirection;
                    return -1;
                }
            }

            result = rayOrigin;
            return 0;
        }


        public static float ClosestPointOnRayFactor(Vector3 point, Vector3 origin, Vector3 direction)
        {
            float t = direction.sqrMagnitude;
            if (t < MathHelper.OneMillionth)
                return 0f;

            return Mathf.Max(Vector3.Dot(point - origin, direction) / t, 0f);
        }

        public static Vector3 ClosestPointOnRay(Vector3 point, Vector3 origin, Vector3 direction)
        {
            return origin + direction * ClosestPointOnRayFactor(point, origin, direction);
        }


        public static float ClosestPointOnSegmentFactor(Vector2 point, Vector2 start, Vector2 end)
        {
            Vector2 direction = end - start;

            float t = direction.sqrMagnitude;
            if (t < MathHelper.OneMillionth)
                return 0f;

            return Mathf.Clamp01(Vector2.Dot(point - start, direction) / t);
        }


        public static Vector2 ClosestPointOnSegment(Vector2 point, Vector2 start, Vector2 end)
        {
            return start + (end - start) * ClosestPointOnSegmentFactor(point, start, end);
        }

        public static float ClosestPointOnSegmentFactor(Vector3 point, Vector3 start, Vector3 end)
        {
            Vector3 direction = end - start;

            float t = direction.sqrMagnitude;
            if (t < MathHelper.OneMillionth) 
                return 0f;

            return Mathf.Clamp01(Vector3.Dot(point - start, direction) / t);
        }


        public static Vector3 ClosestPointOnSegment(Vector3 point, Vector3 start, Vector3 end)
        {
            return start + (end - start) * ClosestPointOnSegmentFactor(point, start, end);
        }

        public static Vector3 ClosestPointInCircle(Vector3 point, Vector3 center, Vector3 normal, float radius)
        {
            point = ProjectOnPlane(point, center, normal);
            normal = point - center;
            float sqrMagnitude = normal.sqrMagnitude;
            if (sqrMagnitude > radius * radius)
            {
                return radius / Mathf.Sqrt(sqrMagnitude) * normal + center;
            }
            else return point;
        }

        public static Vector3 ClosestPointInSphere(Vector3 point, Vector3 center, float radius)
        {
            Vector3 direction = point - center;
            float sqrMagnitude = direction.sqrMagnitude;
            if (sqrMagnitude > radius * radius)
            {
                return radius / Mathf.Sqrt(sqrMagnitude) * direction + center;
            }
            else return point;
        }

        public static Vector3 ClosestPointInCuboid(Vector3 point, Vector3 cuboidMin, Vector3 cuboidMax)
        {
            point.x = Mathf.Clamp(point.x, cuboidMin.x, cuboidMax.x);
            point.y = Mathf.Clamp(point.y, cuboidMin.y, cuboidMax.y);
            point.z = Mathf.Clamp(point.z, cuboidMin.z, cuboidMax.z);
            return point;
        }


        public static float AngleBetweenVectorAndSector(Vector3 vector, Vector3 sectorNormal, Vector3 sectorDirection, float sectorAngle)
        {
            return Vector3.Angle(
                Vector3.RotateTowards(
                    sectorDirection,
                    Vector3.ProjectOnPlane(vector, sectorNormal),
                    sectorAngle * 0.5f * Mathf.Deg2Rad,
                    0f),
                vector);
        }


        public static void ClosestPointBetweenSegments(
            Vector3 startA, Vector3 endA,
            Vector3 startB, Vector3 endB,
            out Vector3 pointA, out Vector3 pointB)
        {
            Vector3 directionA = endA - startA;
            Vector3 directionB = endB - startB;

            float k0 = Vector3.Dot(directionA, directionB);
            float k1 = directionA.sqrMagnitude;
            float k2 = Vector3.Dot(startA - startB, directionA);
            float k3 = directionB.sqrMagnitude;
            float k4 = Vector3.Dot(startA - startB, directionB);

            float t = k3 * k1 - k0 * k0;
            float a = (k0 * k4 - k3 * k2) / t;
            float b = (k1 * k4 - k0 * k2) / t;

            if (float.IsNaN(a) || float.IsNaN(b))
            {
                pointB = ClosestPointOnSegment(startB, endB, startA);
                pointA = ClosestPointOnSegment(startB, endB, endA);

                if ((pointB - startA).sqrMagnitude < (pointA - endA).sqrMagnitude)
                {
                    pointA = startA;
                }
                else
                {
                    pointB = pointA;
                    pointA = endA;
                }
                return;
            }

            if (a < 0f)
            {
                if (b < 0f)
                {
                    pointA = ClosestPointOnSegment(startA, endA, startB);
                    pointB = ClosestPointOnSegment(startB, endB, startA);

                    if ((pointA - startB).sqrMagnitude < (pointB - startA).sqrMagnitude)
                    {
                        pointB = startB;
                    }
                    else pointA = startA;
                }
                else if (b > 1f)
                {
                    pointA = ClosestPointOnSegment(startA, endA, endB);
                    pointB = ClosestPointOnSegment(startB, endB, startA);

                    if ((pointA - endB).sqrMagnitude < (pointB - startA).sqrMagnitude)
                    {
                        pointB = endB;
                    }
                    else pointA = startA;
                }
                else
                {
                    pointA = startA;
                    pointB = ClosestPointOnSegment(startB, endB, startA);
                }
            }
            else if (a > 1f)
            {
                if (b < 0f)
                {
                    pointA = ClosestPointOnSegment(startA, endA, startB);
                    pointB = ClosestPointOnSegment(startB, endB, endA);

                    if ((pointA - startB).sqrMagnitude < (pointB - endA).sqrMagnitude)
                    {
                        pointB = startB;
                    }
                    else pointA = endA;
                }
                else if (b > 1f)
                {
                    pointA = ClosestPointOnSegment(startA, endA, endB);
                    pointB = ClosestPointOnSegment(startB, endB, endA);

                    if ((pointA - endB).sqrMagnitude < (pointB - endA).sqrMagnitude)
                    {
                        pointB = endB;
                    }
                    else pointA = endA;
                }
                else
                {
                    pointA = endA;
                    pointB = ClosestPointOnSegment(startB, endB, endA);
                }
            }
            else
            {
                if (b < 0f)
                {
                    pointB = startB;
                    pointA = ClosestPointOnSegment(startA, endA, startB);
                }
                else if (b > 1f)
                {
                    pointB = endB;
                    pointA = ClosestPointOnSegment(startA, endA, endB);
                }
                else
                {
                    pointA = startA + a * directionA;
                    pointB = startB + b * directionB;
                }
            }
        }

        public static Vector3 GetPositionOfMatrix(ref Matrix4x4 matrix)
        {
	        return new Vector3(matrix.m03, matrix.m13, matrix.m23);
        }

        public static Quaternion GetRotationOfMatrix(ref Matrix4x4 matrix)
        {
	        return Quaternion.LookRotation(
		        new Vector3(matrix.m02, matrix.m12, matrix.m22),
		        new Vector3(matrix.m01, matrix.m11, matrix.m21)
	        );
        }
  
        public static Vector3 GetScaleOfMatrix(ref Matrix4x4 matrix)
        {
	        return new Vector3(
		        new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude,
		        new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude,
		        new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude
	        );
        }
        
        /// <summary>
        /// ?? Ordered Dithering ??
        /// </summary>
        /// <param name="size"> ??? 2 ????????? 2 </param>
        public static int[,] CreateOrderedDitheringMatrix(int size)
        {
	        if (size <= 1 || !Mathf.IsPowerOfTwo(size))
	        {
		        Debug.LogError("Size of ordered dithering matrix must be larger than 1 and be power of 2. this: " + size);
		        return null;
	        }

	        int inputSize = 1;
	        int outputSize;
	        int[,] input = new int[,] { { 0 } };
	        int[,] output;

	        while (true)
	        {
		        outputSize = inputSize * 2;
		        output = new int[outputSize, outputSize];
		        for (int i=0; i<inputSize; i++)
		        {
			        for (int j=0; j<inputSize; j++)
			        {
				        int value = input[i, j] * 4;
				        output[i, j] = value;
				        output[i + inputSize, j + inputSize] = value + 1;
				        output[i + inputSize, j] = value + 2;
				        output[i, j + inputSize] = value + 3;
			        }
		        }

		        if (outputSize >= size) return output;
		        else
		        {
			        inputSize = outputSize;
			        input = output;
		        }
	        }
        }
        
		public static Vector2 Cross(Vector2 dir)
		{
			return new Vector2(dir.y, -dir.x);
		}

		public static Vector2 Rotate(Vector2 v, float degrees)
		{
			float radians = degrees * Mathf.Deg2Rad;
			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);

			float tx = v.x;
			float ty = v.y;

			return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
		}

		public static Vector2 RotateTowards(Vector2 current, Vector2 target, float maxDegreesDelta)
		{
			float angle = AngleBetween(current, target);

			if (Mathf.Abs(angle) < maxDegreesDelta)
			{
				return target;
			}
			else
			{
				return Rotate(current, angle > 0 ? maxDegreesDelta : -maxDegreesDelta);
			}
		}

		public static Vector3 GetClosestPointToLine(Vector3 start, Vector3 end, Vector3 point)
		{
			Vector3 line = (end - start);
			float len = line.magnitude;
			line.Normalize();

			Vector3 v = point - start;
			float d = Vector3.Dot(v, line);
			d = Mathf.Clamp(d, 0f, len);

			return start + line * d;
		}

		public static float SignedAngleBetween(Vector2 a, Vector2 b)
		{
			float perpDot = (a.x * b.y) - (a.y * b.x);
			return Mathf.Atan2(perpDot, Vector2.Dot(a, b));
		}

		public static Vector2 To2DVector(Vector3 vector)
		{
			return new Vector2(vector.x, vector.z);
		}

		public static Vector2 ClampToUnit(Vector2 vector)
		{
			float lengthSqr = vector.sqrMagnitude;

			if (lengthSqr > 1.0f)
				return (vector / Mathf.Sqrt(lengthSqr));

			return vector;
		}
		
		public static Vector3 GetTranslationFromMatrix(ref Matrix4x4 matrix)
		{
			Vector3 translate;
			translate.x = matrix.m03;
			translate.y = matrix.m13;
			translate.z = matrix.m23;
			return translate;
		}
		
		public static Quaternion GetRotationFromMatrix(ref Matrix4x4 matrix)
		{
			Vector3 forward;
			forward.x = matrix.m02;
			forward.y = matrix.m12;
			forward.z = matrix.m22;

			Vector3 upwards;
			upwards.x = matrix.m01;
			upwards.y = matrix.m11;
			upwards.z = matrix.m21;

			return Quaternion.LookRotation(forward, upwards);
		}
		
		public static Vector3 GetScaleFromMatrix(ref Matrix4x4 matrix)
		{
			Vector3 scale;
			scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
			scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
			scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
			return scale;
		}
		
		public static bool ClosestPointsOnTwoLines(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2, out float closestPointLine1T, out float closestPointLine2T)
		{
			closestPointLine1T = 0.0f;
			closestPointLine2T = 0.0f;

			float a = Vector3.Dot(lineVec1, lineVec1);
			float b = Vector3.Dot(lineVec1, lineVec2);
			float e = Vector3.Dot(lineVec2, lineVec2);

			float d = a * e - b * b;

			//lines are not parallel
			if (d != 0.0f)
			{
				Vector3 r = linePoint1 - linePoint2;
				float c = Vector3.Dot(lineVec1, r);
				float f = Vector3.Dot(lineVec2, r);

				float s = (b * f - c * e) / d;
				float t = (a * f - c * b) / d;

				closestPointLine1T = s;
				closestPointLine2T = t;

				return true;
			}

			else
			{
				return false;
			}
		}

		public static bool IntercetionOfTwoNormalisedLines(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2, out Vector3 intercetion)
		{
			float line1DirDotLine2Dir = Vector3.Dot(lineVec1, lineVec2);

			float d = 1.0f - line1DirDotLine2Dir * line1DirDotLine2Dir;

			//lines are not parallel
			if (d != 0.0f)
			{
				Vector3 r = linePoint1 - linePoint2;
				float c = Vector3.Dot(lineVec1, r);
				float f = Vector3.Dot(lineVec2, r);
				float s = (line1DirDotLine2Dir * f - c) / d;

				intercetion = linePoint1 + lineVec1 * s;

				return true;
			}

			else
			{
				intercetion = Vector3.zero;

				return false;
			}
		}

		public static bool Approximately(float a, float b, float epsilon)
		{
			float absA = Mathf.Abs(a);
			float absB = Mathf.Abs(b);
			float diff = Mathf.Abs(a - b);

			if (a == b)
			{
				// shortcut, handles infinities
				return true;
			}
			else if (a == 0 || b == 0 || diff < float.Epsilon)
			{
				// a or b is zero or both are extremely close to it
				// relative error is less meaningful here
				return diff < (epsilon * float.Epsilon);
			}
			else
			{ 
				// use relative error
				return diff / Mathf.Min((absA + absB), float.Epsilon) < epsilon;
			}
		}

		public static bool Approximately(Vector2 a, Vector2 b, float epsilon)
		{
			return Approximately(a.x, b.x, epsilon) && Approximately(a.y, b.y, epsilon);
		}

		public static bool Approximately(Vector3 a, Vector3 b, float epsilon)
		{
			return Approximately(a.x, b.x, epsilon) && Approximately(a.y, b.y, epsilon) && Approximately(a.z, b.z, epsilon);
		}

		public static bool Is2DPointInsideConvexPoly(Vector2[] polyPoints, Vector2 point)
		{
			if (polyPoints.Length < 3)
				return false;

			float lastLineSign = 0.0f;

			for (int i = 0; i < polyPoints.Length; i++)
			{
				//Work out which side of the line the point is, right (positive) or left (negative)
				// (y - y0) (x1 - x0) - (x - x0) (y1 - y0)
				int j = (i + 1) % polyPoints.Length;

				float lineSide = ((point.y - polyPoints[i].y) * (polyPoints[j].x - polyPoints[i].x)) - ((point.x - polyPoints[i].x) * (polyPoints[j].y - polyPoints[i].y));
				
				//if on a different side to previous lines, not in poly
				if (i > 0 && ((lastLineSign <= 0.0f && lineSide > 0.0f) || (lastLineSign >= 0.0f && lineSide < 0.0f)))
				{
					return false;
				}

				lastLineSign = lineSide;
			}

			return true;
		}

		public static bool IsSphereInFrustrum(ref Plane[] frustrumPlanes, ref Vector3 center, float radius, float frustumPadding = 0f)
		{
			for (int i = 0; i < frustrumPlanes.Length; i++)
			{
				Vector3 normal = frustrumPlanes[i].normal;
				float distance = frustrumPlanes[i].distance;

				float dist = normal.x * center.x + normal.y * center.y + normal.z * center.z + distance;

				if (dist < -radius - frustumPadding)
				{
					return false;
				}
			}

			return true;
		}

		public static Vector3 GetPosition(ref Matrix4x4 matrix)
		{
			return new Vector3(matrix.m03, matrix.m13, matrix.m23);
		}

		public static void SetPosition(ref Matrix4x4 matrix, Vector3 position)
		{
			matrix.m03 = position.x;
			matrix.m13 = position.y;
			matrix.m23 = position.z;
		}

		public static Vector3 GetForward(ref Matrix4x4 matrix)
		{
			return matrix.GetColumn(2);
		}

		public static Vector3 GetUp(ref Matrix4x4 matrix)
		{
			return matrix.GetColumn(1);
		}

		public static Vector2 ClampToRect(Vector2 value, Rect rect)
		{
			value.x = Mathf.Clamp(value.x, rect.xMin, rect.xMax);
			value.y = Mathf.Clamp(value.y, rect.yMin, rect.yMax);
			return value;
		}
	}
}