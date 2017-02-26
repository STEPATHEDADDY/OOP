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
    public class Tetragons : Shapes
    {
        private int X1 { get; set; }
        private int Y1 { get; set; }
        private int X2 { get; set; }
        private int Y2 { get; set; }
        private int X3 { get; set; }
        private int Y3 { get; set; }
        private int X4 { get; set; }
        private int Y4 { get; set; }

        public Tetragons() { }

        public Tetragons(string Name,
            int startX, int startY, int finishX, int finishY): base(
                Name)
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
            X2 = Width / 3;
            Y2 = 0;
            X3 = 2 * X2;
            Y3 = 0;
            X4 = Width;
            Y4 = Height;
        }

        public override void Draw()
        {
            var tetragon = new Polygon();
            tetragon.StrokeThickness = 2;
            tetragon.Stroke = ColorStroke;
            tetragon.Fill = ColorFill;
            var pointCollection = GetPointCollection();
            tetragon.Points = pointCollection;
            GlobalProperties.MainCanvas.Children.Add(tetragon);
            Canvas.SetLeft(tetragon, startX);
            Canvas.SetTop(tetragon, startY);
        }

        private PointCollection GetPointCollection()
        {
            var point1 = new Point(X1, Y1);
            var point2 = new Point(X2, Y2);
            var point3 = new Point(X3, Y3);
            var point4 = new Point(X4, Y4);
            var pointCollection = new PointCollection();
            pointCollection.Add(point1);
            pointCollection.Add(point2);
            pointCollection.Add(point3);
            pointCollection.Add(point4);
            return pointCollection;
        }
    }
    
    class TetragonCreator : ICreator
    {
        public  Shapes FactoryMethod(string Name,
            int startX, int startY, int finishX, int finishY)
        {
            return new Tetragons(Name, startX, startY, finishX, finishY);
        }
    }
}
