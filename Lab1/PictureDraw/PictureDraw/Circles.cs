using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PictureDraw
{
    class Circles : Shapes
    {
        private int Radius { get; set; }

        public Circles(int Radius, string Name, Canvas mainCanvas) : base(
                Name, mainCanvas)
        {
            this.Radius = Radius;
        }

        public override void Draw()
        {
            Ellipse circle = new Ellipse();
            circle.Width = Radius*2;
            circle.Height = Radius*2;                       
            circle.Stroke = ColorStroke;
            circle.Fill = ColorFill;
            mainCanvas.Children.Add(circle);            
            Canvas.SetTop(circle, CanvasOffsetY);
            Canvas.SetLeft(circle, CanvasOffsetX);
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
    }
}
