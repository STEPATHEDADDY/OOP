using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PictureDraw
{
    class Rectangles : Shapes
    {
        private int Width { get; set; }
        private int Height { get; set; }

        public Rectangles(string Name, Canvas MainCanvas,
            int startX, int startY, int finishX, int finishY): base(
                Name, MainCanvas)
        {
            //finish not initialize 
            this.startX = Math.Min(startX, finishX);
            this.startY = Math.Min(startY, finishY);
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
            MainCanvas.Children.Add(rect);
            Canvas.SetLeft(rect, startX);
            Canvas.SetTop(rect, startY);
        }      
    }

    class RectangleCreator : ICreator
    {
        public Shapes FactoryMethod(string Name, Canvas MainCanvas,
            int startX, int startY, int finishX, int finishY)
        {
            return new Rectangles(Name, MainCanvas, startX, startY, finishX, finishY);
        }
    }
}
