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

namespace PictureDraw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonRectangle_Click(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            var rectangle = new Rectangle("MyRect", "Red", rand.Next(100, 200), rand.Next(100, 200), mainGrid);
            rectangle.Draw(rand.Next(100, 200), rand.Next(100, 200));
        }
    }
}
