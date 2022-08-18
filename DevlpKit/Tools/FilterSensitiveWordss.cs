using System.Collections.Generic;
using System.Text;

namespace Kits.DevlpKit.Tools
{
    public class FilterSensitiveWordss
    {
        static string[] sensitiveWordsArray = null;
        static string fileName = "sensitivewords.u";
        static string ReplaceValue = "*";
        static Dictionary<char, IList<string>> keyDict;

        public static void Initialize()
        {
            string path = "txt/sensitivewords.u";
            string name = "sensitivewords";
            //LoadHelp.LoadAssetBundle(path, name, ar =>
            //{
            //    string content = ar.Bundle.LoadAsset<TextAsset>(name).text;
            //    if (!(content.Equals("") || content.Equals(null)))
            //    {
            //        sensitiveWordsArray = content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            //        keyDict = new Dictionary<char, IList<string>>();
            //        foreach (string s in sensitiveWordsArray)
            //        {
            //            if (string.IsNullOrEmpty(s))
            //                continue;
            //            if (keyDict.ContainsKey(s[0]))
            //                keyDict[s[0]].Add(s.Trim(new char[] { '\r' }));
            //            else
            //                keyDict.Add(s[0], new List<string> { s.Trim(new char[] { '\r' }) });
            //        }
            //    }
            //});
        }

        //??????????????????????????????????????*
        public static bool IsContainSensitiveWords(ref string text, out string SensitiveWords)
        {
            bool isFind = false;
            SensitiveWords = "";
            if (null == sensitiveWordsArray || string.IsNullOrEmpty(text))
                return isFind;

            int len = text.Length;
            StringBuilder sb = new StringBuilder(len);
            bool isOK = true;
            for (int i = 0; i < len; i++)
            {
                if (keyDict.ContainsKey(text[i]))
                {
                    foreach (string s in keyDict[text[i]])
                    {
                        isOK = true;
                        int j = i;
                        foreach (char c in s)
                        {
                            if (j >= len || c != text[j++])
                            {
                                isOK = false;
                                break;
                            }
                        }
                        if (isOK)
                        {
                            SensitiveWords = s;
                            isFind = true;
                            i += s.Length - 1;
                            sb.Append('*', s.Length);
                            break;
                        }

                    }
                    if (!isOK)
                        sb.Append(text[i]);
                }
                else
                    sb.Append(text[i]);
            }
            if (isFind)
                text = sb.ToString();

            return isFind;
        }
    }
}