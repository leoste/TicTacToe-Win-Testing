using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToeWinSolver
{
    public partial class Form1 : Form
    {
        //change the numbers to test different boards
        const int boardLength = 5;
        const int winLength = 4;

        //this is so different functions can access the helper
        TicTacToeHelper helper;

        //this is used for the interactive label
        int lWidth;
        int lHeight;

        public Form1()
        {
            InitializeComponent();

            helper = new TicTacToeHelper(boardLength, winLength);
            
            //longer text because i had some weird issues with shorter one not accurately
            //depicting the length of the text
            lWidth = TextRenderer.MeasureText("n n n n n n n n n ", label5.Font).Width / 9;
            lHeight = TextRenderer.MeasureText("n", label5.Font).Height;

            StringBuilder label5builder = new StringBuilder();

            for (int i = 0; i < helper.BoardLength; i++)
            {
                for (int k = 0; k < helper.BoardLength; k++)
                { 
                    label5builder.Append("n ");
                }
                label5builder.Append('\n');
            }

            label5.Text = label5builder.ToString();

            ShowWinString();
        }

        #region Information Displaying Functions

        //this function isn't optimal but it works; after all it isn't the main focus of this project anyways.
        void ShowWinString()
        {
            List<char> chars = Enumerable.Repeat('n', helper.BoardLength * helper.BoardLength).ToList();            
            StringBuilder label3builder = new StringBuilder();

            //replace some characters with x to reflect win case
            for (int i = 0; i < helper.WinLength; i++)
            {
                chars[helper.wins[selection, i]] = 'x';
                label3builder.Append(helper.wins[selection, i].ToString() + "; ");
            }

            label3.Text = label3builder.ToString();
            label3builder.Clear();

            //add spaces to make it look more like a square
            for (int i = helper.BoardLength * helper.BoardLength - 1; i >= 1; i--)
            {
                chars.Insert(i, ' ');
            }

            //add newlines to make it look square instead of one line
            for (int i = helper.BoardLength - 1; i > 0; i--)
            {
                chars.Insert(i * helper.BoardLength * 2, '\n');
            }

            label1.Text = new string(chars.ToArray());
            label2.Text = selection.ToString();
        }

        //this function asks the helper about the game state
        void ShowGameState()
        {
            //first we need to convert the text to a form that the helper understands
            char[] text = label5.Text.ToCharArray();

            List<TicTacToeFieldState> board = new List<TicTacToeFieldState>();

            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    TicTacToeFieldState fieldState = TicTacToeHelper.GetFieldFromChar(text[i]);
                    board.Add(fieldState);
                }
                catch
                {

                }
            }

            //get the game state
            TicTacToeGameState gameState = helper.CheckGameState(board.ToArray());

            if (gameState == TicTacToeGameState.Continuable)
            {
                label7.Text = "game is continuable.";
            }
            else if (gameState == TicTacToeGameState.Tied)
            {
                label7.Text = "game is tied.";
            }
            else if (gameState == TicTacToeGameState.XWon)
            {
                label7.Text = "x has won the game.";
            }
            else if (gameState == TicTacToeGameState.OWon)
            {
                label7.Text = "o has won the game.";
            }
        }
        #endregion

        int selection = 0;

        #region Event Functions

        private void button1_Click(object sender, EventArgs e)
        {
            selection--;
            if (selection < 0) selection = helper.wins.GetLength(0) - 1;
            ShowWinString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selection++;
            if (selection >= helper.wins.GetLength(0)) selection = 0;
            ShowWinString();
        }

        private void label5_MouseClick(object sender, MouseEventArgs e)
        {
            //this is the position that was clicked on            
            int pos = e.X / lWidth * 2 + e.Y / lHeight * (1 + helper.BoardLength * 2);
            
            //convert to char array to work on it better
            char[] text = label5.Text.ToCharArray();

            if (pos >= text.Length) return;

            //left mouse click - scroll forward. other mouse click - scroll backward (try it out)
            if (text[pos] == 'n')
            {
                if (e.Button == MouseButtons.Left)
                {
                    text[pos] = 'x';
                }
                else
                {
                    text[pos] = 'o';
                }
            }
            else if (text[pos] == 'x')
            {
                if (e.Button == MouseButtons.Left)
                {
                    text[pos] = 'o';
                }
                else
                {
                    text[pos] = 'n';
                }
            }
            else if (text[pos] == 'o')
            {
                if (e.Button == MouseButtons.Left)
                {
                    text[pos] = 'n';
                }
                else
                {
                    text[pos] = 'x';
                }
            }

            label5.Text = new string(text);

            ShowGameState();
        }
        #endregion
    }
}
