using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rmays.ChessEngine
{
    /// <summary>
    /// Contains properties and methods associated with a chess game
    /// (ruleset (default: Standard Chess), position of pieces, all moves played, information about the players,
    ///   whether or not each player can castle / en passant)
    /// In essence, a ChessGame is a board state (as FEN) and the history of moves.
    /// This will do two things:
    /// 1. Return a list of possible moves (used for encoding / decoding text)
    /// 2. 
    /// </summary>
    public class ChessGame
    {
        protected ChessBoardState boardState;

        /// <summary>
        /// Initialize a chess game.
        /// </summary>
        public ChessGame()
        {
            // Create a new ChessBoardState.
            // This keeps track of the position of all pieces, whether or not each king can castle to each side,
            // en passant capture squares, whose turn it is, and number of moves made since the last pawn move / capture.
            boardState = new ChessBoardState();
            boardState.Initialize();
        }

        public void PrintBoard()
        {
            Console.WriteLine(boardState.ToString());

        }

        /// <summary>
        /// Returns a list of chess moves from the given position.
        /// </summary>
        /// <returns></returns>
        public List<ChessMove> PossibleMoves()
        {
            return new List<ChessMove>();
        }
    }
}
