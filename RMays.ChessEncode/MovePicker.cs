using Rmays.ChessEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMays.ChessEncode
{
    public partial class MovePicker : Form
    {
        public MovePicker()
        {
            InitializeComponent();
            this.Hide();

            //AddButtons(250);
        }

        private void AddButtons(int buttonsToAdd)
        {
            for (var buttonId = 0; buttonId < buttonsToAdd; buttonId++)
            {
                var button = new Button { Name = $"button{buttonId}", Text = "?", Tag = buttonId.ToString() };
                button.Top = (buttonId / 5) * 24 + 50;
                button.Left = (buttonId % 5) * 90 + 10;
                button.Click += new System.EventHandler(((ChessEncodeUI)Owner).btnMakeMoveButton_Click);
                button.Hide();
                Controls.Add(button);
            }
        }

        public void RefreshMoves(ChessBoardState board)
        {
            if (!Controls.Find("button1", false).Any())
            {
                AddButtons(250);
            }
            
            // Update the active player.
            this.label1.Text = (board.GetSideToMove() == ChessColor.White ? "White's turn" : "Black's turn");

            // Hide all buttons.
            foreach (Button button in Controls.OfType<Button>())
            {
                button.Hide();
            }

            /*
            int safety = 0;
            while(controlsToRemove.Any() && safety < 1000)
            {
                Controls.Remove(controlsToRemove.First());
                safety++;
            }

            if (safety >= 1000)
            {
                throw new ApplicationException("Caught in an infinite loop removing controls in MovePicker.");
            }
            */

            // Show the buttons that correspond with possible moves.
            var movesAdded = 0;
            foreach(var move in board.PossibleMoves())
            {
                var button = (Button)Controls.Find($"button{movesAdded}", false).First();
                if (button == null)
                {
                    throw new ApplicationException($"Couldn't find the control 'button{movesAdded}'.");
                }
                button.Text = move.ToString();
                button.Show();
                /*var button = new Button { Name = "button1", Text = move.ToString(), Tag = movesAdded.ToString() };
                button.Top = (movesAdded / 5) * 24 + 50;
                button.Left = (movesAdded % 5) * 90 + 10;
                button.Click += new System.EventHandler(((ChessEncodeUI)Owner).btnMakeMoveButton_Click);
                Controls.Add(button);
                */
                movesAdded++;
            }
        }
    }
}
