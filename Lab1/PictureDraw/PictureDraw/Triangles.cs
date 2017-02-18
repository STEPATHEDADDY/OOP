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

        public Triangles(string Name, Canvas mainCanvas) : base(
                Name, mainCanvas)
        { 

        }

        public override void Draw()
        {
            var triangle = new Polygon();            
            triangle.StrokeThickness = 2;
            triangle.Stroke = ColorStroke;
            triangle.Fill = ColorFill;
            triangle.HorizontalAlignment = HorizontalAlignment.Left;
            triangle.VerticalAlignment = VerticalAlignment.Center;
            var pointCollection = GetPointCollection();
            triangle.Points = pointCollection;            
            mainCanvas.Children.Add(triangle);
            Canvas.SetTop(triangle, CanvasOffsetY);
            Canvas.SetLeft(triangle, CanvasOffsetX);
        }

        public override void SetCanvasOffset(int CanvasOffsetX, int CanvasOffsetY)
        {
            this.CanvasOffsetX = CanvasOffsetX;
            this.CanvasOffsetY = CanvasOffsetY;
        }

        public override void SetColors(SolidColorBrush Fill, SolidColorBrush Stroke)
        {
            ColorFill = Fill;
            ColorStroke = Stroke;
        }

        private PointCollection GetPointCollection()
        {
            var point1 = new Point(X1, Y1);
            var point2 = new Point(X2, Y2);
            var point3 = new Point(X3, Y3);            
            var pointCollection = new PointCollection();
            pointCollection.Add(point1);
            pointCollection.Add(point2);
            pointCollection.Add(point3);            
            return pointCollection;
        }

        public void SetPoints(int X1, int Y1, int X2, int Y2,
            int X3, int Y3)
        {
            this.X1 = X1;
            this.Y1 = Y1;
            this.X2 = X2;
            this.Y2 = Y2;
            this.X3 = X3;
            this.Y3 = Y3;            
        }
    }
}
