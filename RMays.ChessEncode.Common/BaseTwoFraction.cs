using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RMays.ChessEncode.Common
{
    /// <summary>
    /// Fraction where the numerator is an integer, AND the denominator is a positif power of 2. (eg. 123 / 2^8)
    /// OR, a real number that's a product of 2 to the power of an integer (eg. 123 * 2^-8)
    /// The numerator is never divisible by 2.  (Fractions are automatically reduced.)
    /// Similar to 'BigInteger'.
    /// NOTE: This won't work; we need to create a new class called 'BigFraction'.   Not a problem.mtg st
    /// </summary>
    public class BaseTwoFraction : IEquatable<BaseTwoFraction>
    {
        private BigInteger Numerator = BigInteger.Zero;
        private int DenominatorPow2 = 1;

        public BaseTwoFraction()
        {
        }

        public BaseTwoFraction(BigInteger numerator, int denominatorPow2)
        {
            Numerator = numerator;
            DenominatorPow2 = denominatorPow2;
            Reduce();
        }

        public static BaseTwoFraction operator *(BaseTwoFraction left, long right)
        {
            var result = new BaseTwoFraction(left.Numerator * right, left.DenominatorPow2);
            result.Reduce();
            return result;
        }

        #region Equality / Inequality Operators

        public static bool operator >(BaseTwoFraction left, BaseTwoFraction right)
        {
            return left.GetValue() > right.GetValue();
        }

        public static bool operator ==(BaseTwoFraction left, BaseTwoFraction right)
        {
            return left.Numerator == right.Numerator && left.DenominatorPow2 == right.DenominatorPow2;
        }

        public static bool operator <=(BaseTwoFraction left, BaseTwoFraction right)
        {
            return !(left > right);
        }

        public static bool operator >=(BaseTwoFraction left, BaseTwoFraction right)
        {
            return (left > right) || (left == right);
        }

        public static bool operator <(BaseTwoFraction left, BaseTwoFraction right)
        {
            return !(left > right) && !(left == right);
        }

        public static bool operator !=(BaseTwoFraction left, BaseTwoFraction right)
        {
            return !(left == right);
        }

        #endregion

        private void Reduce()
        {
            // If the numerator is 0, set the actual denominator to 1 (which means set DenominatorPow2 to 0.)
            if (Numerator.IsZero)
            {
                DenominatorPow2 = 0;
                return;
            }

            // Make sure the numerator is positif.
            while (DenominatorPow2 < 0)
            {
                Numerator *= 2;
                DenominatorPow2++;
            }

            // Reduce; divide the top by 2 until the top is odd.
            while (!Numerator.IsZero && Numerator.IsEven && DenominatorPow2 > 0)
            {
                Numerator /= 2;
                DenominatorPow2--;
            }

            // Catch; if the numerator is 0, set the actual denominator to 1 (which means set DenominatorPow2 to 0.)
            if (Numerator.IsZero)
            {
                DenominatorPow2 = 0;
            }
        }

        public long GetValue()
        {
            var result = Numerator;
            for(int i = 0; i < DenominatorPow2; i++)
            {
                if (result == 0) return 0;
                result /= 2;
            }

            return (long)result;
        }

        /// <summary>
        /// Return the fraction as a base-10 decimal.
        /// Pretty slow; each digit is calculated individually (time complexity:  O(log N))
        /// </summary>
        /// <returns>The fraction as a base-10 decimal.</returns>
        public string GetValueDecimalString()
        {
            var copy = (BaseTwoFraction)this.MemberwiseClone();
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
            var copy = (BaseTwoFraction)this.MemberwiseClone();
            var numerator = copy.Numerator;
            var denominatorPow2 = copy.DenominatorPow2;
            while (denominatorPow2 > 0 && numerator > 0)
            {
                numerator /= 2;
                denominatorPow2--;
            }

            if (numerator == 0)
            {
                // The decimal is less than 1; we don't need to change anything.
                return 0;
            }

            // Get the copy's numerator.  We're returning this at the end.
            var wholeNumberPart = numerator;

            // Copy the original to get the num and denum.
            // Keep multiplying num and denum by 2 until the denum matches the previous denum,
            // then find the difference in numerators, and assign to the original.
            copy = (BaseTwoFraction)this.MemberwiseClone();
            var wholeNumerator = numerator;
            var wholeDenominatorPow2 = denominatorPow2;
            while (wholeDenominatorPow2 < copy.DenominatorPow2)
            {
                wholeNumerator *= 2;
                wholeDenominatorPow2++;
            }

            this.Numerator = copy.Numerator - wholeNumerator;
            //this.DenominatorPow2 = copy.DenominatorPow2;

            return wholeNumberPart;
        }

        public bool IsLessThanOne()
        {
            var copy = (BaseTwoFraction)this.MemberwiseClone();
            var numerator = copy.Numerator;
            var denominatorPow2 = copy.DenominatorPow2;
            while (denominatorPow2 > 0 && numerator > 0)
            {
                numerator /= 2;
                denominatorPow2--;
            }

            return numerator == 0;
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
            return $"{Numerator}/2^{DenominatorPow2}";
        }

        public override int GetHashCode()
        {
            return $"{this.Numerator}|{this.DenominatorPow2}".GetHashCode();
        }

        #region IEquatable

        public override bool Equals(object obj)
        {
            return this == (BaseTwoFraction)obj;
        }

        public bool Equals(BaseTwoFraction other)
        {
            return this == other;
        }

        #endregion
    }
}
