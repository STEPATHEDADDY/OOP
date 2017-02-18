using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace PictureDraw
{
    abstract class Shapes
    {
        public string Name { get; set; }
        public SolidColorBrush ColorFill { get; set; }        
        public SolidColorBrush ColorStroke { get; set; }  
        public int CanvasOffsetX { get; set; } 
        public int CanvasOffsetY { get; set; } 
        public Canvas mainCanvas { get; set; }

        public Shapes(string Name, SolidColorBrush ColorFill, SolidColorBrush ColorStroke, Canvas mainCanvas,
            int CanvasOffsetX, int CanvasOffsetY)
        {

            this.Name = Name;
            this.ColorFill = ColorFill;
            this.ColorStroke = ColorStroke;
            this.mainCanvas = mainCanvas;
            this.CanvasOffsetX = CanvasOffsetX;
            this.CanvasOffsetY = CanvasOffsetY;

        }

        public abstract void Draw();
    }
}
