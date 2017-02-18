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
    class Lines : Shapes
    {
        private int X1 { get; set; }
        private int Y1 { get; set; }
        private int X2 { get; set; }
        private int Y2 { get; set; }

        public Lines(int X1, int Y1, int X2, int Y2,
            string Name, SolidColorBrush ColorFill,
            SolidColorBrush ColorStroke, Canvas mainCanvas,
            int CanvasOffsetX, int CanvasOffsetY) : base(
                Name, ColorFill, ColorStroke, mainCanvas,
                CanvasOffsetX, CanvasOffsetY)
        {
            this.X1 = X1;
            this.Y1 = Y1;
            this.X2 = X2;
            this.Y2 = Y2;
        }

        public override void Draw()
        {
            Line line = new Line();
            line.X1 = X1;
            line.Y1 = Y1;
            line.X2 = X2;
            line.Y2 = Y2;
            line.Stroke = ColorStroke;
            mainCanvas.Children.Add(line);
            Canvas.SetTop(line, CanvasOffsetY);
            Canvas.SetLeft(line, CanvasOffsetX);
        }
    }
}
