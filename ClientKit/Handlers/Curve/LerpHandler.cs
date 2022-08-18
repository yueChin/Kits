using System;
using System.Collections;
using Kits.ClientKit.Collections.Math;
using Kits.DevlpKit.Helpers.CurveHelpers;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Curve
{
	public enum InterpolationType
	{
		Linear,
		InQuad,
		OutQuad,
		InOutQuad,
		InCubic,
		OutCubic,
		InOutCubic,
		InSine,
		OutSine,
		InOutSine,
		InOutSineInv,
		InElastic,
		OutElastic,
		InOutElastic,
		InBounce,
		OutBounce,
		InOutBounce,
	}
	
	public static class LerpHandler
	{
		#region Utility Functions
		
		public static Rect Lerp(Rect from, Rect to, float t)
		{
			Rect lerp = from;
			lerp.x = Mathf.Lerp(from.x, to.x, t);
			lerp.y = Mathf.Lerp(from.y, to.y, t);
			lerp.width = Mathf.Lerp(from.width, to.width, t);
			lerp.height = Mathf.Lerp(from.height, to.height, t);
			return lerp;
		}

		public static FloatRange Lerp(FloatRange from, FloatRange to, float t)
		{
			FloatRange lerp = from;
			lerp.Min = Mathf.Lerp(from.Min, to.Min, t);
			lerp.Max = Mathf.Lerp(from.Max, to.Max, t);
			return lerp;
		}
		
		public static Vector2 Lerp(InterpolationType type, Vector2 from, Vector2 to, float t)
		{
			float lerp = Lerp(type, 0.0f, 1.0f, t);
			return Vector2.Lerp(from, to, lerp);
		}

		public static Vector3 Lerp(InterpolationType type, Vector3 from, Vector3 to, float t)
		{
			float lerp = Lerp(type, 0.0f, 1.0f, t);
			return Vector3.Lerp(from, to, lerp);
		}

		public static Quaternion Lerp(InterpolationType type, Quaternion from, Quaternion to, float t)
		{
			float lerp = Lerp(type, 0.0f, 1.0f, t);
			return Quaternion.Slerp(from, to, lerp);
		}

		public static Color Lerp(InterpolationType type, Color from, Color to, float t)
		{
			float lerp = Lerp(type, 0.0f, 1.0f, t);
			return Color.Lerp(from, to, lerp);
		}

		public static Rect Lerp(InterpolationType type, Rect from, Rect to, float t)
		{
			float lerp = Lerp(type, 0.0f, 1.0f, t);
			return Lerp(from, to, lerp);
		}

		public static FloatRange Lerp(InterpolationType type, FloatRange from, FloatRange to, float t)
		{
			float lerp = Lerp(type, 0.0f, 1.0f, t);
			return Lerp(from, to, lerp);
		}

		public static double DampSmoothAngle(double current, double target, ref double currentVelocity, double smoothTime, double maxSpeed)
		{
			double deltaTime = (double)Time.deltaTime;
			return EasingHelper.DampSmoothAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static double DampSmoothAngle(double current, double target, ref double currentVelocity, double smoothTime)
		{
			double deltaTime = (double)Time.deltaTime;
			double maxSpeed = double.PositiveInfinity;
			return EasingHelper.DampSmoothAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}
		
		public static float Damp(float a, float b, float lambda, float deltaTime, InterpolationType type = InterpolationType.Linear)
		{
			return Lerp(type, a, b, 1.0f - Mathf.Exp(-lambda * deltaTime));
		}

		public static double Damp(double a, double b, double lambda, double deltaTime, InterpolationType type = InterpolationType.Linear)
		{
			return Lerp(type, a, b, 1.0d - Math.Exp(-lambda * deltaTime));
		}

		public static Vector2 Damp(Vector2 a, Vector2 b, float lambda, float deltaTime, InterpolationType type = InterpolationType.Linear)
		{
			float lerp = Damp(0.0f, 1.0f, lambda, deltaTime, type);
			return Vector2.Lerp(a, b, lerp);
		}

		public static Vector3 Damp(Vector3 a, Vector3 b, float lambda, float deltaTime, InterpolationType type = InterpolationType.Linear)
		{
			float lerp = Damp(0.0f, 1.0f, lambda, deltaTime, type);
			return Vector3.Lerp(a, b, lerp);
		}

		public static Quaternion Damp(Quaternion a, Quaternion b, float lambda, float deltaTime, InterpolationType type = InterpolationType.Linear)
		{
			float lerp = Damp(0.0f, 1.0f, lambda, deltaTime, type);
			return Quaternion.Slerp(a, b, lerp);
		}
		
		public static float Lerp(InterpolationType type, float start, float end, float t)
		{
			switch (type)
			{
				case InterpolationType.InQuad:
					return LerpInQuad(start, end, t);
				case InterpolationType.OutQuad:
					return LerpOutQuad(start, end, t);
				case InterpolationType.InOutQuad:
					return LerpInOutQuad(start, end, t);
				case InterpolationType.InOutCubic:
					return LerpInOutCubic(start, end, t);
				case InterpolationType.InCubic:
					return LerpInCubic(start, end, t);
				case InterpolationType.OutCubic:
					return LerpOutCubic(start, end, t);
				case InterpolationType.InSine:
					return LerpInSine(start, end, t);
				case InterpolationType.OutSine:
					return LerpOutSine(start, end, t);
				case InterpolationType.InOutSine:
					return LerpInOutSine(start, end, t);
				case InterpolationType.InOutSineInv:
					return LerpInOutSineInv(start, end, t);
				case InterpolationType.InElastic:
					return LerpInElastic(start, end, t);
				case InterpolationType.OutElastic:
					return LerpOutElastic(start, end, t);
				case InterpolationType.InOutElastic:
					return LerpInOutElastic(start, end, t);
				case InterpolationType.InBounce:
					return LerpInBounce(start, end, t);
				case InterpolationType.OutBounce:
					return LerpOutBounce(start, end, t);
				case InterpolationType.InOutBounce:
					return LerpInOutBounce(start, end, t);
				default:
				case InterpolationType.Linear:
					return LerpLinear(start, end, t);
			}
		}

		public static double Lerp(InterpolationType type, double start, double end, double t)
		{
			switch (type)
			{
				case InterpolationType.InQuad:
					return LerpInQuad(start, end, t);
				case InterpolationType.OutQuad:
					return LerpOutQuad(start, end, t);
				case InterpolationType.InOutQuad:
					return LerpInOutQuad(start, end, t);
				case InterpolationType.InOutCubic:
					return LerpInOutCubic(start, end, t);
				case InterpolationType.InCubic:
					return LerpInCubic(start, end, t);
				case InterpolationType.OutCubic:
					return LerpOutCubic(start, end, t);
				case InterpolationType.InSine:
					return LerpInSine(start, end, t);
				case InterpolationType.OutSine:
					return LerpOutSine(start, end, t);
				case InterpolationType.InOutSine:
					return LerpInOutSine(start, end, t);
				case InterpolationType.InOutSineInv:
					return LerpInOutSineInv(start, end, t);
				case InterpolationType.InElastic:
					return LerpInElastic(start, end, t);
				case InterpolationType.OutElastic:
					return LerpOutElastic(start, end, t);
				case InterpolationType.InOutElastic:
					return LerpInOutElastic(start, end, t);
				case InterpolationType.InBounce:
					return LerpInBounce(start, end, t);
				case InterpolationType.OutBounce:
					return LerpOutBounce(start, end, t);
				case InterpolationType.InOutBounce:
					return LerpInOutBounce(start, end, t);
				default:
				case InterpolationType.Linear:
					return LerpLinear(start, end, t);
			}
		}
		
		public static IEnumerator Lerp(float value, Action<float> valueSetter, InterpolationType type, float to, float time)
		{
			if (time > 0.0f)
			{
				float t = 0.0f;
				float start = value;
				float invTime = 1.0f / time;

				while (t < 1.0f)
				{
					t += Time.deltaTime * invTime;

					if (t > 1.0f)
						value = to;
					else
						value = Lerp(type, start, to, t);

					valueSetter(value);

					yield return null;
				}
			}
			else
			{
				valueSetter(to);
			}

			yield break;
		}

		public static IEnumerator Lerp(double value, Action<double> valueSetter, InterpolationType type, double to, float time)
		{
			if (time > 0.0f)
			{
				double t = 0.0d;
				double start = value;
				double invTime = 1.0d / time;

				while (t < 1.0d)
				{
					t += Time.deltaTime * invTime;

					if (t > 1.0d)
						value = to;
					else
						value = Lerp(type, start, to, t);

					valueSetter(value);

					yield return null;
				}
			}
			else
			{
				valueSetter(to);
			}

			yield break;
		}
		#endregion

		#region Linear
		public static float LerpLinear(float start, float end, float t)
		{
			end -= start;
			return end * t + start;
		}

		public static double LerpLinear(double start, double end, double t)
		{
			end -= start;
			return end * t + start;
		}
		#endregion

		#region Quad
		public static float LerpInQuad(float start, float end, float t)
		{
			end -= start;
			return end * t * t + start;
		}

		public static double LerpInQuad(double start, double end, double t)
		{
			end -= start;
			return end * t * t + start;
		}

		public static float LerpOutQuad(float start, float end, float t)
		{
			end -= start;
			return -end * t * (t - 2.0f) + start;
		}

		public static double LerpOutQuad(double start, double end, double t)
		{
			end -= start;
			return -end * t * (t - 2.0d) + start;
		}

		public static float LerpInOutQuad(float start, float end, float t)
		{
			t /= 0.5f;
			end -= start;
			if (t < 1.0f) return end * 0.5f * t * t + start;
			t--;
			return -end * 0.5f * (t * (t - 2.0f) - 1.0f) + start;
		}

		public static double LerpInOutQuad(double start, double end, double t)
		{
			t /= 0.5d;
			end -= start;

			if (t < 1.0d)
				return end * 0.5d * t * t + start;

			t--;
			return -end * 0.5d * (t * (t - 2.0d) - 1.0d) + start;
		}
		#endregion

		#region Cubic
		public static float LerpInCubic(float start, float end, float t)
		{
			t--;
			end -= start;
			return end * (t * t * t + 1.0f) + start;
		}

		public static double LerpInCubic(double start, double end, double t)
		{
			t--;
			end -= start;
			return end * (t * t * t + 1.0d) + start;
		}

		public static float LerpOutCubic(float start, float end, float t)
		{
			t /= 0.5f;
			end -= start;
			if (t < 1.0f)
				return end * 0.5f * t * t * t + start;
			else
			{
				t -= 2.0f;
				return end * 0.5f * (t * t * t + 2.0f) + start;
			}
		}


		public static double LerpOutCubic(double start, double end, double t)
		{
			t /= 0.5d;
			end -= start;
			if (t < 1.0d)
				return end * 0.5f * t * t * t + start;
			else
			{
				t -= 2.0d;
				return end * 0.5d * (t * t * t + 2.0d) + start;
			}
		}

		public static float LerpInOutCubic(float start, float end, float t)
		{
			t /= 0.5f;
			end -= start;
			if (t < 1.0f)
				return end * 0.5f * t * t * t + start;
			else
			{
				t -= 2.0f;
				return end * 0.5f * (t * t * t + 2.0f) + start;
			}
		}

		public static double LerpInOutCubic(double start, double end, double t)
		{
			t /= 0.5d;
			end -= start;
			if (t < 1.0d)
				return end * 0.5d * t * t * t + start;
			else
			{
				t -= 2.0d;
				return end * 0.5d * (t * t * t + 2.0d) + start;
			}
		}
		#endregion

		#region Sine
		public static float LerpInSine(float start, float end, float t)
		{
			end -= start;
			return -end * Mathf.Cos(t * (Mathf.PI * 0.5f)) + end + start;
		}

		public static double LerpInSine(double start, double end, double t)
		{
			end -= start;
			return -end * Math.Cos(t * (Math.PI * 0.5d)) + end + start;
		}

		public static float LerpOutSine(float start, float end, float t)
		{
			end -= start;
			return end * Mathf.Sin(t * (Mathf.PI * 0.5f)) + start;
		}

		public static double LerpOutSine(double start, double end, double t)
		{
			end -= start;
			return end * Math.Sin(t * (Math.PI * 0.5d)) + start;
		}

		public static float LerpInOutSine(float start, float end, float t)
		{
			end -= start;
			return -end * 0.5f * (Mathf.Cos(Mathf.PI * t) - 1.0f) + start;
		}

		public static double LerpInOutSine(double start, double end, double t)
		{
			end -= start;
			return -end * 0.5d * (Math.Cos(Math.PI * t) - 1.0d) + start;
		}

		public static float LerpInOutSineInv(float start, float end, float t)
		{
			end -= start;
			return end * (Mathf.Acos(-2.0f * (t - 0.5f)) / Mathf.PI) + start;
		}

		public static double LerpInOutSineInv(double start, double end, double t)
		{
			end -= start;
			return end * (Math.Acos(-2.0d * (t - 0.5d)) / Math.PI) + start;
		}
		#endregion

		#region Elastic
		public static float LerpInElastic(float start, float end, float t)
		{
			end -= start;

			float d = 1f;
			float p = d * .3f;
			float s = 0;
			float a = 0;

			if (t == 0) return start;

			if ((t /= d) == 1) return start + end;

			if (a == 0f || a < Mathf.Abs(end))
			{
				a = end;
				s = p / 4;
			}
			else {
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			return -(a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + start;
		}

		public static double LerpInElastic(double start, double end, double t)
		{
			end -= start;

			double d = 1d;
			double p = d * .3d;
			double s = 0;
			double a = 0;

			if (t == 0) return start;

			if ((t /= d) == 1) return start + end;

			if (a == 0f || a < Math.Abs(end))
			{
				a = end;
				s = p / 4;
			}
			else {
				s = p / (2 * Math.PI) * Math.Asin(end / a);
			}

			return -(a * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + start;
		}

		public static float LerpOutElastic(float start, float end, float t)
		{
			end -= start;

			float d = 1f;
			float p = d * .3f;
			float s = 0;
			float a = 0;

			if (t == 0) return start;

			if ((t /= d) == 1) return start + end;

			if (a == 0f || a < Mathf.Abs(end))
			{
				a = end;
				s = p * 0.25f;
			}
			else {
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			return (a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + end + start);
		}

		public static double LerpOutElastic(double start, double end, double t)
		{
			end -= start;

			double d = 1d;
			double p = d * .3d;
			double s = 0;
			double a = 0;

			if (t == 0) return start;

			if ((t /= d) == 1) return start + end;

			if (a == 0f || a < Math.Abs(end))
			{
				a = end;
				s = p * 0.25d;
			}
			else {
				s = p / (2 * Math.PI) * Math.Asin(end / a);
			}

			return (a * Math.Pow(2, -10 * t) * Math.Sin((t * d - s) * (2 * Math.PI) / p) + end + start);
		}

		public static float LerpInOutElastic(float start, float end, float t)
		{
			end -= start;

			float d = 1f;
			float p = d * .3f;
			float s = 0;
			float a = 0;

			if (t == 0) return start;

			if ((t /= d * 0.5f) == 2) return start + end;

			if (a == 0f || a < Mathf.Abs(end))
			{
				a = end;
				s = p / 4;
			}
			else {
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			if (t < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + start;
			return a * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
		}

		public static double LerpInOutElastic(double start, double end, double t)
		{
			end -= start;

			double d = 1f;
			double p = d * .3f;
			double s = 0;
			double a = 0;

			if (t == 0) return start;

			if ((t /= d * 0.5f) == 2) return start + end;

			if (a == 0f || a < Math.Abs(end))
			{
				a = end;
				s = p / 4;
			}
			else {
				s = p / (2 * Math.PI) * Math.Asin(end / a);
			}

			if (t < 1) return -0.5f * (a * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + start;
			return a * Math.Pow(2, -10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
		}
		#endregion

		#region Bounce
		public static float LerpInBounce(float start, float end, float t)
		{
			end -= start;
			float d = 1f;
			return end - LerpOutBounce(0, end, d - t) + start;
		}

		public static double LerpInBounce(double start, double end, double t)
		{
			end -= start;
			double d = 1d;
			return end - LerpOutBounce(0, end, d - t) + start;
		}

		public static float LerpOutBounce(float start, float end, float t)
		{
			t /= 1f;
			end -= start;
			if (t < (1 / 2.75f))
			{
				return end * (7.5625f * t * t) + start;
			}
			else if (t < (2 / 2.75f))
			{
				t -= (1.5f / 2.75f);
				return end * (7.5625f * (t) * t + .75f) + start;
			}
			else if (t < (2.5 / 2.75))
			{
				t -= (2.25f / 2.75f);
				return end * (7.5625f * (t) * t + .9375f) + start;
			}
			else {
				t -= (2.625f / 2.75f);
				return end * (7.5625f * (t) * t + .984375f) + start;
			}
		}

		public static double LerpOutBounce(double start, double end, double t)
		{
			t /= 1d;
			end -= start;
			if (t < (1 / 2.75d))
			{
				return end * (7.5625d * t * t) + start;
			}
			else if (t < (2 / 2.75d))
			{
				t -= (1.5d / 2.75d);
				return end * (7.5625d * (t) * t + .75d) + start;
			}
			else if (t < (2.5 / 2.75))
			{
				t -= (2.25d / 2.75d);
				return end * (7.5625d * (t) * t + .9375d) + start;
			}
			else {
				t -= (2.625d / 2.75d);
				return end * (7.5625d * (t) * t + .984375d) + start;
			}
		}

		public static float LerpInOutBounce(float start, float end, float t)
		{
			end -= start;
			float d = 1f;
			if (t < d * 0.5f) return LerpOutBounce(0, end, t * 2) * 0.5f + start;
			else return LerpOutBounce(0, end, t * 2 - d) * 0.5f + end * 0.5f + start;
		}

		public static double LerpInOutBounce(double start, double end, double t)
		{
			end -= start;
			double d = 1d;
			if (t < d * 0.5d) return LerpOutBounce(0, end, t * 2) * 0.5d + start;
			else return LerpOutBounce(0, end, t * 2 - d) * 0.5d + end * 0.5d + start;
		}
		#endregion
	}
}