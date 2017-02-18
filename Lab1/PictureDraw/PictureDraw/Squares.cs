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

        public Squares(string Name, Canvas mainCanvas) : base(
                Name, mainCanvas)
        {
                        
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

        public override void SetColors(SolidColorBrush Fill, SolidColorBrush Stroke)
        {
            ColorFill = Fill;
            ColorStroke = Stroke;
        }

        public override void SetCanvasOffset(int CanvasOffsetX, int CanvasOffsetY)
        {
            this.CanvasOffsetX = CanvasOffsetX;
            this.CanvasOffsetY = CanvasOffsetY;
        }

        public void SetSize(int Size)
        {
            this.Size = Size;
        }
    }
}
