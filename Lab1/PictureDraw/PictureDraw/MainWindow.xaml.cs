using System;
using System.Collections;
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
        private ArrayList ListShapes = new ArrayList();
        private readonly Random _rand = new Random();
        private int RandomPositionX { get; set; }
        private int RandomPositionY { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }        

        private void buttonRectangle_Click(object sender, RoutedEventArgs e)
        {
            RandomPositionX = _rand.Next(100, 800);            
            RandomPositionY = _rand.Next(100, 400);            
            var rectangle = new Rectangles(
                100, 200, "MyRect", GlobalProperties.ColorFill, GlobalProperties.ColorStroke, 
                mainCanvas, RandomPositionX, RandomPositionY
            );
            rectangle.Draw();
            ListShapes.Add(rectangle);
        }

        private void buttonCircle_Click(object sender, RoutedEventArgs e)
        {
            RandomPositionX = _rand.Next(100, 800);
            RandomPositionY = _rand.Next(100, 400);
            var circle = new Circles(
                30, "MyCircle", GlobalProperties.ColorFill, GlobalProperties.ColorStroke,
                mainCanvas, RandomPositionX, RandomPositionY
            );
            circle.Draw();
            ListShapes.Add(circle);
        }

        private void buttonTriangle_Click(object sender, RoutedEventArgs e)
        {
            RandomPositionX = _rand.Next(100, 800);
            RandomPositionY = _rand.Next(100, 400);
            var triangle = new Triangles(
                10, 20, 40, 20, 25, 60, "MyTriangle", GlobalProperties.ColorFill, GlobalProperties.ColorStroke,
                mainCanvas, RandomPositionX, RandomPositionY
            );
            triangle.Draw();
            ListShapes.Add(triangle);
        }

        private void buttonLine_Click(object sender, RoutedEventArgs e)
        {
            RandomPositionX = _rand.Next(100, 800);
            RandomPositionY = _rand.Next(100, 400);
            var line = new Lines(
                10, 20, 30, 40, "MyLine", GlobalProperties.ColorFill, GlobalProperties.ColorStroke,
                mainCanvas, RandomPositionX, RandomPositionY
            );
            line.Draw();
            ListShapes.Add(line);
        }

        private void buttonSquare_Click(object sender, RoutedEventArgs e)
        {
            RandomPositionX = _rand.Next(100, 800);
            RandomPositionY = _rand.Next(100, 400);
            var size = _rand.Next(100, 200);
            var square = new Squares(
                size, "MySquare", GlobalProperties.ColorFill, GlobalProperties.ColorStroke,
                mainCanvas, RandomPositionX, RandomPositionY
            );            
            square.Draw();
            ListShapes.Add(square);
        }

        private void buttonTetragon_Click(object sender, RoutedEventArgs e)
        {
            RandomPositionX = _rand.Next(100, 800);
            RandomPositionY = _rand.Next(100, 400);
            var tetragon = new Tetragons(
                10, 10, 40, 10, 80, 60, 10, 60, "MyTrapezes", GlobalProperties.ColorFill, GlobalProperties.ColorStroke,
                mainCanvas, RandomPositionX, RandomPositionY
            );
            tetragon.Draw();
            ListShapes.Add(tetragon);
        }

        private void color_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var colorRect = (Rectangle)sender;            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                GlobalProperties.ColorFill = (SolidColorBrush)colorRect.Fill;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                GlobalProperties.ColorStroke = (SolidColorBrush)colorRect.Fill;
            }
        }
    }
}
