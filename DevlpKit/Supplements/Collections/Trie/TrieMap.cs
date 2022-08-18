/***
 * Trie Map.
 * 
 * This is an implementation of a Trie where complete words (words that end with terminal nodes) have associated records of any type.
 * This version of Trie uses the custom generic class TrieMapNode<TRecord> for its nodes.
 * 
 * This class implements the IDictionary and IEnumerable interfaces.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kits.DevlpKit.Supplements.Collections.Trie
{
    /// <summary>
    /// The Trie Map Data Structure (a.k.a Prefix Tree).
    /// </summary>
    /// <typeparam name="TRecord">The type of records attached to words</typeparam>
    
    public class TrieMap<TRecord> : IDictionary<List<short>, TRecord>, IEnumerable<KeyValuePair<List<short>, TRecord>>
    {
        private int count { get; set; }
        private TrieMapNode<TRecord> root { get; set; }
        private readonly EqualityComparer<TRecord> m_RecordsComparer = EqualityComparer<TRecord>.Default;

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public TrieMap()
        {
            count = 0;
            root = new TrieMapNode<TRecord>(' ', default(TRecord), false);
        }

        /// <summary>
        /// Return count of words.
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// Checks if element is empty.
        /// </summary> throw new
        public bool IsEmpty
        {
            get { return count == 0; }
        }
        
        /// <summary>
        /// string 转为list
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        List<short> AnalyzeWord(string word)
        {
            List<short> wordList = new List<short>();
            if (string.IsNullOrEmpty(word))
            {
                return wordList;
            }
            for (int i = 0, count = word.Length; i < count; i++)
            {
                byte[] array = System.Text.Encoding.ASCII.GetBytes(wordList[i].ToString());
                short asciicode = (short)(array[0]);
                wordList.Add(asciicode);
            }
            //Debug.LogError("_word:" + LitJson.JsonMapper.ToJson(_word));
            return wordList;
        }
        /// <summary>
        /// Add word to trie
        /// </summary>
        public void Add(string word, TRecord record)
        {
#if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("TrieMap Add word:" + word);
#endif
            this.Add(AnalyzeWord(word),record);
#if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }

        public void Add(List<short> word, TRecord record)
        {
            if (word == null || word.Count == 0)
            {
                UnityEngine.Debug.LogError("Word is empty or null");
                return;
            }
            // throw new ArgumentException("Word is empty or null.");

            TrieMapNode<TRecord> current = root;

            for (int i = 0,count = word.Count; i < count; ++i)
            {
                if (!current.Children.ContainsKey((char)word[i]))
                {
                    TrieMapNode<TRecord> newTrieNode = new TrieMapNode<TRecord>((char)word[i], default(TRecord));
                    newTrieNode.Parent = current;
                    current.Children.Add((char)word[i], newTrieNode);
                }

                current = current.Children[(char)word[i]];
            }

            if (current.IsTerminal) {
                UnityEngine.Debug.LogError("Word already exists in Trie."+ word);
                return;
            }
               // throw new ApplicationException("Word already exists in Trie.");

            ++count;
            current.IsTerminal = true;
            current.Record = record;
        }

        /// <summary>
        /// Updates a terminal word with a new record. Throws an exception if word was not found or if it is not a terminal word.
        /// </summary>
        public void UpdateWord(string word, TRecord newRecord)
        {
            if (string.IsNullOrEmpty(word)) {
                UnityEngine.Debug.LogError("Word is either null or empty.");
                return;
            }
              //  throw new ApplicationException("Word is either null or empty.");

            TrieMapNode<TRecord> current = root;
			TrieMapNode<TRecord> temp = root;
			for (int i = 0; i < word.Length; ++i)
            {
                bool getValue = current.Children.TryGetValue(word[i], out temp);
				if (!getValue)
				{
					return;
				}
				current = temp;
            }

            if (!current.IsTerminal) {
                UnityEngine.Debug.LogError("Word doesn't belong to trie.  ");
                return;
            }
             //   throw new KeyNotFoundException("Word doesn't belong to trie.");

            current.Record = newRecord;
        }

        /// <summary>
        /// Removes a word from the trie.
        /// </summary>
        public void Remove(List<short> word)
        {
            if (word == null || word.Count == 0)
            {
                UnityEngine.Debug.LogError("Word is empty or null.");
                return;
            }
            //  throw new ArgumentException("Word is empty or null.");

            TrieMapNode<TRecord> current = root;

            for (int i = 0, count = word.Count; i < count; ++i)
            {
                if (!current.Children.ContainsKey((char)word[i]))
                {
                    UnityEngine.Debug.LogError("Word doesn't belong to trie.  ");
                    return;
                }
                //  throw new KeyNotFoundException("Word doesn't belong to trie.");

                current = current.Children[(char)word[i]];
            }

            if (!current.IsTerminal) {
                UnityEngine.Debug.LogError("Word doesn't belong to trie.  ");
                return;
            }
               // throw new KeyNotFoundException("Word doesn't belong to trie.");

            --count;
            current.Remove();
        }

        /// <summary>
        /// Checks whether the trie has a specific word.
        /// </summary>
        public bool ContainsWord(string word)
        {
            TRecord record;
            return this.SearchByWord(AnalyzeWord(word), out record);
        }

        /// <summary>
        /// Checks whether the trie has a specific prefix.
        /// </summary>
        public bool ContainsPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix)) {
                UnityEngine.Debug.LogError("Prefix is either null or empty. ");
                return false;
            }
              //  throw new ApplicationException("Prefix is either null or empty.");

            TrieMapNode<TRecord> current = root;

            for (int i = 0; i < prefix.Length; ++i)
            {
                if (!current.Children.ContainsKey(prefix[i]))
                    return false;

                current = current.Children[prefix[i]];
            }

            return true;
        }

        /// <summary>
        /// Searchs the trie for a word and returns the associated record, if found; otherwise returns false.
        /// </summary>
        public bool SearchByWord(List<short> word, out TRecord record)
        {
            record = default(TRecord);
            if (word == null || word.Count == 0)
            {
                UnityEngine.Debug.LogError("Word is either null or empty. ");
                return false;
            }
            //   throw new ApplicationException("Word is either null or empty.");

            TrieMapNode<TRecord> current = root;

            for (int i = 0,count = word.Count; i < count; ++i)
            {
                if (!current.Children.ContainsKey((char)word[i]))
                    return false;

                current = current.Children[(char)word[i]];
            }

            if (!current.IsTerminal)
                return false;

            record = current.Record;
            return true;
        }

        public IEnumerable<TRecord> SearchListByPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                UnityEngine.Debug.LogError("Prefix is either null or empty. ");
                return null;
            }

            TrieMapNode<TRecord> current = root;

            for (int i = 0; i < prefix.Length; ++i)
            {
                if (!current.Children.ContainsKey(prefix[i]))
                    return null;

                current = current.Children[prefix[i]];
            }

            return current.GetListByPrefix();
        }

        /// <summary>
        /// Searches the entire trie for words that has a specific prefix. （Hight GC） 
        /// </summary>
        //public IEnumerable<KeyValuePair<String, TRecord>> SearchByPrefix(string prefix)
        //{
        //    if (string.IsNullOrEmpty(prefix)) {
        //        UnityEngine.Debug.LogError("Prefix is either null or empty. ");
        //        return new List<KeyValuePair<String, TRecord>>();
        //    }
        //     //   throw new ApplicationException("Prefix is either null or empty.");

        //    var current = _root;

        //    for (int i = 0; i < prefix.Length; ++i)
        //    {
        //        if (!current.Children.ContainsKey(prefix[i]))
        //            return null;

        //        current = current.Children[prefix[i]];
        //    }

        //    return current.GetByPrefix();
        //}

        /// <summary>
        /// Clears this insance.
        /// </summary>
        public void Clear()
        {
            count = 0;
            root.Clear();
            root = new TrieMapNode<TRecord>(' ', default(TRecord), false);
        }


        #region IDictionary implementation
        bool ICollection<KeyValuePair<List<short>, TRecord>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Checks whether a specific key exists in trie as a word (terminal word).
        /// </summary>
        bool IDictionary<List<short>, TRecord>.ContainsKey(List<short> key)
        {
            TRecord record;
            return SearchByWord(key, out record);
        }

        /// <summary>
        /// Return all terminal words in trie.
        /// </summary>
        ICollection<List<short>> IDictionary<List<short>, TRecord>.Keys
        {
            get
            {
                List<List<short>> collection = new List<List<short>>(Count);

                IEnumerable<TrieMapNode<TRecord>> terminalNodes = root.GetTerminalChildren();
                foreach (TrieMapNode<TRecord> node in terminalNodes)
                    collection.Add(node.Word);

                return collection;
            }
        }

        /// <summary>
        /// Return all the associated records to terminal words.
        /// </summary>
        ICollection<TRecord> IDictionary<List<short>, TRecord>.Values
        {
            get
            {
                List<TRecord> collection = new List<TRecord>(Count);

                IEnumerable<TrieMapNode<TRecord>> terminalNodes = root.GetTerminalChildren();
                foreach (TrieMapNode<TRecord> node in terminalNodes)
                    collection.Add(node.Record);

                return collection;
            }
        }

        /// <summary>
        /// Tries to get the associated record of a terminal word from trie. Returns false if key was not found.
        /// </summary>
        bool IDictionary<List<short>, TRecord>.TryGetValue(List<short> key, out TRecord value)
        {
            return SearchByWord(key, out value);
        }

        /// <summary>
        /// Checks whether a specific word-record pair exists in trie. The key of item must be a terminal word not a prefix.
        /// </summary>
        bool ICollection<KeyValuePair<List<short>, TRecord>>.Contains(KeyValuePair<List<short>, TRecord> item)
        {
            TRecord record;
            bool status = SearchByWord(item.Key, out record);
            return (status == true && m_RecordsComparer.Equals(item.Value, record));
        }

        void ICollection<KeyValuePair<List<short>, TRecord>>.CopyTo(KeyValuePair<List<short>, TRecord>[] array, int arrayIndex)
        {
            KeyValuePair<List<short>, TRecord>[] tempArray = root.GetTerminalChildren()
                .Select<TrieMapNode<TRecord>, KeyValuePair<List<short>, TRecord>>(item => new KeyValuePair<List<short>, TRecord>(item.Word, item.Record))
                .ToArray();

            Array.Copy(tempArray, 0, array, arrayIndex, Count);
        }

        /// <summary>
        /// Get/Set the associated record of a terminal word in trie.
        /// </summary>
        TRecord IDictionary<List<short>, TRecord>.this[List<short> key]
        {
            get
            {
                TRecord record;
                if (SearchByWord(key, out record))
                    return record;
                else
                {
                    UnityEngine.Debug.LogError("key "+ key + " not found");
                    return Activator.CreateInstance<TRecord>();
                }
            }
            set
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0, count = key.Count; i < count; i++)
                {
                    sb.Append(key[i]);
                }
                UpdateWord(sb.ToString(), value);
                //UpdateWord(key, value);
            }
        }

        void ICollection<KeyValuePair<List<short>, TRecord>>.Add(KeyValuePair<List<short>, TRecord> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Remove a word from trie.
        /// </summary>
        bool IDictionary<List<short>, TRecord>.Remove(List<short> key)
        {
            try
            {
                this.Remove(word: key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a word from trie.
        /// </summary>
        bool ICollection<KeyValuePair<List<short>, TRecord>>.Remove(KeyValuePair<List<short>, TRecord> item)
        {
            try
            {
                this.Remove(word: item.Key);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion IDictionary implementation


        #region IEnumerable implementation
        public IEnumerator<KeyValuePair<List<short>, TRecord>> GetEnumerator()
        {
            return root.GetTerminalChildren()
                    .Select<TrieMapNode<TRecord>, KeyValuePair<List<short>, TRecord>>(item => new KeyValuePair<List<short>, TRecord>(item.Word, item.Record))
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion IEnumerable implementation
    }

}
