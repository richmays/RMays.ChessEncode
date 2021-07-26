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
    public class ChessBoardState
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

        public void SetSpot(int f, int r, ChessPiece piece)
        {
            this.chessBoard[f, r] = piece;
        }

        /// <summary>
        /// Remove all pieces from the board, and set the board's state to the default avlues.
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
            var spot = this.chessBoard[f, r];

            return spot == 0 ? ChessColor.None
                : spot > 0 ? ChessColor.White
                : ChessColor.Black;
        }



        /// <summary>
        /// Returns an ordered list of chess moves from the given position.
        /// For each square (starting with a1, going to a8, then b1 to b8, etc), if a piece on that square is movable by the player,
        /// find all moves that piece can make (destination square starting with a1 to a8, then b1 to b8, etc).
        /// </summary>
        /// <returns></returns>
        public List<ChessMove> PossibleMoves()
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
                            if (this.SideToMove == ChessColor.White)
                            {
                                if (r == 2)
                                {
                                    // Starting row; can double-push
                                    if (this.chessBoard[f, r + 1] == ChessPiece.Space)
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f, r + 1)
                                        });

                                        if (this.chessBoard[f, r + 2] == ChessPiece.Space)
                                        {
                                            pieceMoves.Add(new ChessMove
                                            {
                                                Piece = spot,
                                                StartSquare = ChessBoardSquare.GetAN(f, r),
                                                EndSquare = ChessBoardSquare.GetAN(f, r + 2)
                                            });
                                        }
                                    }
                                }
                                else if (r == 7)
                                {
                                    // Promotion
                                    if (this.chessBoard[f, r + 1] == ChessPiece.Space)
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f, r + 1),
                                            PawnPromotedTo = PromotionChessPiece.Bishop
                                        });
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f, r + 1),
                                            PawnPromotedTo = PromotionChessPiece.Knight
                                        });
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f, r + 1),
                                            PawnPromotedTo = PromotionChessPiece.Queen
                                        });
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f, r + 1),
                                            PawnPromotedTo = PromotionChessPiece.Rook
                                        });
                                    }
                                }
                                else
                                {
                                    // Push
                                    if (this.chessBoard[f, r + 1] == ChessPiece.Space)
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = spot,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f, r + 1)
                                        });
                                    }
                                }

                                // Check for captures
                                if (this.SpotHasCapturableEnemyPiece(ChessColor.White, ChessBoardSquare.GetAN(f + 1, r + 1)))
                                {
                                    pieceMoves.Add(new ChessMove
                                    {
                                        Piece = spot,
                                        WasPieceCaptured = true,
                                        StartSquare = ChessBoardSquare.GetAN(f, r),
                                        EndSquare = ChessBoardSquare.GetAN(f + 1, r + 1)
                                    });
                                }
                                else if (this.SpotHasCapturableEnemyPiece(ChessColor.White, ChessBoardSquare.GetAN(f - 1, r + 1)))
                                {
                                    pieceMoves.Add(new ChessMove
                                    {
                                        Piece = spot,
                                        WasPieceCaptured = true,
                                        StartSquare = ChessBoardSquare.GetAN(f, r),
                                        EndSquare = ChessBoardSquare.GetAN(f - 1, r + 1)
                                    });
                                }

                            }
                            else if (this.SideToMove == ChessColor.Black)
                            {
                                if (r == 7)
                                {
                                    if (this.chessBoard[f, r - 1] == ChessPiece.Space && this.chessBoard[f, r - 2] == ChessPiece.Space)
                                    {
                                        pieceMoves.Add(new ChessMove
                                        {
                                            Piece = ChessPiece.BlackPawn,
                                            StartSquare = ChessBoardSquare.GetAN(f, r),
                                            EndSquare = ChessBoardSquare.GetAN(f, r - 2)
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
                            break;
                    }

                    moves.AddRange(pieceMoves.OrderBy(x => x.EndSquare).ThenBy(x => x.PawnPromotedTo.ToString()));
                }
            }

            return moves;
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
    }
}
