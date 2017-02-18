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
    class Tetragons : Shapes
    {
        private int X1 { get; set; }
        private int Y1 { get; set; }
        private int X2 { get; set; }
        private int Y2 { get; set; }
        private int X3 { get; set; }
        private int Y3 { get; set; }
        private int X4 { get; set; }
        private int Y4 { get; set; }

        public Tetragons(int X1, int Y1, int X2, int Y2, int X3, int Y3, int X4, int Y4,
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
            this.X3 = X3;
            this.Y3 = Y3;
            this.X4 = X4;
            this.Y4 = Y4;
        }

        public override void Draw()
        {
            var tetragon = new Polygon();
            tetragon.StrokeThickness = 2;
            tetragon.HorizontalAlignment = HorizontalAlignment.Left;
            tetragon.VerticalAlignment = VerticalAlignment.Center;
            var point1 = new Point(X1, Y1);
            var point2 = new Point(X2, Y2);
            var point3 = new Point(X3, Y3);
            var point4 = new Point(X4, Y4);
            var pointCollection = new PointCollection();
            pointCollection.Add(point1);
            pointCollection.Add(point2);
            pointCollection.Add(point3);
            pointCollection.Add(point4);
            tetragon.Points = pointCollection;
            tetragon.Stroke = ColorStroke;
            tetragon.Fill = ColorFill;
            mainCanvas.Children.Add(tetragon);
            Canvas.SetTop(tetragon, CanvasOffsetY);
            Canvas.SetLeft(tetragon, CanvasOffsetX);
        }
    }
}
