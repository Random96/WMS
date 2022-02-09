using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.Utilities
{
    public static class BytesExt
    {
        public static byte[] ToBytes(this string str, Encoding ? type = null)
        {
            if (type == null)
                type = Encoding.UTF8;

            return type.GetBytes(str);
        }
        public static string ByteToString(this byte[] bytes, Encoding ? type = null)
        {
            if (type == null)
                type = Encoding.UTF8;

            return type.GetString(bytes);
        }
    }
}