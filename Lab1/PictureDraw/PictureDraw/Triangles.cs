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
    [Serializable]
    public class Triangles : Shapes
    {
        private float X1 { get; set; }
        private float Y1 { get; set; }
        private float X2 { get; set; }
        private float Y2 { get; set; }
        private float X3 { get; set; }
        private float Y3 { get; set; }

        public Triangles() { }
    
        public Triangles(string Name,
            float startX, float startY, float finishX, float finishY): base(Name)
        {
            //finish not initialize 
            this.startX = Math.Min(startX, finishX);
            this.startY = Math.Min(startY, finishY);
            this.finishX = Math.Max(startX, finishX);
            this.finishY = Math.Max(startY, finishY);
            Width = Math.Abs(startX - finishX);
            Height = Math.Abs(startY - finishY);
            X1 = 0;
            Y1 = Height;
            X2 = Width / 2;
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
            GlobalProperties.MainCanvas.Children.Add(triangle);
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
    
    class TriangleCreator : ICreator
    {
        public Shapes FactoryMethod(string Name,
            float startX, float startY, float finishX, float finishY)
        {
            return new Triangles(Name, startX, startY, finishX, finishY);
        }
    }
}
