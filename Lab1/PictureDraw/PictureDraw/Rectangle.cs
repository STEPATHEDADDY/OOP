using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PictureDraw
{
    class Rectangle : Shape
    {
        private string Name;

        public Rectangle(string Name, string Color,
            int x, int y, Grid mainGrid) : base (
                Color, x, y, mainGrid)
        {
            this.Name = Name;
        }

        public override void Draw(int x, int y)
        {
            Coordinates.Add("X", x);
            Coordinates.Add("Y", y);
            Line topLine = new Line(),
                bottomLine = new Line(),
                leftLine = new Line(),
                rightLine = new Line();

            topLine.Stroke = System.Windows.Media.Brushes.Black;
            topLine.StrokeThickness = 2;
            rightLine.Stroke = System.Windows.Media.Brushes.Black;
            bottomLine.Stroke = System.Windows.Media.Brushes.Black;
            leftLine.Stroke = System.Windows.Media.Brushes.Black;

            topLine.X1 = x;
            topLine.Y1 = y;
            topLine.X2 = x + Size["Width"];
            topLine.Y2 = y;

            rightLine.X1 = topLine.X2;
            rightLine.Y1 = topLine.Y2;
            rightLine.X2 = rightLine.X1;
            rightLine.Y2 = rightLine.Y1 + Size["Height"];

            bottomLine.X1 = rightLine.X2;
            bottomLine.Y1 = rightLine.Y2;
            bottomLine.X2 = bottomLine.X1 - Size["Width"];
            bottomLine.Y2 = bottomLine.Y1;

            leftLine.X1 = bottomLine.X2;
            leftLine.Y1 = bottomLine.Y2;
            leftLine.X2 = leftLine.X1;
            leftLine.Y2 = leftLine.Y1 - Size["Height"];

            //topLine.SetValue(Canvas.LeftProperty, x * 1.0);
            //topLine.SetValue(Canvas.TopProperty, y * 1.0);            
            mainGrid.Children.Add(topLine);        
            mainGrid.Children.Add(rightLine);
            mainGrid.Children.Add(bottomLine);
            mainGrid.Children.Add(leftLine);
        }
    }
}
