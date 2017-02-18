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
        private int Size { get; set; }

        public Squares(int Size, string Name, 
            SolidColorBrush ColorFill, SolidColorBrush ColorStroke, 
            Canvas mainCanvas, int CanvasOffsetX, int CanvasOffsetY) : base(
                Name, ColorFill, ColorStroke, mainCanvas,
                CanvasOffsetX, CanvasOffsetY)
        {
            this.Size = Size;
        }

        public override void Draw()
        {
            var square = new Rectangle();
            square.Width = Size;
            square.Height = Size;
            square.Stroke = ColorStroke;
            square.Fill = ColorFill;
            mainCanvas.Children.Add(square);
            Canvas.SetTop(square, CanvasOffsetY);
            Canvas.SetLeft(square, CanvasOffsetX);
        }
    }
}
