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
            for(int i = 0 ; i < 7 ; i++)
            {
                for(int j = 0 ; j < 9 ; j++)
                {

                    Rectangle rect = new Rectangle();
                    rect.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, j);

                    board.Children.Add(rect);
                }
            }
        }
    }
}
