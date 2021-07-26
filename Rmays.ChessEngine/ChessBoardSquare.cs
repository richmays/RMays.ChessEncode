using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rmays.ChessEngine
{
    /// <summary>
    /// Represents a square on a chessboard.  Used for converting file/rank numbers into Standard Algebraic Notation (eg. 'e4').
    /// </summary>
    public class ChessBoardSquare
    {
        public int File;
        public int Rank;

        public ChessBoardSquare()
        {
            File = 1;
            Rank = 1;
        }

        public ChessBoardSquare(int newFile, int newRank)
        {
            if (newFile < 1 || newFile > 8 || newRank < 1 || newRank > 8)
            {
                File = 0;
                Rank = 0;
            }
            else
            {
                File = newFile;
                Rank = newRank;
            }
        }

        public ChessBoardSquare(char newFile, int newRank)
        {
            File = newFile.ToString().ToLower()[0] - 'a' + 1;
            Rank = newRank;
        }

        public ChessBoardSquare(string algebraicSquare)
            : this(algebraicSquare[0], int.Parse(algebraicSquare[1].ToString()))
        {
        }

        public string GetAN()
        {
            return $"{(char)(File + 'a' - 1)}{Rank}";
        }

        public static string GetAN(int newFile, int newRank)
        {
            return $"{(char)(newFile + 'a' - 1)}{newRank}";
        }

        public override string ToString()
        {
            return $"({File},{Rank})";
        }
    }
}
