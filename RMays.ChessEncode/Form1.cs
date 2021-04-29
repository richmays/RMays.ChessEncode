using RMays.ChessEncode.Common;
using System;
using System.Numerics;
using System.Windows.Forms;

namespace RMays.ChessEncode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtPlaintext_TextChanged(object sender, EventArgs e)
        {
            txtBase64.Text = TextEncoder.EncodeToBase64(txtPlaintext.Text);
        }

        private void txtBase64_TextChanged(object sender, EventArgs e)
        {
            // For every 4 characters in the Base64 text, there's 3 UTF-8 characters encoded.
            var plaintextLength = 3 * (txtBase64.Text.Length / 4);

            // If the Base64 text ends with '=' (the Base64 padding character), the plaintext length is one less.
            // There can be up to 2 '=' at the end of a Base64-encoded string.
            if (txtBase64.Text.EndsWith("=="))
            {
                plaintextLength -= 2;
            }
            else if (txtBase64.Text.EndsWith("="))
            {
                plaintextLength -= 1;
            }

            
            var numerator = TextEncoder.GetNumeratorFromBase64(txtBase64.Text);
            var denominator = BigInteger.Pow(2, plaintextLength * 8);
            txtBigNumerator.Text = numerator.ToString();
            txtBigDenominator.Text = denominator.ToString();

            // This is sufficient for encoding a full message with no data loss (numerator and either the denominator OR plaintext message length).

            // To encode (to turn this long numerator into chess moves, assuming each player has one of 30 moves to make):
            //   Create a new variable that represents N/D of the original message.  (This can be done with BigInteger for numerator and denominator.)
            //   Then, if we want to pull an integer value in the range [0,30),
            //     we can multiply the numerator by 30, and divide without remainder.
            // Let's do an example with real numbers.
            //   Encoded N/D:  16705/(2^16)) (the denominator will always be a power of 256.)
            //   Get first encoded digit (from 0 to 30, but not 30):
            //     Multiply the N/D by 30.  (Shortcut: Multiply N by 15, divide D by 2.)
            //     (16705*15)/(2^15)
            //     Now, figure out the first digit.  (Do the division.)
            //     250575/32768
            //     .. which is the same as: 7 + 21199/32768
            //   First digit: 7 (this is the first chess move)
            //     Repeat process for next digits.
            //     Multiply the N/D by 30.  (Shortcut: Multiply N by 15, divide D by 2.)
            //     (21199*15)/(2^14)
            //     Figure out next digit.  (Do the division.)
            //     317985/16384
            //     .. which is the same as: 19 + 6689/16384
            //   Next digit: 19 (this is the 2nd chess move)
            //     Multiply the N/D by 30.  (Shortcut: Multiply N by 15, divide D by 2.)
            //     (6689*15)/(2^13)
            //     Division:
            //     ....
            // Tools needed to make this happen:
            //   New class: BigFraction (has 2 parts: Numerator (BigInteger) and Denominator (BigInteger).)

            // To decode:
            //   For simplicity, let's assume we're seeing moves on a chess board.
            //   Each player, in turn, has X possible moves (in an ordered list), then chooses 1 of those moves.
            //   We can build up a running 'numerator' and 'denominator' to approximate the original message.
            //     Numerator (N) starts at 0, denominator (D) starts at 1.
            //     For move X out of Y:
            //       N = (N * Y) + X;
            //       D *= Y;
            //   We can derive the complete message when D >= PlaintextMessageLength.
            //   We can derive partial messages for any (N, D) because we'll have a Range of actual N we can use.
            // ^ Possible, in theory.  But difficult.  Let's find an easier way.


            // This is a tricky problem.  We don't have infinite precision.  (We don't Need infinite precision.)
            // One ultimate goal is to get back to the original BigInteger; then we'd know the original message Exactly.
            //   We know the plaintext length (given to us in the chessgame header).
            // Subgoal (nice-to-have): Decode the message one byte (move?) at a time.  (eg. move 'a4' is move 0 out of 20 for white, so ...
            //   ... the decimal is between 0.00 and 0.05 (dividing the search space into 20 chunks, and taking the first chunk.)
            //   If the next move is 'a6' (move 0 out of 20 for black), then we'd divide that space into another 20 chunks, and take the first one,
            //   which is 0.000 through 0.004.  So we know the first two digits in the decimal are '00'.  Not sure if this helps us.
            // OR: Instead of using an infinite-precision decimal, let's use a big integer.  That makes more sense.  As we get each digit,
            //   we learn more about the moves.  And we can turn each move into a range (just like before), because we know the total range
            //   of the encoded text-integer (it's 2^[messagelength]).  So we can keep track of a 'min' and 'max', and narrowing it down until we get
            //   to the final plaintext.
            //    ^ Won't work because we'd have fractional parts.  If, for example, the final number is somewhere in [0,10), and we find out that
            //    it's in chunk 2 of 6, we can't easily divide the search space.
            // Maybe revisit the infinite decimal [0-1)?  I don't want to have to make a calculation across ALL digits in the number each time we want
            //   to pop a digit.  (It would be O(n^2), and that's just terrible.  We want O(n).)

            // GetValueString

            if (chkAutoUpdate.Checked)
            {
                /*
                var f = new BaseTwoFraction(numerator, plaintextLength * 8);
                txtDecimal.Text = f.GetValueDecimalString();
                */
                var f = new BigFraction(numerator, denominator);
                txtDecimal.Text = f.GetValueDecimalString();
            }
            else
            {
                txtDecimal.Text = "";
            }
        }

        private void txtBigNumerator_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            txtBase64_TextChanged(sender, e);
        }
    }
}
