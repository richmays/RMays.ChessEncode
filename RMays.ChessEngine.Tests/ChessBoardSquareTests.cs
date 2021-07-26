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
    public class ChessBoardSquareTests
    {
        [DataTestMethod]
        [DataRow(1, 1, "a1")]
        [DataRow(8, 8, "h8")]
        [DataRow(1, 8, "a8")]
        [DataRow(8, 1, "h1")]
        [DataRow(2, 4, "b4")]
        [DataRow(7, 3, "g3")]
        public void IsValidSquare_Constructor(int file, int rank, string expectedAN)
        {
            var square = new ChessBoardSquare(file, rank);
            Assert.AreEqual(expectedAN, square.GetAN());
        }

        [DataTestMethod]
        [DataRow('a', 1, "a1")]
        [DataRow('h', 8, "h8")]
        [DataRow('a', 8, "a8")]
        [DataRow('h', 1, "h1")]
        [DataRow('b', 4, "b4")]
        [DataRow('g', 3, "g3")]
        public void IsValidSquare_ConstructWithChar(char file, int rank, string expectedAN)
        {
            var square = new ChessBoardSquare(file, rank);
            Assert.AreEqual(expectedAN, square.GetAN());
        }

        [DataTestMethod]
        [DataRow(1, 1, "a1")]
        [DataRow(8, 8, "h8")]
        [DataRow(1, 8, "a8")]
        [DataRow(8, 1, "h1")]
        [DataRow(2, 4, "b4")]
        [DataRow(7, 3, "g3")]
        public void IsValidSquare_StaticMethod(int file, int rank, string expectedAN)
        {
            var anResult = ChessBoardSquare.GetAN(file, rank);
            Assert.AreEqual(expectedAN, anResult);
        }

    }
}
