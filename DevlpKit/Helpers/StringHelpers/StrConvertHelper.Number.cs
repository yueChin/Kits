using System;

namespace Kits.DevlpKit.Helpers.StringHelpers
{
    public partial class StrConvertHelper
    {
        static readonly double[] pow309 = new double[309];
        static readonly double[] pow309negative = new double[309];

        static unsafe double StringToDouble(string str)
        {
            int length = str.Length;
            int i = 0;
            int spaceCount = 0;
            int index = 0;
            fixed (char* vs = str)
            {
                char* now = vs;
                for (; spaceCount < length; ++spaceCount, ++now)
                {
                    if (*now != ' ')
                    {
                        break;
                    }
                }

                i += spaceCount;
                bool negative = false;
                if (*now < '0' || *now > '9')
                {
                    if (*now == '-')
                    {
                        ++spaceCount;
                        negative = true;
                        //负号后面可以加空格
                        for (++i, ++now; i < length; ++i, ++now)
                        {
                            if (*now != ' ')
                            {
                                ++spaceCount;
                                break;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("错误字符" + *now);
                    }
                }

                double v_double = (*now - '0');
                for (++i, ++now; i < length; ++i, ++now)
                {
                    if (*now < '0' || *now > '9')
                    {
                        if (*now == '.')
                        {
                            goto Dot2;
                        }
                        else
                        {
                            return negative ? -v_double : v_double;
                        }
                    }
                    else
                    {
                        v_double *= 10;
                        v_double += (*now - '0');
                    }
                }

                //long
                return negative ? -v_double : v_double;

                Dot2:
                //点

                double v_decimal = 0.1;
                for (++i, ++now; i < length; ++i, ++now)
                {
                    if (*now < '0' || *now > '9')
                    {
                        if (*now == 'E' || *now == 'e')
                        {
                            ++now;
                            ++i;
                            if (*now == '-')
                            {
                                index = 0;

                                for (++i, ++now; i < length; ++i, ++now)
                                {
                                    if (*now < '0' || *now > '9')
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        index *= 10;
                                        index += (*now - '0');
                                    }
                                }

                                v_double *= pow309negative[index];
                                return negative ? -v_double : v_double;
                            }
                            else if (*now == '+')
                            {
                                index = 0;

                                for (++i, ++now; i < length; ++i, ++now)
                                {
                                    if (*now < '0' || *now > '9')
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        index *= 10;
                                        index += (*now - '0');
                                    }
                                }

                                v_double *= pow309[index];
                                return negative ? -v_double : v_double;
                            }

                        }

                        break;
                    }
                    else
                    {
                        v_double += (*now - '0') * v_decimal;
                        v_decimal *= 0.1;
                    }
                }

                return negative ? -v_double : v_double;
            }
        }

        static unsafe object StringToNumber(string str)
        {
            int length = str.Length;
            int i = 0;
            int spaceCount = 0;
            fixed (char* vs = str)
            {
                char* now = vs;
                for (; spaceCount < length; ++spaceCount, ++now)
                {
                    if (*now != ' ')
                    {
                        break;
                    }
                }

                i += spaceCount;
                bool negative = false;
                if (*now < '0' || *now > '9')
                {
                    if (*now == '-')
                    {
                        ++spaceCount;
                        negative = true;
                        //负号后面可以加空格
                        for (++i, ++now; i < length; ++i, ++now)
                        {
                            if (*now != ' ')
                            {
                                ++spaceCount;
                                break;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("错误字符" + *now);
                    }
                }

                long v_long = (*now - '0');
                for (++i, ++now; i < length; ++i, ++now)
                {
                    if (*now < '0' || *now > '9')
                    {
                        if (*now == '.')
                        {
                            goto Dot2;
                        }
                        else if (*now == 'x' && v_long == 0 && i == spaceCount + 1)
                        {
                            //16进制，16进制目前不支持浮点数
                            for (++i, ++now; i < length; ++i, ++now)
                            {
                                v_long <<= 4;
                                if (*now < '0' || *now > '9')
                                {
                                    switch (*now)
                                    {
                                        case 'a':
                                        case 'A':
                                            v_long += 10;
                                            break;
                                        case 'b':
                                        case 'B':
                                            v_long += 11;
                                            break;
                                        case 'c':
                                        case 'C':
                                            v_long += 12;
                                            break;
                                        case 'd':
                                        case 'D':
                                            v_long += 13;
                                            break;
                                        case 'e':
                                        case 'E':
                                            v_long += 14;
                                            break;
                                        case 'f':
                                        case 'F':
                                            v_long += 15;
                                            break;
                                        default:
                                            return negative ? -v_long : v_long;
                                    }
                                }
                                else
                                {
                                    v_long += (*now - '0');
                                }
                            }


                            return negative ? -v_long : v_long;
                        }
                        else
                        {
                            return negative ? -v_long : v_long;
                        }
                    }
                    else
                    {
                        v_long *= 10;
                        v_long += (*now - '0');
                    }
                }

                //long
                return negative ? -v_long : v_long;

                Dot2:
                double v_double = v_long;
                //点

                double v_decimal = 0.1;
                for (++i, ++now; i < length; ++i, ++now)
                {
                    if (*now < '0' || *now > '9')
                    {
                        if (*now == 'E' || *now == 'e')
                        {
                            ++now;
                            ++i;
                            if (*now == '-')
                            {
                                v_long = 0;

                                for (++i, ++now; i < length; ++i, ++now)
                                {
                                    if (*now < '0' || *now > '9')
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        v_long *= 10;
                                        v_long += (*now - '0');
                                    }
                                }

                                v_double *= pow309negative[v_long];
                                return negative ? -v_double : v_double;
                            }
                            else if (*now == '+')
                            {
                                v_long = 0;

                                for (++i, ++now; i < length; ++i, ++now)
                                {
                                    if (*now < '0' || *now > '9')
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        v_long *= 10;
                                        v_long += (*now - '0');
                                    }
                                }

                                v_double *= pow309[v_long];
                                return negative ? -v_double : v_double;
                            }

                        }

                        break;
                    }
                    else
                    {
                        v_double += (*now - '0') * v_decimal;
                        v_decimal *= 0.1;
                    }
                }

                return negative ? -v_double : v_double;
            }
        }

        static unsafe long StringToLongAnd16(string str)
        {
            int length = str.Length;
            int i = 0;
            int spaceCount = 0;
            fixed (char* vs = str)
            {
                char* now = vs;
                for (; spaceCount < length; ++spaceCount, ++now)
                {
                    if (*now != ' ')
                    {
                        break;
                    }
                }

                i += spaceCount;
                bool negative = false;
                if (*now < '0' || *now > '9')
                {
                    if (*now == '-')
                    {
                        ++spaceCount;
                        negative = true;
                        //负号后面可以加空格
                        for (++i, ++now; i < length; ++i, ++now)
                        {
                            if (*now != ' ')
                            {
                                ++spaceCount;
                                break;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("错误字符" + *now);
                    }
                }

                long v_long = (*now - '0');
                for (++i, ++now; i < length; ++i, ++now)
                {
                    if (*now < '0' || *now > '9')
                    {
                        if (*now == 'x' && v_long == 0 && i == spaceCount + 1)
                        {
                            //16进制，16进制目前不支持浮点数
                            for (++i, ++now; i < length; ++i, ++now)
                            {
                                v_long <<= 4;
                                if (*now < '0' || *now > '9')
                                {
                                    switch (*now)
                                    {
                                        case 'a':
                                        case 'A':
                                            v_long += 10;
                                            break;
                                        case 'b':
                                        case 'B':
                                            v_long += 11;
                                            break;
                                        case 'c':
                                        case 'C':
                                            v_long += 12;
                                            break;
                                        case 'd':
                                        case 'D':
                                            v_long += 13;
                                            break;
                                        case 'e':
                                        case 'E':
                                            v_long += 14;
                                            break;
                                        case 'f':
                                        case 'F':
                                            v_long += 15;
                                            break;
                                        default:
                                            return negative ? -v_long : v_long;
                                    }
                                }
                                else
                                {
                                    v_long += (*now - '0');
                                }
                            }
                            return negative ? -v_long : v_long;
                        }
                        else
                        {
                            return negative ? -v_long : v_long;
                        }
                    }
                    else
                    {
                        v_long *= 10;
                        v_long += (*now - '0');
                    }
                }
                //long
                return negative ? -v_long : v_long;
            }
        }


        static unsafe long StringToLongStrict(string str)
        {
            int length = str.Length;
            int i = 0;
            fixed (char* vs = str)
            {
                char* now = vs;
                bool negative = false;
                if (*now < '0' || *now > '9')
                {
                    if (*now == '-')
                    {
                        negative = true;
                        ++i;
                        ++now;
                    }
                    else
                    {
                        throw new Exception("错误字符" + *now);
                    }
                }

                long v_long = (*now - '0');
                for (++i, ++now; i < length; ++i, ++now)
                {
                    if (*now < '0' || *now > '9')
                    {
                        return negative ? -v_long : v_long;
                    }
                    else
                    {
                        v_long *= 10;
                        v_long += (*now - '0');
                    }
                }
                //long
                return negative ? -v_long : v_long;
            }
        }

        static unsafe Double StringToDoubleStrict(string str)
        {
            int length = str.Length;
            int i = 0;
            int pow = 0;
            fixed (char* vs = str)
            {
                char* now = vs;
                bool negative = false;
                if (*now < '0' || *now > '9')
                {
                    if (*now == '-')
                    {
                        negative = true;
                        ++i;
                        ++now;
                    }
                    else
                    {
                        throw new Exception("错误字符" + *now);
                    }
                }

                double v_double = (*now - '0');
                for (++i, ++now; i < length; ++i, ++now)
                {
                    if (*now < '0' || *now > '9')
                    {
                        if (*now == '.')
                        {
                            goto Dot2;
                        }
                        else
                        {
                            return negative ? -v_double : v_double;
                        }
                    }
                    else
                    {
                        v_double *= 10;
                        v_double += (*now - '0');
                    }
                }

                //long
                return negative ? -v_double : v_double;
                
                Dot2:
                //点
                double v_decimal = 0.1;
                for (++i, ++now; i < length; ++i, ++now)
                {
                    if (*now < '0' || *now > '9')
                    {
                        if (*now == 'E' || *now == 'e')
                        {
                            ++now;
                            ++i;
                            if (*now == '-')
                            {
                                pow = 0;
                                for (++i, ++now; i < length; ++i, ++now)
                                {
                                    if (*now < '0' || *now > '9')
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        pow *= 10;
                                        pow += (*now - '0');
                                    }
                                }
                                v_double *= pow309negative[pow];
                                return negative ? -v_double : v_double;
                            }
                            else if (*now == '+')
                            {
                                pow = 0;

                                for (++i, ++now; i < length; ++i, ++now)
                                {
                                    if (*now < '0' || *now > '9')
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        pow *= 10;
                                        pow += (*now - '0');
                                    }
                                }

                                v_double *= pow309[pow];
                                return negative ? -v_double : v_double;
                            }
                        }
                        break;
                    }
                    else
                    {
                        v_double += (*now - '0') * v_decimal;
                        v_decimal *= 0.1;
                    }
                }
                return negative ? -v_double : v_double;
            }
        }


        private static unsafe long StringToLongStrict2(string str)
        {
            int length = str.Length;
            int i = 0;
            fixed (char* vs = str)
            {
                char* now = vs;
                bool negative = false;
                if (*now < '0' || *now > '9')
                {
                    if (*now != '-')
                    {
                        throw new Exception("错误字符" + *now);
                    }

                    negative = true;
                    i++;
                    now++;
                }

                long v_long = *now - '0';
                i++;
                now++;
                while (i < length)
                {
                    if (*now < '0' || *now > '9')
                    {
                        if (!negative)
                        {
                            return v_long;
                        }

                        return -v_long;
                    }

                    v_long *= 10;
                    v_long += *now - '0';
                    i++;
                    now++;
                }

                if (!negative)
                {
                    return v_long;
                }

                return -v_long;
            }
        }
    }
}