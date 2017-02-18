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
        protected string Name { get; set; }
        protected SolidColorBrush ColorFill { get; set; }        
        protected SolidColorBrush ColorStroke { get; set; }  
        protected int CanvasOffsetX { get; set; } 
        protected int CanvasOffsetY { get; set; } 
        protected Canvas mainCanvas { get; set; }

        public Shapes(string Name, Canvas mainCanvas)
        {
            this.Name = Name;            
            this.mainCanvas = mainCanvas;            
        }

        public abstract void Draw();
        public abstract void SetColors(SolidColorBrush Fill, SolidColorBrush Stroke);
        public abstract void SetCanvasOffset(int CanvasOffsetX, int CanvasOffsetY);              
    }
}
