using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RMays.ChessEncode.Common
{
    public class TextEncoder
    {
        public static string EncodeToBase64(string plaintext)
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            return Convert.ToBase64String(plaintextBytes);
        }
        
        public static string DecodeFromBase64(string encodedText)
        {
            byte[] data = Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Returns the numerator of the fraction from the given encodedText.
        /// Examples:
        ///   '' returns 0
        ///   'QQ==' returns 65 (ASCII code for the character 'A')
        ///   'QUE=' returns (65*256)+65 = 16705 (representing 'AA').
        /// </summary>
        /// <param name="encodedText"></param>
        /// <returns></returns>
        public static BigInteger GetNumeratorFromBase64(string encodedText)
        {
            byte[] data = Convert.FromBase64String(encodedText);
            BigInteger result = BigInteger.Zero;
            foreach(var d in data)
            {
                result = (result * 256) + d;
            }

            return result;
        }

        public static string DecodeBigInteger(BigInteger numerator, int messageLength)
        {
            return "?";
        }

    }
}
