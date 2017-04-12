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
        public List<Shapes> ListShapes = new List<Shapes>();
        private Dictionary<string, ICreator> creators = new Dictionary<string, ICreator>
            {
//                { "Circle", new CircleCreator() },
//                { "Line", new LineCreator() },
                { "Rectangle", new RectangleCreator() },
//                { "Square", new SquareCreator() },
//                { "Tetragon", new TetragonCreator() },
//                { "Triangle", new TriangleCreator() },
            };

        public MainWindow()
        {
            InitializeComponent();
            GlobalProperties.MainCanvas = mainCanvas;
            GlobalProperties.RectCanvas = rectMainCanvas;
            GlobalProperties.selectedShape = null;
            GlobalProperties.isShapeSelected = false;
            GlobalProperties.frameSize = 12;
            GlobalProperties.PropertiesPanel = dpProperties;
            GlobalProperties.PropertiesPanel.Visibility = Visibility.Hidden;
            GlobalProperties.FillSelected = ClrPckerFillSelected;
            GlobalProperties.BorderSelected = ClrPckerBorderSelected;
            ClrPckerFill.SelectedColor = GlobalProperties.ColorFill = Color.FromArgb(255, 100, 100, 100);
            ClrPckerBorder.SelectedColor = GlobalProperties.ColorStroke = Color.FromArgb(255, 255, 100, 100);
        }

        private void buttonShape_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            GlobalProperties.currentShape = creators[button.Content.ToString()];
        }

        private void ClrPcker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var cp = (ColorPicker)sender;
            ColorPicker cpFill = ClrPckerFill;
            ColorPicker cpBorder = ClrPckerBorder;
            if (Equals(cp, cpFill))
            {
                GlobalProperties.ColorFill = cp.SelectedColor.Value;
            }
            if (Equals(cp, cpBorder))
            {
                GlobalProperties.ColorStroke = cp.SelectedColor.Value;
            }
        }

        private void mainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (GlobalProperties.isDraw)
            {
                var point = e.GetPosition(GlobalProperties.MainCanvas);
                GlobalProperties.startX = (float)point.X;
                GlobalProperties.startY = (float)point.Y;
                GlobalProperties.MainCanvas.Children.Add(new Rectangle());
            }
            else
            {
                if (!GlobalProperties.isAngleSelected)
                {
                    if (!GlobalProperties.isShapeSelected)
                    {
                        if (GlobalProperties.selectedShape != null)
                        {
                            GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape.selection);
                            foreach (var angle in GlobalProperties.AnglesBorder)
                            {
                                GlobalProperties.MainCanvas.Children.Remove(angle);
                            }
                            GlobalProperties.PropertiesPanel.Visibility = Visibility.Hidden;
                        }
                        GlobalProperties.selectedShape = null;
                    }
                    GlobalProperties.isShapeSelected = false;
                }
            }
        }

        private void mainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && GlobalProperties.isDraw)
            {
                var point = e.GetPosition(GlobalProperties.MainCanvas);
                GlobalProperties.finishX = (float)point.X - 1;
                GlobalProperties.finishY = (float)point.Y - 1;
                Shapes shape = GlobalProperties.currentShape.FactoryMethod("Default",
                    GlobalProperties.startX, GlobalProperties.startY,
                    GlobalProperties.finishX, GlobalProperties.finishY,
                    GlobalProperties.ColorFill, GlobalProperties.ColorStroke);
                GlobalProperties.MainCanvas.Children.RemoveAt(GlobalProperties.MainCanvas.Children.Count - 1);
                shape.Draw();
                GlobalProperties.drawShape = shape;
            }
        }

        private void mainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (GlobalProperties.isDraw)
            {
                ListShapes.Add(GlobalProperties.drawShape);
                Debug.WriteLine($"{ListShapes.Count}");
            }
        }

        private void buttonSaveLoad_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

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
            var button = (Button)sender;
            if (Equals(button, buttonDraw))
            {
                GlobalProperties.isDraw = true;
            }
            if (Equals(button, buttonSelect))
            {
                GlobalProperties.isDraw = false;
            }
        }

        private void sliderThickness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GlobalProperties.Thickness = (float)sliderThickness.Value;
        }

        private void ClrPckerSelected_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!GlobalProperties.isShapeSelected)
            {
                GlobalProperties.selectedShape.ColorFill = ClrPckerFillSelected.SelectedColor.Value;
                GlobalProperties.selectedShape.ColorStroke = ClrPckerBorderSelected.SelectedColor.Value;
                GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape);
                ListShapes.Remove(GlobalProperties.selectedShape);
                Shapes shape = GlobalProperties.currentShape.FactoryMethod("Default",
                    GlobalProperties.selectedShape.startX, GlobalProperties.selectedShape.startY,
                    GlobalProperties.selectedShape.finishX, GlobalProperties.selectedShape.finishY,
                    GlobalProperties.selectedShape.ColorFill, GlobalProperties.selectedShape.ColorStroke);
                shape.Draw();
                ListShapes.Add(GlobalProperties.drawShape);
            }
        }
    }
}