using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArcOthelloDV
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            OthelloBoard othelloBoard = new OthelloBoard();

            for (int i = 0 ; i < 9 ; i++)
            {
                for (int j = 0 ; j < 11 ; j++)
                {
                    if (i > 0 && i < 8 && j > 0 && j < 10)
                    {
                        Rectangle rect = new Rectangle();
                        rect.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        rect.Fill = new SolidColorBrush(Color.FromRgb(0, 178, 0));
                        Grid.SetRow(rect, i);
                        Grid.SetColumn(rect, j);
                        board.Children.Add(rect);
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
        }   
    }
}
