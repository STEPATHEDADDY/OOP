using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace PictureDraw
{    
    [Serializable]
    public class Circles : Shapes
    {
        public float Radius { get; set; }

        public Circles() { }

        public Circles(string Name,
            float startX, float startY, float finishX, float finishY): base(
                Name)
        {
            //finish not initialize 
            this.startX = Math.Min(startX, finishX);
            this.startY = Math.Min(startY, finishY);
            this.finishX = Math.Max(startX, finishX);
            this.finishY = Math.Max(startY, finishY);
            Width = Math.Abs(startX - finishX);
            Height = Math.Abs(startY - finishY);
            Radius = Width < Height ? Width / 2 : Height / 2;
        }            

        public override void Draw()
        {            
            Ellipse circle = new Ellipse();
            circle.Stroke = ColorStroke;
            circle.Fill = ColorFill;
            circle.Width = Radius*2;
            circle.Height = Radius*2;
            GlobalProperties.MainCanvas.Children.Add(circle);            
            Canvas.SetLeft(circle, startX);
            Canvas.SetTop(circle, startY);
        }
    }
    
    class CircleCreator : ICreator
    {
        public Shapes FactoryMethod(string Name,
            float startX, float startY, float finishX, float finishY)
        {
            return new Circles(Name, startX, startY, finishX, finishY);
        }
    }
}
