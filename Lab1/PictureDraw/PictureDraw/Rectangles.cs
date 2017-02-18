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

        public Rectangles(int Width, int Height, string Name, 
            SolidColorBrush ColorFill, SolidColorBrush ColorStroke, Canvas mainCanvas, 
            int CanvasOffsetX, int CanvasOffsetY) : base(
                Name, ColorFill, ColorStroke, mainCanvas, 
                CanvasOffsetX, CanvasOffsetY)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public override void Draw()
        {
            Rectangle rect = new Rectangle();
            rect.Width = Width;
            rect.Height = Height;            
            rect.Stroke = ColorStroke;
            rect.Fill = ColorFill;
            mainCanvas.Children.Add(rect);
            Canvas.SetTop(rect, CanvasOffsetY);
            Canvas.SetLeft(rect, CanvasOffsetX);
        }
    }
}
