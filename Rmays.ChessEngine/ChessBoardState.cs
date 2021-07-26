using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rmays.ChessEngine
{
    /// <summary>
    /// Represents the current state of the chess board (current location of all pieces, whose turn it is,
    /// whether or not each side can castle (and which way), if an en passant capture is possible, and how many moves
    /// have occurred since the last capture / pawn move.)  Can be converted to a FEN representation.
    /// </summary>
    public class ChessBoardState : ICloneable
    {
        protected ChessPiece[,] chessBoard;
        protected ChessColor SideToMove = ChessColor.White;
        protected char SideToMoveChar =>
            SideToMove == ChessColor.White ? 'w' :
            SideToMove == ChessColor.Black ? 'b' :
            '?';
        protected bool WhiteCanCastleKingside = true;
        protected bool WhiteCanCastleQueenside = true;
        protected bool BlackCanCastleKingside = true;
        protected bool BlackCanCastleQueenside = true;
        protected bool EnPassantPossible => !string.IsNullOrWhiteSpace(EnPassantTargetSquare);
        protected string EnPassantTargetSquare = "";
        protected int HalfMoveClock = 0;
        protected int FullMoves = 1;

        protected string GetCastleAbilityString()
        {
            var castlingRights =
                $"{(WhiteCanCastleKingside ? "K" : "")}" +
                $"{(WhiteCanCastleQueenside ? "Q" : "")}" +
                $"{(BlackCanCastleKingside ? "k" : "")}" +
                $"{(BlackCanCastleQueenside ? "q" : "")}";

            if (castlingRights == "")
            {
                castlingRights = "-";
            }

            return castlingRights;
        }

        public ChessBoardState()
        {
            chessBoard = new ChessPiece[10, 10]; // We're using indexes 1-8 for File, and 1-8 for Rank. 
            Initialize();
        }

        public ChessBoardState(ChessBoardState orig)
        {
            this.chessBoard = orig.chessBoard;
            this.SideToMove = orig.SideToMove;
            this.WhiteCanCastleKingside = orig.WhiteCanCastleKingside;
            this.WhiteCanCastleQueenside = orig.WhiteCanCastleQueenside;
            this.BlackCanCastleKingside = orig.BlackCanCastleKingside;
            this.BlackCanCastleQueenside = orig.BlackCanCastleQueenside;
            this.EnPassantTargetSquare = orig.EnPassantTargetSquare;
            this.HalfMoveClock = orig.HalfMoveClock;
            this.FullMoves = orig.FullMoves;
        }

        public void SetSpot(int f, int r, ChessPiece piece)
        {
            this.chessBoard[f, r] = piece;
        }

        public void SetSideToMove(ChessColor color)
        {
            this.SideToMove = color;
        }

        public ChessColor GetSideToMove()
        {
            return this.SideToMove;
        }

        public void SetWhiteCanCastleKingside(bool canCastle)
        {
            this.WhiteCanCastleKingside = canCastle;
        }

        public void SetWhiteCanCastleQueenside(bool canCastle)
        {
            this.WhiteCanCastleQueenside = canCastle;
        }

        public void SetBlackCanCastleKingside(bool canCastle)
        {
            this.BlackCanCastleKingside = canCastle;
        }

        public void SetBlackCanCastleQueenside(bool canCastle)
        {
            this.BlackCanCastleQueenside = canCastle;
        }

        public void SetEnPassantCaptureSquare(int file, int rank)
        {
            this.EnPassantTargetSquare = ChessBoardSquare.GetAN(file, rank);
        }

        /// <summary>
        /// Remove all pieces from the board, and set the board's state to the default values.
        /// </summary>
        public void Clear()
        {
            // Set the border.
            for (int x = 0; x < 10; x++)
            {
                chessBoard[0, x] = ChessPiece.OutsideBoardRange;
                chessBoard[9, x] = ChessPiece.OutsideBoardRange;
                chessBoard[x, 0] = ChessPiece.OutsideBoardRange;
                chessBoard[x, 9] = ChessPiece.OutsideBoardRange;
            }

            // Set all spots to empty spaces.
            for (int f = 1; f <= 8; f++)
            {
                for (int r = 1; r <= 8; r++)
                {
                    chessBoard[f, r] = ChessPiece.Space;
                }
            }

            SideToMove = ChessColor.White;
            this.WhiteCanCastleKingside = false;
            this.WhiteCanCastleQueenside = false;
            this.BlackCanCastleKingside = false;
            this.BlackCanCastleQueenside = false;
            EnPassantTargetSquare = null;
            HalfMoveClock = 0;
            FullMoves = 1;
        }

        /// <summary>
        /// Set up all 32 pieces.
        /// </summary>
        public void Initialize()
        {
            Clear();

            for (int f = 1; f <= 8; f++)
            {
                chessBoard[f, 2] = ChessPiece.WhitePawn;
                chessBoard[f, 7] = ChessPiece.BlackPawn;
            }

            chessBoard[1, 1] = ChessPiece.WhiteRook;
            chessBoard[2, 1] = ChessPiece.WhiteKnight;
            chessBoard[3, 1] = ChessPiece.WhiteBishop;
            chessBoard[4, 1] = ChessPiece.WhiteQueen;
            chessBoard[5, 1] = ChessPiece.WhiteKing;
            chessBoard[6, 1] = ChessPiece.WhiteBishop;
            chessBoard[7, 1] = ChessPiece.WhiteKnight;
            chessBoard[8, 1] = ChessPiece.WhiteRook;

            chessBoard[1, 8] = ChessPiece.BlackRook;
            chessBoard[2, 8] = ChessPiece.BlackKnight;
            chessBoard[3, 8] = ChessPiece.BlackBishop;
            chessBoard[4, 8] = ChessPiece.BlackQueen;
            chessBoard[5, 8] = ChessPiece.BlackKing;
            chessBoard[6, 8] = ChessPiece.BlackBishop;
            chessBoard[7, 8] = ChessPiece.BlackKnight;
            chessBoard[8, 8] = ChessPiece.BlackRook;

            SideToMove = ChessColor.White;
            this.WhiteCanCastleKingside = true;
            this.WhiteCanCastleQueenside = true;
            this.BlackCanCastleKingside = true;
            this.BlackCanCastleQueenside = true;
        }

        public override string ToString()
        {
            var result = "";
            for (int r = 8; r >= 1; r--)
            {
                for (int f = 1; f <= 8; f++)
                {
                    var spot = chessBoard[f, r];
                    if (spot == ChessPiece.Space)
                    {
                        result += "-- ";
                        continue;
                    }

                    result += $"{(spot < 0 ? "B" : "W")}";

                    var pieceType = Math.Abs((int)spot);
                    
                    // Write 'N' if it's a knight; otherwise, use the 6th character
                    // in the enumeration (the first letter in the piece name, after the color).
                    result += (pieceType == 2 ? 'N' : spot.ToString()[5]);
                    result += " ";
                }

                result += Environment.NewLine;
            }

            return result;
        }

        public string GetFEN()
        {
            var result = "";
            var spacesSoFar = 0;
            for (int r = 8; r >= 1; r--)
            {
                for (int f = 1; f <= 8; f++)
                {
                    var spot = chessBoard[f, r];
                    if (spot == ChessPiece.Space)
                    {
                        spacesSoFar++;
                    }
                    else
                    {
                        if (spacesSoFar > 0)
                        {
                            result += spacesSoFar.ToString();
                            spacesSoFar = 0;
                        }

                        // Now print the piece.
                        char piece = '?';
                        if (Math.Abs((int)spot) == 2)
                        {
                            piece = 'N';
                        }
                        else
                        {
                            piece = spot.ToString()[5];
                        }

                        // Make it lowercase if it's a black piece.
                        if (spot < 0)
                        {
                            piece = $"{piece}".ToLower()[0];
                        }

                        result += piece;
                    }
                }

                if (spacesSoFar > 0)
                {
                    result += spacesSoFar.ToString();
                    spacesSoFar = 0;
                }

                if (r != 1)
                {
                    result += "/";
                }
            }

            result += $" {this.SideToMoveChar}"
                + $" {this.GetCastleAbilityString()}"
                + $" {(this.EnPassantPossible ? this.EnPassantTargetSquare : "-")}"
                + $" {this.HalfMoveClock}"
                + $" {this.FullMoves}";

            return result;
        }

        private bool SpotHasCapturableEnemyPiece(ChessColor currPlayer, string targetSpot)
        {
            var square = new ChessBoardSquare(targetSpot);
            var spot = this.chessBoard[square.File, square.Rank];

            if (spot == ChessPiece.Space || spot == ChessPiece.OutsideBoardRange)
            {
                return false;
            }
            return ((int)spot * (int)currPlayer) < 0;
        }

        private ChessColor GetSpotPieceColor(string targetSpot)
        {
            var square = new ChessBoardSquare(targetSpot);
            return GetSpotPieceColor(square.File, square.Rank);
        }

        private ChessColor GetSpotPieceColor(int f, int r)
        {
            if (f < 1 || f > 8 || r < 1 || r > 8)
            {
                return ChessColor.None;
            }

            var spot = this.chessBoard[f, r];

            return spot == 0 ? ChessColor.None
                : spot > 0 ? ChessColor.White
                : ChessColor.Black;
        }

        private ChessPiece GetChessPieceFromAN(string AN)
        {
            return this.chessBoard[AN[0] - 'a' + 1, AN[1] - '0'];
        }

        public bool TryMakeMove(int moveId)
        {
            var moves = PossibleMoves();
            if (moveId <= moves.Count() && moveId >= 0)
            {
                var result = TryMakeMove(moves[moveId]);
                if (result)
                {
                    Console.WriteLine("Making move: " + moveId);
                }
                else
                {
                    Console.WriteLine("Problem making move: " + moveId);
                }
                return result;
            }
            else
            {
                Console.WriteLine("Problem making move (out of range): " + moveId);
                return false;
            }
        }

        public bool TryMakeMove(ChessMove move)
        {
            if (move.Piece != GetChessPieceFromAN(move.StartSquare))
            {
                // Invalid move; the move's piece doesn't match the move's start square.
                return false;
            }

            // Make the move without checking whether or not it's legal.  We'll check it later if it puts one or both players into check.
            // Which piece made the move?
            if (move.KingsideCastle)
            {
                // For castling, we'll do an early check-check.
                // We can't castle across check; jump out if that's the case.
                if (move.Piece == ChessPiece.WhiteKing)
                {
                    WhiteCanCastleKingside = false;
                    WhiteCanCastleQueenside = false;

                    // King jump square check
                    this.chessBoard[5, 1] = ChessPiece.Space;
                    this.chessBoard[6, 1] = ChessPiece.WhiteKing;
                    if (this.IsOpponentInCheck())
                    {
                        this.chessBoard[5, 1] = ChessPiece.WhiteKing;
                        this.chessBoard[6, 1] = ChessPiece.Space;
                        return false;
                    }

                    this.chessBoard[8, 1] = ChessPiece.Space;
                    this.chessBoard[7, 1] = ChessPiece.WhiteKing;
                    this.chessBoard[6, 1] = ChessPiece.WhiteRook;
                    this.chessBoard[5, 1] = ChessPiece.Space;
                }
                else if (move.Piece == ChessPiece.BlackKing)
                {
                    BlackCanCastleKingside = false;
                    BlackCanCastleQueenside = false;

                    // King jump square check
                    this.chessBoard[5, 8] = ChessPiece.Space;
                    this.chessBoard[6, 8] = ChessPiece.BlackKing;
                    if (this.IsOpponentInCheck())
                    {
                        this.chessBoard[5, 8] = ChessPiece.BlackKing;
                        this.chessBoard[6, 8] = ChessPiece.Space;
                        return false;
                    }

                    this.chessBoard[8, 8] = ChessPiece.Space;
                    this.chessBoard[7, 8] = ChessPiece.BlackKing;
                    this.chessBoard[6, 8] = ChessPiece.BlackRook;
                    this.chessBoard[5, 8] = ChessPiece.Space;
                }
            }
            else if (move.QueensideCastle)
            {
                if (move.Piece == ChessPiece.WhiteKing)
                {
                    WhiteCanCastleKingside = false;
                    WhiteCanCastleQueenside = false;

                    // King jump square check
                    this.chessBoard[5, 1] = ChessPiece.Space;
                    this.chessBoard[4, 1] = ChessPiece.WhiteKing;
                    if (this.IsOpponentInCheck())
                    {
                        this.chessBoard[5, 1] = ChessPiece.WhiteKing;
                        this.chessBoard[4, 1] = ChessPiece.Space;
                        return false;
                    }

                    this.chessBoard[1, 1] = ChessPiece.Space;
                    this.chessBoard[3, 1] = ChessPiece.WhiteKing;
                    this.chessBoard[4, 1] = ChessPiece.WhiteRook;
                    this.chessBoard[5, 1] = ChessPiece.Space;
                }
                else if (move.Piece == ChessPiece.BlackKing)
                {
                    BlackCanCastleKingside = false;
                    BlackCanCastleQueenside = false;

                    // King jump square check
                    this.chessBoard[5, 8] = ChessPiece.Space;
                    this.chessBoard[4, 8] = ChessPiece.BlackKing;
                    if (this.IsOpponentInCheck())
                    {
                        this.chessBoard[5, 8] = ChessPiece.BlackKing;
                        this.chessBoard[4, 8] = ChessPiece.Space;
                        return false;
                    }

                    this.chessBoard[1, 8] = ChessPiece.Space;
                    this.chessBoard[3, 8] = ChessPiece.BlackKing;
                    this.chessBoard[4, 8] = ChessPiece.BlackRook;
                    this.chessBoard[5, 8] = ChessPiece.Space;
                }
            }
            else
            {
                this.chessBoard[move.StartSquare[0] - 'a' + 1, move.StartSquare[1] - '0'] = ChessPiece.Space;
                if (move.PawnPromotedTo == PromotionChessPiece.None)
                {
                    this.chessBoard[move.EndSquare[0] - 'a' + 1, move.EndSquare[1] - '0'] = move.Piece;
                }
                else
                {
                    var targetPiece = (ChessPiece)((int)move.PawnPromotedTo * (move.Piece.ToString().StartsWith("W") ? 1 : -1));
                    this.chessBoard[move.EndSquare[0] - 'a' + 1, move.EndSquare[1] - '0'] = targetPiece;
                }

                if (move.Piece == ChessPiece.WhiteKing)
                {
                    WhiteCanCastleKingside = false;
                    WhiteCanCastleQueenside = false;
                }
                else if (move.Piece == ChessPiece.BlackKing)
                {
                    BlackCanCastleKingside = false;
                    BlackCanCastleQueenside = false;
                }
                else if (move.Piece == ChessPiece.WhiteRook)
                {
                    if (move.StartSquare == "a1")
                    {
                        WhiteCanCastleQueenside = false;
                    }
                    else if (move.StartSquare == "h1")
                    {
                        WhiteCanCastleKingside = false;
                    }
                }
                else if (move.Piece == ChessPiece.BlackRook)
                {
                    if (move.StartSquare == "a8")
                    {
                        BlackCanCastleQueenside = false;
                    }
                    else if (move.StartSquare == "h8")
                    {
                        BlackCanCastleKingside = false;
                    }
                }
            }

            if (this.IsPlayerInCheck())
            {
                return false;
            }

            this.FullMoves++;
            this.SideToMove = (ChessColor)((int)this.SideToMove * -1);
            return true;
        }

        /// <summary>
        /// Returns an ordered list of chess moves from the given position.
        /// For each square (starting with a1, going to a8, then b1 to b8, etc), if a piece on that square is movable by the player,
        /// find all moves that piece can make (destination square starting with a1 to a8, then b1 to b8, etc).
        /// </summary>
        /// <returns></returns>
        public List<ChessMove> PossibleMoves(bool onlyReturnCaptures = false)
        {
            var moves = new List<ChessMove>();
            for (var r = 1; r <= 8; r++)
            {
                for (var f = 1; f <= 8; f++)
                {
                    var pieceMoves = new List<ChessMove>();
                    var spot = this.chessBoard[f, r];
                    if (spot == ChessPiece.Space) continue;
                    if ((int)spot * (int)this.SideToMove < 0) continue;
                    switch (Math.Abs((int)spot))
                    {
                        case 1: // pawn
                            // deltaRank is the change in rank for each pawn move.
                            // White will increase their rank with each move, Black will decrease their rank.
                            var deltaRank = (int)SideToMove;
                            if ((SideToMove == ChessColor.White && r == 7) || (SideToMove == ChessColor.Black && r == 2))
                            {
                                // Push + Promotion
                                if (this.chessBoard[f, r + deltaRank] == ChessPiece.Space)
                                {
                                    pieceMoves.Add(new ChessMove
                                    {
                                        Piece = spot,
                                        StartSquare = ChessBoardSquare.GetAN(f, r),
                                        EndSquare = ChessBoardSquare.GetAN(f, r + deltaRank),
                                        PawnPromotedTo = PromotionChessPiece.Bishop
                                    });
                                    pieceMoves.Add(new ChessMove
                                    {
                                        Piece = spot,
                                        StartSquare = ChessBoardSquare.GetAN(f, r),
                                        EndSquare = ChessBoardSquare.GetAN(f, r + deltaRank),
                                        PawnPromotedTo = PromotionChessPiece.Knight
                                    });
                                    pieceMoves.Add(new ChessMove
                                    {
                                        Piece = spot,
                                        StartSquare = ChessBoardSquare.GetAN(f, r),
                                        EndSquare = ChessBoardSquare.GetAN(f, r + deltaRank),
                                        PawnPromotedTo = PromotionChessPiece.Queen
                                    });
                                    pieceMoves.Add(new ChessMove
                                    {
                                        Piece = spot,
                                        StartSquare = ChessBoardSquare.GetAN(f, r),
                                        EndSquare = ChessBoardSquare.GetAN(f, r + deltaRank),
                                        PawnPromotedTo = PromotionChessPiece.Rook
                                    });
                                }

                                // Capture + Promotion
                                // Look for capture forward-left and forward-right.
                                for (var deltaFile = -1; deltaFile <= 1; deltaFile += 2)
                                {
                                    if (this.GetSpotPieceColor(f + deltaFile, r + deltaRank) == (ChessColor)(-1 * (int)SideToMove))
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f + deltaFile, r + deltaRank),
                                            WasPieceCaptured = true,
                                            PawnPromotedTo = PromotionChessPiece.Bishop
                                        });
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f + deltaFile, r + deltaRank),
                                            WasPieceCaptured = true,
                                            PawnPromotedTo = PromotionChessPiece.Knight
                                        });
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f + deltaFile, r + deltaRank),
                                            WasPieceCaptured = true,
                                            PawnPromotedTo = PromotionChessPiece.Queen
                                        });
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f + deltaFile, r + deltaRank),
                                            WasPieceCaptured = true,
                                            PawnPromotedTo = PromotionChessPiece.Rook
                                        });
                                    }
                                }
                            }
                            else
                            {
                                if ((SideToMove == ChessColor.White && r == 2) || (SideToMove == ChessColor.Black && r == 7))
                                {
                                    // Starting row; can double-push
                                    if (this.chessBoard[f, r + deltaRank] == ChessPiece.Space
                                        && this.chessBoard[f, r + deltaRank + deltaRank] == ChessPiece.Space)
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f, r + deltaRank + deltaRank)
                                        });
                                    }
                                }

                                // Try pushing pawn forward.
                                if (this.chessBoard[f, r + deltaRank] == ChessPiece.Space)
                                {
                                    // Push pawn
                                    pieceMoves.Add(new ChessMove
                                    {
                                        Piece = spot,
                                        StartSquare = ChessBoardSquare.GetAN(f, r),
                                        EndSquare = ChessBoardSquare.GetAN(f, r + deltaRank)
                                    });
                                }

                                // Check for captures.
                                for (var deltaFile = -1; deltaFile <= 1; deltaFile += 2)
                                {
                                    if (this.GetSpotPieceColor(f + deltaFile, r + deltaRank) == (ChessColor)(-1 * (int)SideToMove))
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f + deltaFile, r + deltaRank),
                                            WasPieceCaptured = true
                                        });
                                    }

                                    if (EnPassantTargetSquare == ChessBoardSquare.GetAN(f + deltaFile, r + deltaRank))
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f + deltaFile, r + deltaRank),
                                            WasPieceCaptured = true
                                        });
                                    }
                                }
                            }
                            break;
                        case 2: // Knight
                            var targets = new List<string> { "1,2", "2,1", "1,-2", "2,-1", "-1,-2", "-2,-1", "-1,2", "-2,1" };
                            foreach (var target in targets)
                            {
                                var newFile = f + int.Parse(target.Split(',')[0]);
                                var newRank = r + int.Parse(target.Split(',')[1]);
                                if (newFile < 1 || newFile > 8 || newRank < 1 || newRank > 8) continue;
                                var canCapture = this.SpotHasCapturableEnemyPiece(this.SideToMove, ChessBoardSquare.GetAN(newFile, newRank));
                                if (this.chessBoard[newFile, newRank] == ChessPiece.Space || canCapture)
                                {
                                    pieceMoves.Add(new ChessMove
                                    {
                                        Piece = spot,
                                        WasPieceCaptured = canCapture,
                                        StartSquare = ChessBoardSquare.GetAN(f, r),
                                        EndSquare = ChessBoardSquare.GetAN(newFile, newRank)
                                    });
                                }
                            }
                            break;
                        case 3: // Bishop
                            pieceMoves.AddRange(GetSlideMoves(f, r, -1, -1, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, -1, 1, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, 1, -1, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, 1, 1, spot));
                            break;
                        case 4: // Rook
                            pieceMoves.AddRange(GetSlideMoves(f, r, 1, 0, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, -1, 0, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, 0, -1, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, 0, 1, spot));
                            break;
                        case 5: // Queen
                            pieceMoves.AddRange(GetSlideMoves(f, r, -1, -1, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, -1, 1, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, 1, -1, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, 1, 1, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, 1, 0, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, -1, 0, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, 0, -1, spot));
                            pieceMoves.AddRange(GetSlideMoves(f, r, 0, 1, spot));
                            break;
                        case 6: // King
                            pieceMoves.AddRange(GetAdjacentMoves(f, r, spot));
                            if (this.SideToMove == ChessColor.White)
                            {
                                if (this.chessBoard[5,1] == ChessPiece.WhiteKing)
                                {
                                    if (this.chessBoard[1, 1] == ChessPiece.WhiteRook
                                        && this.chessBoard[2, 1] == ChessPiece.Space && this.chessBoard[3, 1] == ChessPiece.Space && this.chessBoard[4, 1] == ChessPiece.Space
                                        && this.WhiteCanCastleQueenside)
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = ChessPiece.WhiteKing,
                                            StartSquare = "e1",
                                            EndSquare = "c1",
                                            QueensideCastle = true
                                        });
                                    }

                                    if (this.chessBoard[8, 1] == ChessPiece.WhiteRook
                                        && this.chessBoard[7, 1] == ChessPiece.Space && this.chessBoard[6, 1] == ChessPiece.Space
                                        && this.WhiteCanCastleKingside)
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = ChessPiece.WhiteKing,
                                            StartSquare = "e1",
                                            EndSquare = "g1",
                                            KingsideCastle = true
                                        });
                                    }
                                }
                            }
                            else if (this.SideToMove == ChessColor.Black)
                            {
                                if (this.chessBoard[5, 8] == ChessPiece.BlackKing)
                                {
                                    if (this.chessBoard[1, 8] == ChessPiece.BlackRook
                                        && this.chessBoard[2, 8] == ChessPiece.Space && this.chessBoard[3, 8] == ChessPiece.Space && this.chessBoard[4, 8] == ChessPiece.Space
                                        && this.WhiteCanCastleQueenside)
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = ChessPiece.BlackKing,
                                            StartSquare = "e8",
                                            EndSquare = "c8",
                                            QueensideCastle = true
                                        });
                                    }

                                    if (this.chessBoard[8, 8] == ChessPiece.BlackRook
                                        && this.chessBoard[7, 8] == ChessPiece.Space && this.chessBoard[6, 8] == ChessPiece.Space
                                        && this.WhiteCanCastleKingside)
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = ChessPiece.BlackKing,
                                            StartSquare = "e8",
                                            EndSquare = "g8",
                                            KingsideCastle = true
                                        });
                                    }
                                }
                            }
                            break;
                    }

                    moves.AddRange(pieceMoves.OrderBy(x => x.EndSquare).ThenBy(x => x.PawnPromotedTo.ToString()));
                }
            }

            // Jump out if we only want to return captures.
            // This is used when determining if the king is in check.
            if (onlyReturnCaptures)
            {
                return moves.Where(x => x.WasPieceCaptured).ToList();
            }

            // Reject moves that cause the current player's turn's king to be in check.
            var rejectedMoves = new List<ChessMove>();
            foreach (var move in moves)
            {
                ChessBoardState checkBoard = (ChessBoardState)this.Clone();
                if (!checkBoard.TryMakeMove(move))
                {
                    rejectedMoves.Add(move);
                }

                if (checkBoard.IsOpponentInCheck())
                {
                    // Reject the move; the player is in check after making this move.
                    rejectedMoves.Add(move);
                }

                // When the player castles, make sure the King isn't in check, AND not jumping over a check square.
                if (move.KingsideCastle)
                {
                    if (this.IsPlayerInCheck())
                    {
                        rejectedMoves.Add(move);
                    }

                    if (checkBoard.SideToMove == ChessColor.Black)
                    {
                        // White made a move, so now it's Black's turn.  Make sure White's move was legal.
                        this.chessBoard[5, 1] = ChessPiece.Space;
                        this.chessBoard[6, 1] = ChessPiece.WhiteKing;
                        if (this.IsPlayerInCheck())
                        {
                            rejectedMoves.Add(move);
                        }

                        this.chessBoard[5, 1] = ChessPiece.WhiteKing;
                        this.chessBoard[6, 1] = ChessPiece.Space;
                    }
                    else if (checkBoard.SideToMove == ChessColor.White)
                    {
                        // Black made a move, so now it's White's turn.  Make sure Black's move was legal.
                        this.chessBoard[5, 8] = ChessPiece.Space;
                        this.chessBoard[6, 8] = ChessPiece.BlackKing;
                        if (this.IsPlayerInCheck())
                        {
                            rejectedMoves.Add(move);
                        }

                        this.chessBoard[5, 8] = ChessPiece.BlackKing;
                        this.chessBoard[6, 8] = ChessPiece.Space;
                    }
                }
                else if (move.QueensideCastle)
                {
                    if (this.IsPlayerInCheck())
                    {
                        rejectedMoves.Add(move);
                    }

                    if (checkBoard.SideToMove == ChessColor.Black)
                    {
                        // White made a move, so now it's Black's turn.  Make sure White's move was legal.
                        this.chessBoard[5, 1] = ChessPiece.Space;
                        this.chessBoard[4, 1] = ChessPiece.WhiteKing;
                        if (this.IsPlayerInCheck())
                        {
                            rejectedMoves.Add(move);
                        }

                        this.chessBoard[5, 1] = ChessPiece.WhiteKing;
                        this.chessBoard[4, 1] = ChessPiece.Space;
                    }
                    else if (checkBoard.SideToMove == ChessColor.White)
                    {
                        // Black made a move, so now it's White's turn.  Make sure Black's move was legal.
                        this.chessBoard[5, 8] = ChessPiece.Space;
                        this.chessBoard[4, 8] = ChessPiece.BlackKing;
                        if (this.IsPlayerInCheck())
                        {
                            rejectedMoves.Add(move);
                        }

                        this.chessBoard[5, 8] = ChessPiece.BlackKing;
                        this.chessBoard[4, 8] = ChessPiece.Space;
                    }
                }
            }

            moves = moves.Where(x => !rejectedMoves.Contains(x)).ToList();

            return moves
                .OrderBy(x => x.StartSquare[1])
                .ThenBy(x => x.StartSquare[0])
                .ThenBy(x => x.EndSquare[1])
                .ThenBy(x => x.EndSquare[0])
                .ThenBy(x => x.PawnPromotedTo.ToString())
                .ToList();
        }

        private bool IsPlayerInCheck()
        {
            this.SideToMove = (this.SideToMove == ChessColor.White ? ChessColor.Black : ChessColor.White);
            var result = IsOpponentInCheck();
            this.SideToMove = (this.SideToMove == ChessColor.White ? ChessColor.Black : ChessColor.White);
            return result;
        }

        private bool IsOpponentInCheck()
        {
            // Find the king.
            var target = (SideToMove == ChessColor.White ? ChessPiece.BlackKing : ChessPiece.WhiteKing);
            for (var r = 1; r <= 8; r++)
            {
                for (var f = 1; f <= 8; f++)
                {
                    if (this.chessBoard[f, r] != target)
                    {
                        continue;
                    }

                    var spot = new ChessBoardSquare(f, r);

                    // Found the king at spot (f,r).
                    // Call 'PossibleMoves', but only return the moves with a capture.
                    // THEN, find any moves where the king Could be captured.
                    var captureMoves = this.PossibleMoves(true).Where(x => x.EndSquare == spot.GetAN());
                    return captureMoves.Any();
                }
            }

            // Didn't find the king!  This is probably bad.
            return false;
        }

        private List<ChessMove> GetSlideMoves(int f, int r, int deltaF, int deltaR, ChessPiece piece)
        {
            ChessColor sideToMove = GetSpotPieceColor(f, r);
            int newF = f + deltaF;
            int newR = r + deltaR;
            var moves = new List<ChessMove>();
            while (newR >= 1 && newR <= 8 && newF >= 1 && newF <= 8)
            {
                var targetColor = GetSpotPieceColor(newF, newR);

                // Figure out if we landed on a same-color piece, an enemy piece, or an empty space.
                if ((int)targetColor == (int)sideToMove * -1)
                {
                    // We landed on an enemy piece.
                    moves.Add(new ChessMove
                    {
                        Piece = piece,
                        WasPieceCaptured = true,
                        StartSquare = ChessBoardSquare.GetAN(f, r),
                        EndSquare = ChessBoardSquare.GetAN(newF, newR)
                    });

                    // All done!  We can't slide any further.
                    return moves;
                }
                else if (targetColor == sideToMove)
                {
                    // we're on a same-color piece.  We can't move here, and we're done sliding.
                    return moves;
                }
                else
                {
                    // We landed on an empty space.  Add this move.
                    moves.Add(new ChessMove
                    {
                        Piece = piece,
                        WasPieceCaptured = false,
                        StartSquare = ChessBoardSquare.GetAN(f, r),
                        EndSquare = ChessBoardSquare.GetAN(newF, newR)
                    });
                }

                newR += deltaR;
                newF += deltaF;
            }

            // We're outside the bounds of the board; return the moves we found.
            return moves;
        }

        private List<ChessMove> GetAdjacentMoves(int f, int r, ChessPiece piece)
        {
            ChessColor sideToMove = GetSpotPieceColor(f, r);
            var moves = new List<ChessMove>();
            for (var newF = f - 1; newF <= f + 1; newF++)
            {
                for (var newR = r - 1; newR <= r + 1; newR++)
                {
                    if (newR < 1 || newR > 8 || newF < 1 || newF > 8) continue;
                    if (newF == f && newR == r) continue;

                    var targetColor = GetSpotPieceColor(newF, newR);

                    // Figure out if we landed on a same-color piece, an enemy piece, or an empty space.
                    if (targetColor == ChessColor.None || (int)targetColor == (int)sideToMove * -1)
                    {
                        // We landed on an enemy piece or a blank space.
                        moves.Add(new ChessMove
                        {
                            Piece = piece,
                            WasPieceCaptured = (targetColor != ChessColor.None),
                            StartSquare = ChessBoardSquare.GetAN(f, r),
                            EndSquare = ChessBoardSquare.GetAN(newF, newR)
                        });
                    }
                }
            }

            // We've checked all adjacent squares.  Return the moves we found.
            return moves;
        }

        public bool IsGameOver(out int gameResult)
        {
            gameResult = 0;
            var totalMoves = PossibleMoves().Count();
            if (totalMoves > 0)
            {
                return false;
            }

            // The game is over; who won?
            // If the player is in check, it's a checkmate.  Otherwise, it's a stalemate.
            gameResult = this.IsPlayerInCheck() ? -1 * (int)this.SideToMove : 0;
            return true;
        }

        public object Clone()
        {
            var newState = new ChessBoardState
            {
                BlackCanCastleKingside = this.BlackCanCastleKingside,
                BlackCanCastleQueenside = this.BlackCanCastleQueenside,
                WhiteCanCastleKingside = this.WhiteCanCastleKingside,
                WhiteCanCastleQueenside = this.WhiteCanCastleQueenside,
                EnPassantTargetSquare = this.EnPassantTargetSquare,
                chessBoard = (ChessPiece[,])this.chessBoard.Clone(),
                FullMoves = this.FullMoves,
                HalfMoveClock = this.HalfMoveClock,
                SideToMove = this.SideToMove
            };

            return newState;
        }
    }
}
