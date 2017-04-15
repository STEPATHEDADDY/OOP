using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace PictureDraw
{       
//    [XmlInclude(typeof(Rectangles))]
//    [XmlInclude(typeof(Circles))]
//    [XmlInclude(typeof(Triangles))]
//    [XmlInclude(typeof(MatrixTransform))]
    public abstract class Shapes : UIElement
    {
        public string Name { get; set; }
        public Color ColorFill { get; set; }        
        public Color ColorStroke { get; set; }  
        public Point startPoint { get; set; }
        public Point finishPoint { get; set; }        
        public double Width { get; set; }
        public double Height { get; set; }
        public UIElement Selection { get; set; }
        public Dictionary<string, Rectangle> AnglesBorder { get; set; }
        public Point dragPoint { get; set; }                  

        public Shapes() { }

        public Shapes(string Name, Color ColorFill, Color ColorStroke)
        {                        
            this.ColorFill = ColorFill;            
            this.ColorStroke = ColorStroke;
            this.Name = Name;
            Selection = null;
        }
        
        public abstract void Draw();

    }

    public interface ICreator
    {
        Shapes FactoryMethod(string Name,
            Point startPoint, Point finisgPoint, Color colorFill, Color colorStroke);
    }
}
