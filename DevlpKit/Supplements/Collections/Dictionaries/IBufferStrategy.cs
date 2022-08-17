using AFForUnity.Kits.DevlpKit.Supplements.Structs;
using AFForUnity.Kits.DevlpKit.Tools;

namespace AFForUnity.Kits.DevlpKit.Supplements.Collections.Dictionaries
{
    public interface IBufferStrategy<T>
    {
        int  capacity { get; }
        bool isValid  { get; }

        void Alloc(uint size, Allocator nativeAllocator);
        void Resize(uint newCapacity, bool copyContent = true);
        void Clear();
        
        ref T this[uint index] { get ; }
        ref T this[int index] { get ; }
        
        IBuffer<T> ToBuffer();
        
        Allocator allocationStrategy { get; }
        void Dispose();
    }
}