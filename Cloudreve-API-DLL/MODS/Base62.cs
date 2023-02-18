using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudreve_API_DLL.MODS
{
    internal class Base62
    {
        // Base62编码
        public static string ConvertToBase62(long num)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string result = string.Empty;
            int mod = 0;

            while (num > 0)
            {
                mod = (int)(num % 62);
                result = chars[mod] + result;
                num = num / 62;
            }

            return result;
        }

        public static long ConvertFromBase62(string base62String)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            long result = 0;
            int len = base62String.Length - 1;
            for (int i = 0; i <= len; i++)
            {
                result += chars.IndexOf(base62String[i]) * (long)Math.Pow(62, len - i);
            }

            return result;
        }
    }
}
