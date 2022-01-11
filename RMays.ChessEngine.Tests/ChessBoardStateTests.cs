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

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetPossibleFirstMovesForBlack()
        {
            var board = new ChessBoardState();
            board.TryMakeMove(5); // White moves a pawn.

            var expectedMoves = new List<string>
            {
                "Pa7-a5",
                "Pa7-a6",
                "Pb7-b5",
                "Pb7-b6",
                "Pc7-c5",
                "Pc7-c6",
                "Pd7-d5",
                "Pd7-d6",
                "Pe7-e5",
                "Pe7-e6",
                "Pf7-f5",
                "Pf7-f6",
                "Pg7-g5",
                "Pg7-g6",
                "Ph7-h5",
                "Ph7-h6",
                "Nb8-a6",
                "Nb8-c6",
                "Ng8-f6",
                "Ng8-h6"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteKnight()
        {
            GetMoves_Knight(ChessColor.White);
        }

        [TestMethod]
        public void GetMoves_BlackKnight()
        {
            GetMoves_Knight(ChessColor.Black);
        }

        private void GetMoves_Knight(ChessColor color)
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(color);
            board.SetSpot(4, 5, color == ChessColor.White ? ChessPiece.WhiteKnight : ChessPiece.BlackKnight);

            var expectedMoves = new List<string>
            {
                "Nd5-c3",
                "Nd5-e3",
                "Nd5-b4",
                "Nd5-f4",
                "Nd5-b6",
                "Nd5-f6",
                "Nd5-c7",
                "Nd5-e7"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteKnight_Corner()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(1, 1, ChessPiece.WhiteKnight);

            var expectedMoves = new List<string>
            {
                "Na1-c2",
                "Na1-b3"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_BlackKnight_Corner()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.Black);
            board.SetSpot(1, 1, ChessPiece.BlackKnight);

            var expectedMoves = new List<string>
            {
                "Na1-c2",
                "Na1-b3"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteBishop()
        {
            GetMoves_Bishop(ChessColor.White);
        }

        [TestMethod]
        public void GetMoves_BlackBishop()
        {
            GetMoves_Bishop(ChessColor.Black);
        }

        private void GetMoves_Bishop(ChessColor color)
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(color);
            board.SetSpot(4, 5, color == ChessColor.White ? ChessPiece.WhiteBishop : ChessPiece.BlackBishop);

            var expectedMoves = new List<string>
            {
                "Bd5-h1",
                "Bd5-a2",
                "Bd5-g2",
                "Bd5-b3",
                "Bd5-f3",
                "Bd5-c4",
                "Bd5-e4",
                "Bd5-c6",
                "Bd5-e6",
                "Bd5-b7",
                "Bd5-f7",
                "Bd5-a8",
                "Bd5-g8"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteRook()
        {
            GetMoves_Rook(ChessColor.White);
        }

        [TestMethod]
        public void GetMoves_BlackRook()
        {
            GetMoves_Rook(ChessColor.Black);
        }

        public void GetMoves_Rook(ChessColor color)
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(color);
            board.SetSpot(4, 5, color == ChessColor.White ? ChessPiece.WhiteRook : ChessPiece.BlackRook);

            var expectedMoves = new List<string>
            {
                "Rd5-d1",
                "Rd5-d2",
                "Rd5-d3",
                "Rd5-d4",
                "Rd5-a5",
                "Rd5-b5",
                "Rd5-c5",
                "Rd5-e5",
                "Rd5-f5",
                "Rd5-g5",
                "Rd5-h5",
                "Rd5-d6",
                "Rd5-d7",
                "Rd5-d8"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteKing()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 5, ChessPiece.WhiteKing);

            var expectedMoves = new List<string>
            {
                "Kd5-c4",
                "Kd5-d4",
                "Kd5-e4",
                "Kd5-c5",
                "Kd5-e5",
                "Kd5-c6",
                "Kd5-d6",
                "Kd5-e6"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteKing_CanCastleBothSides()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(1, 1, ChessPiece.WhiteRook);
            board.SetSpot(8, 1, ChessPiece.WhiteRook);
            board.SetSpot(1, 2, ChessPiece.BlackRook);
            board.SetSpot(8, 2, ChessPiece.BlackRook);
            board.SetWhiteCanCastleKingside(true);
            board.SetWhiteCanCastleQueenside(true);

            var expectedMoves = new List<string>
            {
                // A1 Rook
                "Ra1-b1",
                "Ra1-c1",
                "Ra1-d1",
                "Ra1xa2",

                // King
                "O-O-O",
                "Ke1-d1",
                "Ke1-f1",
                "O-O",

                // H1 Rook
                "Rh1-f1",
                "Rh1-g1",
                "Rh1xh2"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_BlackKing_CanCastleQueenside()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(5, 1, ChessPiece.WhiteRook);
            board.SetSpot(6, 1, ChessPiece.WhiteRook);
            board.SetSpot(7, 1, ChessPiece.WhiteKing);

            board.SetSpot(1, 2, ChessPiece.WhitePawn);
            board.SetSpot(3, 2, ChessPiece.WhiteBishop);
            board.SetSpot(5, 2, ChessPiece.WhiteQueen);
            board.SetSpot(7, 2, ChessPiece.WhitePawn);

            board.SetSpot(3, 3, ChessPiece.WhitePawn);
            board.SetSpot(7, 3, ChessPiece.WhitePawn);
            board.SetSpot(8, 3, ChessPiece.WhitePawn);

            board.SetSpot(2, 4, ChessPiece.WhitePawn);
            board.SetSpot(6, 4, ChessPiece.WhiteBishop);

            board.SetSpot(2, 5, ChessPiece.BlackPawn);
            board.SetSpot(3, 5, ChessPiece.WhitePawn);
            board.SetSpot(4, 5, ChessPiece.BlackPawn);
            board.SetSpot(5, 5, ChessPiece.BlackKnight);
            board.SetSpot(8, 5, ChessPiece.BlackPawn);

            board.SetSpot(1, 6, ChessPiece.BlackPawn);
            board.SetSpot(5, 6, ChessPiece.BlackPawn);
            board.SetSpot(6, 6, ChessPiece.BlackPawn);

            board.SetSpot(4, 7, ChessPiece.BlackBishop);
            board.SetSpot(7, 7, ChessPiece.BlackQueen);

            board.SetSpot(1, 8, ChessPiece.BlackRook);
            board.SetSpot(5, 8, ChessPiece.BlackKing);
            board.SetSpot(8, 8, ChessPiece.BlackRook);

            board.SetWhiteCanCastleKingside(false);
            board.SetWhiteCanCastleQueenside(false);
            board.SetBlackCanCastleKingside(true);
            board.SetBlackCanCastleQueenside(true);
            board.SetSideToMove(ChessColor.Black);

            var moves = board.PossibleMoves();
            Assert.AreEqual(1, moves.Count(x => x.QueensideCastle), "Black should be allowed to castle queenside.");
            Assert.AreEqual(1, moves.Count(x => x.KingsideCastle), "Black should be allowed to castle kingside.");
        }

        [TestMethod]
        public void GetMoves_WhiteKing_CantCastleWhileInCheck()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(1, 1, ChessPiece.WhiteRook);
            board.SetSpot(8, 1, ChessPiece.WhiteRook);
            board.SetSpot(1, 2, ChessPiece.BlackRook);
            board.SetSpot(8, 2, ChessPiece.BlackRook);
            board.SetSpot(5, 8, ChessPiece.BlackRook);

            board.SetWhiteCanCastleKingside(true);
            board.SetWhiteCanCastleQueenside(true);

            // White is able to castle, but can't because they're in check.  They can only move their king out of the way.
            var expectedMoves = new List<string>
            {
                // A1 Rook
                //"Ra1-b1",
                //"Ra1-c1",
                //"Ra1-d1",
                //"Ra1xa2",

                // King
                //"O-O-O",
                "Ke1-d1",
                "Ke1-f1",
                //"O-O",

                // H1 Rook
                //"Rh1-f1",
                //"Rh1-g1",
                //"Rh1xh2"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteKing_CastleRights_AcrossCheck_QSDest()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(5, 8, ChessPiece.BlackKing);
            board.SetSpot(1, 1, ChessPiece.WhiteRook);
            board.SetSpot(8, 1, ChessPiece.WhiteRook);
            board.SetSpot(1, 2, ChessPiece.BlackRook);
            board.SetSpot(3, 3, ChessPiece.BlackRook); // King can't castle queenside when there's a Black Rook on this square.
            board.SetSpot(8, 2, ChessPiece.BlackRook);
            board.SetWhiteCanCastleKingside(true);
            board.SetWhiteCanCastleQueenside(true);

            var expectedMoves = new List<string>
            {
                // A1 Rook
                "Ra1-b1",
                "Ra1-c1",
                "Ra1-d1",
                "Ra1xa2",

                // King
                //"O-O-O",
                "Ke1-d1",
                "Ke1-f1",
                "O-O",

                // H1 Rook
                "Rh1-f1",
                "Rh1-g1",
                "Rh1xh2"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteKing_CastleRights_AcrossCheck_QSJump()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(5, 8, ChessPiece.BlackKing);
            board.SetSpot(1, 1, ChessPiece.WhiteRook);
            board.SetSpot(8, 1, ChessPiece.WhiteRook);
            board.SetSpot(1, 2, ChessPiece.BlackRook);
            board.SetSpot(4, 3, ChessPiece.BlackRook); // King can't castle queenside when there's a Black Rook on this square.
            board.SetSpot(8, 2, ChessPiece.BlackRook);
            board.SetWhiteCanCastleKingside(true);
            board.SetWhiteCanCastleQueenside(true);

            var expectedMoves = new List<string>
            {
                // A1 Rook
                "Ra1-b1",
                "Ra1-c1",
                "Ra1-d1",
                "Ra1xa2",

                // King
                //"O-O-O",
                //"Ke1-d1",
                "Ke1-f1",
                "O-O",

                // H1 Rook
                "Rh1-f1",
                "Rh1-g1",
                "Rh1xh2"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteKing_CastleRights_AcrossCheck_KSDest()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(5, 8, ChessPiece.BlackKing);
            board.SetSpot(1, 1, ChessPiece.WhiteRook);
            board.SetSpot(8, 1, ChessPiece.WhiteRook);
            board.SetSpot(1, 2, ChessPiece.BlackRook);
            board.SetSpot(7, 3, ChessPiece.BlackRook); // King can't castle kingside when there's a Black Rook on this square.
            board.SetSpot(8, 2, ChessPiece.BlackRook);
            board.SetWhiteCanCastleKingside(true);
            board.SetWhiteCanCastleQueenside(true);

            var expectedMoves = new List<string>
            {
                // A1 Rook
                "Ra1-b1",
                "Ra1-c1",
                "Ra1-d1",
                "Ra1xa2",

                // King
                "O-O-O",
                "Ke1-d1",
                "Ke1-f1",
                //"O-O",

                // H1 Rook
                "Rh1-f1",
                "Rh1-g1",
                "Rh1xh2"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteKing_CastleRights_AcrossCheck_KSJump()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(5, 8, ChessPiece.BlackKing);
            board.SetSpot(1, 1, ChessPiece.WhiteRook);
            board.SetSpot(8, 1, ChessPiece.WhiteRook);
            board.SetSpot(1, 2, ChessPiece.BlackRook);
            board.SetSpot(6, 3, ChessPiece.BlackRook); // King can't castle kingside when there's a Black Rook on this square.
            board.SetSpot(8, 2, ChessPiece.BlackRook);
            board.SetWhiteCanCastleKingside(true);
            board.SetWhiteCanCastleQueenside(true);

            var expectedMoves = new List<string>
            {
                // A1 Rook
                "Ra1-b1",
                "Ra1-c1",
                "Ra1-d1",
                "Ra1xa2",

                // King
                "O-O-O",
                "Ke1-d1",
                //"Ke1-f1",
                //"O-O",

                // H1 Rook
                "Rh1-f1",
                "Rh1-g1",
                "Rh1xh2"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteKing_WithCapture()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 5, ChessPiece.WhiteKing);
            board.SetSpot(4, 6, ChessPiece.BlackPawn);
            board.SetSpot(2, 2, ChessPiece.BlackPawn);
            board.SetSpot(3, 2, ChessPiece.BlackKing);

            var expectedMoves = new List<string>
            {
                "Kd5-c4",
                "Kd5-d4",
                "Kd5-e4",
                //"Kd5-c5", // Black pawn controls this square
                //"Kd5-e5", // Black pawn controls this square
                "Kd5-c6",
                "Kd5xd6",
                "Kd5-e6"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhiteQueen()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 5, ChessPiece.WhiteQueen);

            var expectedMoves = new List<string>
            {
                "Qd5-d1",
                "Qd5-h1",
                "Qd5-a2",
                "Qd5-d2",
                "Qd5-g2",
                "Qd5-b3",
                "Qd5-d3",
                "Qd5-f3",
                "Qd5-c4",
                "Qd5-d4",
                "Qd5-e4",
                "Qd5-a5",
                "Qd5-b5",
                "Qd5-c5",
                "Qd5-e5",
                "Qd5-f5",
                "Qd5-g5",
                "Qd5-h5",
                "Qd5-c6",
                "Qd5-d6",
                "Qd5-e6",
                "Qd5-b7",
                "Qd5-d7",
                "Qd5-f7",
                "Qd5-a8",
                "Qd5-d8",
                "Qd5-g8"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhitePawn_PushCapture()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 5, ChessPiece.WhitePawn);
            board.SetSpot(3, 6, ChessPiece.BlackPawn);
            board.SetSpot(5, 6, ChessPiece.BlackPawn);

            var expectedMoves = new List<string>
            {
                "Pd5xc6",
                "Pd5-d6",
                "Pd5xe6",
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhitePawn_PromotionCapture()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 7, ChessPiece.WhitePawn);
            board.SetSpot(3, 8, ChessPiece.BlackKnight);
            board.SetSpot(5, 8, ChessPiece.BlackKnight);

            var expectedMoves = new List<string>
            {
                "Pd7xc8=b",
                "Pd7xc8=n",
                "Pd7xc8=q",
                "Pd7xc8=r",
                "Pd7-d8=b",
                "Pd7-d8=n",
                "Pd7-d8=q",
                "Pd7-d8=r",
                "Pd7xe8=b",
                "Pd7xe8=n",
                "Pd7xe8=q",
                "Pd7xe8=r",
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhitePawn_PromotionCapture_Blockers()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 7, ChessPiece.WhitePawn);
            board.SetSpot(3, 8, ChessPiece.BlackKnight);
            board.SetSpot(5, 8, ChessPiece.WhiteKnight);

            var expectedMoves = new List<string>
            {
                "Pd7xc8=b",
                "Pd7xc8=n",
                "Pd7xc8=q",
                "Pd7xc8=r",
                "Pd7-d8=b",
                "Pd7-d8=n",
                "Pd7-d8=q",
                "Pd7-d8=r",

                // Knight moves (don't forget!)
                "Ne8-d6",
                "Ne8-f6",
                "Ne8-c7",
                "Ne8-g7"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_WhitePawn_PushCapture_StartFile()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(4, 2, ChessPiece.WhitePawn);
            board.SetSpot(3, 3, ChessPiece.BlackPawn);
            board.SetSpot(5, 3, ChessPiece.BlackPawn);

            var expectedMoves = new List<string>
            {
                "Pd2xc3",
                "Pd2-d3",
                "Pd2xe3",
                "Pd2-d4"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_BlackPawn_EnPassant()
        {
            /*
{-- -- -- -- -- -- -- --
 -- -- -- -- -- -- -- --
 -- BP -- -- BK -- -- --
 BP WP -- -- -- -- -- --
 -- -- -- -- -- WK -- BP
 -- -- WP -- -- WN -- WP
 -- -- -- -- -- -- WP --
 -- -- -- -- -- -- -- --
 }
             * */
            var board = new ChessBoardState();
            board.Clear();
            board.SetBlackCanCastleKingside(false);
            board.SetBlackCanCastleQueenside(false);
            board.SetWhiteCanCastleKingside(false);
            board.SetWhiteCanCastleQueenside(false);
            board.SetSideToMove(ChessColor.Black);

            board.SetSpot(7, 2, ChessPiece.WhitePawn);

            board.SetSpot(3, 3, ChessPiece.WhitePawn);
            board.SetSpot(6, 3, ChessPiece.WhiteKnight);
            board.SetSpot(8, 3, ChessPiece.WhitePawn);

            board.SetSpot(6, 4, ChessPiece.WhiteKing);
            board.SetSpot(8, 4, ChessPiece.BlackPawn);

            //board.SetSpot(1, 5, ChessPiece.BlackPawn);
            board.SetSpot(2, 5, ChessPiece.WhitePawn);

            board.SetSpot(2, 6, ChessPiece.BlackPawn);
            board.SetSpot(5, 6, ChessPiece.BlackKing);

            // for en passant
            board.SetSpot(1, 7, ChessPiece.BlackPawn);

            var moveBlackPushPawn = new ChessMove
            {
                Piece = ChessPiece.BlackPawn,
                StartSquare = "a7",
                EndSquare = "a5"
            };

            Assert.AreEqual("", board.GetEnPassantTargetSquare(), $"En passant target square should be blank, but it was [{board.GetEnPassantTargetSquare()}].");
            Assert.IsTrue(board.TryMakeMove(moveBlackPushPawn), "Prerequisite failed; we couldn't push the A pawn.");
            Assert.AreEqual("a6", board.GetEnPassantTargetSquare(), $"En passant target square should be a6, but it was [{board.GetEnPassantTargetSquare()}].");

            var possibleMoves = board.PossibleMoves();

            // Verify that white can capture the pawn en passant.
            Assert.IsTrue(board.PossibleMoves().Select(x => x.ToString()).Contains("Pb5xa6"));

            var moveWhiteCapEnPassant = new ChessMove
            {
                Piece = ChessPiece.WhitePawn,
                StartSquare = "b5",
                EndSquare = "a6",
                WasPieceCaptured = true
            };

            Assert.IsTrue(board.TryMakeMove(moveWhiteCapEnPassant), "Failed to capture en passant.");
            Assert.AreEqual("", board.GetEnPassantTargetSquare(), $"After capturing en passant, the en passant target square should be blank, but it was [{board.GetEnPassantTargetSquare()}].");

            // NOW, there should NOT be a black pawn on A5; it was captured in teh previous move.
            Assert.IsTrue(board.GetSpot(1, 5) == ChessPiece.Space, $"The spot 'a5' should be empty (it was a black pawn captured by a white pawn en passant), but it has: {board.GetSpot(1, 5)}.");
        }

        [TestMethod]
        public void GetMoves_BlackPawn_PushCapture()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.Black);
            board.SetSpot(4, 5, ChessPiece.BlackPawn);
            board.SetSpot(3, 4, ChessPiece.WhitePawn);
            board.SetSpot(5, 4, ChessPiece.WhitePawn);

            var expectedMoves = new List<string>
            {
                "Pd5xc4",
                "Pd5-d4",
                "Pd5xe4",
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_BlackPawn_PromotionCapture()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.Black);
            board.SetSpot(4, 2, ChessPiece.BlackPawn);
            board.SetSpot(3, 1, ChessPiece.WhiteKnight);
            board.SetSpot(5, 1, ChessPiece.WhiteKnight);

            var expectedMoves = new List<string>
            {
                "Pd2xc1=b",
                "Pd2xc1=n",
                "Pd2xc1=q",
                "Pd2xc1=r",
                "Pd2-d1=b",
                "Pd2-d1=n",
                "Pd2-d1=q",
                "Pd2-d1=r",
                "Pd2xe1=b",
                "Pd2xe1=n",
                "Pd2xe1=q",
                "Pd2xe1=r",
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_BlackPawn_PromotionCapture_Blockers()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.Black);
            board.SetSpot(4, 2, ChessPiece.BlackPawn);
            board.SetSpot(3, 1, ChessPiece.WhiteKnight);
            board.SetSpot(5, 1, ChessPiece.BlackKnight);

            var expectedMoves = new List<string>
            {
                // Knight moves (don't forget!)
                "Ne1-c2",
                "Ne1-g2",
                "Ne1-d3",
                "Ne1-f3",

                "Pd2xc1=b",
                "Pd2xc1=n",
                "Pd2xc1=q",
                "Pd2xc1=r",
                "Pd2-d1=b",
                "Pd2-d1=n",
                "Pd2-d1=q",
                "Pd2-d1=r",
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        /// <summary>
        /// Black is in check; they can only move their king out of the way.  (The queen can't move because the king is in check.)
        /// </summary>
        [TestMethod]
        public void GetMoves_WhiteQueen_SimpleCheck()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.Black);
            board.SetSpot(2, 2, ChessPiece.WhiteQueen);
            board.SetSpot(1, 1, ChessPiece.WhiteKing);
            board.SetSpot(4, 4, ChessPiece.BlackKing);
            board.SetSpot(8, 8, ChessPiece.BlackQueen);

            var expectedMoves = new List<string>
            {
                "Kd4-d3",
                "Kd4-e3",
                "Kd4-c4",
                "Kd4-e4",
                "Kd4-c5",
                "Kd4-d5"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        /// <summary>
        /// Players are allowed to make a move that puts their opponent's king into check.
        /// (Probably obvious, but it's important to verify.)
        /// </summary>
        [TestMethod]
        public void GetMoves_WhiteCanDeliverCheck()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSpot(1, 1, ChessPiece.WhiteRook);
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(5, 8, ChessPiece.BlackKing);
            board.SetSpot(1, 8, ChessPiece.BlackRook);

            var expectedMoves = new List<string>
            {
                "Ra1-b1",
                "Ra1-c1",
                "Ra1-d1",
                "Ra1-a2",
                "Ra1-a3",
                "Ra1-a4",
                "Ra1-a5",
                "Ra1-a6",
                "Ra1-a7",
                "Ra1xa8",
                "Ke1-d1",
                "Ke1-f1",
                "Ke1-d2",
                "Ke1-e2",
                "Ke1-f2",
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        /// <summary>
        /// Simple checkmate; it's white's turn, but they're in check and they can't move.
        /// </summary>
        [TestMethod]
        public void GetMoves_Checkmate()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.White);
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(5, 8, ChessPiece.BlackKing);
            board.SetSpot(1, 1, ChessPiece.BlackRook);
            board.SetSpot(2, 2, ChessPiece.BlackRook);

            var expectedMoves = new List<string>
            {
            };

            CheckPossibleMoves(board, expectedMoves);
            var isGameOver = board.IsGameOver(out int gameResult);
            Assert.IsTrue(isGameOver, "The game should be over, but it's not.");
            Assert.AreEqual(-1, gameResult, $"Black should be the winner, but they're not.  Expected result of -1 (Black wins), but result was {gameResult}.");
        }

        /// <summary>
        /// Simple stalemate; it's white's turn, but they're NOT in check and they can't move.
        /// </summary>
        [TestMethod]
        public void GetMoves_Stalemate()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.White);
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(5, 8, ChessPiece.BlackKing);
            board.SetSpot(4, 8, ChessPiece.BlackRook);
            board.SetSpot(6, 8, ChessPiece.BlackRook);
            board.SetSpot(2, 2, ChessPiece.BlackRook);

            var expectedMoves = new List<string>
            {
            };

            CheckPossibleMoves(board, expectedMoves);
            var isGameOver = board.IsGameOver(out int gameResult);
            Assert.IsTrue(isGameOver, "The game should be over, but it's not.");
            Assert.AreEqual(0, gameResult, $"The game should be a stalemate, but it's not.  Expected result of 0 (stalemate), but result was {gameResult}.");
        }

        /// <summary>
        /// White's turn; they can take black's pawn en passant in two ways.
        /// </summary>
        [TestMethod]
        public void GetMoves_WhitePawn_EnPassant()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.White);
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(5, 8, ChessPiece.BlackKing);

            // Black's pawn has doublemoved.
            board.SetSpot(3, 5, ChessPiece.BlackPawn);

            // White has two pawns that could capture it.
            board.SetSpot(2, 5, ChessPiece.WhitePawn);
            board.SetSpot(4, 5, ChessPiece.WhitePawn);

            board.SetEnPassantCaptureSquare(3, 6);

            var expectedMoves = new List<string>
            {
                "Ke1-d1",
                "Ke1-f1",
                "Ke1-d2",
                "Ke1-e2",
                "Ke1-f2",

                "Pb5-b6",
                "Pb5xc6", // En Passant Capture

                "Pd5xc6", // En Passant Capture
                "Pd5-d6"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        /// <summary>
        /// Scholar's mate; make the moves, and White wins with a checkmate.
        /// </summary>
        [TestMethod]
        public void GetMoves_ScholarsMate()
        {
            var board = new ChessBoardState();
            PrintBoardState(board);
            board.TryMakeMove(13); // Pe2-e4
            PrintBoardState(board);
            board.TryMakeMove(8);  //          .. Pe7-e5
            PrintBoardState(board);
            board.TryMakeMove(9);  // Bf1-c4
            PrintBoardState(board);
            board.TryMakeMove(15);  //          .. Nb8-c6
            PrintBoardState(board);
            board.TryMakeMove(5);  // Qd1-h5
            PrintBoardState(board);
            board.TryMakeMove(25);  //          .. Ng8-f6
            PrintBoardState(board);
            board.TryMakeMove(41);  // Qh5-f7++
            PrintBoardState(board);

            var expectedMoves = new List<string>
            {
            };

            CheckPossibleMoves(board, expectedMoves);

            var isGameOver = board.IsGameOver(out int gameResult);
            Assert.IsTrue(isGameOver, "The game should be over, but it's not.");
            Assert.AreEqual(1, gameResult, $"The game should be a White win, but it's not.  Expected result of 1 (White wins), but result was {gameResult}.");
        }

        [TestMethod]
        public void GetMoves_InsufficientMaterial_TwoKings()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.White);
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(5, 8, ChessPiece.BlackKing);

            var expectedMoves = new List<string>
            {
            };

            CheckPossibleMoves(board, expectedMoves);

            // Game should be over due to insufficient material.
            var isGameOver = board.IsGameOver(out var gameResult);
            Assert.IsTrue(isGameOver);
            Assert.AreEqual(0, gameResult);
        }

        [TestMethod]
        public void GetMoves_InsufficientMaterial_K_vs_KN()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.White);
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(2, 1, ChessPiece.WhiteKnight);
            board.SetSpot(5, 8, ChessPiece.BlackKing);

            var expectedMoves = new List<string>
            {
            };

            CheckPossibleMoves(board, expectedMoves);

            // Game should be over due to insufficient material.
            var isGameOver = board.IsGameOver(out var gameResult);
            Assert.IsTrue(isGameOver);
            Assert.AreEqual(0, gameResult);
        }

        [TestMethod]
        public void GetMoves_InsufficientMaterial_K_vs_KB()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.White);
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(3, 1, ChessPiece.WhiteBishop);
            board.SetSpot(5, 8, ChessPiece.BlackKing);

            var expectedMoves = new List<string>
            {
            };

            CheckPossibleMoves(board, expectedMoves);

            // Game should be over due to insufficient material.
            var isGameOver = board.IsGameOver(out var gameResult);
            Assert.IsTrue(isGameOver);
            Assert.AreEqual(0, gameResult);
        }

        /// <summary>
        /// White king can't capture a piece to put itself in check,
        /// even if it results in insufficient material.
        /// </summary>
        [TestMethod]
        public void GetMoves_KB_vs_KB()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.Black);
            board.SetSpot(1, 1, ChessPiece.WhiteKing);
            board.SetSpot(2, 2, ChessPiece.WhiteBishop);
            board.SetSpot(3, 1, ChessPiece.BlackKing);
            board.SetSpot(4, 6, ChessPiece.BlackBishop);

            var expectedMoves = new List<string>
            {
                "Kc1-d1",
                "Kc1-c2",
                "Kc1-d2"
            };

            CheckPossibleMoves(board, expectedMoves);
        }

        [TestMethod]
        public void GetMoves_InsufficientMaterial_KB_vs_KB()
        {
            var board = new ChessBoardState();
            board.Clear();
            board.SetSideToMove(ChessColor.White);
            board.SetSpot(5, 1, ChessPiece.WhiteKing);
            board.SetSpot(3, 1, ChessPiece.WhiteBishop);
            board.SetSpot(5, 8, ChessPiece.BlackKing);
            board.SetSpot(3, 8, ChessPiece.BlackBishop);

            var expectedMoves = new List<string>
            {
            };

            CheckPossibleMoves(board, expectedMoves);

            // Game should be over due to insufficient material.
            // Bishops are on opposite-colored squares.
            var isGameOver = board.IsGameOver(out var gameResult);
            Assert.IsTrue(isGameOver);
            Assert.AreEqual(0, gameResult);
        }

        private void PrintBoardState(ChessBoardState board)
        {
            Console.WriteLine("******************************************");
            Console.WriteLine((board.GetSideToMove() == ChessColor.White ? "White" : "Black") + "'s turn.");
            var moves = board.PossibleMoves().Select(x => x.ToString()).ToList();
            Console.WriteLine(board);
            int moveId = 0;
            foreach (var move in moves)
            {
                Console.WriteLine($"{moveId}: {move}");
                moveId++;
            }
        }

        private void CheckPossibleMoves(ChessBoardState board, List<string> expectedMoves)
        {
            var moves = board.PossibleMoves().Select(x => x.ToString()).ToList();
            Console.WriteLine(board);
            foreach (var move in moves)
            {
                Console.WriteLine(move);
            }

            Assert.AreEqual(expectedMoves.Count(), moves.Count(), $"The move counts were different.  Expected {expectedMoves.Count()}, but returned {moves.Count()}.");
            for (int i = 0; i < expectedMoves.Count(); i++)
            {
                Assert.AreEqual(expectedMoves[i], moves[i]);
            }
        }
    }
}
