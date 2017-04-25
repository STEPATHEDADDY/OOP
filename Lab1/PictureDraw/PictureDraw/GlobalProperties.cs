using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace PictureDraw
{
    //[Serializable]
    public static class GlobalProperties
    {
        public static Canvas MainCanvas;
        public static double Thickness { get; set; }
        public static Point startPoint { get; set; }        
        public static Point finishPoint { get; set; }
        public static Shapes selectedShape { get; set; }
        public static bool IsColorChanged { get; set; }
        public static ICreator currentShape { get; set; }
        public static bool DrawModeOn { get; set; }
        public static Shapes drawShape { get; set; }
        public static double MinShapeSize { get; set; }        
        public static Rectangle ResizeCanvas { get; set; }
        public static Canvas SecondaryCanvas { get; set; }
        public static Rectangle selectedAngle { get; set; }        
        public static Point selectedShapePoint { get; set; }
        public static Point selectedAnglePoint { get; set; }
        public static Rectangle RectCanvas { get; set; }
        public static double frameSize { get; set; }
        public static DockPanel PropertiesPanel { get; set; }
        public static ColorPicker FillSelected { get; set; }
        public static ColorPicker BorderSelected { get; set; }        
    }

}