using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waes.Diffly.Core.Domain.Helpers
{
    public static class ConvertHelper
    {
        public static string FromAsciiBase64(string base64Encoded)
        {
            byte[] bytes = Convert.FromBase64String(base64Encoded);
            string decoded = Encoding.ASCII.GetString(bytes);
            return decoded;
        }

        public static string ToAsciiBase64(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }
    }
}
