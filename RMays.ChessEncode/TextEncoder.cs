using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.ChessEncode
{
    public class TextEncoder
    {
        public static string EncodeToBase64(string plaintext)
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            return Convert.ToBase64String(plaintextBytes);
        }

        public static string EncodeToBinary(string base64text)
        {
            return "?";
            /*
            var base64bytes = Encoding.UTF8.GetBytes(plaintext);
            return Convert.ToBase64String(plaintextBytes);
            */

        }

        public static byte DecodeBase64Char(char base64char)
        {
            var b = Convert.FromBase64CharArray(new char[] { base64char, 'A', 'A', 'A' }, 0, 4);
            return (byte)(b[0] / 4);
        }

    }
}
