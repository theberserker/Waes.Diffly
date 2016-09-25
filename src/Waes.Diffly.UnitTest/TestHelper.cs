using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waes.Diffly.UnitTest
{
    public static class TestHelper
    {
        /// <summary>
        /// Retuns byte[4] for the integer representation.
        /// </summary>
        /// <param name="intValue"></param>
        /// <returns>Array of 4 bytes, representing int32</returns>
        public static byte[] GetBytes(int intValue)
        {
            byte[] intBytes = BitConverter.GetBytes(intValue);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes);
            }
            byte[] result = intBytes;
            return result;
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetBase64EncodedBytes(int value)
        {
            var bytes = GetBytes(value);
            return Convert.ToBase64String(bytes);
        }
    }
}
