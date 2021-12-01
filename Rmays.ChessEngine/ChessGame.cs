using RMays.ChessEncode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        protected Dictionary<string, string> pgnValues = new Dictionary<string, string>();

        /// <summary>
        /// Sequential list of all moves (plies) made in the game.
        /// eg. In this game: '1. Nf3 d5 2. d3 c6', there will be 4 elements in the 'moves' list.
        /// </summary>
        protected List<ChessMove> moves;

        /// <summary>
        /// The PGN of the game without comments.
        /// </summary>
        protected string pgnWithoutComments;

        protected GameState gameState;

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
            moves = new List<ChessMove>();
            gameState = GameState.InProgress;
            pgnValues = new Dictionary<string, string>
            {
                { "Date", DateTime.Now.ToString("dd.MM.yyyy") },
                { "UTCDate", DateTime.UtcNow.ToString("dd.MM.yyyy") },
                { "UTCTime", DateTime.UtcNow.ToString("hh:mm:ss") }
            };
        }

        public void Encode(string plaintext)
        {
            pgnValues.Add("PlaintextLength", $"{plaintext.Length}");
            var messageLength = plaintext.Length;
            var numerator = TextEncoder.GetNumeratorFromBase64(TextEncoder.EncodeToBase64(plaintext));
            var denominator = BigInteger.Pow(256, messageLength);

            BigInteger runningDenominator = BigInteger.One;
            BigInteger goalDenominator = 2 * BigInteger.Pow(256, messageLength) + 1;

            //var chessMoves = new List<int>();
            var bigFraction = new BigFraction(numerator, denominator);
            while (bigFraction > 0 && runningDenominator < goalDenominator)
            {
                if (boardState.IsGameOver(out var dummy))
                {
                    break;
                }
                var possibleMoves = boardState.PossibleMoves();
                var chunkSize = possibleMoves.Count();
                bigFraction *= chunkSize;
                runningDenominator *= chunkSize;
                var nextValue = (int)bigFraction.ReduceToFractionalPart();
                this.MakeMove(nextValue);
            }
        }

        public static string Decode(string pgn)
        {
            var game = new ChessGame();
            game.LoadPGN(pgn);
            var gameInProgress = new ChessGame();
            var plaintextLength = game.pgnValues.ContainsKey("PlaintextLength") ? int.Parse(game.pgnValues["PlaintextLength"]) : 0;

            BigInteger numerator = BigInteger.Zero;
            BigInteger denominator = BigInteger.One;
            BigInteger goalDenominator = BigInteger.Pow(256, plaintextLength);
            var moveId = 0;
            while (gameInProgress.IsGameInProgress() && moveId < game.moves.Count() && (denominator < goalDenominator || goalDenominator == 1))
            {
                var possibleMoves = gameInProgress.boardState.PossibleMoves();
                var found = false;
                for (int i = 0; i < possibleMoves.Count(); i++)
                {
                    if (possibleMoves[i].ToString() == game.moves[moveId].ToString())
                    {
                        found = true;
                        // Found the move!  ID is 'i'.
                        numerator = numerator * possibleMoves.Count() + i;
                        denominator *= possibleMoves.Count();
                        gameInProgress.MakeMove(i);
                        moveId++;
                        break;
                    }
                }
                if (!found)
                {
                    Console.WriteLine($"Error: Move wasn't found in the list: {game.moves[moveId]}");
                    Console.WriteLine("Possible moves:");
                    foreach(var move in possibleMoves)
                    {
                        Console.WriteLine($"  {move}");
                    }
                    throw new ApplicationException("Error: Move not found in list.");
                }
            }

            // We have a big numerator and denominator.
            // Turn it into a string by peeling off 256 at a time.
            var result = "";
            var bigFraction = new BigFraction(numerator, denominator);

            for (int i = 0; i < (plaintextLength == 0 ? 1000 : plaintextLength); i++)
            //while(!bigFraction.IsZero())
            {
                bigFraction *= 256;
                var c = (char)bigFraction.ReduceToFractionalPart();
                result = $"{result}{c}";
            }

            // 'Hack' / Fix:
            // The last character has to be incremented by 1.
            // I'm not exactly sure why.  Something about finding a fraction between two other fractions,
            // where all 3 fractions have different denominators.
            if (plaintextLength > 0)
            {
                result = $"{result.Substring(0, result.Length - 1)}{(char)(result[result.Length - 1] + 1)}";
            }

            return result;
        }

        public void MakeMove(int moveId)
        {
            if (this.boardState.CurrentGameState != GameState.InProgress)
            {
                // Game is already over; jump out.
                return;
            }

            var possibleMoves = this.PossibleMoves();
            if (possibleMoves.Count() == 0)
            {
                // No moves left!
                return;
            }
            var move = possibleMoves[moveId % possibleMoves.Count()];
            move.SanString = ComputeSanString(this.boardState, move);
            this.boardState.TryMakeMove(move);
            this.moves.Add(move);
        }

        public void LoadPGN(string pgn)
        {
            pgnValues.Clear();
            foreach(var line in pgn.Split('\r').Select(x => x.Trim()).Where(x => x!=string.Empty))
            {
                if (line[0] == '[')
                {
                    var splitIndex = line.IndexOf(" \"");
                    if (splitIndex < 0)
                    {
                        throw new InvalidOperationException("Invalid PGN; line started with '[', but wasn't formatted correctly.  Expected ' \"'.");
                    }

                    var pgnKey = line.Substring(1, splitIndex - 1);
                    var pgnValue = line.Substring(splitIndex + 2, line.Length - splitIndex - 4);
                    pgnValues.Add(pgnKey, pgnValue);
                }
                else if (line[0] == '1')
                {
                    // Grab this entire line; this is the PGN of the game.
                    pgnWithoutComments = StripCommentsFromLine(line);

                    // Create an empty chess board.  We have to play the game as we read the moves
                    // so we'll know what each move is.
                    var board = new ChessBoardState();
                    board.Initialize();
                    // One line for all the moves.  Let's split them up.
                    // The only time a period appears in the list is after the move ID, so ... let's split on periods.
                    var fullMoves = pgnWithoutComments.Split(' ').Select(x => x.Trim()).Where(x => x != string.Empty).ToList();
                    var moveId = 1;
                    var nextPlayerMove = ChessColor.White;
                    var failedLines = 0;
                    foreach (var moveToken in fullMoves)
                    {
                        if (moveToken.EndsWith(".") || moveToken == "*")
                        {
                            // Move ID.  Don't process this.
                            continue;
                        }
                        //Console.WriteLine($"Processing move: [{moveToken}]...");

                        // Probably looks like ' Nf3 d5 2' or ' Nf3 d5' or ' Nf3'
                        //var halfMoves = fullMove.Split(' ').Where(x => x.Trim().Length > 0).ToList();
                        var currMove = GetAndPlayMoveFromSAN(board, nextPlayerMove, moveToken);
                        if (currMove != null)
                        {
                            moves.Add(currMove);
                            nextPlayerMove = (ChessColor)((int)nextPlayerMove * -1);
                            moveId++;
                        }
                        else
                        {
                            // Hopefully the game is over.  Hmm.
                            //Console.WriteLine($"*** Failed to process this move.  Is the game over?  Move: {moveToken}");
                            failedLines++;
                            if (failedLines >= 2)
                            {
                                throw new ApplicationException("Multiple lines failed to be processed; check the source PGN.");
                            }
                        }

                        //Console.WriteLine($"Done processing move: {moveToken}.");
                    }
                }
            }
        }

        public string GetPGNWithoutComments()
        {
            return this.pgnWithoutComments;
        }

        public string GetPGN()
        {
            var result = "";
            foreach(var kvp in this.pgnValues)
            {
                result += $"[{kvp.Key} \"{kvp.Value}\"]{Environment.NewLine}";
            }

            for (int i = 0; i < moves.Count(); i++)
            {
                if (i % 2 == 0)
                {
                    result += $"{i / 2 + 1}. ";
                }
                result += $"{moves[i].SanString} ";
            }

            return result;
        }

        /// <summary>
        /// Remove anything from the line between two curly brackets, including the curly brackets themselves.
        /// No escape characters.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string StripCommentsFromLine(string line)
        {
            var result = new StringBuilder();
            var insideCurly = false;
            var insideParens = false;
            foreach (var c in line.ToCharArray())
            {
                if (insideCurly)
                {
                    if (c == '}')
                    {
                        insideCurly = false;
                    }
                }
                else if (insideParens)
                {
                    if (c == ')')
                    {
                        insideParens = false;
                    }
                }
                else
                {
                    if (c == '{')
                    {
                        insideCurly = true;
                    }
                    else if (c == '(')
                    {
                        insideParens = true;
                    }
                    else
                    {
                        result.Append(c);
                    }
                }
            }

            return result.ToString().Trim().Replace("  ", " ");
        }

        public ChessMove GetMove(int moveId)
        {
            if (moveId < 0 || moveId > moves.Count())
            {
                return new ChessMove();
            }
            return moves[moveId];
        }

        private ChessMove GetAndPlayMoveFromSAN(ChessBoardState board, ChessColor color, string moveStr)
        {
            var origMoveStr = moveStr;

            // Check for the end-of-game.
            if (moveStr == "1-0")
            {
                this.gameState = GameState.WhiteWins;
                return null;
            }
            else if (moveStr == "0-1")
            {
                this.gameState = GameState.BlackWins;
                return null;
            }
            else if (moveStr == "1/2-1/2")
            {
                var possibleMoves = board.PossibleMoves();
                if (!possibleMoves.Any())
                {
                    this.gameState = GameState.Stalemate;
                }
                else
                {
                    // TODO: Figure out why this was a draw.  Maybe it doesn't matter.
                    this.gameState = GameState.DrawAgreement;
                }
                return null;
            }

            // Strip any move annotations (eg. '?', '??', '?!', '!', '!?', or '!!').
            moveStr = moveStr.Replace("?", "").Replace("!", "");

            // The last 2 characters are the destination square; the first character is the piece (except for pawns).
            // Let's handle each case.
            var allPossibleMoves = board.PossibleMoves();
            var move = new ChessMove(moveStr);
            if (moveStr.EndsWith("+"))
            {
                //move.IsCheckingMove = true;
                moveStr = moveStr.Substring(0, moveStr.Length - 1);
            }
            else if (moveStr.EndsWith("#"))
            {
                //move.IsCheckmateMove = true;
                moveStr = moveStr.Substring(0, moveStr.Length - 1);
            }

            if (moveStr == "O-O")
            {
                move = (ChessMove)allPossibleMoves.First(x => x.KingsideCastle).Clone();
            }
            else if (moveStr == "O-O-O")
            {
                move = (ChessMove)allPossibleMoves.First(x => x.QueensideCastle).Clone();
            }
            else
            {
                // Pawn promotion
                if (moveStr.Length > 3 && moveStr[moveStr.Length - 2] == '=')
                {
                    move.PawnPromotedTo = GetPromotionPieceByInitial(moveStr[moveStr.Length - 1]);
                    moveStr = moveStr.Substring(0, moveStr.Length - 2);
                }

                // Normal move (no check, no checkmate, no castling).
                // The last 2 chars are the destination square.
                // The first char is the piece (if it's uppercase); otherwise it's a pawn.
                if (moveStr[0] >= 'A' && moveStr[0] <= 'Z')
                {
                    move.Piece = GetPieceByInitial(color, moveStr[0]);
                    moveStr = moveStr.Substring(1);
                }
                else
                {
                    move.Piece = GetPieceByInitial(color, 'P');
                }

                if (moveStr.Contains('x'))
                {
                    move.WasPieceCaptured = true;
                    moveStr = moveStr.Replace("x", "");
                }

                // NOW, let's find all moves we Could make with this board,
                // and find the move that ENDS with the last 2 chars of moveStr,
                // and the piece moved is the given piece.
                // If there's multiple pieces that could make that move, look at the first character of the moveStr for a clue.
                var endSquare = moveStr.Substring(moveStr.Length - 2, 2);
                var possibleMoves = allPossibleMoves.Where(x => x.EndSquare == endSquare && x.Piece == move.Piece && (x.PawnPromotedTo == PromotionChessPiece.None || x.PawnPromotedTo == move.PawnPromotedTo));

                if (possibleMoves.Count() == 1)
                {
                    // We found exactly one match.  Perfect; let's grab the details we didn't have yet.
                    move = (ChessMove)possibleMoves.First().Clone();
                }
                else if (possibleMoves.Count() > 1)
                {
                    // Which move should we take?
                    // Get the starting zone (either a rank, file, or both) of the piece if it's ambiguous.
                    var startZone = moveStr.Substring(0, moveStr.Length - 2);

                    // If the first char is a letter, get the piece that matches.
                    if (startZone.Length == 2)
                    {
                        move = (ChessMove)possibleMoves.First(x => x.StartSquare == startZone).Clone();
                    }
                    else if (startZone[0] >= 'a' && startZone[0] <= 'z')
                    {
                        move = (ChessMove)possibleMoves.First(x => x.StartSquare[0] == startZone[0]).Clone();
                    }
                    else if (startZone[0] >= '1' && startZone[0] <= '8')
                    {
                        move = (ChessMove)possibleMoves.First(x => x.StartSquare[1] == startZone[0]).Clone();
                    }
                    else
                    {
                        throw new InvalidOperationException($"Couldn't translate a zone into a start square.  moveStr: {moveStr}, startZone: {startZone}");
                    }
                }

                //Console.WriteLine($"Remaining: {moveStr}");
            }

            move.SanString = ComputeSanString(board, move);

            if (!board.TryMakeMove(move))
            {
                throw new ApplicationException($"Tried to make a move, but failed.  Move: {move}");
            }

            move.OriginalSanString = origMoveStr;
            return move;
        }

        /// <summary>
        /// Using the current board state, encode the given move into a SAN string.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private string ComputeSanString(ChessBoardState board, ChessMove move)
        {
            // To remove ambiguity, we have to see if we need the starting rank and/or file.
            if (move.KingsideCastle)
            {
                return $"O-O{(move.IsCheckmateMove?"#":move.IsCheckingMove?"+":"")}";
            }
            else if (move.QueensideCastle)
            {
                return $"O-O-O{(move.IsCheckmateMove?"#":move.IsCheckingMove?"+":"")}";
            }
            else if (move.PawnPromotedTo != PromotionChessPiece.None)
            {
                return $"{move.EndSquare}={move.GetPromotionPieceInitial(move.PawnPromotedTo).ToString().ToUpper()}{(move.IsCheckmateMove?"#":move.IsCheckingMove?"+":"")}";
            }

            // NOW, let's find all moves we Could make with this board,
            // and find the move that ENDS with the last 2 chars of moveStr,
            // and the piece moved is the given piece.
            // If there's multiple pieces that could make that move, look at the first character of the moveStr for a clue.
            var endSquare = move.EndSquare;
            var possibleMoves = board.PossibleMoves().Where(x => x.EndSquare == endSquare && x.Piece == move.Piece && (x.PawnPromotedTo == PromotionChessPiece.None || x.PawnPromotedTo == move.PawnPromotedTo));

            if (possibleMoves.Count() == 1)
            {
                // We found exactly one match.  Perfect; let's grab the details we didn't have yet.
                if (move.Piece == ChessPiece.BlackPawn || move.Piece == ChessPiece.WhitePawn)
                {
                    if (move.WasPieceCaptured)
                    {
                        return $"{move.StartSquare[0]}x{move.EndSquare}{(move.IsCheckmateMove ? "#" : move.IsCheckingMove ? "+" : "")}";
                    }
                    else
                    {
                        return $"{move.EndSquare}{(move.IsCheckmateMove ? "#" : move.IsCheckingMove ? "+" : "")}";
                    }
                }
                else
                {
                    return $"{move.GetPieceInitial(move.Piece)}{(move.WasPieceCaptured ? "x" : "")}{move.EndSquare}{(move.IsCheckmateMove ? "#" : move.IsCheckingMove ? "+" : "")}";
                }
            }
            else if (possibleMoves.Count() > 1)
            {
                // What do we do?
                // First, compare the starting File (the letter).
                var subPossibilities = possibleMoves.Where(x => x.StartSquare[0] == move.StartSquare[0]);
                if (subPossibilities.Count() == 1)
                {
                    if (move.Piece == ChessPiece.BlackPawn || move.Piece == ChessPiece.WhitePawn)
                    {
                        return $"{move.StartSquare[0]}{(move.WasPieceCaptured ? "x" : "")}{move.EndSquare}{(move.IsCheckmateMove ? "#" : move.IsCheckingMove ? "+" : "")}";
                    }
                    else
                    {
                        return $"{move.GetPieceInitial(move.Piece)}{move.StartSquare[0]}{(move.WasPieceCaptured ? "x" : "")}{move.EndSquare}{(move.IsCheckmateMove ? "#" : move.IsCheckingMove ? "+" : "")}";
                    }
                }

                // Oh no, the starting rank isn't enough to tell them apart.
                // Compare the starting Rank (the number).
                subPossibilities = possibleMoves.Where(x => x.StartSquare[1] == move.StartSquare[1]);
                if (subPossibilities.Count() == 1)
                {
                    if (move.Piece == ChessPiece.BlackPawn || move.Piece == ChessPiece.WhitePawn)
                    {
                        return $"{move.StartSquare[1]}{(move.WasPieceCaptured ? "x" : "")}{move.EndSquare}{(move.IsCheckmateMove ? "#" : move.IsCheckingMove ? "+" : "")}";
                    }
                    else
                    {
                        return $"{move.GetPieceInitial(move.Piece)}{move.StartSquare[1]}{(move.WasPieceCaptured ? "x" : "")}{move.EndSquare}{(move.IsCheckmateMove ? "#" : move.IsCheckingMove ? "+" : "")}";
                    }
                }

                // Not good enough; need to use the entire start square now.
                if (move.Piece == ChessPiece.BlackPawn || move.Piece == ChessPiece.WhitePawn)
                {
                    return $"{move.StartSquare}{(move.WasPieceCaptured ? "x" : "")}{move.EndSquare}{(move.IsCheckmateMove ? "#" : move.IsCheckingMove ? "+" : "")}";
                }
                else
                {
                    return $"{move.GetPieceInitial(move.Piece)}{move.StartSquare}{(move.WasPieceCaptured ? "x" : "")}{move.EndSquare}{(move.IsCheckmateMove ? "#" : move.IsCheckingMove ? "+" : "")}";
                }
            }

            // If we try to make an invalid move, we'll get here.  Not a big deal.
            return "?";
        }

        private ChessPiece GetPieceByInitial(ChessColor color, char firstInitial)
        {
            switch (firstInitial)
            {
                case 'P':
                    return color == ChessColor.White ? ChessPiece.WhitePawn : ChessPiece.BlackPawn;
                case 'B':
                    return color == ChessColor.White ? ChessPiece.WhiteBishop : ChessPiece.BlackBishop;
                case 'N':
                    return color == ChessColor.White ? ChessPiece.WhiteKnight : ChessPiece.BlackKnight;
                case 'R':
                    return color == ChessColor.White ? ChessPiece.WhiteRook : ChessPiece.BlackRook;
                case 'Q':
                    return color == ChessColor.White ? ChessPiece.WhiteQueen : ChessPiece.BlackQueen;
                case 'K':
                    return color == ChessColor.White ? ChessPiece.WhiteKing : ChessPiece.BlackKing;
                default:
                    throw new ApplicationException($"Invalid piece intial: {firstInitial}.");
            }
        }

        private PromotionChessPiece GetPromotionPieceByInitial(char firstInitial)
        {
            switch (firstInitial)
            {
                case 'Q':
                    return PromotionChessPiece.Queen;
                case 'R':
                    return PromotionChessPiece.Rook;
                case 'N':
                    return PromotionChessPiece.Knight;
                case 'B':
                    return PromotionChessPiece.Bishop;
                default:
                    throw new ApplicationException($"Invalid promotion piece intial: {firstInitial}.");
            }
        }

        public string GetPgnValue(string key)
        {
            if (pgnValues.ContainsKey(key))
            {
                return pgnValues[key];
            }
            return "";
        }

        public void PrintBoard()
        {
            Console.WriteLine(boardState.ToString());
        }

        /// <summary>z
        /// Returns a list of chess moves from the given position.
        /// </summary>
        /// <returns></returns>
        public List<ChessMove> PossibleMoves()
        {
            return boardState.PossibleMoves();
        }

        public bool IsGameInProgress()
        {
            return this.gameState == GameState.InProgress;
        }
    }
}
