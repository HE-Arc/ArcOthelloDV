﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ArcOthelloDV
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SolidColorBrush EMPTY = new SolidColorBrush(Color.FromRgb(0, 178, 0));
        SolidColorBrush WHITE = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        SolidColorBrush BLACK = new SolidColorBrush(Color.FromRgb(0, 0, 0));

        Ellipse[,] ellipses;
        OthelloBoard othelloBoard;

        public MainWindow()
        {
            InitializeComponent();

            othelloBoard = new OthelloBoard();

            this.DataContext = othelloBoard;

            ellipses = new Ellipse[9, 7];

            for (int i = 0 ; i < 9 ; i++)
            {
                for (int j = 0 ; j < 11 ; j++)
                {
                    if (i > 0 && i < 8 && j > 0 && j < 10)
                    {
                        Rectangle rect = new Rectangle();
                        rect.Stroke = BLACK;
                        rect.Fill = EMPTY;
                        Grid.SetRow(rect, i);
                        Grid.SetColumn(rect, j);
                        board.Children.Add(rect);
                        
                        Ellipse ellipse = new Ellipse();
                        ellipse.Fill = EMPTY;
                        ellipse.Margin = new Thickness(3);
                        Grid.SetRow(ellipse, i);
                        Grid.SetColumn(ellipse, j);
                        board.Children.Add(ellipse);

                        ellipses[j - 1, i - 1] = ellipse;

                        rect.MouseEnter += new MouseEventHandler(r_MouseEnter);
                        rect.MouseLeave += new MouseEventHandler(r_MouseLeave);
                        rect.MouseLeftButtonDown += new MouseButtonEventHandler(r_MouseClick);

                        ellipse.MouseEnter += new MouseEventHandler(r_MouseEnter);
                        ellipse.MouseLeave += new MouseEventHandler(r_MouseLeave);
                        ellipse.MouseLeftButtonDown += new MouseButtonEventHandler(r_MouseClick);

                        void r_MouseClick(object sender, MouseEventArgs e)
                        {
                            var element = (UIElement)e.Source;

                            int c = Grid.GetColumn(element) - 1;
                            int r = Grid.GetRow(element) - 1;

                            othelloBoard.PlayMove(c, r, othelloBoard.WhiteTurn); //Play on the board at this position

                            updateBoardDisplay(othelloBoard.GetBoard());
                        }

                        void r_MouseLeave(object sender, MouseEventArgs e)
                        {
                            var element = (UIElement)e.Source;

                            this.Cursor = Cursors.Arrow;

                            int c = Grid.GetColumn(element) - 1;
                            int r = Grid.GetRow(element) - 1;

                            if (othelloBoard.isEmpty(c, r))
                            {
                                ellipse.Fill = EMPTY;
                            }
                        }

                        void r_MouseEnter(object sender, MouseEventArgs e)
                        {
                            var element = (UIElement)e.Source;

                            this.Cursor = Cursors.Hand;

                            int c = Grid.GetColumn(element) - 1;
                            int r = Grid.GetRow(element) - 1;
                            
                            if (othelloBoard.IsPlayable(c, r, othelloBoard.WhiteTurn))
                            {
                                if (othelloBoard.WhiteTurn)
                                {
                                    ellipse.Fill = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));
                                }
                                else
                                {
                                    ellipse.Fill = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
                                }
                            }
                        }

                    }
                    else
                    {
                        if (i == 0 && j > 0 && j < 10)
                        {
                            TextBlock tb = new TextBlock();
                            tb.Inlines.Add(((char)(j+64)).ToString());
                            tb.TextAlignment = TextAlignment.Center;
                            tb.VerticalAlignment = VerticalAlignment.Center;
                            tb.FontSize = 22;
                            tb.FontWeight = FontWeights.Bold;
                            Grid.SetRow(tb, i);
                            Grid.SetColumn(tb, j);
                            board.Children.Add(tb);
                        }
                        else if (j == 0 && i > 0 && i <8)
                        {
                            TextBlock tb = new TextBlock();
                            tb.Inlines.Add((i).ToString());
                            tb.TextAlignment = TextAlignment.Center;
                            tb.VerticalAlignment = VerticalAlignment.Center;
                            tb.FontSize = 22;
                            tb.FontWeight = FontWeights.Bold;
                            Grid.SetRow(tb, i);
                            Grid.SetColumn(tb, j);
                            board.Children.Add(tb);
                        }
                    }
                }
            }
            
            updateBoardDisplay(othelloBoard.GetBoard());
        }

        private void updateBoardDisplay(int[,] board)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (board[x, y] == 0)
                    {
                        ellipses[x, y].Fill = WHITE;
                    }
                    else if(board[x,y] == 1)
                    {
                        ellipses[x, y].Fill = BLACK;
                    }
                    else
                    {
                        ellipses[x, y].Fill = EMPTY;
                    }
                }
            }
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuNewGame_Click(object sender, RoutedEventArgs e)
        {
            othelloBoard.NewGame();
            updateBoardDisplay(othelloBoard.GetBoard());
        }

        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.FileName = "othelloSave.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);

                othelloBoard.Save();

                formatter.Serialize(stream, othelloBoard);
                stream.Close();
            }
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                try
                {
                    othelloBoard = (OthelloBoard)formatter.Deserialize(stream);

                    this.DataContext = othelloBoard;

                    othelloBoard.Restore();

                    updateBoardDisplay(othelloBoard.GetBoard());
                }
                catch
                {
                    MessageBox.Show("Invalid File Type", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
