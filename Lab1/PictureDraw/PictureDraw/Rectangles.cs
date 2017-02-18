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

        public Rectangles(string Name, Canvas mainCanvas) : base(
                Name, mainCanvas)
        {

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

        public void SetSize(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }        
    }
}
