using Rmays.ChessEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RMays.ChessEncode
{
    public partial class Form2 : Form
    {
        private static readonly HttpClient client = new HttpClient();

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

        private void txtOpenPGN_Click(object sender, EventArgs e)
        {
            Task.Run(() => PostPGNString(txtPGN.Text));
        }

        private async Task PostPGNString(string pgn)
        {
            MessageBox.Show($"Copy the text in the right box, then paste into the browser window that pops up.");
            System.Diagnostics.Process.Start($"https://lichess.org/paste");
            return;

            // Old; this used to work before Lichess added authentication / required cookies to the import endpoint.
            //var values = new Dictionary<string, string>
            //{
            //    { "pgn", pgn },
            //    { "pgnFile", "" }
            //};
            //var content = new FormUrlEncodedContent(values);
            //var response = await client.PostAsync("https://lichess.org/import", content);

            //if (response.StatusCode == HttpStatusCode.RedirectMethod || response.StatusCode == HttpStatusCode.OK)
            //{
            //    System.Diagnostics.Process.Start($"{response.RequestMessage.RequestUri}");
            //}
            //else
            //{
            //    MessageBox.Show($"Error: Unexpected status code from server: {response.StatusCode}");
            //}
        }
    }
}
