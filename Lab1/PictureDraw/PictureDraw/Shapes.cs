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
        protected SolidColorBrush ColorFill { get; set; }        
        protected SolidColorBrush ColorStroke { get; set; }  
        public int startX { get; set; } 
        public int startY { get; set; } 
        public int finishX { get; set; } 
        public int finishY { get; set; } 
        public Canvas MainCanvas { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Shapes(string Name, Canvas MainCanvas)
        {
            ColorFill = GlobalProperties.ColorFill;
            ColorStroke = GlobalProperties.ColorStroke;
            this.Name = Name;
            this.MainCanvas = MainCanvas;
        }
        
        public abstract void Draw();           
    }

    interface ICreator
    {
        Shapes FactoryMethod(string Name, Canvas MainCanvas,
            int startX, int startY, int finishX, int finishY);
    }
}
