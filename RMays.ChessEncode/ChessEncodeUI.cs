using Rmays.ChessEngine;
using RMays.ChessEncode.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMays.ChessEncode
{
    public partial class ChessEncodeUI : Form
    {
        public event EventHandler MoveButtonClicked;
        protected PictureBox[,] squares = new PictureBox[8,8];
        private Bitmap darkSquares;
        private Bitmap lightSquares;
        private ResourceManager resourceManager;
        private MovePicker movePicker;

        private int squareLength => Math.Max(GetInt(txtSquareSideLength), 1);
        private int imgLeftOffset => GetInt(txtImgLeftOffset);
        private int imgTopOffset => GetInt(txtImgTopOffset);
        private int imgHPadding => GetInt(txtImgHPadding);
        private int imgVPadding => GetInt(txtImgVPadding);
        private int boardLeftOffset => GetInt(txtBoardLeftOffset);
        private int boardTopOffset => GetInt(txtBoardTopOffset);
        private int boardHPadding => GetInt(txtBoardHPadding);
        private int boardVPadding => GetInt(txtBoardVPadding);

        private ChessBoardState board = new ChessBoardState();

        public ChessEncodeUI()
        {
            InitializeComponent();

            resourceManager = Resources.ResourceManager;
            darkSquares = (Bitmap)resourceManager.GetObject("chesspieces_darkBG");
            lightSquares = (Bitmap)resourceManager.GetObject("chesspieces_whiteBG");

            //board.TryMakeMove(0);
            //board.TryMakeMove(0);

            PrintBoard();
            ResetMovePicker();
        }

        private void ResetMovePicker()
        {
            this.movePicker = new MovePicker { Owner = this };
            this.movePicker.Show();
            this.movePicker.RefreshMoves(board);
        }

        public void HandleMove(object sender, EventArgs e)
        {

        }

        private Dictionary<string, (int, int)> PieceImageOffsets = new Dictionary<string, (int, int)>
        {
            {  "WhiteKing", (1,1) },
            {  "WhiteQueen", (1,2) },
            {  "WhiteBishop", (1,3) },
            {  "WhiteKnight", (1,4) },
            {  "WhiteRook", (1,5) },
            {  "WhitePawn", (1,6) },
            {  "Space", (1,7) },
            {  "BlackKing", (2,1) },
            {  "BlackQueen", (2,2) },
            {  "BlackBishop", (2,3) },
            {  "BlackKnight", (2,4) },
            {  "BlackRook", (2,5) },
            {  "BlackPawn", (2,6) }
        };

        private void PrintBoard()
        {
            this.SuspendLayout();
            for (int r = 1; r <= 8; r++)
            {
                for (int f = 1; f <= 8; f++)
                {
                    var piece = board.GetSpot(f, r);
                    var offsets = PieceImageOffsets[piece.ToString()];

                    // To prevent excessive flashing (64x -> 2x), only replace the images that actually changed.
                    var newPiece = GetPieceWithOffset(f, r, offsets.Item1, offsets.Item2, piece.ToString());
                    var currSquare = squares[f - 1, r - 1];
                    if (currSquare == null)
                    {
                        squares[f - 1, r - 1] = newPiece;
                    }
                    else if (currSquare.Tag != newPiece.Tag)
                    {
                        squares[f - 1, r - 1].Dispose();
                        squares[f - 1, r - 1] = newPiece;
                    }
                }
            }
            this.ResumeLayout();
        }

        private void PrintBoard_old()
        {
            this.SuspendLayout();

            // Let's print an empty chessboard.
            for (int r = 1; r <= 8; r++) // rank (row)
            {
                for (int f = 1; f <= 8; f++) // file (col)
                {
                    if (squares[f - 1, r - 1] != null)
                    {
                        squares[f - 1, r - 1].Dispose();
                    }

                    squares[f - 1, r - 1] = GetPieceWithOffset(f, r, r, f, "Blank");
                }
            }

            this.ResumeLayout();
        }

        private PictureBox GetPieceWithOffset(int f, int r, int row, int col, string label)
        {
            var pb = new PictureBox();
            pb.Tag = label;
            pb.BackgroundImage = cropImage((f + r) % 2 == 1 ? lightSquares : darkSquares,
                new Rectangle(
                    this.imgHPadding * col + this.imgLeftOffset,
                    this.imgVPadding * row + this.imgTopOffset,
                    squareLength, squareLength));
            pb.Parent = this;
            pb.Location = new Point(f * squareLength + this.boardLeftOffset, (9-r) * squareLength + this.boardTopOffset);
            pb.Height = squareLength;
            pb.Width = squareLength;
            return pb;
        }

        private int GetInt(NumericUpDown tb)
        {
            var success = int.TryParse(tb.Text, out int result);
            return (success ? result : 0);
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            if (cropArea.Left < 0) return BrokenImage;
            if (cropArea.Top < 0) return BrokenImage;
            if (cropArea.Left + cropArea.Width > bmpImage.Width) return BrokenImage;
            if (cropArea.Top + cropArea.Height > bmpImage.Height) return BrokenImage;
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        private static Image _brokenImage = null;
        private static Image BrokenImage
        {
            get
            {
                if (_brokenImage == null)
                {
                    _brokenImage = (Bitmap)Resources.ResourceManager.GetObject("broken");
                }
                return _brokenImage;
            }
        }

        private void txtSquareSideLength_ValueChanged(object sender, EventArgs e)
        {
            PrintBoard();
        }

        private void txtImgLeftOffset_ValueChanged(object sender, EventArgs e)
        {
            PrintBoard();
        }

        private void txtImgTopOffset_ValueChanged(object sender, EventArgs e)
        {
            PrintBoard();
        }

        private void txtImgHPadding_ValueChanged(object sender, EventArgs e)
        {
            PrintBoard();
        }

        private void txtImgVPadding_ValueChanged(object sender, EventArgs e)
        {
            PrintBoard();
        }

        private void txtBoardLeftOffset_ValueChanged(object sender, EventArgs e)
        {
            PrintBoard();
        }

        private void txtBoardTopOffset_ValueChanged(object sender, EventArgs e)
        {
            PrintBoard();
        }

        private void btnToggleBoardLayoutNumbers_Click(object sender, EventArgs e)
        {
            ToggleVisibility(label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label11, label11,
                txtSquareSideLength,
                txtImgHPadding, txtImgVPadding, txtImgLeftOffset, txtImgTopOffset,
                txtBoardHPadding, txtBoardVPadding, txtBoardTopOffset, txtBoardLeftOffset);
        }

        private void ToggleVisibility(params Control[] controls)
        {
            if (controls.Length == 0) return;
            var targetVisibility = !controls[0].Visible;
            foreach (Control control in controls)
            {
                control.Visible = targetVisibility;
            }
        }

        private void btnToggleMovePicker_Click(object sender, EventArgs e)
        {
            if (movePicker == null)
            {
                ResetMovePicker();
            }
            else
            {
                movePicker.Visible = !movePicker.Visible;
            }
        }

        internal void btnMakeMoveButton_Click(object sender, EventArgs e)
        {
            var moveId = int.Parse(((Button)sender).Tag.ToString());
            board.TryMakeMove(moveId);
            PrintBoard();
            this.movePicker.RefreshMoves(board);
            //MessageBox.Show($"Move: {moveText}");
        }
    }
}
