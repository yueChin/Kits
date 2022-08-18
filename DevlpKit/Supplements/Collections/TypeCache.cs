using System;
using Kits.DevlpKit.Helpers.TypeHelpers;

namespace Kits.DevlpKit.Supplements.Collections
{
    public class TypeCache<T>
    {
        public static readonly Type Type = typeof(T);
        public static readonly bool IsUnmanaged = Type.IsUnmanagedEx();
    }

    public class TypeHash<T>
    {
#if !UNITY_COLLECTIONS        
        public static readonly int Hash = TypeCache<T>.Type.GetHashCode();
#else
        public static readonly int hash = Unity.Burst.BurstRuntime.GetHashCode32<T>();
#endif        
    }
}