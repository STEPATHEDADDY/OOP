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
    class Squares : Shapes
    {
        public Squares(string Name, Canvas MainCanvas,
            int startX, int startY, int finishX, int finishY): base(
                Name, MainCanvas)
        {
            //finish not initialize 
            this.startX = Math.Min(startX, finishX);
            this.startY = Math.Min(startY, finishY);
            Width = Math.Abs(startX - finishX);
            Height = Math.Abs(startY - finishY);
            Width = Height = Width < Height ? Width : Height;
        }

        public override void Draw()
        {
            var square = new Rectangle();
            square.Stroke = ColorStroke;
            square.Fill = ColorFill;
            square.Width = square.Height = Width;            
            MainCanvas.Children.Add(square);
            Canvas.SetLeft(square, startX);
            Canvas.SetTop(square, startY);
        }
    }

    class SquareCreator : ICreator
    {
        public Shapes FactoryMethod(string Name, Canvas MainCanvas,
            int startX, int startY, int finishX, int finishY)
        {
            return new Squares(Name, MainCanvas, startX, startY, finishX, finishY);
        }
    }
}
