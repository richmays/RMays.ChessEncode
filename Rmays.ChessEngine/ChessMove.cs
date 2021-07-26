using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rmays.ChessEngine
{
    /// <summary>
    /// Represents a valid move on a chess board.
    /// The representation can be made in different ways; these are handled with this class.
    /// </summary>
    public class ChessMove
    {
        /// <summary>
        /// The starting square in Standard Algebraic Notation (eg, 'e4').
        /// </summary>
        public string StartSquare { get; set; }

        /// <summary>
        /// The end square in Standard Algebraic Notation (eg, 'e4').
        /// </summary>
        public string EndSquare { get; set; }

        /// <summary>
        /// Which piece made the move?
        /// </summary>
        public ChessPiece Piece { get; set; }

        /// <summary>
        /// Was this a capturing move?
        /// </summary>
        public bool WasPieceCaptured { get; set; } = false;

        /// <summary>
        /// Was this a kingside castle?
        /// </summary>
        public bool KingsideCastle { get; set; } = false;

        /// <summary>
        /// Was this a queenside castle?
        /// </summary>
        public bool QueensideCastle { get; set; } = false;

        /// <summary>
        /// Does this move put the opposing king in check?
        /// </summary>
        public bool IsCheckingMove { get; set; } = false;

        /// <summary>
        /// Does this move put the opposing king in checkmate?
        /// </summary>
        public bool IsCheckmateMove { get; set; } = false;

        /// <summary>
        /// If this was a pawn promotion move, what did the pawn get promoted to?
        /// </summary>
        public PromotionChessPiece PawnPromotedTo { get; set; } = PromotionChessPiece.None;

        /// <summary>
        /// Return the move in Standard Algebraic Notation (SAN).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var showSourceSquare = true;

            if (KingsideCastle)
            {
                return "O-O"
                    + $"{(IsCheckingMove ? "+" : "")}"
                    + $"{(IsCheckmateMove ? "#" : "")}";
            }
            else if (QueensideCastle)
            {
                return "O-O-O"
                    + $"{(IsCheckingMove ? "+" : "")}"
                    + $"{(IsCheckmateMove ? "#" : "")}";
            }

            var result = $"{GetPieceInitial(Piece)}"
                + (showSourceSquare ?
                        $"{StartSquare}" +
                        $"{(WasPieceCaptured ? "x" : "-")}"
                    : "")
                + $"{EndSquare}"
                + $"{(PawnPromotedTo == PromotionChessPiece.None ? "" : "=" + GetPromotionPieceInitial(PawnPromotedTo))}"
                + $"{(IsCheckingMove ? "+" : "")}"
                + $"{(IsCheckmateMove ? "#" : "")}";

            return result;
        }

        protected char GetPieceInitial(ChessPiece piece)
        {
            if (Math.Abs((int)piece) == 2)
            {
                return 'N';
            }
            return piece.ToString()[5];
        }

        protected string GetPromotionPieceInitial(PromotionChessPiece piece)
        {
            if (Math.Abs((int)piece) == 2)
            {
                return "n";
            }
            return piece.ToString()[0].ToString().ToLower();
        }
    }

    public enum PromotionChessPiece
    {
        Queen = 5,
        Rook = 4,
        Bishop = 3,
        Knight = 2,
        None = 0
    }
}
