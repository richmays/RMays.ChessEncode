using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RMays.ChessEncode.Common
{
    /// <summary>
    /// Fraction where the numerator and denominator are both BigIntegers.
    /// </summary>
    public class BigFraction : IEquatable<BigFraction>
    {
        private BigInteger Numerator = BigInteger.Zero;
        private BigInteger Denominator = BigInteger.One;

        public BigFraction()
        {
        }

        public BigFraction(BigInteger numerator, BigInteger denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
            Reduce();
        }

        public static BigFraction operator *(BigFraction left, long right)
        {
            var result = new BigFraction(left.Numerator * right, left.Denominator);
            result.Reduce();
            return result;
        }

        #region BigInteger methods

        public bool IsZero()
        {
            return Numerator == 0;
        }

        public bool IsOne()
        {
            return Numerator == Denominator;
        }

        #endregion

        #region Equality / Inequality Operators

        public static bool operator >(BigFraction left, BigFraction right)
        {
            return (left.Numerator * right.Denominator) > (right.Numerator * left.Denominator);
        }

        public static bool operator ==(BigFraction left, BigFraction right)
        {
            return (left.Numerator * right.Denominator) == (right.Numerator * left.Denominator);
        }

        public static bool operator <=(BigFraction left, BigFraction right)
        {
            return !(left > right);
        }

        public static bool operator >=(BigFraction left, BigFraction right)
        {
            return (left > right) || (left == right);
        }

        public static bool operator <(BigFraction left, BigFraction right)
        {
            return !(left > right) && !(left == right);
        }

        public static bool operator !=(BigFraction left, BigFraction right)
        {
            return !(left == right);
        }

        public static bool operator >(BigFraction left, long right)
        {
            return (left.Numerator) > (right * left.Denominator);
        }

        public static bool operator ==(BigFraction left, long right)
        {
            return (left.Numerator) == (right * left.Denominator);
        }

        public static bool operator <=(BigFraction left, long right)
        {
            return !(left > right);
        }

        public static bool operator >=(BigFraction left, long right)
        {
            return (left > right) || (left == right);
        }

        public static bool operator <(BigFraction left, long right)
        {
            return !(left > right) && !(left == right);
        }

        public static bool operator !=(BigFraction left, long right)
        {
            return !(left == right);
        }

        #endregion

        private void Reduce()
        {
            // If the numerator is 0, set the denominator to 1.
            if (Numerator.IsZero)
            {
                Denominator = 1;
                return;
            }

            List<int> firstPrimes = new List<int> { 2 }; //, 3, 5 };

            // Reduce; if the numerator and denominator are divisible by the given prime, divide both.
            foreach (var prime in firstPrimes)
            {
                while (Numerator != 0 && Numerator % prime == 0 && Denominator % prime == 0)
                {
                    Numerator /= prime;
                    Denominator /= prime;
                }
            }

            // Catch; if the numerator is 0, set the actual denominator to 1.
            if (Numerator.IsZero)
            {
                Denominator = 1;
            }
        }

        public long GetValue()
        {
            return (long)(Numerator / Denominator);
        }

        /// <summary>
        /// Return the fraction as a base-10 decimal.
        /// Pretty slow; each digit is calculated individually (time complexity:  O(log N))
        /// </summary>
        /// <returns>The fraction as a base-10 decimal.</returns>
        public string GetValueDecimalString()
        {
            var copy = (BigFraction)this.MemberwiseClone();
            var wholeNumber = copy.ReduceToFractionalPart();
            if (copy.Numerator == 0)
            {
                return wholeNumber.ToString();
            }

            var result = $"{wholeNumber}.";
            while (copy.Numerator > 0)
            {
                copy *= 10;
                result += copy.ReduceToFractionalPart();
            }
            return result;
        }

        /// <summary>
        /// Split the fraction into its Whole and Fractional parts, return the Whole part, then set the fraction to its Fractional
        /// part.  The resulting fraction is less than 1.
        /// </summary>
        /// <returns></returns>
        public BigInteger ReduceToFractionalPart()
        {
            // Get the copy's numerator.  We're returning this at the end.
            var wholeNumberPart = Numerator / Denominator;
            if (wholeNumberPart == 0)
            {
                // The decimal is less than 1; we don't need to change anything.
                return 0;
            }

            Numerator -= (wholeNumberPart * Denominator);
            Reduce();
            return wholeNumberPart;
        }

        public bool IsLessThanOne()
        {
            return Numerator < Denominator;
        }

        /// <summary>
        /// Multiply the fraction by 'maxRange', return the whole number part, and set the fraction to the decimal part.
        /// eg. If this object's value is 0.7, and we want to return Pop(5):
        ///   Multiply the object's value by 5.  new value: 3.5
        ///   Return 3, and set the object's value to 0.5.
        /// </summary>
        /// <param name="maxRange"></param>
        /// <returns></returns>
        public int Yield(int maxRange)
        {
            this.Numerator *= maxRange;
            this.Reduce();

            return -1;
        }


        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public override int GetHashCode()
        {
            // TODO: This means '4/2' is different from '2/1'.  Is this OK?
            return $"{this.Numerator}|{this.Denominator}".GetHashCode();
        }

        #region IEquatable

        public override bool Equals(object obj)
        {
            return this == (BigFraction)obj;
        }

        public bool Equals(BigFraction other)
        {
            return this == other;
        }

        #endregion
    }
}
