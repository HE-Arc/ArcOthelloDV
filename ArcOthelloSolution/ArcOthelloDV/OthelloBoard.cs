using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;

namespace ArcOthelloDV
{
    
    public class OthelloBoard : IPlayable.IPlayable, INotifyPropertyChanged
    {
        private const int EMPTY = -1;
        private const int WHITE = 0;
        private const int BLACK = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        private int[,] board;

        private bool whiteTurn;

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
            if (stopWatchWhite.IsRunning)
            {
                displayWhiteClock();
            }
            else if (stopWatchBlack.IsRunning)
            {
                displayBlackClock();
            }
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
            whiteTurn = true;
            board = new int[9, 7];
            for(int x = 0 ; x < 9 ; x++)
            {
                for (int y = 0 ; y < 7 ; y++)
                {
                    board[x, y] = EMPTY;
                }
            }
        }

        private void nextPlayer()
        {
            whiteTurn = !whiteTurn;
            if (whiteTurn)
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

        public int GetBlackScore()
        {
            return getScore(BLACK);
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public string GetName()
        {
            return "ArcOthello Donzé-Vorpe";
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            // IA Part
            throw new NotImplementedException();
        }

        public int GetWhiteScore()
        {
            return getScore(WHITE);
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            // TODO
            return true;
        }

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

                whiteTurn = !whiteTurn;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}