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
    class Lines : Shapes
    {
        public override void SetInitProperties(
            string Name, Canvas MainCanvas,
            int startX, int startY, int finishX, int finishY)
        {
            ColorFill = GlobalProperties.ColorFill;
            ColorStroke = GlobalProperties.ColorStroke;
            this.startX = startX;
            this.startY = startY;
            this.finishX = finishX;
            this.finishY = finishY;
            this.Name = Name;
            this.MainCanvas = MainCanvas;
        }

        public override void Draw()
        {
            Line line = new Line();
            line.X1 = startX;
            line.Y1 = startY;
            line.X2 = finishX;
            line.Y2 = finishY;
            line.Stroke = ColorStroke;
            line.Fill = ColorFill;
            MainCanvas.Children.Add(line);
        }
    }
}
