using System;
using System.Collections.Generic;

namespace Kits.DevlpKit.Helpers.CollectionHelpers
{
    public static partial class ListHelper
    {
        public static void Resize<T>(this List<T> list, int size, T c)
        {
            int cur = list.Count;
            if (size < cur)
            {
                list.RemoveRange(size, cur - size);
            }
            else if (size > cur)
            {
                //this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                if (size > list.Count)
                {
                    list.Capacity = size;
                }
                int count = size - cur;
                for (int i = 0; i < count; ++i)
                {
                    list.Add(c);
                }
            }
        }

        public static void Resize<T>(this List<T> list, int size)
        {
            Resize(list, size, default(T));
        }

        public static int FindIndex<T, C>(this IList<T> list, C ctx, Func<T, C, bool> match)
        {
            for (int i = 0, count = list.Count; i < count; ++i)
            {
                if (match(list[i], ctx))
                {
                    return i;
                }
            }
            return -1;
        }

        public static int FindLastIndex<T, C>(this IList<T> list, C ctx, Func<T, C, bool> match)
        {
            for (int i = list.Count - 1; i >= 0; --i)
            {
                if (match(list[i], ctx))
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool RemoveFirstOf<T, C>(this IList<T> list, C ctx, Func<T, C, bool> match)
        {
            int index = list.FindIndex(ctx, match);
            if (index != -1)
            {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }

        public static bool RemoveLastOf<T, C>(this IList<T> list, C ctx, Func<T, C, bool> match)
        {
            int index = list.FindLastIndex(ctx, match);
            if (index != -1)
            {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }

        public static int RemoveAllNull<T>(this List<T> list) where T : class
        {
            int count = list.Count;
            int removeCount = 0;
            for (int i = 0; i < count; ++i)
            {
                if (list[i] == null)
                {
                    int newCount = i++;
                    for (; i < count; ++i)
                    {
                        if (list[i] != null)
                        {
                            list[newCount++] = list[i];
                        }
                    }
                    removeCount = count - newCount;
                    list.RemoveRange(newCount, removeCount);
                    break;
                }
            }
            return removeCount;
        }

        public static int RemoveAllNullUnordered<T>(this List<T> list) where T : class
        {
            int count = list.Count;
            int last = count - 1;
            int removeCount = 0;
            for (int i = 0; i <= last;)
            {
                if (list[i] == null)
                {
                    if (last != i)
                    {
                        list[i] = list[last];
                    }
                    --last;
                    ++removeCount;
                }
                else
                {
                    ++i;
                }
            }
            if (removeCount > 0)
            {
                list.RemoveRange(count - removeCount, removeCount);
            }
            return removeCount;
        }

        public static int RemoveAll<T, C>(this List<T> list, C ctx, Func<T, C, bool> match)
        {
            int count = list.Count;
            int removeCount = 0;
            for (int i = 0; i < count; ++i)
            {
                if (match(list[i], ctx))
                {
                    int newCount = i++;
                    for (; i < count; ++i)
                    {
                        if (!match(list[i], ctx))
                        {
                            list[newCount++] = list[i];
                        }
                    }
                    removeCount = count - newCount;
                    list.RemoveRange(newCount, removeCount);
                    break;
                }
            }
            return removeCount;
        }

        public static int RemoveAllUnordered<T, C>(this List<T> list, C ctx, Func<T, C, bool> match)
        {
            int count = list.Count;
            int last = count - 1;
            int removeCount = 0;
            for (int i = 0; i <= last;)
            {
                if (match(list[i], ctx))
                {
                    if (last != i)
                    {
                        list[i] = list[last];
                    }
                    --last;
                    ++removeCount;
                }
                else
                {
                    ++i;
                }
            }
            if (removeCount > 0)
            {
                list.RemoveRange(count - removeCount, removeCount);
            }
            return removeCount;
        }
        
        // public static int FindIndex<T, TC>( this IList<T> list, TC ctx, Func<T, TC, bool> match ) {
        //     for ( int i = 0, count = list.Count; i < count; ++i ) {
        //         if ( match( list[ i ], ctx ) ) {
        //             return i;
        //         }
        //     }
        //     return -1;
        // }
        //
        // public static int RemoveAllEx<T, TC>( List<T> list, TC ctx, Func<T, TC, bool> match ) {
        //     int count = list.Count;
        //     int removeCount = 0;
        //     for ( int i = 0; i < count; ++i ) {
        //         if ( match( list[ i ], ctx ) ) {
        //             int newCount = i++;
        //             for ( ; i < count; ++i ) {
        //                 if ( !match( list[ i ], ctx ) ) {
        //                     list[ newCount++ ] = list[ i ];
        //                 }
        //             }
        //             removeCount = count - newCount;
        //             list.RemoveRange( newCount, removeCount );
        //             break;
        //         }
        //     }
        //     return removeCount;
        // }
        //
        // public static int RemoveAll<T, TC>( this List<T> list, TC ctx, Func<T, TC, bool> match ) {
        //     return RemoveAllEx( list, ctx, match );
        // }
    }
}