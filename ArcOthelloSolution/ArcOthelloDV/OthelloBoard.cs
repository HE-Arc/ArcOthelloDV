using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;

namespace ArcOthelloDV
{
    /// <summary>
    /// Class OthelloBoard
    /// Contains the logic of the game and stores the informations.
    /// Implements Iplayable
    /// </summary>
    [Serializable]
    public class OthelloBoard : IPlayable.IPlayable, INotifyPropertyChanged
    {
        private const int EMPTY = -1;
        private const int WHITE = 0;
        private const int BLACK = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        private int[,] board;

        public bool WhiteTurn { get; set; }

        public string TimeElapsedWhite { get; set; }
        public string TimeElapsedBlack { get; set; }

        private DispatcherTimer timer;
        private Stopwatch stopWatchWhite;
        private Stopwatch stopWatchBlack;

        public OthelloBoard()
        {
            initBoard();
            
            startTimer();
        }
        
        /// <summary>
        /// Check if the board is empty at this position
        /// </summary>
        /// <param name="column">the column</param>
        /// <param name="line">the row</param>
        /// <returns>true if the cell is empty</returns>        
        public bool isEmpty(int column, int line)
        {
            return board[column, line] == EMPTY;
        }

        private void startTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += dispatcherTimerTick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            stopWatchWhite = new Stopwatch();
            stopWatchBlack = new Stopwatch();

            displayWhiteClock();
            displayBlackClock();

            stopWatchWhite.Start();
            
            timer.Start();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void dispatcherTimerTick(object sender, EventArgs e)
        {
            displayBlackClock();
            displayWhiteClock();
        }

        private void displayWhiteClock()
        {
            TimeSpan ts = stopWatchWhite.Elapsed;
            TimeElapsedWhite = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            OnPropertyChanged("TimeElapsedWhite");
        }

        private void displayBlackClock()
        {
            TimeSpan ts = stopWatchBlack.Elapsed;
            TimeElapsedBlack = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            OnPropertyChanged("TimeElapsedBlack");
        }

        /// <summary>
        /// Initialises the board with empty values
        /// </summary>
        private void initBoard()
        {
            WhiteTurn = true;
            OnPropertyChanged("WhiteTurn");

            board = new int[9, 7];
            for(int x = 0 ; x < 9 ; x++)
            {
                for (int y = 0 ; y < 7 ; y++)
                {
                    board[x, y] = EMPTY;
                }
            }

            //setup pawns of the start of the game
            board[3, 3] = WHITE;
            board[4, 3] = BLACK;
            board[3, 4] = BLACK;
            board[4, 4] = WHITE;
        }

        private void nextPlayer()
        {
            WhiteTurn = !WhiteTurn;
            OnPropertyChanged("WhiteTurn");

            if (WhiteTurn)
            {
                stopWatchBlack.Stop();
                stopWatchWhite.Start();
            }
            else
            {
                stopWatchWhite.Stop();
                stopWatchBlack.Start();
            }
        }

        /// <summary>
        /// prints the board in the console
        /// </summary>
        public void printBoard()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    Console.Write(board[y,x]);
                    Console.Write(" ");
                }
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Get the score of one of the player
        /// </summary>
        /// <param name="color">the color of the player (0 or 1)</param>
        /// <returns>the score of the player</returns>
        private int getScore(int color)
        {
            int score = 0;

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (board[x, y] == color)
                    {
                        score++;
                    }
                }
            }

            return score;
        }

        /// <summary>
        /// gets the score of the black player
        /// </summary>
        /// <returns>the score of the black player</returns>
        public int GetBlackScore()
        {
            return getScore(BLACK);
        }

        /// <summary>
        /// get the board with -1 for empty, 0 for white and 1 for black
        /// </summary>
        /// <returns>a 2d Array of int whith the values for the pawns</returns>
        public int[,] GetBoard()
        {
            return board;
        }

        /// <summary>
        /// get the name of the IA Project
        /// </summary>
        /// <returns>the name of the IA</returns>
        public string GetName()
        {
            return "ArcOthello Donzé-Vorpe";
        }

        /// <summary>
        /// Finds the next best move to play
        /// </summary>
        /// <param name="game">the board to play on</param>
        /// <param name="level">the depth of the search</param>
        /// <param name="whiteTurn">if it is the turn of the white player</param>
        /// <returns>(col, row) of the next best move</returns>
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            // IA Part
            throw new NotImplementedException();
        }

        /// <summary>
        /// get the score of the white player
        /// </summary>
        /// <returns>the score of the white player</returns>
        public int GetWhiteScore()
        {
            return getScore(WHITE);
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            // TODO
            return true;
        }

        /// <summary>
        /// Play at a (col, row) position if possible
        /// </summary>
        /// <param name="column">the column to play on</param>
        /// <param name="line">the row to play on</param>
        /// <param name="isWhite">if it is the white player's turn</param>
        /// <returns>true if could play</returns>
        public bool PlayMove(int column, int line, bool isWhite)
        {
            if (IsPlayable(column, line, isWhite))
            {
                if (isWhite)
                {
                    board[column, line] = WHITE;
                }
                else
                {
                    board[column, line] = BLACK;
                }

                nextPlayer();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}