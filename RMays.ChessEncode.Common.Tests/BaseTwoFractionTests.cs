using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.ChessEncode.Common.Tests
{
    [TestClass]
    public class BaseTwoFractionTests
    {
        [DataTestMethod]
        [DataRow(80, 3, 10)]
        [DataRow(1, 0, 1)]
        [DataRow(8, 2, 2)]
        [DataRow(0, int.MaxValue - 1, 0)]
        [DataRow(      int.MaxValue,     30, 1)] // 2^31 - 1
        [DataRow((long)int.MaxValue + 1, 31, 1)] // 2^31
        [DataRow(16705 * 30, 16, 7)] // 7 is the 1st digit of 'AA' encrypted to a decimal [0,1), base 30
        [DataRow(1000, 0, 1000)]
        [DataRow(1000, 1, 500)]
        [DataRow(1000, 2, 250)]
        [DataRow(1000, 3, 125)]
        [DataRow(1000, 4, 62)]
        [DataRow(1000, 5, 31)]
        [DataRow(1000, 6, 15)]
        [DataRow(1000, 7, 7)]
        [DataRow(1000, 8, 3)]
        [DataRow(1000, 9, 1)]
        [DataRow(1000, 10, 0)]
        [DataRow(1000, 11, 0)]
        [DataRow(1000, 12, 0)]
        public void GetValue_Tests(long numerator, int denominatorPow2, long expectedQuotient)
        {
            var fraction = new BaseTwoFraction(numerator, denominatorPow2);
            Assert.AreEqual(expectedQuotient, fraction.GetValue());
        }

        [DataTestMethod]
        [DataRow(1, 0, 1, 0, true)] // 1
        [DataRow(10, 1, 20, 2, true)] // 5
        [DataRow(100, 0, 800, 3, true)] // 100
        [DataRow(0, 0, 0, int.MaxValue - 1, true)] // 0
        [DataRow(2, 0, 1, 0, false)] // 2 vs 1
        [DataRow(10, 1, 20, 3, false)] // 5 vs 2
        [DataRow(800, 4, 100, 0 , false)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 0, false)] // 0 vs 1
        public void Operator_Equals(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BaseTwoFraction(numerator1, denominator1);
            var fraction2 = new BaseTwoFraction(numerator2, denominator2);
            var result = (fraction1 == fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(1, 0, 1, 0, false)] // 1
        [DataRow(10, 1, 20, 2, false)] // 5
        [DataRow(100, 0, 800, 3, false)] // 100
        [DataRow(0, 0, 0, int.MaxValue - 1, false)] // 0
        [DataRow(2, 0, 1, 0, true)] // 2 vs 1
        [DataRow(10, 1, 20, 3, true)] // 5 vs 2
        [DataRow(800, 4, 100, 0, true)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 0, true)] // 0 vs 1
        public void Operator_NotEquals(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BaseTwoFraction(numerator1, denominator1);
            var fraction2 = new BaseTwoFraction(numerator2, denominator2);
            var result = (fraction1 != fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(1, 0, 1, 0, false)] // 1
        [DataRow(10, 1, 20, 2, false)] // 5
        [DataRow(100, 0, 800, 3, false)] // 100
        [DataRow(0, 0, 0, int.MaxValue - 1, false)] // 0
        [DataRow(2, 0, 1, 0, true)] // 2 vs 1
        [DataRow(10, 1, 20, 3, true)] // 5 vs 2
        [DataRow(800, 4, 100, 0, false)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 0, false)] // 0 vs 1
        public void Operator_GreaterThan(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BaseTwoFraction(numerator1, denominator1);
            var fraction2 = new BaseTwoFraction(numerator2, denominator2);
            var result = (fraction1 > fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(1, 0, 1, 0, false)] // 1
        [DataRow(10, 1, 20, 2, false)] // 5
        [DataRow(100, 0, 800, 3, false)] // 100
        [DataRow(0, 0, 0, int.MaxValue - 1, false)] // 0
        [DataRow(2, 0, 1, 0, false)] // 2 vs 1
        [DataRow(10, 1, 20, 3, false)] // 5 vs 2
        [DataRow(800, 4, 100, 0, true)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 0, true)] // 0 vs 1
        public void Operator_LessThan(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BaseTwoFraction(numerator1, denominator1);
            var fraction2 = new BaseTwoFraction(numerator2, denominator2);
            var result = (fraction1 < fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(1, 0, 1, 0, true)] // 1
        [DataRow(10, 1, 20, 2, true)] // 5
        [DataRow(100, 0, 800, 3, true)] // 100
        [DataRow(0, 0, 0, int.MaxValue - 1, true)] // 0
        [DataRow(2, 0, 1, 0, true)] // 2 vs 1
        [DataRow(10, 1, 20, 3, true)] // 5 vs 2
        [DataRow(800, 4, 100, 0, false)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 0, false)] // 0 vs 1
        public void Operator_GreaterThanOrEqual(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BaseTwoFraction(numerator1, denominator1);
            var fraction2 = new BaseTwoFraction(numerator2, denominator2);
            var result = (fraction1 >= fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(1, 0, 1, 0, true)] // 1
        [DataRow(10, 1, 20, 2, true)] // 5
        [DataRow(100, 0, 800, 3, true)] // 100
        [DataRow(0, 0, 0, int.MaxValue - 1, true)] // 0
        [DataRow(2, 0, 1, 0, false)] // 2 vs 1
        [DataRow(10, 1, 20, 3, false)] // 5 vs 2
        [DataRow(800, 4, 100, 0, true)] // 50 vs 100
        [DataRow(0, int.MaxValue - 1, 1, 0, true)] // 0 vs 1
        public void Operator_LessThanOrEqual(long numerator1, int denominator1, long numerator2, int denominator2, bool expectedValue)
        {
            var fraction1 = new BaseTwoFraction(numerator1, denominator1);
            var fraction2 = new BaseTwoFraction(numerator2, denominator2);
            var result = (fraction1 <= fraction2);
            Assert.AreEqual(expectedValue, result);
        }

        [DataTestMethod]
        [DataRow(10, 2, "2.5")]
        [DataRow(0, 2, "0")]
        [DataRow(1, 1, "0.5")]
        [DataRow(1000, 0, "1000")]
        [DataRow(1000, 1, "500")]
        [DataRow(1000, 2, "250")]
        [DataRow(1000, 3, "125")]
        [DataRow(1000, 4, "62.5")]
        [DataRow(1000, 5, "31.25")]
        [DataRow(1000, 6, "15.625")]
        public void GetFractionAsDecimalString(long numerator, int denominatorPow2, string expectedResult)
        {
            var fraction = new BaseTwoFraction(numerator, denominatorPow2);
            Assert.AreEqual(expectedResult, fraction.GetValueDecimalString());
        }

        [TestMethod]
        public void ReduceToFractionalPart_Simple()
        {
            // Simple test; start with 2.5, get the whole number part, then the decimal part.
            var fraction = new BaseTwoFraction(10, 2); // represents 10 / (2^2), or 2.5
            var result = fraction.ReduceToFractionalPart();
            Assert.AreEqual(2, result);
            fraction *= 10;
            result = fraction.ReduceToFractionalPart();
            Assert.AreEqual(5, result);
        }
    }
}
