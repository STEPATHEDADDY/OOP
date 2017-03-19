using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Controls;

namespace PictureDraw
{
    //[Serializable]
    public static class GlobalProperties
    {
        public static SolidColorBrush ColorFill { get; set; }
        public static SolidColorBrush ColorStroke { get; set; }
        public static Canvas MainCanvas;
        public static int Thickness { get; set; }
        public static int startX { get; set; }
        public static int startY { get; set; }
        public static int finishX { get; set; }
        public static int finishY { get; set; }
        public static ICreator currentShape { get; set; }
        public static bool isDraw { get; set; }
        public static Shapes drawShape { get; set; }
        public static Shapes selectedShape { get; set; }
        public static bool isShapeSelected { get; set; }
    }

}
