using System;
using System.Collections.Generic;
using System.Linq;//TODO 干掉linq
using Random = Kits.DevlpKit.Supplements.Structs.Random;

namespace Kits.DevlpKit.Helpers.ValueTypeHelpers
{
    public static class EnumHelper
    {
        public static int EnumIndex<T>(int value)
        {
            int i = 0;
            foreach (object v in Enum.GetValues(typeof(T)))
            {
                if ((int)v == value)
                {
                    return i;
                }

                ++i;
            }
            return -1;
        }

        public static T FromString<T>(string str)
        {
            if (!Enum.IsDefined(typeof(T), str))
            {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), str);
        }

        /// <summary>
        /// Checks wheter or not this enums is valid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static void CheckEnumWithFlags<T>()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(string.Format("Type '{0}' is not an enum", typeof(T).FullName));
            if (!Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
                throw new ArgumentException(string.Format("Type '{0}' doesn't have the 'Flags' attribute", typeof(T).FullName));
        }

        /// <summary>
        /// Checks if the Enums contains given flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool HasFlag<T>(this T value, T flag) where T : struct
        {
            CheckEnumWithFlags<T>();
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flag);
            return (lValue & lFlag) != 0;
        }

        /// <summary>
        /// Gets flags contained by Enum as IEnumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetFlags<T>(this T value) where T : struct
        {
            CheckEnumWithFlags<T>();
            foreach (T flag in Enum.GetValues(typeof(T)).Cast<T>())
            {
                if (value.HasFlag(flag))
                    yield return flag;
            }
        }

        /// <summary>
        /// Gets flags contained by Enum as Array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] GetFlagsAsArray<T>(this T value) where T : struct
        {
            CheckEnumWithFlags<T>();
            return Enum.GetValues(typeof(T)).Cast<T>().Where(x => value.HasFlag(x)).ToArray();
        }

        /// <summary>
        /// Gets number of flags contained by Enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetNumberOfFlagsSet<T>(this T value) where T : struct
        {
            CheckEnumWithFlags<T>();
            return Enum.GetValues(typeof(T)).Cast<T>().Count(x => value.HasFlag(x));
        }

        /// <summary>
        /// Adds or Removes given flag from the Enum depending on "on" parameter..
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <param name="on">True: Add, False: Remove</param>
        /// <returns></returns>
        public static T SetFlags<T>(this T value, T flags, bool on) where T : struct
        {
            CheckEnumWithFlags<T>();
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flags);
            if (on)
            {
                lValue |= lFlag;
            }
            else
            {
                lValue &= (~lFlag);
            }

            return (T)Enum.ToObject(typeof(T), lValue);
        }

        /// <summary>
        /// Adds given flag to the Enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <param name="on"></param>
        /// <returns></returns>
        public static T SetFlags<T>(this T value, T flags) where T : struct
        {
            return value.SetFlags(flags, true);
        }

        /// <summary>
        /// Removes given flags from the enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static T ClearFlags<T>(this T value, T flags) where T : struct
        {
            return value.SetFlags(flags, false);
        }

        /// <summary>
        /// Combines flags.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static T CombineFlags<T>(this IEnumerable<T> flags) where T : struct
        {
            CheckEnumWithFlags<T>();
            long lValue = 0;
            foreach (T flag in flags)
            {
                long lFlag = Convert.ToInt64(flag);
                lValue |= lFlag;
            }

            return (T)Enum.ToObject(typeof(T), lValue);
        }

        /// <summary>
        /// Get random flag from the Enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetRandomValue<T>(this T value) where T : struct
        {
            CheckEnumWithFlags<T>();
            Random random = new Random();
            T[] flagValues = Enum.GetValues(typeof(T)).Cast<T>().Where(x => value.HasFlag(x)).ToArray();
            return flagValues[random.Range(0, flagValues.Length)];
        }


        /// <summary>
        /// Returns the order number of an enum (this is NOT the enum int value, but the position in the definition of possible enum values)
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        public static int GetEnumOrder<T>(T enumValue) where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select((x, i) => new { item = x, index = i }).Single(x => (Enum)x.item == (Enum)enumValue).index;
        }
    }
}