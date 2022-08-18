using System;
using Kits.DevlpKit.Helpers;
using UnityEngine;

namespace Kits.ClientKit.Collections
{
    public struct Vector2Double
    {
        public const double CEpsilon = 1E-05d;
        public double X;
        public double Y;

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.X;
                    case 1:
                        return this.Y;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2d index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.X = value;
                        break;
                    case 1:
                        this.Y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2d index!");
                }
            }
        }

        public Vector2Double normalized
        {
            get
            {
                Vector2Double vector2d = new Vector2Double(this.X, this.Y);
                vector2d.Normalize();
                return vector2d;
            }
        }

        public double Magnitude
        {
            get
            {
                return MathHelper.Sqrt(this.X * this.X + this.Y * this.Y);
            }
        }

        public double SqrMagnitude
        {
            get
            {
                return this.X * this.X + this.Y * this.Y;
            }
        }

        public static Vector2Double Zero
        {
            get
            {
                return new Vector2Double(0.0d, 0.0d);
            }
        }

        public static Vector2Double One
        {
            get
            {
                return new Vector2Double(1d, 1d);
            }
        }

        public static Vector2Double Up
        {
            get
            {
                return new Vector2Double(0.0d, 1d);
            }
        }

        public static Vector2Double Right
        {
            get
            {
                return new Vector2Double(1d, 0.0d);
            }
        }

        public Vector2Double(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static implicit operator Vector2Double(Vector3Double v)
        {
            return new Vector2Double(v.x, v.y);
        }

        public static implicit operator Vector3Double(Vector2Double v)
        {
            return new Vector3Double(v.X, v.Y, 0.0d);
        }

        public static implicit operator Vector2Double(Vector2 v2)
        {
            return new Vector2Double((double)v2.x, (double)v2.y);
        }


        public static implicit operator Vector2(Vector2Double v2)
        {
            return new Vector2((float)v2.X, (float)v2.Y);
        }


        public static Vector2Double operator +(Vector2Double a, Vector2Double b)
        {
            return new Vector2Double(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2Double operator -(Vector2Double a, Vector2Double b)
        {
            return new Vector2Double(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2Double operator -(Vector2Double a)
        {
            return new Vector2Double(-a.X, -a.Y);
        }

        public static Vector2Double operator *(Vector2Double a, double d)
        {
            return new Vector2Double(a.X * d, a.Y * d);
        }

        public static Vector2Double operator *(float d, Vector2Double a)
        {
            return new Vector2Double(a.X * d, a.Y * d);
        }

        public static Vector2Double operator /(Vector2Double a, double d)
        {
            return new Vector2Double(a.X / d, a.Y / d);
        }

        public static bool operator ==(Vector2Double lhs, Vector2Double rhs)
        {
            return Vector2Double.GetSqrMagnitude(lhs - rhs) < 0.0 / 1.0;
        }

        public static bool operator !=(Vector2Double lhs, Vector2Double rhs)
        {
            return (double)Vector2Double.GetSqrMagnitude(lhs - rhs) >= 0.0 / 1.0;
        }

        public void Set(double new_x, double new_y)
        {
            this.X = new_x;
            this.Y = new_y;
        }

        public static Vector2Double Lerp(Vector2Double from, Vector2Double to, double t)
        {
            t = MathHelper.Clamp01(t);
            return new Vector2Double(from.X + (to.X - from.X) * t, from.Y + (to.Y - from.Y) * t);
        }

        public static Vector2Double MoveTowards(Vector2Double current, Vector2Double target, double maxDistanceDelta)
        {
            Vector2Double vector2 = target - current;
            double magnitude = vector2.Magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0.0d)
                return target;
            else
                return current + vector2 / magnitude * maxDistanceDelta;
        }

        public static Vector2Double Scale(Vector2Double a, Vector2Double b)
        {
            return new Vector2Double(a.X * b.X, a.Y * b.Y);
        }

        public void Scale(Vector2Double scale)
        {
            this.X *= scale.X;
            this.Y *= scale.Y;
        }

        public void Normalize()
        {
            double magnitude = this.Magnitude;
            if (magnitude > 9.99999974737875E-06)
                this = this / magnitude;
            else
                this = Vector2Double.Zero;
        }

        public override string ToString()
        {
            /*
      string fmt = "({0:D1}, {1:D1})";
      object[] objArray = new object[2];
      int index1 = 0;
      // ISSUE: variable of a boxed type
      __Boxed<double> local1 = (ValueType) this.x;
      objArray[index1] = (object) local1;
      int index2 = 1;
      // ISSUE: variable of a boxed type
      __Boxed<double> local2 = (ValueType) this.y;
      objArray[index2] = (object) local2;
      */
            return "not implemented";
        }

        public string ToString(string format)
        {
            /* TODO:
      string fmt = "({0}, {1})";
      object[] objArray = new object[2];
      int index1 = 0;
      string str1 = this.x.ToString(format);
      objArray[index1] = (object) str1;
      int index2 = 1;
      string str2 = this.y.ToString(format);
      objArray[index2] = (object) str2;
      */
            return "not implemented";
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() << 2;
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector2Double))
                return false;
            Vector2Double vector2d = (Vector2Double)other;
            if (this.X.Equals(vector2d.X))
                return this.Y.Equals(vector2d.Y);
            else
                return false;
        }

        public static double Dot(Vector2Double lhs, Vector2Double rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }

        public static double Angle(Vector2Double from, Vector2Double to)
        {
            return MathHelper.Acos(MathHelper.Clamp(Vector2Double.Dot(from.normalized, to.normalized), -1d, 1d)) * 57.29578d;
        }

        public static double Distance(Vector2Double a, Vector2Double b)
        {
            return (a - b).Magnitude;
        }

        public static Vector2Double ClampMagnitude(Vector2Double vector, double maxLength)
        {
            if (vector.SqrMagnitude > maxLength * maxLength)
                return vector.normalized * maxLength;
            else
                return vector;
        }

        public static double GetSqrMagnitude(Vector2Double a)
        {
            return (a.X * a.X + a.Y * a.Y);
        }

        public double GetSqrMagnitude()
        {
            return (this.X * this.X + this.Y * this.Y);
        }

        public static Vector2Double Min(Vector2Double lhs, Vector2Double rhs)
        {
            return new Vector2Double(MathHelper.Min(lhs.X, rhs.X), MathHelper.Min(lhs.Y, rhs.Y));
        }

        public static Vector2Double Max(Vector2Double lhs, Vector2Double rhs)
        {
            return new Vector2Double(MathHelper.Max(lhs.X, rhs.X), MathHelper.Max(lhs.Y, rhs.Y));
        }
    }
}