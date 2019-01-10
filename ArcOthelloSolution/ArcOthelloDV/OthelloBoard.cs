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

        public int BlackScore { get; set; }
        public int WhiteScore { get; set; }

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

        private void initScore()
        {
            WhiteScore = 0;
            OnPropertyChanged("WhiteScore");
            BlackScore = 0;
            OnPropertyChanged("BlackScore");
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
            // board[column, line]
            // empty = -1, white = 0, black = 1

            if (board[column, line] != -1)
            {
                return false;
            }

            bool droite = true, droiteBas = true, bas = true, basGauche = true, gauche = true, gaucheHaut = true, haut = true, hautDroite = true;

            for (int i = 1; i <= 8; i++)
            {
                int testColumn = column;
                int testLine = line;

                // si i = 1, il faut tester que c'est la couleur opposée sinon c'est pas valide dans cette 
                // TODO
                if (i == 1)
                {
                    // test à droite :
                    testColumn += i;
                    if (testColumn >= 9 || board[testColumn, testLine] == -1 || (isWhite && board[testColumn, testLine] == 0) || (!isWhite && board[testColumn, testLine] == 1))
                    {
                        droite = false;
                    }

                    // test en bas à droite
                    testLine += i;
                    if (testColumn >= 9 || testLine > 7 || board[testColumn, testLine] == -1 || (isWhite && board[testColumn, testLine] == 0) || (!isWhite && board[testColumn, testLine] == 1))
                    {
                        droiteBas = false;
                    }

                    // test en bas
                    testColumn -= i;
                    if (testLine > 7 || board[testColumn, testLine] == -1 || (isWhite && board[testColumn, testLine] == 0) || (!isWhite && board[testColumn, testLine] == 1))
                    {
                        bas = false;
                    }

                    // test en bas à gauche
                    testColumn -= i;
                    if (testColumn < 0 || testLine > 7 || board[testColumn, testLine] == -1 || (isWhite && board[testColumn, testLine] == 0) || (!isWhite && board[testColumn, testLine] == 1))
                    {
                        basGauche = false;
                    }

                    // test à gauche
                    testLine -= i;
                    if (testColumn < 0 || board[testColumn, testLine] == -1 || (isWhite && board[testColumn, testLine] == 0) || (!isWhite && board[testColumn, testLine] == 1))
                    {
                        gauche = false;
                    }

                    // test en haut à gauche
                    testLine -= i;
                    if (testColumn < 0 || testLine < 0 || board[testColumn, testLine] == -1 || (isWhite && board[testColumn, testLine] == 0) || (!isWhite && board[testColumn, testLine] == 1))
                    {
                        gaucheHaut = false;
                    }

                    // test en haut
                    testColumn += i;
                    if (testLine < 0 || board[testColumn, testLine] == -1 || (isWhite && board[testColumn, testLine] == 0) || (!isWhite && board[testColumn, testLine] == 1))
                    {
                        haut = false;
                    }

                    // test en haut à droite
                    testColumn += i;
                    if (testColumn >= 9 || testLine < 0 || board[testColumn, testLine] == -1 || (isWhite && board[testColumn, testLine] == 0) || (!isWhite && board[testColumn, testLine] == 1))
                    {
                        hautDroite = false;
                    }
                }

                // si i > 1 : 
                // si vide --> pas valide dans cette direction
                // si la case était blanche --> si cette case est noire on continue, si elle est blanche on a un coup valide
                // même idée si la case était noire
                // test à droite :
                testColumn += i;
                if (droite)
                {
                    if (testColumn >= 9 || board[testColumn, testLine] == -1)
                    {
                        droite = false;
                    }
                    else if (isWhite)
                    {
                        if (board[testColumn, testLine] == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board[testColumn, testLine] == 1)
                        {
                            return true;
                        }
                    }
                }

                // test en bas à droite
                testLine += i;
                if (droiteBas)
                {
                    if (testColumn >= 9 || testLine > 7 || board[testColumn, testLine] == -1)
                    {
                        droiteBas = false;
                    }
                    else if (isWhite)
                    {
                        if (board[testColumn, testLine] == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board[testColumn, testLine] == 1)
                        {
                            return true;
                        }
                    }
                }

                // test en bas
                testColumn -= i;
                if (bas)
                {
                    if (testLine > 7 || board[testColumn, testLine] == -1)
                    {
                        bas = false;
                    }
                    else if (isWhite)
                    {
                        if (board[testColumn, testLine] == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board[testColumn, testLine] == 1)
                        {
                            return true;
                        }
                    }
                }

                // test en bas à gauche
                testColumn -= i;
                if (basGauche)
                {
                    if (testColumn < 0 || testLine > 7 || board[testColumn, testLine] == -1)
                    {
                        basGauche = false;
                    }
                    else if (isWhite)
                    {
                        if (board[testColumn, testLine] == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board[testColumn, testLine] == 1)
                        {
                            return true;
                        }
                    }
                }

                // test à gauche
                testLine -= i;
                if (gauche)
                {
                    if (testColumn < 0 || board[testColumn, testLine] == -1)
                    {
                        gauche = false;
                    }
                    else if (isWhite)
                    {
                        if (board[testColumn, testLine] == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board[testColumn, testLine] == 1)
                        {
                            return true;
                        }
                    }
                }

                // test en haut à gauche
                testLine -= i;
                if (gaucheHaut)
                {
                    if (testColumn < 0 || testLine < 0 || board[testColumn, testLine] == -1)
                    {
                        gaucheHaut = false;
                    }
                    else if (isWhite)
                    {
                        if (board[testColumn, testLine] == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board[testColumn, testLine] == 1)
                        {
                            return true;
                        }
                    }
                }

                // test en haut
                testColumn += i;
                if (haut)
                {
                    if (testLine < 0 || board[testColumn, testLine] == -1)
                    {
                        haut = false;
                    }
                    else if (isWhite)
                    {
                        if (board[testColumn, testLine] == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board[testColumn, testLine] == 1)
                        {
                            return true;
                        }
                    }
                }

                // test en haut à droite
                testColumn += i;
                if (hautDroite)
                {
                    if (testColumn >= 9 || testLine < 0 || board[testColumn, testLine] == -1)
                    {
                        hautDroite = false;
                    }
                    else if (isWhite)
                    {
                        if (board[testColumn, testLine] == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board[testColumn, testLine] == 1)
                        {
                            return true;
                        }
                    }
                }

            }

            return false;
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

                WhiteScore = GetWhiteScore();
                OnPropertyChanged("WhiteScore");
                BlackScore = GetBlackScore();
                OnPropertyChanged("BlackScore");

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}