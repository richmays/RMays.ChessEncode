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
    public class ChessMoveTests
    {
        [TestMethod]
        public void CreateKingsideCastle()
        {
            var move = new ChessMove { KingsideCastle = true };
            Assert.AreEqual("O-O", move.ToString());
        }

        [TestMethod]
        public void CreateQueensideCastle()
        {
            var move = new ChessMove { QueensideCastle = true };
            Assert.AreEqual("O-O-O", move.ToString());
        }

        [TestMethod]
        public void PushEPawn()
        {
            var move = new ChessMove {
                StartSquare = ChessBoardSquare.GetAN(5, 2),
                EndSquare = ChessBoardSquare.GetAN(5, 4),
                Piece = ChessPiece.WhitePawn
            };
            Assert.AreEqual("Pe2-e4", move.ToString());
        }

        [TestMethod]
        public void PromotionCaptureCheckmate()
        {
            var move = new ChessMove
            {
                StartSquare = ChessBoardSquare.GetAN(1, 7),
                EndSquare = ChessBoardSquare.GetAN(2, 8),
                WasPieceCaptured = true,
                PawnPromotedTo = PromotionChessPiece.Queen,
                IsCheckmateMove = true,
                Piece = ChessPiece.WhitePawn
            };
            Assert.AreEqual("Pa7xb8=q#", move.ToString());
        }
    }
}
