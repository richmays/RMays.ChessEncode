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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var game = new ChessGame();
            game.Encode(txtPlaintext.Text);
            txtPGN.Text = game.GetPGN();
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            var pgn = txtPGN.Text;
            var result = ChessGame.Decode(pgn);
            txtPlaintext.Text = result;

        }
    }
}
