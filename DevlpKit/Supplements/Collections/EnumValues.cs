using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Kits.DevlpKit.Supplements.Collections
{
        public struct EnumValue {
        public int Index;
        public object Boxed;
        public string Name;
        public Int64 Value;
    }

        public static class EnumHelper
        {

            internal class EnumValues
            {
                internal Dictionary<Int64, EnumValue> InfoLut = null;
                internal Dictionary<string, Int64> Name2ValueLut = null;
                internal EnumValue[] Values = null;
            }

            static readonly Dictionary<Type, EnumValues> s_EnumValueCache = new Dictionary<Type, EnumValues>();

            internal static EnumValues GetCache(Type t)
            {
                if (!t.IsEnum)
                {
                    return null;
                }

                EnumValues cache;
                if (!s_EnumValueCache.TryGetValue(t, out cache))
                {
                    int enumSize = Marshal.SizeOf(Enum.GetUnderlyingType(t));
                    int intSize = Marshal.SizeOf(typeof(Int64));
                    if (enumSize > intSize)
                    {
                        System.Diagnostics.Debug.Assert(false, string.Format("EnumInt64<{0}>: Can't convert {0} to Int64.", t.FullName));
                        return null;
                    }

                    cache = new EnumValues();
                    Array values = Enum.GetValues(t);
                    string[] names = Enum.GetNames(t);
                    cache.InfoLut = new Dictionary<Int64, EnumValue>(values.Length);
                    cache.Name2ValueLut = new Dictionary<string, Int64>(names.Length);
                    cache.Values = new EnumValue[values.Length];
                    try
                    {
                        for (int i = 0; i < values.Length; ++i)
                        {
                            string str = names.GetValue(i) as string;
                            //Debug.Assert( !string.IsNullOrEmpty( str ) );
                            object boxed = values.GetValue(i);
                            long ival = Convert.ToInt64(boxed);
                            EnumValue eval = new EnumValue
                            {
                                Index = i,
                                Boxed = boxed,
                                Name = str,
                                Value = ival
                            };
                            cache.Values[i] = eval;
                            if (!cache.InfoLut.ContainsKey(ival))
                            {
                                cache.InfoLut.Add(ival, eval);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //Debug.LogException( e );
                    }

                    s_EnumValueCache.Add(t, cache);
                }

                return cache;
            }

            public static object ToObject(Type enumType, Int32 value)
            {
                return ToObject(enumType, (Int64)value);
            }

            public static object ToObject(Type enumType, Int64 value)
            {
                try
                {
                    EnumValues cache = GetCache(enumType);
                    if (cache != null)
                    {
                        EnumValue e;
                        if (cache.InfoLut.TryGetValue(value, out e))
                        {
                            return e.Boxed;
                        }
                    }
                    else if (enumType.IsEnum)
                    {
                        return Enum.ToObject(enumType, value);
                    }
                }
                catch (Exception e)
                {
                    //Debug.LogException( e );
                }

                return null;
            }
        }

        public struct EnumInt32<T> where T : struct, IConvertible
    {

        static EnumInt32()
        {
            try
            {
                Type et = typeof(T);
                if (et.IsEnum)
                {
                    int enumSize = Marshal.SizeOf(Enum.GetUnderlyingType(et));
                    int intSize = Marshal.SizeOf(typeof(Int32));
                    if (enumSize == intSize)
                    {
                        return;
                    }
                }
            }
            catch
            {
            }

            System.Diagnostics.Debug.Assert(false, string.Format("EnumInt32<{0}>: Can't convert {0} to Int32.", typeof(T).FullName));
        }

        static EnumHelper.EnumValues s_Cache = null;

        static void _EnsureLUTs()
        {
            if (s_Cache == null)
            {
                s_Cache = EnumHelper.GetCache(typeof(T));
            }
        }

        public static EnumValue[] GetDefines()
        {
            _EnsureLUTs();
            return s_Cache.Values;
        }
    }
}
