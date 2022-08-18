/********************************************************************
** Filename : SensitiveWords  
** Author : ake
** Date : 2018/1/10 21:55:33
** Summary : SensitiveWords 
***********************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Kits.ClientKit.Utilities
{
    public struct WordNodeKeyValue   //????
    {
        public string key;
        public WordNode value;
    }


    public sealed class WordNodeMap
    {
        public WordNodeKeyValue[] wordNodes = null;

        public WordNode this[string key]
        {
            get                                      // ??get???????
            {
                if (wordNodes == null)
                {
                    return null;
                }

                for (int i = 0; i < wordNodes.Length; ++i)
                {
                    if (wordNodes[i].key == key)
                    {
                        return wordNodes[i].value;
                    }
                }

                return null;
            }

            set                                         //??set????????
            {
                if (wordNodes == null)
                {
                    wordNodes = new WordNodeKeyValue[1];
                    wordNodes[0] = new WordNodeKeyValue() { key = key, value = value };  //???
                }

                else
                {
                    bool needAdd = true;
                    for (int i = 0; i < wordNodes.Length; ++i)
                    {
                        if (wordNodes[i].key == key)//??wordNodes?i??value?????key?????
                        {
                            wordNodes[i].value = value;
                            needAdd = false;
                        }
                    }

                    if (needAdd)        //?key???????????
                    {
                        WordNodeKeyValue[] newWordNodes = new WordNodeKeyValue[wordNodes.Length + 1];  //??newWordNodes??????wordNodes.Length + 1??????
                        Array.Copy(wordNodes, newWordNodes, wordNodes.Length);//??wordNodes?????newWordNodes??????wordNodes.Length
                        newWordNodes[wordNodes.Length] = new WordNodeKeyValue() { key = key, value = value };
                        wordNodes = newWordNodes;
                    }
                }
            }
        }


        public bool TryGetValue(string key, out WordNode value)
        {
            value = null;
            if (wordNodes == null)
            {
                return false;
            }
            for (int i = 0; i < wordNodes.Length; ++i)
            {
                if (wordNodes[i].key == key)
                {
                    value = wordNodes[i].value;
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            wordNodes = null;
        }
    }

    public sealed class WordNode   //sealed:?????????
    {
        public WordNode(string word)
        {
            Reset(word);
        }

        public void Reset(string word) // ??word
        {
            this.word = word;
            endTag = 0;
            wordNodes.Clear();
        }

        public void Dispose()
        {
            Reset(string.Empty);
        }

        public string word;
        public int endTag;
        public WordNodeMap wordNodes = new WordNodeMap();
        //public Dictionary<string, WordNode> wordNodes = new Dictionary<string, WordNode>();
    }

    public class SensitiveWords
    {
        //public Regex _regex = null;
        #region Fields

        private static SensitiveWords _instance;
        public static SensitiveWords Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SensitiveWords();
                }
                return _instance;
            }
        }

        private Regex _regex;

        #endregion

        #region public

        private List<string> allSensitiveWords = new List<string>();
        private WordNode rootWordNode = null;
        private bool isInit = false;

        public void InitSensitiveWords(string words)   //???????????????????
        {
            string[] wordArr = words.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            this.allSensitiveWords.Clear();
            this.allSensitiveWords.Capacity = wordArr.Length;
            for (int index = 0; index < wordArr.Length; ++index)
            {
                this.allSensitiveWords.Add(wordArr[index]);
            }
            BuildWordTree();
            this.allSensitiveWords.Clear();
            this.isInit = true;
        }

        private void BuildWordTree()  //??????
        {
            if (null == this.rootWordNode)
            {
                this.rootWordNode = new WordNode("R");   //??word
            }
            this.rootWordNode.Reset("R");
            for (int index = 0; index < this.allSensitiveWords.Count; ++index)
            {
                string strTmp = this.allSensitiveWords[index];
                //Debug.Log("allSensitiveWords[index]:" + strTmp);
                if (strTmp.Length > 0)
                {
                    InsertNode(this.rootWordNode, strTmp); //???????
                }
            }
        }

        private void InsertNode(WordNode node, string content)   //???????
        {
            //Debug.Log("InsertNode InsertNode InsertNode InsertNode InsertNode InsertNode InsertNode InsertNode InsertNode InsertNode");
            if (null == node)  //????node????
            {
                Debug.Log("the root node is null! the root node is null! the root node is null! the root node is null! the root node is null! the root node is null!");
                return;
            }
            string strTmp = content.Substring(0, 1);   //???????????
            WordNode wordNode = FindNode(node, strTmp); //???????node????????
            if (null == wordNode)  //??????????????????????????
            {
                //Debug.Log("wordNode is null !!!!   wordNode is null !!!!   wordNode is null !!!!   wordNode is null !!!!" + strTmp);
                wordNode = new WordNode(strTmp);
                node.wordNodes[strTmp] = wordNode;  //?????
            }

            strTmp = content.Substring(1);  //????????
            if (string.IsNullOrEmpty(strTmp))   //????????????????????????????
            {
                //Debug.Log("wordNode.endTag is 1 !!!!   wordNode.endTag is 1 !!!!   wordNode.endTag is 1 !!!!   wordNode.endTag is 1 !!!!" + strTmp);
                wordNode.endTag = 1;
            }
            else //????????????????
            {
                //Debug.Log("else   else  else  else  else  else  else  else  else  else else" + strTmp);
                InsertNode(wordNode, strTmp);
            }
        }

        private WordNode FindNode(WordNode node, string content)  //?????????????????????null???????????wordNode?null
        {
            if (null == node)
            {
                Debug.Log("node is null !!!!   node is null !!!!   node is null !!!!   node is null !!!!");
                return null;
            }

            WordNode wordNode = null;
            node.wordNodes.TryGetValue(content, out wordNode);
            return wordNode;
        }

        public string FilterSensitiveWords(string content)
        {
            if (!isInit || null == rootWordNode)
            {
                return content;
            }

            string originalValue = content;
            content = content.ToLower();
            WordNode node = this.rootWordNode;
            StringBuilder buffer = new StringBuilder();
            List<string> badLst = new List<string>();
            int a = 0;
            while (a < content.Length)
            {
                string contnetTmp = content.Substring(a);  //??????????
                string strTmp = contnetTmp.Substring(0, 1); //???????
                node = FindNode(node, strTmp); //??????node
                if (null == node)  //????????????????,??????buffer
                {
                    node = this.rootWordNode;
                    a = a - badLst.Count;
                    if (a < 0)
                    {
                        a = 0;
                    }
                    badLst.Clear();
                    string beginContent = content.Substring(a);
                    if (beginContent.Length > 0)
                    {
                        buffer.Append(beginContent[0]);
                    }
                }
                else if (node.endTag == 1)  //????????????*??
                {
                    badLst.Add(strTmp);
                    for (int index = 0; index < badLst.Count; ++index)
                    {
                        buffer.Append("*");
                    }
                    node = this.rootWordNode;
                    badLst.Clear();
                }
                else
                {
                    badLst.Add(strTmp);
                    if (a == content.Length - 1)
                    {
                        for (int index = 0; index < badLst.Count; ++index)
                        {
                            buffer.Append(badLst[index]);
                        }
                    }
                }
                contnetTmp = contnetTmp.Substring(1);
                ++a;
            }

            // to avoid english word don't fill enough
            string newValue = buffer.ToString();
            if (0 != newValue.CompareTo(originalValue.ToLower()))
            {
                int idx = newValue.IndexOf('*');  //??*??????
                char[] originalArr = originalValue.ToCharArray();
                while (idx != -1)
                {
                    originalArr[idx] = '*';
                    idx = newValue.IndexOf('*', idx + 1);
                }
                originalValue = new string(originalArr);
            }

            return originalValue;
        }

        public void Init()
        {
            TextAsset t = Resources.Load<TextAsset>("Config/SensitiveWords");
            string s = Encoding.UTF8.GetString(t.bytes);
            InitSensitiveWords(s);
            //LuaDataAgent.GlobalData.ReplaceSensitiveWordsToStarFunction = OutputCheckOutWords;
        }

        public string OutputCheckOutWords(string input)
        {
            string res = "";
            if (input != null)
            {
                res = FilterSensitiveWords(input);
            }
            return res;
        }

        #endregion

        #region private

        #endregion
    }
}
