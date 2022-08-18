
namespace Kits.DevlpKit.Supplements.Structs
{
    /// <summary>
    /// 键值对
    /// 因为 System.Collections.Generic.KeyValuePair 没有修改功能，因此添加了此实现
    /// </summary>
    public struct KeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;


        public KeyValuePair(TKey key)
        {
            this.Key = key;
            Value = default;
        }


        public KeyValuePair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

    } // struct KeyValuePair

} // namespace UnityExtensions