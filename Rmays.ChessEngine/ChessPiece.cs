using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rmays.ChessEngine
{
    /// <summary>
    /// Represents a piece on a chess board.  Each space must be occupied by exactly one element of this enumeration.
    /// </summary>
    public enum ChessPiece
    {
        BlackKing = -6,
        BlackQueen = -5,
        BlackRook = -4,
        BlackBishop = -3,
        BlackKnight = -2,
        BlackPawn = -1,
        Space = 0,
        WhitePawn = 1,
        WhiteKnight = 2,
        WhiteBishop = 3,
        WhiteRook = 4,
        WhiteQueen = 5,
        WhiteKing = 6,

        /// <summary>
        /// This position is outside of the board; no piece can move here.
        /// </summary>
        OutsideBoardRange = 7
    }
}
