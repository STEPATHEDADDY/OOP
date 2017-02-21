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
            X1 = 0;
            Y1 = Height;
            X2 = Width/2;
            Y2 = 0;
            X3 = Width;
            Y3 = Height;
        }

        public override void Draw()
        {
            var triangle = new Polygon();            
            triangle.StrokeThickness = 2;
            triangle.Stroke = ColorStroke;
            triangle.Fill = ColorFill;
            var pointCollection = GetPointCollection();
            triangle.Points = pointCollection;            
            MainCanvas.Children.Add(triangle);
            Canvas.SetLeft(triangle, startX);
            Canvas.SetTop(triangle, startY);
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
    }
}
