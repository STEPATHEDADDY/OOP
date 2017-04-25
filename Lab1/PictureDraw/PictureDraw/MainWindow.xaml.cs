using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using Xceed.Wpf.Toolkit;

namespace PictureDraw
{
    //TODO Activator.CreateInstance(typeof (Circles), 10, 20, 30, 40);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>    
    public partial class MainWindow : Window
    {
//        public List<Shapes> ListShapes = new List<Shapes>();
//        private Dictionary<string, ICreator> creators= new Dictionary<string, ICreator>
//            {
//                { "Circles", new CircleCreator() },
////                { "Line", new LineCreator() },
//                { "Rectangles", new RectangleCreator() },
////                { "Square", new SquareCreator() },
////                { "Tetragon", new TetragonCreator() },
////                { "Triangle", new TriangleCreator() },
//            };

        public MainWindow()
        {
            InitializeComponent();
            GlobalProperties.ShapesList = new ListShapes();
            GlobalProperties.MainCanvas = mainCanvas;
            GlobalProperties.RectCanvas = rectMainCanvas;
            GlobalProperties.selectedShape = null;            
            GlobalProperties.frameSize = 12;
            GlobalProperties.PropertiesPanel = dpProperties;
            GlobalProperties.PropertiesPanel.Visibility = Visibility.Hidden;
            GlobalProperties.FillSelected = ClrPckerFillSelected;
            GlobalProperties.BorderSelected = ClrPckerBorderSelected;
            GlobalProperties.MinShapeSize = 30;
            ClrPckerFill.SelectedColor = Color.FromArgb(255, 100, 100, 100);
            ClrPckerBorder.SelectedColor = Color.FromArgb(255, 255, 100, 100);
        }

        private void buttonShape_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            GlobalProperties.currentShape = CommonMethods.creators[button.Content.ToString()];
        }

        private void mainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (GlobalProperties.DrawModeOn)
            {
                var point = e.GetPosition(GlobalProperties.MainCanvas);
                GlobalProperties.startPoint = new Point(point.X, point.Y);                
                GlobalProperties.MainCanvas.Children.Add(new Rectangle());
            }                       
    }

        private void rectMainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                if (GlobalProperties.selectedShape != null)
                {
                    GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape.Selection);
                    var selectableShape = GlobalProperties.selectedShape as ISelectable;
                    if (selectableShape != null)
                    {
                        Rectangles.RemoveSelection(GlobalProperties.selectedShape);
                    }
                    GlobalProperties.selectedShape = null;
                    GlobalProperties.PropertiesPanel.Visibility = Visibility.Hidden;
                }
            }
        }

        private void mainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (GlobalProperties.DrawModeOn)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    var point = e.GetPosition(GlobalProperties.MainCanvas);
                    GlobalProperties.finishPoint = new Point(point.X - 6, point.Y - 6);                    
                    Shapes shape = GlobalProperties.currentShape.FactoryMethod("Default",
                        GlobalProperties.startPoint, GlobalProperties.finishPoint,
                        ClrPckerFill.SelectedColor.Value, ClrPckerBorder.SelectedColor.Value,
                        sliderThickness.Value);
                    GlobalProperties.MainCanvas.Children.RemoveAt(GlobalProperties.MainCanvas.Children.Count - 1);
                    shape.Draw();                    
                    GlobalProperties.drawShape = shape;
                }
            }            
        }

        private void mainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (GlobalProperties.DrawModeOn)
            {
                GlobalProperties.ShapesList.AddShape(GlobalProperties.drawShape);
//                ListShapes.Add();       
                Debug.WriteLine(GlobalProperties.ShapesList.AllShapes.Count);
                ClrPckerFillSelected.SelectedColor = GlobalProperties.drawShape.ColorFill;                
                ClrPckerBorderSelected.SelectedColor = GlobalProperties.drawShape.ColorStroke;                                
            }
        }

        //TODO : REWRITE 
        private void buttonSaveLoad_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (button.Equals(buttonSaveImage))
            {
                Dialogs.SaveFile(GlobalProperties.ShapesList);
            }
            if (button.Equals(buttonLoadImage))
            {
                GlobalProperties.ShapesList = Dialogs.OpenFile();
            }
        }

        private void buttonDrawSelect_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (Equals(button, buttonDraw))
            {
                GlobalProperties.DrawModeOn = true;
            }
            if (Equals(button, buttonSelect))
            {
                GlobalProperties.DrawModeOn = false;
            }
        }

        private void sliderThickness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GlobalProperties.Thickness = sliderThickness.Value;
        }

        private void ClrPckerSelected_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (GlobalProperties.IsColorChanged)
            {
                if (GlobalProperties.selectedShape != null)
                {
                    var type = GlobalProperties.selectedShape.GetType().Name;
                    GlobalProperties.currentShape = CommonMethods.creators[type];
                    GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape);
                    GlobalProperties.ShapesList.AllShapes.Remove(GlobalProperties.selectedShape);
                    Shapes shape = GlobalProperties.currentShape.FactoryMethod("Default",
                        GlobalProperties.selectedShape.startPoint, GlobalProperties.selectedShape.finishPoint,
                        ClrPckerFillSelected.SelectedColor.Value, ClrPckerBorderSelected.SelectedColor.Value,
                        sliderThickness.Value);
                    shape.Draw();
                    //TODO : INITIALIZE FIELDS WHEN THE PROGRAM IS BEING STARTED
                    shape.dragPoint = new Point(Double.NaN, Double.NaN);  
                    shape.Selection = GlobalProperties.selectedShape.Selection;
                    shape.AnglesBorder = GlobalProperties.selectedShape.AnglesBorder;
                    GlobalProperties.drawShape = shape;                                                                             
                    GlobalProperties.selectedShape = shape;
                    GlobalProperties.ShapesList.AllShapes.Add(shape);
                }                
            }
        }

        private void ClrPckerFillSelected_Opened(object sender, RoutedEventArgs e)
        {
            GlobalProperties.IsColorChanged = true;
        }

        private void ClrPckerFillSelected_Closed(object sender, RoutedEventArgs e)
        {
            GlobalProperties.IsColorChanged = false;
        }
    }
}