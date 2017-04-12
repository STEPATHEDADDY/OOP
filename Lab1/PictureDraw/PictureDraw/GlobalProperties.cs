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
        public static Color ColorFill { get; set; }
        public static Color ColorStroke { get; set; }
        public static Canvas MainCanvas;
        public static float Thickness { get; set; }
        public static float startX { get; set; }
        public static float startY { get; set; }
        public static float finishX { get; set; }
        public static float finishY { get; set; }
        public static ICreator currentShape { get; set; }
        public static bool isDraw { get; set; }
        public static Shapes drawShape { get; set; }
        public static Shapes selectedShape { get; set; }
        public static Rectangle selectedAngle { get; set; }
        public static bool isShapeSelected { get; set; }
        public static bool isAngleSelected { get; set; }
        public static Point selectedShapePoint { get; set; }
        public static Point selectedAnglePoint { get; set; }
        public static Point currentMousePoint { get; set; }
        public static Rectangle RectCanvas { get; set; }
        public static float frameSize { get; set; }
        public static DockPanel PropertiesPanel { get; set; }
        public static ColorPicker FillSelected { get; set; }
        public static ColorPicker BorderSelected { get; set; }
        public static List<Rectangle> AnglesBorder { get; set; }
    }

}