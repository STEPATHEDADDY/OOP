using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PictureDraw
{
    static class GlobalProperties
    {
        public static SolidColorBrush ColorFill { get; set; }
        public static SolidColorBrush ColorStroke { get; set; }
        public static int Thickness { get; set; }
        public static int startX { get; set; }
        public static int startY { get; set; }
        public static int finishX { get; set; }
        public static int finishY { get; set; }
        public static ICreator currentShape { get; set; }
    }
}
