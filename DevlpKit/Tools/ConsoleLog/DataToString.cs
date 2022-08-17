using System.Collections.Generic;
using AFForUnity.Kits.DevlpKit.Helpers.StringHelpers;

namespace AFForUnity.Kits.DevlpKit.Tools.Log
{
    public static class DataToString
    {
        public static string DetailString(Dictionary<string, string> data)
        {
            string detailString = string.Empty;

            {
                int index = 0;
                foreach (KeyValuePair<string, string> value in data)
                {
                    if (index++ < data.Count - 1)
                        detailString = detailString.FastConcat("<color=teal>\"").FastConcat(value.Key, "\"")
                            .FastConcat(":\"", value.Value, "\",</color>");
                    else
                        detailString = detailString.FastConcat("<color=teal>\"").FastConcat(value.Key, "\"")
                            .FastConcat(":\"", value.Value, "\"</color>");
                }
            }

            return detailString;
        }
    }
}