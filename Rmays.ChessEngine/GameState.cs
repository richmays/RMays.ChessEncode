using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rmays.ChessEngine
{
    public enum GameState
    {
        /// <summary>
        /// Game is in progress; no winner has been decided.
        /// </summary>
        InProgress,

        /// <summary>
        /// White has won, either by checkmating Black, or Black resigned.
        /// </summary>
        WhiteWins,

        /// <summary>
        /// Black has won, either by checkmating White, or White resigned.
        /// </summary>
        BlackWins,

        /// <summary>
        /// No legal moves remain, and the current player isn't in check.
        /// </summary>
        Stalemate,

        /// <summary>
        /// Both players agree on a draw.
        /// </summary>
        DrawAgreement,

        /// <summary>
        /// Not enough material on the board for a checkmate.
        /// </summary>
        DrawInsufficientMaterial,

        /// <summary>
        /// The same position has occurred at least 3 times in this game, and a draw was claimed by one of the players.
        /// </summary>
        DrawRepetition,

        /// <summary>
        /// At least 50 moves have been made (100 half-moves) with no captures or pawn moves, and a draw was claimed by one of the players.
        /// </summary>
        Draw50Moves,

        /// <summary>
        /// At least 75 moves have been made (150 half-moves) with no captures or pawn moves.  Happens automatically; it isn't claimed by players.
        /// </summary>
        Draw75Moves
    }
}
