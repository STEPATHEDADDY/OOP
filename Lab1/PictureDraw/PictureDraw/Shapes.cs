using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace PictureDraw
{       
    [XmlInclude(typeof(Rectangles))]
    [XmlInclude(typeof(Circles))]
    [XmlInclude(typeof(MatrixTransform))]
    public abstract class Shapes
    {
        public string Name { get; set; }
        public SolidColorBrush ColorFill { get; set; }        
        public SolidColorBrush ColorStroke { get; set; }  
        public int startX { get; set; } 
        public int startY { get; set; } 
        public int finishX { get; set; } 
        public int finishY { get; set; } 
        //public Canvas MainCanvas { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Shapes() { }

        public Shapes(string Name)
        {
            ColorFill = GlobalProperties.ColorFill;
            ColorStroke = GlobalProperties.ColorStroke;
            this.Name = Name;
        }
        
        public abstract void Draw();           
    }
    
    public interface ICreator
    {
        Shapes FactoryMethod(string Name,
            int startX, int startY, int finishX, int finishY);
    }
}
