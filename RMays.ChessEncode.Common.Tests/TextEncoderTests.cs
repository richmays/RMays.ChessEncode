using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMays.ChessEncode.Common;
using System.Numerics;

namespace RMays.ChessEncode.Common.Tests
{
    [TestClass]
    public class TextEncoderTests
    {
        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("A", "QQ==")]
        [DataRow("B", "Qg==")]
        // From Wikipedia: 'Base64'
        [DataRow("any carnal pleasure.", "YW55IGNhcm5hbCBwbGVhc3VyZS4=")]
        [DataRow("any carnal pleasure",  "YW55IGNhcm5hbCBwbGVhc3VyZQ==")]
        [DataRow("any carnal pleasur",   "YW55IGNhcm5hbCBwbGVhc3Vy")]
        [DataRow("any carnal pleasu",    "YW55IGNhcm5hbCBwbGVhc3U=")]
        [DataRow("any carnal pleas",     "YW55IGNhcm5hbCBwbGVhcw==")]
        [DataRow("pleasure.", "cGxlYXN1cmUu")]
        [DataRow( "leasure.",  "bGVhc3VyZS4=")]
        [DataRow(  "easure.",   "ZWFzdXJlLg==")]
        [DataRow(   "asure.",     "YXN1cmUu")]
        [DataRow(    "sure.",      "c3VyZS4=")]
        public void EncodeToBase64_Tests(string plaintext, string expectedEncodedText)
        {
            var result = TextEncoder.EncodeToBase64(plaintext);
            Assert.AreEqual(expectedEncodedText, result);
        }

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("A", "QQ==")]
        [DataRow("B", "Qg==")]
        // From Wikipedia: 'Base64'
        [DataRow("any carnal pleasure.", "YW55IGNhcm5hbCBwbGVhc3VyZS4=")]
        [DataRow("any carnal pleasure", "YW55IGNhcm5hbCBwbGVhc3VyZQ==")]
        [DataRow("any carnal pleasur", "YW55IGNhcm5hbCBwbGVhc3Vy")]
        [DataRow("any carnal pleasu", "YW55IGNhcm5hbCBwbGVhc3U=")]
        [DataRow("any carnal pleas", "YW55IGNhcm5hbCBwbGVhcw==")]
        [DataRow("pleasure.", "cGxlYXN1cmUu")]
        [DataRow("leasure.", "bGVhc3VyZS4=")]
        [DataRow("easure.", "ZWFzdXJlLg==")]
        [DataRow("asure.", "YXN1cmUu")]
        [DataRow("sure.", "c3VyZS4=")]
        public void DecodeFromBase64_Tests(string expectedPlaintext, string encodedText)
        {
            var result = TextEncoder.DecodeFromBase64(encodedText);
            Assert.AreEqual(expectedPlaintext, result);
        }

        [DataTestMethod]
        [DataRow("", 0)]
        [DataRow("QQ==", 65)] // 'A'
        [DataRow("QUE=", (65*256) + 65)] // 'AA'

        public void GetNumeratorFromSomething_Tests(string encodedText, long expectedResult)
        {
            var result = TextEncoder.GetNumeratorFromBase64(encodedText);
            Assert.AreEqual(expectedResult, (long)result);
        }

        [Ignore]
        /// <summary>
        /// Decode the given intermediary step (numerator + messagelength).
        /// </summary>
        /// <param name="plaintext"></param>
        [DataTestMethod]
        [DataRow(0, 0, "")]
        [DataRow(65, 1, "A")]
        [DataRow((65 * 256) + 65, 2, "AA")]
        public void EncodeDecodeMessage_FromNumerator_Tests(long numerator, int messageLength, string expectedPlaintext)
        {
            // First, we need to take the numerator (the big number, representing the encoded message),
            // and strip off chunks of 30.

            //var bigNumerator = new BigInteger(numerator);
            //var bigDenominator = BigInteger.Pow(2, messageLength * 256);
            var moves = new List<int>();
            var bigFraction = new BigFraction(numerator, BigInteger.Pow(2, messageLength * 256));
            while (bigFraction > 0)
            {
                bigFraction *= 30;
                var nextValue = (int)bigFraction.ReduceToFractionalPart();
                moves.Add(nextValue);
            }

            



            /*
            var result = TextEncoder.DecodeBigInteger(numerator, messageLength);
            Assert.AreEqual(expectedPlaintext, result);
            */
        }
    }
}
