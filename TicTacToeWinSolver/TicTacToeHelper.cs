using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeWinSolver
{
    public class TicTacToeHelper
    {
        /// <summary>
        /// Get a field state from a char.
        /// </summary>
        /// <param name="fieldChar">
        /// The char to be converted. Can only be 'x', 'o' or 'n'.
        /// </param>
        /// <returns></returns>
        public static TicTacToeFieldState GetFieldFromChar(char fieldChar)
        {
            if (fieldChar == 'x') return TicTacToeFieldState.X;
            else if (fieldChar == 'o') return TicTacToeFieldState.O;
            else if (fieldChar == 'n') return TicTacToeFieldState.None;
            else throw new FormatException("fieldchar should be either 'x', 'o' or 'n'");
        }

        public readonly int BoardLength;
        public readonly int WinLength;        
        public readonly int[,] wins;

        public TicTacToeHelper(int boardLength, int winLength)
        {
            if (boardLength < 1) throw new ArgumentOutOfRangeException("boardLength", "Board length can't be less than 1!");
            else if (winLength > boardLength) throw new ArgumentOutOfRangeException("winLength", "Winning line length can't be longer than board length!");
            BoardLength = boardLength;
            WinLength = winLength;

            int multiplier = 1 + BoardLength - WinLength;

            //amount of possible horizontal / vertical wins;
            int horWins = BoardLength * multiplier;

            //amount of possible diagonal wins;
            int diaWins = multiplier * multiplier;

            int allWins = horWins * 2 + diaWins * 2;

            //first dimension - amount of wins, second dimension - length of win
            wins = new int[allWins, WinLength];

            //write horizontal and vertical win cases
            for (int i = 0; i < BoardLength; i += 1)
            {
                //multiplier tells us how many win cases are on the same line
                for (int k = 0; k < multiplier; k += 1)
                {
                    //x-coordinates of the currently calculated win case
                    //x is horizontal and X is vertical
                    int x = i * multiplier + k;
                    int X = x + horWins;

                    //horizontal value offset, refers to the points position
                    //on the board if it is conveyed as a one-dimensional array
                    //this is where the line starts from 
                    int v = i * BoardLength + k;

                    //vertical value offset, same story here
                    int q = i + k * BoardLength;

                    for (int l = 0; l < WinLength; l += 1)
                    {
                        //for horizontal, each new part of the win line is
                        //just one off
                        wins[x, l] = v + l;

                        //for vertical, each new part of the win line is
                        //in a new row, so we add the length of the row
                        wins[X, l] = q + l * BoardLength;
                    }
                }
            }

            //write diagonal up and down win cases
            for (int i = 0; i < multiplier; i += 1)
            {
                for (int k = 0; k < multiplier; k += 1)
                {
                    int x = i * multiplier + k + horWins * 2;
                    int X = x + diaWins;

                    //downwards diagonal offset
                    int v = i * BoardLength + k;

                    //downwards diagonal addition
                    int g = BoardLength + 1;

                    //upwards diagonal offset
                    int q = i * BoardLength + BoardLength - k - 1;

                    //upwards diagonal addition
                    int p = BoardLength - 1;

                    for (int l = 0; l < WinLength; l += 1)
                    {
                        int y = l;

                        wins[x, y] = v + l * g;

                        wins[X, y] = q + l * p;
                    }
                }
            }
        }

        public TicTacToeGameState CheckGameState(TicTacToeFieldState[] board)
        {
            if (board.Length != BoardLength * BoardLength)
                throw new ArgumentOutOfRangeException("board", "The array should be as long as the board!");

            //first assume the game is tied
            TicTacToeGameState gameState = TicTacToeGameState.Tied;

            //check if the game is continuable
            for (int i = 0; i < board.Length; i++)
            {
                //if there's an empty field, the game is continuable.
                if (board[i] == TicTacToeFieldState.None)
                {                    
                    gameState = TicTacToeGameState.Continuable;
                    break;
                }
            }

            //check if x or o has won the game
            for (int i = 0; i < wins.GetLength(0); i++)
            {
                TicTacToeFieldState firstField = board[wins[i, 0]];

                //no need to do the win check if the field we picked is empty
                if (firstField != TicTacToeFieldState.None)
                {
                    //assume this is going to be a victory line
                    bool victory = true;

                    for (int k = 1; k < wins.GetLength(1); k++)
                    {
                        TicTacToeFieldState field = board[wins[i, k]];

                        //if selected field isn't the same as the first picked one,
                        //then this wasn't a winning line.
                        if (firstField != field)
                        {
                            victory = false;
                            break;
                        }
                    }

                    //if a winning line was found, change gameState
                    if (victory)
                    {
                        if (firstField == TicTacToeFieldState.X)
                        {
                            gameState = TicTacToeGameState.XWon;
                        }
                        else
                        {
                            gameState = TicTacToeGameState.OWon;
                        }
                        break;
                    }
                }
            }

            //the true game state is found and we can return it
            return gameState;
        }
    }

    public enum TicTacToeFieldState
    {
        None = 0,
        X = 1,
        O = 2
    }

    public enum TicTacToeGameState
    {
        Continuable = 0,
        XWon = 1,
        OWon = 2,
        Tied = 3
    }
}
