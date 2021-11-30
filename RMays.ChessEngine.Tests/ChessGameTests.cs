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

            Assert.IsTrue(game.IsGameInProgress());
        }

        [TestMethod]
        public void ImportChessGame_GetPgnValue()
        {
            var game = new ChessGame();
            game.LoadPGN(gameCheckmateWhite);
            Assert.AreEqual("1-0", game.GetPgnValue("Result"));
            Assert.AreEqual("Zukertort Opening: Old Indian Attack", game.GetPgnValue("Opening"));
            Assert.AreEqual("", game.GetPgnValue("Not A Real Key"));
        }

        [TestMethod]
        public void ImportChessGame_GetFirstMove()
        {
            var game = new ChessGame();
            game.LoadPGN(gameCheckmateWhite);
            var move0 = game.GetMove(0);
            Assert.AreEqual("Ng1-f3", move0.ToString());
            Assert.AreEqual("Nf3", move0.SanString);
        }

        [TestMethod]
        public void ImportChessGame_RemoveComments()
        {
            var game = new ChessGame();
            game.LoadPGN(gameCheckmateWhite);
            var expected = "1. Nf3 d5 2. d3 c6 3. Nbd2 e6 4. e4 Be7 5. e5 g5 6. d4 g4 7. Ng1 h5 8. h3 g3 9. fxg3 c5 10. c3 Nc6 11. Ngf3 a6 12. Nb3 b5 13. Nxc5 Bxc5 14. dxc5 Qc7 15. Bf4 Nge7 16. b4 Ng6 17. Bd3 Ngxe5 18. Nxe5 Nxe5 19. Bc2 f6 20. Qe2 Qg7 21. O-O Bd7 22. Rae1 O-O-O 23. Bxe5 fxe5 24. Qxe5 Qxe5 25. Rxe5 Kc7 26. Rf7 Rde8 27. a4 Kc8 28. axb5 axb5 29. Re1 Rhg8 30. Ra1 Rd8 31. Ra8+ Kb7 32. Rxd8 Rxd8 33. Bg6 Kc6 34. Bxh5 Ra8 35. g4 Ra3 36. Kh2 Rxc3 37. g5 Ra3 38. g6 Ra8 39. g7 Rg8 40. Bg6 Be8 41. Rf6 Bxg6 42. Rxg6 Kd7 43. h4 Ke7 44. h5 Kf7 45. Rg3 Rxg7 46. Rxg7+ Kxg7 47. c6 d4 48. c7 d3 49. c8=Q Kf6 50. Qd7 e5 51. Qxd3 Ke6 52. Qe4 Kd6 53. Qb7 Ke6 54. Qxb5 Kf6 55. Qc5 e4 56. b5 e3 57. Qxe3 Kf5 58. b6 Kg4 59. b7 Kf5 60. b8=Q Kg4 61. Qbg3+ Kf5 62. Qee5# 1-0";
            Assert.AreEqual(expected, game.GetPGNWithoutComments());
        }

        [TestMethod]
        public void ImportChessGame_CanRead_Game1()
        {
            var game = new ChessGame();
            game.LoadPGN(gameCheckmateWhite);
        }

        [TestMethod]
        public void ImportChessGame_CanRead_Game2()
        {
            var game = new ChessGame();
            game.LoadPGN(gameTimeoutForfeit);
        }

        [TestMethod]
        public void ImportChessGame_CanRead_Game3()
        {
            var game = new ChessGame();
            game.LoadPGN(gameCheckmateBlack);
        }

        [TestMethod]
        public void ImportChessGame_CanRead_Game4()
        {
            var game = new ChessGame();
            game.LoadPGN(gameStalemate);
        }

        [TestMethod]
        public void PrintGame()
        {
            // TODO: Figure out what the PGN should be, and check the result.
            var game = new ChessGame();
            game.LoadPGN(gameCheckmateWhite);
            //game.LoadPGN(gameCheckmateBlack);
            //game.LoadPGN(gameTimeoutForfeit);
            Console.WriteLine("Original PGN:");
            Console.WriteLine(game.GetPGNWithoutComments());
            Console.WriteLine("Smart PGN:");
            Console.WriteLine(game.GetPGN());
        }

        [TestMethod]
        public void OnlyFirstMoves()
        {
            var game = new ChessGame();
            for (int i = 0; i < 20; i++)
            {
                game.MakeMove(i);
            }

            // Print the board after making 20 move 0s.
            game.PrintBoard();

            // Get the PGN.
            Console.WriteLine("{" + game.GetPGN() + "}");
        }


        #region PGN Games

        private static string gameCheckmateWhite = @"[Event ""Rated Bullet game""]
[Site ""https://lichess.org/BAW7dKK6""]
[Date ""2021.10.05""]
[White ""ARM-777777""]
[Black ""DrWondeful""]
[Result ""1-0""]
[UTCDate ""2021.10.05""]
[UTCTime ""15:54:14""]
[WhiteElo ""2971""]
[BlackElo ""2911""]
[WhiteRatingDiff ""+5""]
[BlackRatingDiff ""-5""]
[WhiteTitle ""GM""]
[Variant ""Standard""]
[TimeControl ""30+0""]
[ECO ""A06""]
[Opening ""Zukertort Opening: Old Indian Attack""]
[Termination ""Normal""]
[Annotator ""lichess.org""]

1. Nf3 d5 2. d3 { A06 Zukertort Opening: Old Indian Attack } c6 3. Nbd2 e6 4. e4 Be7 5. e5 g5 6. d4 g4 7. Ng1 h5 8. h3 g3 9. fxg3 c5 10. c3 Nc6 11. Ngf3 a6 12. Nb3 b5 13. Nxc5 Bxc5 14. dxc5 Qc7 15. Bf4 Nge7 16. b4 Ng6 17. Bd3 Ngxe5 18. Nxe5 Nxe5 19. Bc2 f6 20. Qe2 Qg7 21. O-O Bd7 22. Rae1 O-O-O 23. Bxe5 fxe5 24. Qxe5 Qxe5 25. Rxe5 Kc7 26. Rf7 Rde8 27. a4 Kc8 28. axb5 axb5 29. Re1 Rhg8 30. Ra1 Rd8 31. Ra8+ Kb7 32. Rxd8 Rxd8 33. Bg6 Kc6 34. Bxh5 Ra8 35. g4 Ra3 36. Kh2 Rxc3 37. g5 Ra3 38. g6 Ra8 39. g7 Rg8 40. Bg6 Be8 41. Rf6 Bxg6 42. Rxg6 Kd7 43. h4 Ke7 44. h5 Kf7 45. Rg3 Rxg7 46. Rxg7+ Kxg7 47. c6 d4 48. c7 d3 49. c8=Q Kf6 50. Qd7 e5 51. Qxd3 Ke6 52. Qe4 Kd6 53. Qb7 Ke6 54. Qxb5 Kf6 55. Qc5 e4 56. b5 e3 57. Qxe3 Kf5 58. b6 Kg4 59. b7 Kf5 60. b8=Q Kg4 61. Qbg3+ Kf5 62. Qee5# { White wins by checkmate. } 1-0";
        private static string gameTimeoutForfeit = @"[Event ""Rated Blitz game""]
[Site ""https://lichess.org/Jdx6npgd""]
[Date ""2021.10.05""]
[White ""Nadigraj""]
[Black ""AndreiMacovei""]
[Result ""0-1""]
[UTCDate ""2021.10.05""]
[UTCTime ""15:26:03""]
[WhiteElo ""2753""]
[BlackElo ""2683""]
[WhiteRatingDiff ""-6""]
[BlackRatingDiff ""+7""]
[BlackTitle ""IM""]
[Variant ""Standard""]
[TimeControl ""180+0""]
[ECO ""B50""]
[Opening ""Sicilian Defense: Modern Variations""]
[Termination ""Time forfeit""]
[Annotator ""lichess.org""]

1. e4 c5 2. Nf3 d6 { B50 Sicilian Defense: Modern Variations } 3. Bc4 Nc6 4. c3 Nf6 5. d4 cxd4 6. cxd4 Nxe4 7. d5 Qa5+?! { (-0.23 → 0.63) Inaccuracy. Ne5 was best. } (7... Ne5 8. Nxe5 dxe5 9. Qe2 Nd6 10. Nc3 Nxc4 11. Qxc4 e6 12. dxe6) 8. Bd2?! { (0.63 → -0.12) Inaccuracy. Nbd2 was best. } (8. Nbd2 Ne5 9. O-O Nxc4 10. Nxc4 Qa6 11. Qe2 f5 12. Ng5 g6) 8... Nxd2 9. Nbxd2 Ne5?! { (0.00 → 0.89) Inaccuracy. Nb8 was best. } (9... Nb8 10. b4) 10. Nxe5 dxe5 11. Qe2 Bd7?! { (0.90 → 1.59) Inaccuracy. g6 was best. } (11... g6 12. Qxe5) 12. Qxe5?? { (1.59 → 0.00) Blunder. O-O was best. } (12. O-O Qb6) 12... Rc8 13. Qe2 g6 14. O-O Bg7 15. Nb3? { (-0.14 → -1.66) Mistake. Rad1 was best. } (15. Rad1 O-O) 15... Qd8?! { (-1.66 → -0.95) Inaccuracy. Qb4 was best. } (15... Qb4 16. Rfe1 Bf6 17. Nd2 O-O 18. Rad1 Qxb2 19. Bb3 Rc7 20. d6 exd6 21. Ne4 Qxe2 22. Nxf6+) 16. d6 e6 17. Bb5 O-O?! { (-1.02 → -0.19) Inaccuracy. Bxb5 was best. } (17... Bxb5 18. Qxb5+ Qd7 19. Qb4 O-O 20. Rac1 Rfd8 21. Rxc8 Rxc8 22. Rd1 b6 23. Nd2 Qc6 24. h3) 18. Rad1 a6 19. Bxd7 Qxd7 20. Qe3?! { (-0.33 → -0.88) Inaccuracy. Rd2 was best. } (20. Rd2 Rfd8) 20... Rfd8 21. Nc5 Qc6 22. Ne4 f5 23. Ng5 Rxd6 24. Rxd6 Qxd6 25. Nxe6? { (-0.87 → -2.19) Mistake. g3 was best. } (25. g3) 25... Re8 26. Re1 Qd5 27. Qb6? { (-1.78 → -3.24) Mistake. h4 was best. } (27. h4) 27... Bf6?? { (-3.24 → -1.20) Blunder. Bd4 was best. } (27... Bd4 28. Qb3 Qxb3 29. axb3 Bxb2 30. Kf1 a5 31. Rb1 Rxe6 32. Rxb2 Kf7 33. Rc2 Rc6 34. Ra2) 28. g3 Kf7 29. Qc7+ Re7 30. Qc8?? { (-0.76 → -4.61) Blunder. Qd8 was best. } (30. Qd8 Qxd8) 30... Rxe6 31. Rxe6 Qxe6 32. Qxb7+ Be7 33. b3 Qd6 34. Kg2 a5 35. h4 Qc5 36. Qd7 Kf6 37. Qe8?! { (-3.87 → -4.96) Inaccuracy. a3 was best. } (37. a3 Qxa3) 37... Bf8?! { (-4.96 → -3.71) Inaccuracy. Kg7 was best. } (37... Kg7 38. Qd7 Qe5 39. Qd3 Bd6 40. h5 g5 41. Qc4 Kh6 42. Qc6 Qe4+ 43. Qxe4 fxe4 44. g4) 38. Qd8+ Kf7?! { (-4.41 → -3.44) Inaccuracy. Kg7 was best. } (38... Kg7 39. Qd7+) 39. Qd2?! { (-3.44 → -4.59) Inaccuracy. a3 was best. } (39. a3 h5 40. b4 Qc6+ 41. Kh2 axb4 42. axb4 Bxb4 43. Qd3 Be7 44. Qe2 Kf8 45. f3 Qc5) 39... Qc6+ 40. Kg1 Bb4 41. Qh6 Kg8 42. Qe3 Bc5 43. Qd2 Bb6 44. Qd3 Kf7 45. a4 Ke6 46. h5 Qe4 47. Qd1 Qd4?! { (-6.90 → -5.18) Inaccuracy. g5 was best. } (47... g5 48. h6 g4 49. Qc1 Qf3 50. Qe1+ Kd5 51. Qd2+ Kc6 52. Qe1 Qxg3+ 53. Kf1 Qd3+ 54. Qe2) 48. Qe2+ Kf6 49. hxg6 hxg6 50. Kg2 f4 51. Qf3 Kg5 52. gxf4+?! { (-5.83 → -8.32) Inaccuracy. Kf1 was best. } (52. Kf1 Bc5 53. gxf4+ Qxf4 54. Qg2+ Qg4 55. Qd5+ Qf5 56. Qg2+ Kf4 57. Qg3+ Ke4 58. Ke2 Kd4) 52... Qxf4 53. Qd5+ Qf5?! { (-10.86 → -5.55) Inaccuracy. Kh4 was best. } (53... Kh4 54. Qf3 Qg4+ 55. Qg3+ Qxg3+ 56. fxg3+ Kg4 57. Kf1 Kxg3 58. Ke2 g5 59. Kd3 g4 60. Kc4) 54. Qd2+ Kh5 55. Qe2+?! { (-5.60 → -8.45) Inaccuracy. f3 was best. } (55. f3) 55... Qg4+ 56. Qxg4+ Kxg4 57. f3+?! { (-64.04 → Mate in 24) Checkmate is now unavoidable. Kf1 was best. } (57. Kf1) 57... Kf4 58. Kh3?! { (-82.56 → Mate in 10) Checkmate is now unavoidable. Kf1 was best. } (58. Kf1 Kxf3) 58... Kxf3 59. Kh4 Ke3?! { (Mate in 8 → -60.20) Lost forced checkmate sequence. Bd8+ was best. } (59... Bd8+ 60. Kh3 g5 61. Kh2 g4 62. Kg1 Bb6+ 63. Kh2 Kf2 64. b4 Bc7+ 65. Kh1 g3 66. bxa5) 60. Kg5 Kd3 61. Kxg6 Kc3 62. Kf5 Kxb3 63. Ke4?! { (-63.77 → Mate in 25) Checkmate is now unavoidable. Ke6 was best. } (63. Ke6 Kxa4) 63... Kxa4 64. Kd3 Kb3 65. Kd2 Kb2 66. Kd3 a4 67. Kc4 a3 68. Kb5?! { (-61.74 → Mate in 13) Checkmate is now unavoidable. Kd5 was best. } (68. Kd5 Bg1 69. Ke4 Kb1 70. Kd5 Bf2 71. Kc6 a2 72. Kb7 Kc2 73. Kc8 Bg1 74. Kd8 Kb2) 68... a2 69. Kc6 a1=Q 70. Kb7 Qa5 { Black wins on time. } 0-1
";
        private static string gameCheckmateBlack = @"[Event ""Rated Bullet game""]
[Site ""https://lichess.org/g9e14M1R""]
[Date ""2019.12.24""]
[White ""dhoine98""]
[Black ""Templarium""]
[Result ""1/2-1/2""]
[UTCDate ""2019.12.24""]
[UTCTime ""02:40:52""]
[WhiteElo ""2862""]
[BlackElo ""2621""]
[WhiteRatingDiff ""-4""]
[BlackRatingDiff ""+3""]
[WhiteTitle ""GM""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""A46""]
[Opening ""Indian Defense: Knights Variation""]
[Termination ""Normal""]
[Annotator ""lichess.org""]

1. d4 Nf6 2. Nf3 { A46 Indian Defense: Knights Variation } e6 3. g3 d5 4. Bg2 Be7 5. O-O O-O 6. c4 dxc4 7. b3 cxb3 8. axb3 Nbd7 9. Nc3 a6 10. e4 Rb8 11. e5 Nd5 12. Nxd5 exd5 13. Be3 c5 14. dxc5 Nxc5 15. Nd4 Be6 16. f4 Ne4 17. f5 Bd7 18. f6 gxf6 19. Qh5 fxe5 20. Nf5 Bxf5 21. Rxf5 Bf6 22. Raf1 Bg7 23. Rxf7 Rxf7 24. Qxf7+ Kh8 25. Bxe4 dxe4 26. h4 Qd3 27. Re1 Rf8 28. Qxb7 Bh6 29. Qb6 Rg8 30. Qf6+ Bg7 31. Qf5 Rf8 32. Qg4 Rf3 33. Qc8+ Rf8 34. Qc4 Rf3 35. Qc8+ Rf8 36. Qc4 Rf3 37. Qc8+ { The game is a draw. } 1/2-1/2";
        private static string gameStalemate = @"[Event ""Rated Bullet game""]
[Site ""https://lichess.org/a4QzFcY9""]
[Date ""2021.10.05""]
[White ""I_aM_The_BulleT""]
[Black ""ARM-777777""]
[Result ""1/2-1/2""]
[UTCDate ""2021.10.05""]
[UTCTime ""16:17:09""]
[WhiteElo ""2807""]
[BlackElo ""2986""]
[WhiteRatingDiff ""+3""]
[BlackRatingDiff ""-3""]
[BlackTitle ""GM""]
[Variant ""Standard""]
[TimeControl ""60+0""]
[ECO ""B32""]
[Opening ""Sicilian Defense: Löwenthal Variation""]
[Termination ""Normal""]
[Annotator ""lichess.org""]

1. e4 c5 2. Nf3 Nc6 3. d4 cxd4 4. Nxd4 e5 { B32 Sicilian Defense: Löwenthal Variation } 5. Nxc6 dxc6 6. Bd3 Nf6 7. Nc3 Bc5 8. Bg5 Qd4 9. Qf3 Bg4 10. Qg3 Be6 11. Be3 Qb4 12. Bxc5 Qxc5 13. O-O-O O-O 14. f4 exf4 15. Qxf4 Ng4 16. Rhf1 Qe5 17. Qxe5 Nxe5 18. Be2 Rfd8 19. Rxd8+ Rxd8 20. Rd1 Rxd1+ 21. Kxd1 Kf8 22. Kd2 Ke7 23. Ke3 Kd6 24. Kd4 f6 25. b4 b6 26. a4 Bd7 27. b5 c5+ 28. Ke3 Be6 29. Bd3 g5 30. Be2 h6 31. h3 Bf7 32. Bd3 h5 33. Be2 h4 34. Bd3 Be6 35. Be2 Bd7 36. Bd3 Be6 37. Be2 Ng6 38. Bg4 Nf4 39. Bxe6 Kxe6 40. Kf3 Ke5 41. Nd1 Ne6 42. c3 Ng7 43. Ne3 Ke6 44. Nd5 Ne8 45. Ne3 Nd6 46. Nd1 Nc4 47. Nf2 Nb2 48. Ke3 Nxa4 49. Nd1 c4 50. Kd4 Nc5 51. Ne3 Nb3+ 52. Kxc4 Nc5 53. Kd4 g4 54. Nxg4 Nb3+ 55. Ke3 Nc5 56. Kf4 f5 57. exf5+ Kd5 58. Ne5 Ne6+ 59. fxe6 Kxe6 60. Nf3 a5 61. bxa6 Kd6 62. Nxh4 Kc7 63. a7 Kd6 64. a8=Q Kc5 65. Nf5 b5 66. Qe4 Kb6 67. Qb4 Ka6 68. Qxb5+ Kxb5 69. g4 Ka5 70. g5 Kb5 71. g6 Ka6 72. g7 Kb5 73. g8=Q Ka6 74. Qb3 Ka5 75. c4 Ka6 76. c5 Ka5 77. c6 Ka6 78. c7 Ka5 79. c8=Q { Draw by stalemate. } 1/2-1/2";
        #endregion
    }
}
