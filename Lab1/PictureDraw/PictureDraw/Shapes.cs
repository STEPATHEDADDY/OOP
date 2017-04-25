using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;
using YAXLib;

namespace PictureDraw
{       
//    [XmlInclude(typeof(Rectangles))]
//    [XmlInclude(typeof(Circles))]
//    [XmlInclude(typeof(Triangles))]
//    [XmlInclude(typeof(MatrixTransform))]
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]    
    public abstract class Shapes : UIElement
    {
        [YAXSerializableField]
        public string Name { get; set; }
        [YAXSerializableField]    
        public Color ColorFill { get; set; }
        [YAXSerializableField]
        public Color ColorStroke { get; set; }  
        [YAXSerializableField]
        public Point startPoint { get; set; }
        [YAXSerializableField]
        public Point finishPoint { get; set; }        
        [YAXSerializableField]
        public double Width { get; set; }
        [YAXSerializableField]
        public double Height { get; set; }
        public UIElement Selection { get; set; }
        public Dictionary<string, Rectangle> AnglesBorder { get; set; }
        public Point dragPoint { get; set; }   
        [YAXSerializableField]
        public double ThicknessBorder { get; set; }               

        public Shapes() { }

        public Shapes(string Name, Color ColorFill, Color ColorStroke, double ThicknessBorder)
        {                        
            this.ColorFill = ColorFill;            
            this.ColorStroke = ColorStroke;
            this.Name = Name;
            this.ThicknessBorder = ThicknessBorder;
            Selection = null;
        }
        
        public abstract void Draw();
        public abstract void AfterDesirialization();

    }

    public interface ICreator
    {
        Shapes FactoryMethod(string Name,
            Point startPoint, Point finisgPoint, Color colorFill, Color colorStroke, double ThicknessBorder);
    }
}
