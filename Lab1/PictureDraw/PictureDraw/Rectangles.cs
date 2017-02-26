using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace PictureDraw
{
    [Serializable]
    public class Rectangles : Shapes
    {
        public Rectangles() { }

        ///public int Width { get; set; }
        //public int Height { get; set; }

        public Rectangles(string Name,
            int startX, int startY, int finishX, int finishY): base(
                Name)
        {
            //finish not initialize 
            this.startX = Math.Min(startX, finishX);
            this.startY = Math.Min(startY, finishY);
            this.finishX = Math.Max(startX, finishX);            
            this.finishY = Math.Max(startY, finishY);            
            Width = Math.Abs(startX - finishX);
            Height = Math.Abs(startY - finishY);
        }

        public override void Draw()
        {
            Rectangle rect = new Rectangle();
            rect.StrokeThickness = 2;
            rect.Stroke = ColorStroke;
            rect.Fill = ColorFill;
            rect.Width = Width;
            rect.Height = Height;
            GlobalProperties.MainCanvas.Children.Add(rect);
            Canvas.SetLeft(rect, startX);
            Canvas.SetTop(rect, startY);
        }      
    }

    class RectangleCreator : ICreator
    {
        public Shapes FactoryMethod(string Name,
            int startX, int startY, int finishX, int finishY)
        {
            return new Rectangles(Name, startX, startY, finishX, finishY);
        }
    }
}
