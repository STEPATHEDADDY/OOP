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
    //TODO Activator.CreateInstance(typeof (Circles), 10, 20, 30, 40);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArrayList ListShapes = new ArrayList();
        private Dictionary<string, ICreator> creators = new Dictionary<string, ICreator>
            {
                { "Circle", new CircleCreator() },
                { "Line", new LineCreator() },
                { "Rectangle", new RectangleCreator() },
                { "Square", new SquareCreator() },
                { "Tetragon", new TetragonCreator() },
                { "Triangle", new TriangleCreator() },
            };
        //private List<Shapes> ListShapes = new List<Shapes>();

        public MainWindow()
        {
            InitializeComponent();
        }        

        private void buttonShape_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            GlobalProperties.currentShape = creators[button.Content.ToString()];
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
            GlobalProperties.startX = (int) point.X;
            GlobalProperties.startY = (int) point.Y;
        }

        private void mainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(mainCanvas);
            GlobalProperties.finishX = (int)point.X;
            GlobalProperties.finishY = (int)point.Y;
            Shapes shape = GlobalProperties.currentShape.FactoryMethod("Default", mainCanvas,
                GlobalProperties.startX, GlobalProperties.startY,
                GlobalProperties.finishX, GlobalProperties.finishY);            
            ListShapes.Add(shape);
            shape.Draw();                                                
        }
    }
}
