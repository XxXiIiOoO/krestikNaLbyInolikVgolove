using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace YaKrestik
{
    public partial class MainWindow : Window
    {
        private bool isPlayerX = true; 
        private int[,] board = new int[3, 3];
        private object grid;
        private bool isPlayerXSelected = true; 


        public MainWindow()
        {
            InitializeComponent();
        }

        private void CellButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            if (board[row, col] == 0)
            {
                button.Content = "X";
                board[row, col] = 1;

                if (CheckForWinner())
                {
                    MessageBox.Show("я выграл кампуктер");
                    ResetGame();
                }
                else if (IsBoardFull())
                {
                    MessageBox.Show("Никто не выйграл поэтом всем по пивасу");
                    ResetGame();
                }
                else
                {
                    MakeComputerMove();
                }
            }
        }


        private Tuple<int, int> GetBestMove()
        {
            int bestScore = int.MinValue;
            Tuple<int, int> bestMove = null;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        board[i, j] = -1; 
                        int score = MiniMax(board, 0, false);
                        board[i, j] = 0; 

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = new Tuple<int, int>(i, j);
                        }
                    }
                }
            }

            return bestMove;
        }

        private int MiniMax(int[,] board, int depth, bool isMaximizing)
        {
            if (CheckForWinner())
            {
                return isMaximizing ? -1 : 1;
            }

            if (IsBoardFull())
            {
                return 0;
            }

            int bestScore = isMaximizing ? int.MinValue : int.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        board[i, j] = isMaximizing ? -1 : 1;
                        int score = MiniMax(board, depth + 1, !isMaximizing);
                        board[i, j] = 0;

                        bestScore = isMaximizing ? Math.Max(bestScore, score) : Math.Min(bestScore, score);
                    }
                }
            }

            return bestScore;
        }

        private void MakeComputerMove()
        {
            Tuple<int, int> computerMove = GetBestMove();
            Button computerButton = GetButtonByRowAndColumn(computerMove.Item1, computerMove.Item2);
            computerButton.Content = "O";
            board[computerMove.Item1, computerMove.Item2] = -1;

            if (CheckForWinner())
            {
                MessageBox.Show("кампутер выйграл");
                ResetGame();
            }
            else if (IsBoardFull())
            {
                MessageBox.Show("никто не выграл поэтому всем по пивасу");
                ResetGame();
            }
        }


        private Button GetButtonByRowAndColumn(int row, int col)
        {
            string buttonName = $"btn{row}{col}";
            return (Button)this.FindName(buttonName);
        }


        private bool CheckForWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Math.Abs(board[i, 0] + board[i, 1] + board[i, 2]) == 3 ||
                    Math.Abs(board[0, i] + board[1, i] + board[2, i]) == 3)
                {
                    return true;
                }
            }

            if (Math.Abs(board[0, 0] + board[1, 1] + board[2, 2]) == 3 ||
                Math.Abs(board[0, 2] + board[1, 1] + board[2, 0]) == 3)
            {
                return true;
            }

            return false;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void PlayerX_Checked(object sender, RoutedEventArgs e)
        {
            isPlayerXSelected = true;
            ResetGame();
        }

        private void PlayerO_Checked(object sender, RoutedEventArgs e)
        {
            isPlayerXSelected = false;
            MakeComputerMove(); 
        }


        private void ResetGame()
        {
            isPlayerX = true;
            board = new int[3, 3];

            foreach (var button in GetAllButtons())
            {
                button.Content = "";
            }
        }

        private IEnumerable<Button> GetAllButtons()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string buttonName = $"btn{i}{j}";
                    Button button = (Button)this.FindName(buttonName);
                    if (button != null)
                    {
                        yield return button;
                    }
                }
            }
        }
    }
}
