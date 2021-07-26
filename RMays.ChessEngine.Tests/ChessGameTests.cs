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
    public class ChessGameTests
    {
        // There's a lot of overlap between what a ChessGame and ChessBoard represent.  As coded,
        // a ChessGame contains a ChessBoard.  (The ChessGame object contains the full history of moves.  That might
        // be the only difference.  Hmm.  We'll keep going.
        [TestMethod]
        public void ConstructNewChessGame()
        {
            var game = new ChessGame();
            game.PrintBoard();
        }
    }
}
