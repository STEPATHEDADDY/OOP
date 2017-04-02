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
using System.Xml.Serialization;

namespace PictureDraw
{       
    [XmlInclude(typeof(Rectangles))]
//    [XmlInclude(typeof(Circles))]
//    [XmlInclude(typeof(Triangles))]
    [XmlInclude(typeof(MatrixTransform))]
    public abstract class Shapes : UIElement
    {
        public string Name { get; set; }
        public Color ColorFill { get; set; }        
        public Color ColorStroke { get; set; }  
        public float startX { get; set; } 
        public float startY { get; set; } 
        public float finishX { get; set; } 
        public float finishY { get; set; }         
        public float Width { get; set; }
        public float Height { get; set; }
        public UIElement selection { get; set; }

        public Shapes() { }

        public Shapes(string Name, Color ColorFill, Color ColorStroke)
        {                        
            this.ColorFill = ColorFill;            
            this.ColorStroke = ColorStroke;
            this.Name = Name;            
        }
        
        public abstract void Draw();

    }

    public interface ICreator
    {
        Shapes FactoryMethod(string Name,
            float startX, float startY, float finishX, float finishY, Color colorFill, Color colorStroke);
    }
}
