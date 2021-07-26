using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rmays.ChessEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.ChessEngine.Tests
{
    [TestClass]
    public class ChessBoardStateTests
    {
        [TestMethod]
        public void PrintDefaultBoard()
        {
            var board = new ChessBoardState();
            Console.WriteLine(board.ToString());
            Assert.AreEqual(@"
BR BN BB BQ BK BB BN BR 
BP BP BP BP BP BP BP BP 
-- -- -- -- -- -- -- -- 
-- -- -- -- -- -- -- -- 
-- -- -- -- -- -- -- -- 
-- -- -- -- -- -- -- -- 
WP WP WP WP WP WP WP WP 
WR WN WB WQ WK WB WN WR".Trim(), board.ToString().Trim());
        }

        [TestMethod]
        public void GetDefaultFEN()
        {
            var board = new ChessBoardState();
            var boardFEN = board.GetFEN();
            Assert.AreEqual("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", boardFEN);
        }

        [TestMethod]
        public void GetPossibleFirstMovesForWhite()
        {
            var board = new ChessBoardState();
            var moves = board.PossibleMoves().Select(x => x.ToString()).ToList();
            var expectedMoves = new List<string>
            {
                "Nb1-a3",
                "Nb1-c3",
                "Ng1-f3",
                "Ng1-h3",
                "Pa2-a3",
                "Pa2-a4",
                "Pb2-b3",
                "Pb2-b4",
                "Pc2-c3",
                "Pc2-c4",
                "Pd2-d3",
                "Pd2-d4",
                "Pe2-e3",
                "Pe2-e4",
                "Pf2-f3",
                "Pf2-f4",
                "Pg2-g3",
                "Pg2-g4",
                "Ph2-h3",
                "Ph2-h4"
            };

            Assert.AreEqual(expectedMoves.Count(), moves.Count(), $"The move counts were different.  Expected {expectedMoves.Count()}, but returned {moves.Count()}.");
            for(int i = 0; i < expectedMoves.Count(); i++)
            {
                Assert.AreEqual(expectedMoves[i], moves[i]);
            }
        }

        [TestMethod]
        public void GetPossibleMovesForBishop()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 5, ChessPiece.WhiteBishop);
            var moves = board.PossibleMoves().Select(x => x.ToString()).ToList();
            var expectedMoves = new List<string>
            {
                "Bd5-a2",
                "Bd5-a8",
                "Bd5-b3",
                "Bd5-b7",
                "Bd5-c4",
                "Bd5-c6",
                "Bd5-e4",
                "Bd5-e6",
                "Bd5-f3",
                "Bd5-f7",
                "Bd5-g2",
                "Bd5-g8",
                "Bd5-h1"
            };

            Assert.AreEqual(expectedMoves.Count(), moves.Count(), $"The move counts were different.  Expected {expectedMoves.Count()}, but returned {moves.Count()}.");
            for (int i = 0; i < expectedMoves.Count(); i++)
            {
                Assert.AreEqual(expectedMoves[i], moves[i]);
            }
        }

        [TestMethod]
        public void GetPossibleMovesForRook()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 5, ChessPiece.WhiteRook);
            var moves = board.PossibleMoves().Select(x => x.ToString()).ToList();
            var expectedMoves = new List<string>
            {
                "Rd5-a5",
                "Rd5-b5",
                "Rd5-c5",
                "Rd5-d1",
                "Rd5-d2",
                "Rd5-d3",
                "Rd5-d4",
                "Rd5-d6",
                "Rd5-d7",
                "Rd5-d8",
                "Rd5-e5",
                "Rd5-f5",
                "Rd5-g5",
                "Rd5-h5"
            };

            Assert.AreEqual(expectedMoves.Count(), moves.Count(), $"The move counts were different.  Expected {expectedMoves.Count()}, but returned {moves.Count()}.");
            for (int i = 0; i < expectedMoves.Count(); i++)
            {
                Assert.AreEqual(expectedMoves[i], moves[i]);
            }
        }


        [TestMethod]
        public void GetPossibleMovesForKing()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 5, ChessPiece.WhiteKing);
            var moves = board.PossibleMoves().Select(x => x.ToString()).ToList();
            var expectedMoves = new List<string>
            {
                "Kd5-c4",
                "Kd5-c5",
                "Kd5-c6",
                "Kd5-d4",
                "Kd5-d6",
                "Kd5-e4",
                "Kd5-e5",
                "Kd5-e6",
            };

            Assert.AreEqual(expectedMoves.Count(), moves.Count(), $"The move counts were different.  Expected {expectedMoves.Count()}, but returned {moves.Count()}.");
            for (int i = 0; i < expectedMoves.Count(); i++)
            {
                Assert.AreEqual(expectedMoves[i], moves[i]);
            }
        }

        [TestMethod]
        public void GetPossibleMovesForQueen()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 5, ChessPiece.WhiteQueen);
            var moves = board.PossibleMoves().Select(x => x.ToString()).ToList();
            var expectedMoves = new List<string>
            {
                "Qd5-a2",
                "Qd5-a5",
                "Qd5-a8",
                "Qd5-b3",
                "Qd5-b5",
                "Qd5-b7",
                "Qd5-c4",
                "Qd5-c5",
                "Qd5-c6",
                "Qd5-d1",
                "Qd5-d2",
                "Qd5-d3",
                "Qd5-d4",
                "Qd5-d6",
                "Qd5-d7",
                "Qd5-d8",
                "Qd5-e4",
                "Qd5-e5",
                "Qd5-e6",
                "Qd5-f3",
                "Qd5-f5",
                "Qd5-f7",
                "Qd5-g2",
                "Qd5-g5",
                "Qd5-g8",
                "Qd5-h1",
                "Qd5-h5"
            };

            Assert.AreEqual(expectedMoves.Count(), moves.Count(), $"The move counts were different.  Expected {expectedMoves.Count()}, but returned {moves.Count()}.");
            for (int i = 0; i < expectedMoves.Count(); i++)
            {
                Assert.AreEqual(expectedMoves[i], moves[i]);
            }
        }
    }
}
