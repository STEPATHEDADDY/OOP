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
    [Serializable]
    public class Lines : Shapes
    {
        public Lines() { }

        public Lines(string Name,
            float startX, float startY, float finishX, float finishY): base(
                Name)
        {
            this.startX = startX;
            this.startY = startY;
            this.finishX = finishX;
            this.finishY = finishY;
            Width = Math.Abs(startX - finishX);
            Height = Math.Abs(startY - finishY);            
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
            GlobalProperties.MainCanvas.Children.Add(line);
        }
    }
    
    class LineCreator : ICreator
    {
        public Shapes FactoryMethod(string Name,
            float startX, float startY, float finishX, float finishY)
        {
            return new Lines(Name, startX, startY, finishX, finishY);
        }
    }
}
