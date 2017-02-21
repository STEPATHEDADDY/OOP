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
        //private List<Shapes> ListShapes = new List<Shapes>();
        private int startX { get; set; }
        private int startY { get; set; }
        private int finishX { get; set; }
        private int finishY { get; set; }
        private string currentShape { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }        

        private void buttonShape_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            currentShape = button.Content.ToString();
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

        private void mainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(mainCanvas);
            startX = (int) point.X;
            startY = (int) point.Y;
        }

        private void mainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Dictionary<string, Shapes> shapeSelector = new Dictionary<string, Shapes>()
            {
                { "Circle", new Circles() },
                { "Rectangle", new Rectangles() },
                { "Square", new Squares() },
                { "Line", new Lines() },
                { "Triangle", new Triangles() },
                { "Tetragon", new Tetragons() },
            };
            var point = e.GetPosition(mainCanvas);
            finishX = (int)point.X;
            finishY = (int)point.Y;
            Shapes shape = shapeSelector[currentShape];
            shape.SetInitProperties($"My{currentShape}", mainCanvas,
                startX, startY, finishX, finishY);
            ListShapes.Add(shape);
            shape.Draw();                                                
        }
    }
}
