using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;
using Microsoft.Win32;

namespace PictureDraw
{
    //TODO Activator.CreateInstance(typeof (Circles), 10, 20, 30, 40);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>    
    public partial class MainWindow : Window
    {
        public List<Shapes> ListShapes = new List<Shapes>();
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
            GlobalProperties.MainCanvas = mainCanvas;
            GlobalProperties.selectedShape = null;
            GlobalProperties.isShapeSelected = false;
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
            if (GlobalProperties.isDraw)
            {
                var point = e.GetPosition(GlobalProperties.MainCanvas);
                GlobalProperties.startX = (int)point.X;
                GlobalProperties.startY = (int)point.Y;
                GlobalProperties.MainCanvas.Children.Add(new Rectangle());
            }
            else
            {
                if (!GlobalProperties.isShapeSelected)
                {
                    if (GlobalProperties.selectedShape != null)
                    {
                        GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape.selection);
                    }
                    GlobalProperties.selectedShape = null;
                }
                GlobalProperties.isShapeSelected = false;
            }                            
        }

        private void mainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (GlobalProperties.isDraw)
            {
                ListShapes.Add(GlobalProperties.drawShape);
                //GlobalProperties.drawShape.Draw();                               
            }                                               
        }

        private void mainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && GlobalProperties.isDraw)
            {
                var point = e.GetPosition(GlobalProperties.MainCanvas);
                GlobalProperties.finishX = (int)point.X;
                GlobalProperties.finishY = (int)point.Y;
                Shapes shape = GlobalProperties.currentShape.FactoryMethod("Default",
                    GlobalProperties.startX, GlobalProperties.startY,
                    GlobalProperties.finishX, GlobalProperties.finishY);                
                GlobalProperties.MainCanvas.Children.RemoveAt(GlobalProperties.MainCanvas.Children.Count - 1);
                shape.Draw();                
                GlobalProperties.drawShape = shape;                
            }
        }

        private void buttonSaveLoad_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;

            if (button.Equals(buttonSaveImage))
            {
                Dialogs.SaveFile(ListShapes);
            }
            if (button.Equals(buttonLoadImage))
            {                              
                ListShapes = Dialogs.OpenFile();                
            }                        
        }
   

        private void buttonDrawSelect_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            if (Equals(button, buttonDraw))
            {
                GlobalProperties.isDraw = true;
            }
            if (Equals(button, buttonSelect))
            {
                GlobalProperties.isDraw = false;
            }
        }
    }
}