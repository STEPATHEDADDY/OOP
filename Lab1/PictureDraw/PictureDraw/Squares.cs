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

        public override void SetInitProperties(
            string Name, Canvas MainCanvas,
            int startX, int startY, int finishX, int finishY)
        {
            //finish not initialize
            ColorFill = GlobalProperties.ColorFill;
            ColorStroke = GlobalProperties.ColorStroke;
            this.startX = startX < finishX ? startX : finishX;
            this.startY = startY < finishY ? startY : finishY;
            this.Name = Name;
            this.MainCanvas = MainCanvas;
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
}
