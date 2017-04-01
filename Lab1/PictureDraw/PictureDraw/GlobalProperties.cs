using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PictureDraw
{
    //[Serializable]
    public static class GlobalProperties
    {
        public static SolidColorBrush ColorFill { get; set; }
        public static SolidColorBrush ColorStroke { get; set; }
        public static Canvas MainCanvas;
        public static int Thickness { get; set; }
        public static float startX { get; set; }
        public static float startY { get; set; }
        public static float finishX { get; set; }
        public static float finishY { get; set; }
        public static ICreator currentShape { get; set; }
        public static bool isDraw { get; set; }
        public static Shapes drawShape { get; set; }
        public static Shapes selectedShape { get; set; }
        public static bool isShapeSelected { get; set; }
        public static Point selectedShapePoint { get; set; }
        public static Point currentMousePoint { get; set; }
        public static Rectangle RectCanvas { get; set; }
        public static float frameSize { get; set; }
    }

}
