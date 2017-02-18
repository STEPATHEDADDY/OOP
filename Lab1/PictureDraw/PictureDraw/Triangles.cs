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
    class Triangles : Shapes
    {
        private int X1 { get; set; }
        private int Y1 { get; set; }
        private int X2 { get; set; }
        private int Y2 { get; set; }
        private int X3 { get; set; }
        private int Y3 { get; set; }

        public Triangles(int X1, int Y1, int X2, int Y2, int X3, int Y3,
            string Name, SolidColorBrush ColorFill, SolidColorBrush ColorStroke,
            Canvas mainCanvas, int CanvasOffsetX, int CanvasOffsetY) : base(
                Name, ColorFill, ColorStroke, mainCanvas,
                CanvasOffsetX, CanvasOffsetY)
        {
            this.X1 = X1;
            this.Y1 = Y1;
            this.X2 = X2;
            this.Y2 = Y2;
            this.X3 = X3;
            this.Y3 = Y3;
        }

        public override void Draw()
        {
            var triangle = new Polygon();
            triangle.Stroke = System.Windows.Media.Brushes.Black;
            triangle.Fill = System.Windows.Media.Brushes.LightSeaGreen;
            triangle.StrokeThickness = 2;
            triangle.HorizontalAlignment = HorizontalAlignment.Left;
            triangle.VerticalAlignment = VerticalAlignment.Center;
            var point1 = new Point(X1, Y1);
            var point2 = new Point(X2, Y2);
            var point3 = new Point(X3, Y3);
            var pointCollection = new PointCollection();
            pointCollection.Add(point1);
            pointCollection.Add(point2);
            pointCollection.Add(point3);
            triangle.Points = pointCollection;
            triangle.Stroke = ColorStroke;
            triangle.Fill = ColorFill;
            mainCanvas.Children.Add(triangle);
            Canvas.SetTop(triangle, CanvasOffsetY);
            Canvas.SetLeft(triangle, CanvasOffsetX);
        }
    }
}
