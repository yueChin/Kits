using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Kits.DevlpKit.Supplements.Closure
{

    public struct ValueStruct
    {

        static ValueStruct()
        {
            s_Nil.type = Type.Nil;
            s_Nil.m_Val._vec4 = Vector4.Zero;
            s_Nil.obj = null;
        }

        static readonly ValueStruct s_Nil;

        public static ValueStruct nil
        {
            get { return s_Nil; }
        }

        public enum Type
        {
            Nil,
            Boolean,
            Int8,
            UInt8,
            Char,
            Int16,
            UInt16,
            Int32,
            UInt32,
            Int64,
            UInt64,
            Single,
            Double,
            String,
            Object,
            Vector2,
            Vector3,
            Vector4,
            Quaternion,
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct Value
        {
            [FieldOffset(0)] internal Boolean _bool;

            [FieldOffset(0)] internal SByte _int8;

            [FieldOffset(0)] internal Byte _uint8;

            [FieldOffset(0)] internal Char _char;

            [FieldOffset(0)] internal Int16 _int16;

            [FieldOffset(0)] internal UInt16 _uint16;

            [FieldOffset(0)] internal Int32 _int32;

            [FieldOffset(0)] internal UInt32 _uint32;

            [FieldOffset(0)] internal Int64 _int64;

            [FieldOffset(0)] internal UInt64 _uint64;

            [FieldOffset(0)] internal Single _single;

            [FieldOffset(0)] internal Double _double;

            [FieldOffset(0)] internal Vector2 _vec2;

            [FieldOffset(0)] internal Vector3 _vec3;

            [FieldOffset(0)] internal Vector4 _vec4;

            [FieldOffset(0)] internal Quaternion _quat;

        };

        Type type;
        Value m_Val;
        System.Object obj;

        public Type valueType
        {
            get { return type; }
        }

        public override String ToString()
        {
            switch (type)
            {
                case Type.String:
                {
                    string s = obj as String;
                    return s ?? "null";
                }
                case Type.Object:
                {
                    object s = obj as object;
                    return s != null ? s.ToString() : "null";
                }
                case Type.Double:
                    return m_Val._double.ToString();
                case Type.Single:
                    return m_Val._single.ToString();
                case Type.UInt64:
                    return m_Val._uint64.ToString();
                case Type.Int64:
                    return m_Val._int64.ToString();
                case Type.Int32:
                    return m_Val._int32.ToString();
                case Type.UInt32:
                    return m_Val._uint32.ToString();
                case Type.UInt16:
                    return m_Val._uint16.ToString();
                case Type.Int16:
                    return m_Val._int16.ToString();
                case Type.Char:
                    return m_Val._char.ToString();
                case Type.UInt8:
                    return m_Val._uint8.ToString();
                case Type.Int8:
                    return m_Val._int8.ToString();
                case Type.Boolean:
                    return m_Val._bool.ToString();
                case Type.Vector2:
                    return m_Val._vec2.ToString();
                case Type.Vector3:
                    return m_Val._vec3.ToString();
                case Type.Vector4:
                    return m_Val._vec4.ToString();
                case Type.Quaternion:
                    return m_Val._quat.ToString();
                case Type.Nil:
                    return "nil";
            }

            return String.Empty;
        }

        public bool ToBoolean()
        {
            switch (type)
            {
                case Type.Boolean:
                    return m_Val._bool;
                case Type.Int8:
                case Type.UInt8:
                    return m_Val._int8 != 0;
                case Type.Char:
                case Type.Int16:
                case Type.UInt16:
                    return m_Val._int16 != 0;
                case Type.Int32:
                case Type.UInt32:
                    return m_Val._int32 != 0;
                case Type.Int64:
                case Type.UInt64:
                    return m_Val._int64 != 0;
                case Type.Single:
                    return m_Val._single != 0;
                case Type.Double:
                    return m_Val._double != 0;
                case Type.Nil:
                    return false;
                case Type.Object:
                    return true.Equals(obj);
                case Type.String:
                    return String.IsNullOrEmpty(obj as String);
            }

            return false;
        }

        public SByte ToSByte()
        {
            switch (type)
            {
                case Type.Int8:
                    return m_Val._int8;
                case Type.UInt8:
                    return (SByte)m_Val._uint8;
                case Type.Boolean:
                    return (SByte)(m_Val._bool ? 1 : 0);
                case Type.Char:
                    return (SByte)m_Val._char;
                case Type.Int16:
                    return (SByte)m_Val._int16;
                case Type.UInt16:
                    return (SByte)m_Val._uint16;
                case Type.Int32:
                    return (SByte)m_Val._int32;
                case Type.UInt32:
                    return (SByte)m_Val._uint32;
                case Type.Int64:
                    return (SByte)m_Val._int64;
                case Type.UInt64:
                    return (SByte)m_Val._uint64;
                case Type.Single:
                    return (SByte)m_Val._single;
                case Type.Double:
                    return (SByte)m_Val._double;
                case Type.Vector2:
                    return (SByte)m_Val._vec2.X;
                case Type.Vector3:
                    return (SByte)m_Val._vec3.X;
                case Type.Vector4:
                    return (SByte)m_Val._vec4.X;
                case Type.Quaternion:
                    return (SByte)m_Val._quat.X;
            }

            return 0;
        }

        public Byte ToByte()
        {
            switch (type)
            {
                case Type.UInt8:
                    return m_Val._uint8;
                case Type.Int8:
                    return (Byte)m_Val._int8;
                case Type.Boolean:
                    return (Byte)(m_Val._bool ? 1 : 0);
                case Type.Char:
                    return (Byte)m_Val._char;
                case Type.Int16:
                    return (Byte)m_Val._int16;
                case Type.UInt16:
                    return (Byte)m_Val._uint16;
                case Type.Int32:
                    return (Byte)m_Val._int32;
                case Type.UInt32:
                    return (Byte)m_Val._uint32;
                case Type.Int64:
                    return (Byte)m_Val._int64;
                case Type.UInt64:
                    return (Byte)m_Val._uint64;
                case Type.Single:
                    return (Byte)m_Val._single;
                case Type.Double:
                    return (Byte)m_Val._double;
                case Type.Vector2:
                    return (Byte)m_Val._vec2.X;
                case Type.Vector3:
                    return (Byte)m_Val._vec3.X;
                case Type.Vector4:
                    return (Byte)m_Val._vec4.X;
                case Type.Quaternion:
                    return (Byte)m_Val._quat.X;
            }

            return 0;
        }

        public Char ToChar()
        {
            switch (type)
            {
                case Type.Char:
                    return m_Val._char;
                case Type.UInt8:
                    return (Char)m_Val._uint8;
                case Type.Int8:
                    return (Char)m_Val._int8;
                case Type.Boolean:
                    return (Char)(m_Val._bool ? 1 : 0);
                case Type.Int16:
                    return (Char)m_Val._int16;
                case Type.UInt16:
                    return (Char)m_Val._uint16;
                case Type.Int32:
                    return (Char)m_Val._int32;
                case Type.UInt32:
                    return (Char)m_Val._uint32;
                case Type.Int64:
                    return (Char)m_Val._int64;
                case Type.UInt64:
                    return (Char)m_Val._uint64;
                case Type.Single:
                    return (Char)m_Val._single;
                case Type.Double:
                    return (Char)m_Val._double;
                case Type.Vector2:
                    return (Char)m_Val._vec2.X;
                case Type.Vector3:
                    return (Char)m_Val._vec3.X;
                case Type.Vector4:
                    return (Char)m_Val._vec4.X;
                case Type.Quaternion:
                    return (Char)m_Val._quat.X;
            }

            return '\0';
        }

        public Int16 ToInt16()
        {
            switch (type)
            {
                case Type.Int16:
                    return m_Val._int16;
                case Type.Char:
                    return (Int16)m_Val._char;
                case Type.UInt8:
                    return (Int16)m_Val._uint8;
                case Type.Int8:
                    return (Int16)m_Val._int8;
                case Type.Boolean:
                    return (Int16)(m_Val._bool ? 1 : 0);
                case Type.UInt16:
                    return (Int16)m_Val._uint16;
                case Type.Int32:
                    return (Int16)m_Val._int32;
                case Type.UInt32:
                    return (Int16)m_Val._uint32;
                case Type.Int64:
                    return (Int16)m_Val._int64;
                case Type.UInt64:
                    return (Int16)m_Val._uint64;
                case Type.Single:
                    return (Int16)m_Val._single;
                case Type.Double:
                    return (Int16)m_Val._double;
                case Type.Vector2:
                    return (Int16)m_Val._vec2.X;
                case Type.Vector3:
                    return (Int16)m_Val._vec3.X;
                case Type.Vector4:
                    return (Int16)m_Val._vec4.X;
                case Type.Quaternion:
                    return (Int16)m_Val._quat.X;
            }

            return 0;
        }

        public UInt16 ToUInt16()
        {
            switch (type)
            {
                case Type.UInt16:
                    return m_Val._uint16;
                case Type.Int16:
                    return (UInt16)m_Val._int16;
                case Type.Char:
                    return (UInt16)m_Val._char;
                case Type.UInt8:
                    return (UInt16)m_Val._uint8;
                case Type.Int8:
                    return (UInt16)m_Val._int8;
                case Type.Boolean:
                    return (UInt16)(m_Val._bool ? 1 : 0);
                case Type.Int32:
                    return (UInt16)m_Val._int32;
                case Type.UInt32:
                    return (UInt16)m_Val._uint32;
                case Type.Int64:
                    return (UInt16)m_Val._int64;
                case Type.UInt64:
                    return (UInt16)m_Val._uint64;
                case Type.Single:
                    return (UInt16)m_Val._single;
                case Type.Double:
                    return (UInt16)m_Val._double;
                case Type.Vector2:
                    return (UInt16)m_Val._vec2.X;
                case Type.Vector3:
                    return (UInt16)m_Val._vec3.X;
                case Type.Vector4:
                    return (UInt16)m_Val._vec4.X;
                case Type.Quaternion:
                    return (UInt16)m_Val._quat.X;
            }

            return 0;
        }

        public Int32 ToInt32()
        {
            switch (type)
            {
                case Type.Int32:
                    return m_Val._int32;
                case Type.UInt32:
                    return (Int32)m_Val._uint32;
                case Type.UInt16:
                    return (Int32)m_Val._uint16;
                case Type.Int16:
                    return (Int32)m_Val._int16;
                case Type.Char:
                    return (Int32)m_Val._char;
                case Type.UInt8:
                    return (Int32)m_Val._uint8;
                case Type.Int8:
                    return (Int32)m_Val._int8;
                case Type.Boolean:
                    return (Int32)(m_Val._bool ? 1 : 0);
                case Type.Int64:
                    return (Int32)m_Val._int64;
                case Type.UInt64:
                    return (Int32)m_Val._uint64;
                case Type.Single:
                    return (Int32)m_Val._single;
                case Type.Double:
                    return (Int32)m_Val._double;
                case Type.Vector2:
                    return (Int32)m_Val._vec2.X;
                case Type.Vector3:
                    return (Int32)m_Val._vec3.X;
                case Type.Vector4:
                    return (Int32)m_Val._vec4.X;
                case Type.Quaternion:
                    return (Int32)m_Val._quat.X;
            }

            return 0;
        }

        public UInt32 ToUInt32()
        {
            switch (type)
            {
                case Type.UInt32:
                    return m_Val._uint32;
                case Type.UInt16:
                    return (UInt32)m_Val._uint16;
                case Type.Int16:
                    return (UInt32)m_Val._int16;
                case Type.Char:
                    return (UInt32)m_Val._char;
                case Type.UInt8:
                    return (UInt32)m_Val._uint8;
                case Type.Int8:
                    return (UInt32)m_Val._int8;
                case Type.Boolean:
                    return (UInt32)(m_Val._bool ? 1 : 0);
                case Type.Int32:
                    return (UInt32)m_Val._int32;
                case Type.Int64:
                    return (UInt32)m_Val._int64;
                case Type.UInt64:
                    return (UInt32)m_Val._uint64;
                case Type.Single:
                    return (UInt32)m_Val._single;
                case Type.Double:
                    return (UInt32)m_Val._double;
                case Type.Vector2:
                    return (UInt32)m_Val._vec2.X;
                case Type.Vector3:
                    return (UInt32)m_Val._vec3.X;
                case Type.Vector4:
                    return (UInt32)m_Val._vec4.X;
                case Type.Quaternion:
                    return (UInt32)m_Val._quat.X;
            }

            return 0;
        }

        public Int64 ToInt64()
        {
            switch (type)
            {
                case Type.Int64:
                    return m_Val._int64;
                case Type.Int32:
                    return (Int64)m_Val._int32;
                case Type.UInt32:
                    return (Int64)m_Val._uint32;
                case Type.UInt16:
                    return (Int64)m_Val._uint16;
                case Type.Int16:
                    return (Int64)m_Val._int16;
                case Type.Char:
                    return (Int64)m_Val._char;
                case Type.UInt8:
                    return (Int64)m_Val._uint8;
                case Type.Int8:
                    return (Int64)m_Val._int8;
                case Type.Boolean:
                    return (Int64)(m_Val._bool ? 1 : 0);
                case Type.UInt64:
                    return (Int64)m_Val._uint64;
                case Type.Single:
                    return (Int64)m_Val._single;
                case Type.Double:
                    return (Int64)m_Val._double;
                case Type.Vector2:
                    return (Int64)m_Val._vec2.X;
                case Type.Vector3:
                    return (Int64)m_Val._vec3.X;
                case Type.Vector4:
                    return (Int64)m_Val._vec4.X;
                case Type.Quaternion:
                    return (Int64)m_Val._quat.X;
            }

            return 0;
        }

        public UInt64 ToUInt64()
        {
            switch (type)
            {
                case Type.UInt64:
                    return m_Val._uint64;
                case Type.Int64:
                    return (UInt64)m_Val._int64;
                case Type.Int32:
                    return (UInt64)m_Val._int32;
                case Type.UInt32:
                    return (UInt64)m_Val._uint32;
                case Type.UInt16:
                    return (UInt64)m_Val._uint16;
                case Type.Int16:
                    return (UInt64)m_Val._int16;
                case Type.Char:
                    return (UInt64)m_Val._char;
                case Type.UInt8:
                    return (UInt64)m_Val._uint8;
                case Type.Int8:
                    return (UInt64)m_Val._int8;
                case Type.Boolean:
                    return (UInt64)(m_Val._bool ? 1 : 0);
                case Type.Single:
                    return (UInt64)m_Val._single;
                case Type.Double:
                    return (UInt64)m_Val._double;
                case Type.Vector2:
                    return (UInt64)m_Val._vec2.X;
                case Type.Vector3:
                    return (UInt64)m_Val._vec3.X;
                case Type.Vector4:
                    return (UInt64)m_Val._vec4.X;
                case Type.Quaternion:
                    return (UInt64)m_Val._quat.X;
            }

            return 0;
        }

        public Single ToSingle()
        {
            switch (type)
            {
                case Type.Single:
                    return m_Val._single;
                case Type.UInt64:
                    return (Single)m_Val._uint64;
                case Type.Int64:
                    return (Single)m_Val._int64;
                case Type.Int32:
                    return (Single)m_Val._int32;
                case Type.UInt32:
                    return (Single)m_Val._uint32;
                case Type.UInt16:
                    return (Single)m_Val._uint16;
                case Type.Int16:
                    return (Single)m_Val._int16;
                case Type.Char:
                    return (Single)m_Val._char;
                case Type.UInt8:
                    return (Single)m_Val._uint8;
                case Type.Int8:
                    return (Single)m_Val._int8;
                case Type.Boolean:
                    return (Single)(m_Val._bool ? 1 : 0);
                case Type.Double:
                    return (Single)m_Val._double;
                case Type.Vector2:
                    return m_Val._vec2.X;
                case Type.Vector3:
                    return m_Val._vec3.X;
                case Type.Vector4:
                    return m_Val._vec4.X;
                case Type.Quaternion:
                    return m_Val._quat.X;
            }

            return 0;
        }

        public Double ToDouble()
        {
            switch (type)
            {
                case Type.Double:
                    return m_Val._double;
                case Type.Single:
                    return (Double)m_Val._single;
                case Type.UInt64:
                    return (Double)m_Val._uint64;
                case Type.Int64:
                    return (Double)m_Val._int64;
                case Type.Int32:
                    return (Double)m_Val._int32;
                case Type.UInt32:
                    return (Double)m_Val._uint32;
                case Type.UInt16:
                    return (Double)m_Val._uint16;
                case Type.Int16:
                    return (Double)m_Val._int16;
                case Type.Char:
                    return (Double)m_Val._char;
                case Type.UInt8:
                    return (Double)m_Val._uint8;
                case Type.Int8:
                    return (Double)m_Val._int8;
                case Type.Boolean:
                    return (Double)(m_Val._bool ? 1 : 0);
                case Type.Vector2:
                    return (Double)m_Val._vec2.X;
                case Type.Vector3:
                    return (Double)m_Val._vec3.X;
                case Type.Vector4:
                    return (Double)m_Val._vec4.X;
                case Type.Quaternion:
                    return (Double)m_Val._quat.X;
            }

            return 0;
        }

        public Vector2 ToVector2()
        {
            switch (type)
            {
                case Type.Vector2:
                    return m_Val._vec2;
                case Type.Vector3:
                    return new Vector2(m_Val._vec3.X, m_Val._vec3.Y);
                case Type.Vector4:
                    return new Vector2(m_Val._vec4.X, m_Val._vec4.Y);
                case Type.Double:
                    return new Vector2((float)m_Val._double, 0);
                case Type.Single:
                    return new Vector2((float)m_Val._single, 0);
                case Type.UInt64:
                    return new Vector2((float)m_Val._uint64, 0);
                case Type.Int64:
                    return new Vector2((float)m_Val._int64, 0);
                case Type.Int32:
                    return new Vector2((float)m_Val._int32, 0);
                case Type.UInt32:
                    return new Vector2((float)m_Val._uint32, 0);
                case Type.UInt16:
                    return new Vector2((float)m_Val._uint16, 0);
                case Type.Int16:
                    return new Vector2((float)m_Val._int16, 0);
                case Type.Char:
                    return new Vector2((float)m_Val._char, 0);
                case Type.UInt8:
                    return new Vector2((float)m_Val._uint8, 0);
                case Type.Int8:
                    return new Vector2((float)m_Val._int8, 0);
                case Type.Boolean:
                    return new Vector2((m_Val._bool ? 1.0f : 0), 0);
                case Type.Quaternion:
                    return new Vector2(m_Val._quat.X, m_Val._quat.Y);
            }

            return Vector2.Zero;
        }

        public Vector3 ToVector3()
        {
            switch (type)
            {
                case Type.Vector3:
                    return m_Val._vec3;
                case Type.Vector2:
                    return new Vector3(m_Val._vec2.X, m_Val._vec2.Y, 0);
                case Type.Vector4:
                    return new Vector3(m_Val._vec4.X, m_Val._vec4.Y, m_Val._vec4.Z);
                case Type.Double:
                    return new Vector3((float)m_Val._double, 0, 0);
                case Type.Single:
                    return new Vector3((float)m_Val._single, 0, 0);
                case Type.UInt64:
                    return new Vector3((float)m_Val._uint64, 0, 0);
                case Type.Int64:
                    return new Vector3((float)m_Val._int64, 0, 0);
                case Type.Int32:
                    return new Vector3((float)m_Val._int32, 0, 0);
                case Type.UInt32:
                    return new Vector3((float)m_Val._uint32, 0, 0);
                case Type.UInt16:
                    return new Vector3((float)m_Val._uint16, 0, 0);
                case Type.Int16:
                    return new Vector3((float)m_Val._int16, 0, 0);
                case Type.Char:
                    return new Vector3((float)m_Val._char, 0, 0);
                case Type.UInt8:
                    return new Vector3((float)m_Val._uint8, 0, 0);
                case Type.Int8:
                    return new Vector3((float)m_Val._int8, 0, 0);
                case Type.Boolean:
                    return new Vector3((m_Val._bool ? 1.0f : 0), 0, 0);
                case Type.Quaternion:
                    return new Vector3(m_Val._quat.X, m_Val._quat.Y, m_Val._quat.Z);
            }

            return Vector3.Zero;
        }

        public Vector4 ToVector4()
        {
            switch (type)
            {
                case Type.Vector4:
                    return m_Val._vec4;
                case Type.Vector3:
                    return new Vector4(m_Val._vec3.X, m_Val._vec3.Y, m_Val._vec3.Z, 0);
                case Type.Vector2:
                    return new Vector4(m_Val._vec2.X, m_Val._vec2.Y, 0, 0);
                case Type.Double:
                    return new Vector4((float)m_Val._double, 0, 0, 0);
                case Type.Single:
                    return new Vector4((float)m_Val._single, 0, 0, 0);
                case Type.UInt64:
                    return new Vector4((float)m_Val._uint64, 0, 0, 0);
                case Type.Int64:
                    return new Vector4((float)m_Val._int64, 0, 0, 0);
                case Type.Int32:
                    return new Vector4((float)m_Val._int32, 0, 0, 0);
                case Type.UInt32:
                    return new Vector4((float)m_Val._uint32, 0, 0, 0);
                case Type.UInt16:
                    return new Vector4((float)m_Val._uint16, 0, 0, 0);
                case Type.Int16:
                    return new Vector4((float)m_Val._int16, 0, 0, 0);
                case Type.Char:
                    return new Vector4((float)m_Val._char, 0, 0, 0);
                case Type.UInt8:
                    return new Vector4((float)m_Val._uint8, 0, 0, 0);
                case Type.Int8:
                    return new Vector4((float)m_Val._int8, 0, 0, 0);
                case Type.Boolean:
                    return new Vector4((m_Val._bool ? 1.0f : 0), 0, 0, 0);
                case Type.Quaternion:
                    return new Vector4(m_Val._quat.X, m_Val._quat.Y, m_Val._quat.Z, m_Val._quat.W);
            }

            return Vector4.Zero;
        }

        public Quaternion ToQuaternion()
        {
            switch (type)
            {
                case Type.Quaternion:
                    return m_Val._quat;
                case Type.Vector4:
                    return new Quaternion(m_Val._vec4.X, m_Val._vec4.Y, m_Val._vec4.Z, m_Val._vec4.W);
                case Type.Vector3:
                    return new Quaternion(m_Val._vec3.X, m_Val._vec3.Y, m_Val._vec3.Z, 0);
                case Type.Vector2:
                    return new Quaternion(m_Val._vec2.X, m_Val._vec2.Y, 0, 0);
                case Type.Double:
                    return new Quaternion((float)m_Val._double, 0, 0, 0);
                case Type.Single:
                    return new Quaternion((float)m_Val._single, 0, 0, 0);
                case Type.UInt64:
                    return new Quaternion((float)m_Val._uint64, 0, 0, 0);
                case Type.Int64:
                    return new Quaternion((float)m_Val._int64, 0, 0, 0);
                case Type.Int32:
                    return new Quaternion((float)m_Val._int32, 0, 0, 0);
                case Type.UInt32:
                    return new Quaternion((float)m_Val._uint32, 0, 0, 0);
                case Type.UInt16:
                    return new Quaternion((float)m_Val._uint16, 0, 0, 0);
                case Type.Int16:
                    return new Quaternion((float)m_Val._int16, 0, 0, 0);
                case Type.Char:
                    return new Quaternion((float)m_Val._char, 0, 0, 0);
                case Type.UInt8:
                    return new Quaternion((float)m_Val._uint8, 0, 0, 0);
                case Type.Int8:
                    return new Quaternion((float)m_Val._int8, 0, 0, 0);
                case Type.Boolean:
                    return new Quaternion((m_Val._bool ? 1.0f : 0), 0, 0, 0);
            }

            return Quaternion.Identity;
        }

        public System.Object ToObject()
        {
            if (type == Type.Object)
            {
                return obj;
            }

            return null;
        }

        public static ValueStruct FromObject(System.Object val)
        {
            return new ValueStruct
            {
                type = Type.Object,
                obj = val
            };
        }

        public static ValueStruct Create(Boolean val)
        {
            return new ValueStruct
            {
                type = Type.Boolean,
                m_Val = new Value { _bool = val }
            };
        }

        public static ValueStruct Create(Byte val)
        {
            return new ValueStruct
            {
                type = Type.UInt8,
                m_Val = new Value { _uint8 = val }
            };
        }

        public static ValueStruct Create(SByte val)
        {
            return new ValueStruct
            {
                type = Type.Int8,
                m_Val = new Value { _int8 = val }
            };
        }

        public static ValueStruct Create(Char val)
        {
            return new ValueStruct
            {
                type = Type.Char,
                m_Val = new Value { _char = val }
            };
        }

        public static ValueStruct Create(Int16 val)
        {
            return new ValueStruct
            {
                type = Type.Int16,
                m_Val = new Value { _int16 = val }
            };
        }

        public static ValueStruct Create(UInt16 val)
        {
            return new ValueStruct
            {
                type = Type.UInt16,
                m_Val = new Value { _uint16 = val }
            };
        }

        public static ValueStruct Create(Int32 val)
        {
            return new ValueStruct
            {
                type = Type.Int32,
                m_Val = new Value { _int32 = val }
            };
        }

        public static ValueStruct Create(UInt32 val)
        {
            return new ValueStruct
            {
                type = Type.UInt32,
                m_Val = new Value { _uint32 = val }
            };
        }

        public static ValueStruct Create(Int64 val)
        {
            return new ValueStruct
            {
                type = Type.Int64,
                m_Val = new Value { _int64 = val }
            };
        }

        public static ValueStruct Create(UInt64 val)
        {
            return new ValueStruct
            {
                type = Type.UInt64,
                m_Val = new Value { _uint64 = val }
            };
        }

        public static ValueStruct Create(ref Int64 val)
        {
            return new ValueStruct
            {
                type = Type.Int64,
                m_Val = new Value { _int64 = val }
            };
        }

        public static ValueStruct Create(ref UInt64 val)
        {
            return new ValueStruct
            {
                type = Type.UInt64,
                m_Val = new Value { _uint64 = val }
            };
        }

        public static ValueStruct Create(Single val)
        {
            return new ValueStruct
            {
                type = Type.Single,
                m_Val = new Value { _single = val }
            };
        }

        public static ValueStruct Create(Double val)
        {
            return new ValueStruct
            {
                type = Type.Double,
                m_Val = new Value { _double = val }
            };
        }

        public static ValueStruct Create(ref Double val)
        {
            return new ValueStruct
            {
                type = Type.Double,
                m_Val = new Value { _double = val }
            };
        }

        public static ValueStruct Create(Vector2 val)
        {
            return new ValueStruct
            {
                type = Type.Vector2,
                m_Val = new Value { _vec2 = val }
            };
        }

        public static ValueStruct Create(ref Vector2 val)
        {
            return new ValueStruct
            {
                type = Type.Vector2,
                m_Val = new Value { _vec2 = val }
            };
        }

        public static ValueStruct Create(Vector3 val)
        {
            return new ValueStruct
            {
                type = Type.Vector3,
                m_Val = new Value { _vec3 = val }
            };
        }

        public static ValueStruct Create(ref Vector3 val)
        {
            return new ValueStruct
            {
                type = Type.Vector3,
                m_Val = new Value { _vec3 = val }
            };
        }

        public static ValueStruct Create(Vector4 val)
        {
            return new ValueStruct
            {
                type = Type.Vector4,
                m_Val = new Value { _vec4 = val }
            };
        }

        public static ValueStruct Create(ref Vector4 val)
        {
            return new ValueStruct
            {
                type = Type.Vector4,
                m_Val = new Value { _vec4 = val }
            };
        }

        public static ValueStruct Create(Quaternion val)
        {
            return new ValueStruct
            {
                type = Type.Quaternion,
                m_Val = new Value { _quat = val }
            };
        }

        public static ValueStruct Create(ref Quaternion val)
        {
            return new ValueStruct
            {
                type = Type.Quaternion,
                m_Val = new Value { _quat = val }
            };
        }

        public static ValueStruct Create(String val)
        {
            return new ValueStruct
            {
                type = Type.String,
                obj = val
            };
        }

        internal static class ReaderInit
        {
            static ReaderInit()
            {
                Reader<Boolean>.Invoke = (ref ValueStruct s) => s.ToBoolean();
                Reader<Char>.Invoke = (ref ValueStruct s) => s.ToChar();
                Reader<Byte>.Invoke = (ref ValueStruct s) => s.ToByte();
                Reader<SByte>.Invoke = (ref ValueStruct s) => s.ToSByte();
                Reader<Int16>.Invoke = (ref ValueStruct s) => s.ToInt16();
                Reader<UInt16>.Invoke = (ref ValueStruct s) => s.ToUInt16();
                Reader<Int32>.Invoke = (ref ValueStruct s) => s.ToInt32();
                Reader<UInt32>.Invoke = (ref ValueStruct s) => s.ToUInt32();
                Reader<Int64>.Invoke = (ref ValueStruct s) => s.ToInt64();
                Reader<UInt64>.Invoke = (ref ValueStruct s) => s.ToUInt64();
                Reader<String>.Invoke = (ref ValueStruct s) => s.ToString();
                Reader<Single>.Invoke = (ref ValueStruct s) => s.ToSingle();
                Reader<Double>.Invoke = (ref ValueStruct s) => s.ToDouble();
                Reader<Vector2>.Invoke = (ref ValueStruct s) => s.ToVector2();
                Reader<Vector3>.Invoke = (ref ValueStruct s) => s.ToVector3();
                Reader<Vector4>.Invoke = (ref ValueStruct s) => s.ToVector4();
                Reader<Quaternion>.Invoke = (ref ValueStruct s) => s.ToQuaternion();
                Reader<System.Object>.Invoke = (ref ValueStruct s) => s.ToObject();
            }

            public static void DoInit()
            {
            }
        }

        internal static class WriterInit
        {
            static WriterInit()
            {
                Writer<Boolean>.Invoke = v => ValueStruct.Create(v);
                Writer<Char>.Invoke = v => ValueStruct.Create(v);
                Writer<Byte>.Invoke = v => ValueStruct.Create(v);
                Writer<SByte>.Invoke = v => ValueStruct.Create(v);
                Writer<Int16>.Invoke = v => ValueStruct.Create(v);
                Writer<UInt16>.Invoke = v => ValueStruct.Create(v);
                Writer<Int32>.Invoke = v => ValueStruct.Create(v);
                Writer<UInt32>.Invoke = v => ValueStruct.Create(v);
                Writer<Int64>.Invoke = v => ValueStruct.Create(ref v);
                Writer<UInt64>.Invoke = v => ValueStruct.Create(ref v);
                Writer<String>.Invoke = v => ValueStruct.Create(v);
                Writer<Single>.Invoke = v => ValueStruct.Create(v);
                Writer<Double>.Invoke = v => ValueStruct.Create(ref v);
                Writer<Vector2>.Invoke = v => ValueStruct.Create(ref v);
                Writer<Vector3>.Invoke = v => ValueStruct.Create(ref v);
                Writer<Vector4>.Invoke = v => ValueStruct.Create(ref v);
                Writer<Quaternion>.Invoke = v => ValueStruct.Create(ref v);
                Writer<System.Object>.Invoke = v => ValueStruct.FromObject(v);
            }

            public static void DoInit()
            {
            }
        }

        public class Reader<T>
        {
            internal static FuncRef<ValueStruct, T> Invoke = null;
            internal static FuncRef<ValueStruct, T> Default = (ref ValueStruct val) => (T)val.ToObject();
            public static FuncRef<ValueStruct, T> invoke => Invoke ?? Default;

            static Reader()
            {
                ReaderInit.DoInit();
            }
        }

        public class Writer<T>
        {
            internal delegate void Tw(T v);

            internal static Func<T, ValueStruct> Invoke = null;

            internal static Func<T, ValueStruct> Default = v =>
            {
                //Debuger.Assert( typeof( T ).IsValueType == false, "Please avoid value type boxing!" );
                return ValueStruct.FromObject(v);
            };

            public static Func<T, ValueStruct> invoke
            {
                get { return Invoke ?? Default; }
            }

            static Writer()
            {
                WriterInit.DoInit();
            }
        }
    }
}
//EOF
