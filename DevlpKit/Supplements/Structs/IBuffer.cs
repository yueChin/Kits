using System;
using System.Runtime.CompilerServices;
using Kits.DevlpKit.Tools;

namespace Kits.DevlpKit.Supplements.Structs
{
    public interface IBufferBase<T>
    {
    }
    
    public interface IBuffer<T>:IBufferBase<T>
    {
        //ToDo to remove (only implementation can be used)
        ref T this[uint index] { get; }
        //ToDo to remove(only implementation can be used)
        ref T this[int index] { get; }
        
        void CopyTo(uint sourceStartIndex, T[] destination, uint destinationStartIndex, uint size);
        void Clear();
        
        T[]    ToManagedArray();
        IntPtr ToNativeArray(out int capacity);

        int capacity { get; }
    }
    
    public static class IBufferExtensionN
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeBuffers<T> ToFast<T>(this IBuffer<T> buffer) where T:unmanaged
        {
            ContractChecks.Assert(buffer is NativeBuffers<T>, "impossible conversion");
            return (NativeBuffers<T>) buffer;
        }
    }
    
    public static class IBufferExtensionM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ManagedBuffer<T> ToFast<T>(this IBufferBase<T> buffer)
        {
            ContractChecks.Assert(buffer is ManagedBuffer<T>, "impossible conversion");
            return (ManagedBuffer<T>) buffer;
        }
    }
}