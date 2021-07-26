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

        /// <summary>
        /// Decode the given intermediary step (numerator + messagelength).
        /// </summary>
        /// <param name="plaintext"></param>
        [DataTestMethod]
        [DataRow(0, 0, 0, 20, "")]
        [DataRow(255, 256, 1, 20, "19,18,9")]
        [DataRow(65, 256, 1, 20, "5,1,12")]
        [DataRow((65 * 256) + 65, 256 * 256, 2, 20, "5,1,19,4")]
        [DataRow((65 * 256) + 65, 256 * 256, 2, 10, "2,5,4,8,9,9")]
        public void EncodeMessage_FromBigFraction_Tests(long numerator, long denominator, int messageLength, int chunkSize, string expectedMoves)
        {
            // Goal is to encode a BigFraction into a list of 30-chunks.  (These are chess moves, assuming there's exactly 30 different
            //   moves for each ply.)
            // Given: Numerator and denominator.
            // Result: List of 30-chunks (moves), NOT the plaintext.

            // PROBLEM: There's a good chance (almost guaranteed) that we'll get an unending list of moves.
            //   SOLUTION: We stop when we can ensure there's enough data to generate the plaintext message.
            // PROBLEM 2: How do we know how many moves we need?
            //   We know the final Denominator; that's 256 ^ message_length.
            //   As we generate moves, we'll keep a running count of the Denominator.
            //   We can stop when the NEW denominator is more than TWICE the original denominator.  (Proof is on paper.)

            BigInteger runningDenominator = BigInteger.One;
            BigInteger goalDenominator = 2 * BigInteger.Pow(256, messageLength) + 1;
            // Find the numerator by pulling off chunks of 30.
            var chessMoves = new List<int>();
            var bigFraction = new BigFraction(numerator, denominator);
            while (bigFraction > 0 && runningDenominator < goalDenominator)
            {
                bigFraction *= chunkSize;
                runningDenominator *= chunkSize;
                var nextValue = (int)bigFraction.ReduceToFractionalPart();
                chessMoves.Add(nextValue);
            }

            // ADJUSTMENT needed:
            // The final move has to be increased by 1 to ensure it can be
            // decoded to the correct value.  (Proof on paper.)

            // How do we do this to ensure we don't overrun the size?
            // This code is pretty clever.
            if (bigFraction != 0)
            {
                var currIndex = chessMoves.Count() - 1;
                while(chessMoves[currIndex] == chunkSize - 1)
                {
                    chessMoves[currIndex] = 0;
                    currIndex--;
                }

                chessMoves[currIndex] = chessMoves[currIndex] + 1;
            }

            // Compare the chessMoves list with expectedMoves.
            Assert.AreEqual(expectedMoves, string.Join(",", chessMoves));
        }
    }
}
