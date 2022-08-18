/***
 * Trie.
 * 
 * This is the standard/vanilla implementation of a Trie. For an associative version of Trie, checkout the TrieMap<TRecord> class.
 * 
 * This class implements the IEnumerable interface.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Kits.DevlpKit.Supplements.Collections.Trie
{
    /// <summary>
    /// The vanila Trie implementation.
    /// </summary>
    public class Trie : IEnumerable<String>
    {
        private int count { get; set; }
        private TrieNode root { get; set; }

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public Trie()
        {
            count = 0;
            root = new TrieNode(' ', false);
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
        /// </summary>
        public bool IsEmpty
        {
            get { return count == 0; }
        }

        /// <summary>
        /// Add word to trie
        /// </summary>
        public void Add(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                UnityEngine.Debug.LogError("Word is empty or null.");
                return;
            }
              //  throw new ArgumentException();

            TrieNode current = root;

            for (int i = 0; i < word.Length; ++i)
            {
                if (!current.Children.ContainsKey(word[i]))
                {
                    TrieNode newTrieNode = new TrieNode(word[i]);
                    newTrieNode.Parent = current;
                    current.Children.Add(word[i], newTrieNode);
                }

                current = current.Children[word[i]];
            }

            if (current.IsTerminal)
            {
                UnityEngine.Debug.LogError("Word already exists in Trie.");
                return;
            }
           // throw new ApplicationException("");

            ++count;
            current.IsTerminal = true;
        }

        /// <summary>
        /// Removes a word from the trie.
        /// </summary>
        public void Remove(string word)
        {
            if (string.IsNullOrEmpty(word)) {
                UnityEngine.Debug.LogError("Word is empty or null.");
                return;
            }
              //  throw new ArgumentException("Word is empty or null.");

            TrieNode current = root;

            for (int i = 0; i < word.Length; ++i)
            {
                if (!current.Children.ContainsKey(word[i])) {
                    UnityEngine.Debug.LogError("Word doesn't belong to trie.");
                    return;
                }
                  //  throw new KeyNotFoundException("");

                current = current.Children[word[i]];
            }

            if (!current.IsTerminal) {
                UnityEngine.Debug.LogError("Word doesn't belong to trie.");
                return;
            }
              //  throw new KeyNotFoundException("Word doesn't belong to trie.");

            --count;
            current.Remove();
        }

        /// <summary>
        /// Checks whether the trie has a specific word.
        /// </summary>
        public bool ContainsWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                UnityEngine.Debug.LogError("Word is empty or null.");
                return false;
            }

            TrieNode current = root;

            for (int i = 0; i < word.Length; ++i)
            {
                if (!current.Children.ContainsKey(word[i]))
                    return false;

                current = current.Children[word[i]];
            }

            return current.IsTerminal;
        }

        /// <summary>
        /// Checks whether the trie has a specific prefix.
        /// </summary>
        public bool ContainsPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                UnityEngine.Debug.LogError("Prefix is either null or empty.");
                return false;
            }
           // throw new ApplicationException("");

            TrieNode current = root;

            for (int i = 0; i < prefix.Length; ++i)
            {
                if (!current.Children.ContainsKey(prefix[i]))
                    return false;

                current = current.Children[prefix[i]];
            }

            return true;
        }

        /// <summary>
        /// Searches the entire trie for words that has a specific prefix.
        /// </summary>
        public IEnumerable<String> SearchByPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix)) {
                UnityEngine.Debug.LogError("Prefix is either null or empty.");
                return new List<String>();
            }
              //  throw new ApplicationException("Prefix is either null or empty.");

            TrieNode current = root;

            for (int i = 0; i < prefix.Length; ++i)
            {
                if (!current.Children.ContainsKey(prefix[i]))
                    return null;

                current = current.Children[prefix[i]];
            }

            return current.GetByPrefix();
        }

        /// <summary>
        /// Clears this insance.
        /// </summary>
        public void Clear()
        {
            count = 0;
            root.Clear();
            root = new TrieNode(' ', false);
        }


        #region IEnumerable<String> Implementation
        /// <summary>
        /// IEnumerable\<String\>.IEnumerator implementation.
        /// </summary>
        public IEnumerator<string> GetEnumerator()
        {
            return root.GetTerminalChildren().Select(node => node.Word).GetEnumerator();
        }

        /// <summary>
        /// IEnumerable\<String\>.IEnumerator implementation.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion IEnumerable<String> Implementation

    }

}
