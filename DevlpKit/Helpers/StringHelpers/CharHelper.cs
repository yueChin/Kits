
namespace Kits.DevlpKit.Helpers.StringHelpers
{
    public static class CharHelper
    {
        public const int Upper2Lower = 'a' - 'A';
        
        /// <summary>
        /// 判断一个字符是否为阿拉伯数字
        /// </summary>
        public static bool IsDigit(this char c)
        {
            return c >= '0' && c <= '9';
        }


        /// <summary>
        /// 判断一个字符是否为英文小写字母
        /// </summary>
        public static bool IsEnglishLower(this char c)
        {
            return c >= 'a' && c <= 'z';
        }


        /// <summary>
        /// 判断一个字符是否为英文大写字母
        /// </summary>
        public static bool IsEnglishUpper(this char c)
        {
            return c >= 'A' && c <= 'Z';
        }


        /// <summary>
        /// 判断一个字符是否为英文字母
        /// </summary>
        public static bool IsEnglishLetter(this char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
        
        public static char ToLower(char c)
        {
            if (c <= 'Z' && c >= 'A')
                return (char)(c + Upper2Lower);
            else
                return c;
        }

        public static char ToUpper(char c)
        {
            if (c <= 'z' && c >= 'a')
                return (char)(c - Upper2Lower);
            else
                return c;
        }
    }
}
