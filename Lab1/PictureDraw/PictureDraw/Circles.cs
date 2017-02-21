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
            this.Width = Math.Abs(startX - finishX);
            this.Height = Math.Abs(startY - finishY);
            Radius = Width < Height ? Width / 2 : Height / 2;
        }

        public override void Draw()
        {
            Ellipse circle = new Ellipse();
            circle.Stroke = ColorStroke;
            circle.Fill = ColorFill;
            circle.Width = Radius*2;
            circle.Height = Radius*2;                       
            MainCanvas.Children.Add(circle);            
            Canvas.SetLeft(circle, startX);
            Canvas.SetTop(circle, startY);
        }
    }
}
