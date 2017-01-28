using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Core.Domain.Entities;

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

        /// <summary>
        /// Converts the string of format 0,1|2,2 to IEnumerable{DiffDetail}.
        /// </summary>
        /// <param name="diffs"></param>
        /// <returns></returns>
        internal static IEnumerable<DiffDetail> FromTestStringToDiffDetail(string diffs)
        {
            var pairs = diffs.Split('|');
            return from stringPair in pairs
                   let offsetLenghtPair = stringPair.Split(',')
                   select new DiffDetail(int.Parse(offsetLenghtPair[0]), int.Parse(offsetLenghtPair[1]));
        }
    }
}
