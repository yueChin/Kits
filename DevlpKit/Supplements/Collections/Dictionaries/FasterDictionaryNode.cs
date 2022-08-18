namespace Kits.DevlpKit.Supplements.Collections.Dictionaries
{
    public struct FasterDictionaryNode<TKey>
    {
        public readonly TKey Key;
        public readonly int  Hashcode;
        public          int  Previous;
        public          int  Next;

        public FasterDictionaryNode(ref TKey key, int hash, int previousNode)
        {
            this.Key = key;
            Hashcode = hash;
            Previous = previousNode;
            Next = -1;
        }

        public FasterDictionaryNode(ref TKey key, int hash)
        {
            this.Key = key;
            Hashcode = hash;
            Previous = -1;
            Next = -1;
        }
    }
}