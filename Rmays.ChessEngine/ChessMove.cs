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
    public class ChessMove : ICloneable
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
        /// The SAN string of the move (as it appears in a chess game transcript; eg. 'e4' or 'Ne5')
        /// </summary>
        public string SanString { get; set; } = "";

        /// <summary>
        /// If this move came from a transcript, this is the original SAN string.
        /// Otherwise this is empty.
        /// For simplicity, this is assigned in an optional constructor.
        /// </summary>
        public string OriginalSanString { get; set; }

        /// <summary>
        /// If this was a pawn promotion move, what did the pawn get promoted to?
        /// </summary>
        public PromotionChessPiece PawnPromotedTo { get; set; } = PromotionChessPiece.None;

        public ChessMove()
        {
        }

        public ChessMove(string san)
        {
            OriginalSanString = san;
        }

        /// <summary>
        /// Return the move in long Standard Algebraic Notation (SAN).
        /// Includes redundant information ('P' prefix for pawns, starting square, ...)
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

        public char GetPieceInitial(ChessPiece piece)
        {
            if (Math.Abs((int)piece) == 2)
            {
                return 'N';
            }
            return piece.ToString()[5];
        }

        public string GetPromotionPieceInitial(PromotionChessPiece piece)
        {
            if (Math.Abs((int)piece) == 2)
            {
                return "n";
            }
            return piece.ToString()[0].ToString().ToLower();
        }

        public object Clone()
        {
            return new ChessMove
            {
                Piece = this.Piece,
                StartSquare = this.StartSquare,
                EndSquare = this.EndSquare,
                IsCheckingMove = this.IsCheckingMove,
                IsCheckmateMove = this.IsCheckmateMove,
                KingsideCastle = this.KingsideCastle,
                QueensideCastle = this.QueensideCastle,
                PawnPromotedTo = this.PawnPromotedTo,
                WasPieceCaptured = this.WasPieceCaptured,
                OriginalSanString = this.OriginalSanString,
                SanString = this.SanString
            };
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
