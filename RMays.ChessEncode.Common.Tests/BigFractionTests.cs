using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.ChessEncode.Common.Tests
{
    [TestClass]
    public class BigFractionTests
    {
        [DataTestMethod]
        [DataRow(80, 8, 10)]
        [DataRow(1, 1, 1)]
        [DataRow(8, 4, 2)]
        [DataRow(0, int.MaxValue - 1, 0)]
        [DataRow(      int.MaxValue,     int.MaxValue / 2 + 1, 1)] // 2^31 - 1
        [DataRow((long)int.MaxValue + 1, int.MaxValue, 1)] // 2^31
        [DataRow(16705 * 30, 65536, 7)] // 7 is the 1st digit of 'AA' encrypted to a decimal [0,1), base 30
        [DataRow(1000, 1, 1000)]
        [DataRow(1000, 2, 500)]
        [DataRow(1000, 4, 250)]
        [DataRow(1000, 8, 125)]
        [DataRow(1000, 16, 62)]
        [DataRow(1000, 32, 31)]
        [DataRow(1000, 64, 15)]
        [DataRow(1000, 128, 7)]
        [DataRow(1000, 256, 3)]
        [DataRow(1000, 512, 1)]
        [DataRow(1000, 1024, 0)]
        [DataRow(1000, 2048, 0)]
        [DataRow(1000, 4096, 0)]
        [DataRow(100, 3, 33)]
        [DataRow(100, 5, 20)]
        [DataRow(100, 6, 16)]
        [DataRow(100, 7, 14)]
        [DataRow(100, 8, 12)]
        [DataRow(100, 9, 11)]
        [DataRow(100, 10, 10)]
        public void GetValue_Tests(long numerator, int denominator, long expectedQuotient)
        {
            var fraction = new BigFraction(numerator, denominator);
            Assert.AreEqual(expectedQuotient, fraction.GetValue());
        }

        [DataTestMethod]
        [DataRow(1, 1, 1, 1, true)] // 1
        [DataRow(10, 2, 20, 4, true)] // 5
        [DataRow(100, 1, 800, 8, true)] // 100
        [DataRow(0, 1, 0, int.MaxValue - 1, true)] // 0
        [DataRow(2, 1, 1, 1, false)] // 2 vs 1
        [DataRow(10, 2, 20, 8, false)] // 5 vs 2
        [DataRow(800, 16, 100, 1 , false)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 1, false)] // 0 vs 1
        public void Operator_Equals(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BigFraction(numerator1, denominator1);
            var fraction2 = new BigFraction(numerator2, denominator2);
            var result = (fraction1 == fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1, 1, false)] // 1
        [DataRow(10, 2, 20, 4, false)] // 5
        [DataRow(100, 1, 800, 8, false)] // 100
        [DataRow(0, 1, 0, int.MaxValue - 1, false)] // 0
        [DataRow(2, 1, 1, 1, true)] // 2 vs 1
        [DataRow(10, 2, 20, 8, true)] // 5 vs 2
        [DataRow(800, 16, 100, 1, true)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 1, true)] // 0 vs 1
        public void Operator_NotEquals(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BigFraction(numerator1, denominator1);
            var fraction2 = new BigFraction(numerator2, denominator2);
            var result = (fraction1 != fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1, 1, false)] // 1
        [DataRow(10, 2, 20, 4, false)] // 5
        [DataRow(100, 1, 800, 8, false)] // 100
        [DataRow(0, 1, 0, int.MaxValue - 1, false)] // 0
        [DataRow(2, 1, 1, 1, true)] // 2 vs 1
        [DataRow(10, 2, 20, 8, true)] // 5 vs 2
        [DataRow(800, 16, 100, 1, false)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 1, false)] // 0 vs 1
        public void Operator_GreaterThan(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BigFraction(numerator1, denominator1);
            var fraction2 = new BigFraction(numerator2, denominator2);
            var result = (fraction1 > fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1, 1, false)] // 1
        [DataRow(10, 2, 20, 4, false)] // 5
        [DataRow(100, 1, 800, 8, false)] // 100
        [DataRow(0, 1, 0, int.MaxValue - 1, false)] // 0
        [DataRow(2, 1, 1, 1, false)] // 2 vs 1
        [DataRow(10, 2, 20, 8, false)] // 5 vs 2
        [DataRow(800, 16, 100, 1, true)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 1, true)] // 0 vs 1
        public void Operator_LessThan(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BigFraction(numerator1, denominator1);
            var fraction2 = new BigFraction(numerator2, denominator2);
            var result = (fraction1 < fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1, 1, true)] // 1
        [DataRow(10, 2, 20, 4, true)] // 5
        [DataRow(100, 1, 800, 8, true)] // 100
        [DataRow(0, 1, 0, int.MaxValue - 1, true)] // 0
        [DataRow(2, 1, 1, 1, true)] // 2 vs 1
        [DataRow(10, 2, 20, 8, true)] // 5 vs 2
        [DataRow(800, 16, 100, 1, false)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 1, false)] // 0 vs 1
        public void Operator_GreaterThanOrEqual(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BigFraction(numerator1, denominator1);
            var fraction2 = new BigFraction(numerator2, denominator2);
            var result = (fraction1 >= fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(1, 1, 1, 1, true)] // 1
        [DataRow(10, 2, 20, 4, true)] // 5
        [DataRow(100, 1, 800, 8, true)] // 100
        [DataRow(0, 1, 0, int.MaxValue - 1, true)] // 0
        [DataRow(2, 1, 1, 1, false)] // 2 vs 1
        [DataRow(10, 2, 20, 8, false)] // 5 vs 2
        [DataRow(800, 16, 100, 1, true)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 1, true)] // 0 vs 1
        public void Operator_LessThanOrEqual(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BigFraction(numerator1, denominator1);
            var fraction2 = new BigFraction(numerator2, denominator2);
            var result = (fraction1 <= fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(10, 4, "2.5")]
        [DataRow(0, 4, "0")]
        [DataRow(1, 2, "0.5")]
        [DataRow(1000, 1, "1000")]
        [DataRow(1000, 2, "500")]
        [DataRow(1000, 4, "250")]
        [DataRow(1000, 8, "125")]
        [DataRow(1000, 16, "62.5")]
        [DataRow(1000, 32, "31.25")]
        [DataRow(1000, 64, "15.625")]
        public void GetFractionAsDecimalString(long numerator, int denominator, string expectedResult)
        {
            var fraction = new BigFraction(numerator, denominator);
            Assert.AreEqual(expectedResult, fraction.GetValueDecimalString());
        }

        [TestMethod]
        public void ReduceToFractionalPart_Simple()
        {
            // Simple test; start with 2.5, get the whole number part, then the decimal part.
            var fraction = new BigFraction(10, 4); // represents 10 / (2^2), or 2.5
            var result = fraction.ReduceToFractionalPart();
            Assert.AreEqual(2, result);
            fraction *= 10;
            result = fraction.ReduceToFractionalPart();
            Assert.AreEqual(5, result);
        }
    }
}
