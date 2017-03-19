using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace PictureDraw
{
    [Serializable]
    public class Rectangles : Shapes, ISelectable
    {        

        public Rectangles() { }        

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            
            drawingContext.DrawRectangle(Brushes.Aquamarine,
                new Pen(Brushes.Black, 1),
                new Rect(startX, startY, Width, Height));
        }

        public Rectangles(string Name,
            int startX, int startY, int finishX, int finishY): base(
                Name)
        {
            this.startX = Math.Min(startX, finishX);
            this.startY = Math.Min(startY, finishY);
            this.finishX = Math.Max(startX, finishX);            
            this.finishY = Math.Max(startY, finishY);            
            Width = Math.Abs(startX - finishX);
            Height = Math.Abs(startY - finishY);        
            MouseDown += selectShape;            
        }

        public override void Draw()
        {
            GlobalProperties.MainCanvas.Children.Add(this);
        }

        public void selectShape(object sender, MouseEventArgs e)
        {
            var rect = (Rectangles)sender;
            if (GlobalProperties.selectedShape != rect)
            {
                if (GlobalProperties.selectedShape != null)
                {
                    GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape.selection);
                }
                GlobalProperties.selectedShape = rect; 
                Rectangle focus = new Rectangle();
                focus.Stroke = new SolidColorBrush(Colors.SlateBlue);
                focus.StrokeDashArray = new DoubleCollection(new List<double> { 5, 1 });
                focus.StrokeThickness = 2.0;
                focus.Width = rect.Width;
                focus.Height = rect.Height;
                GlobalProperties.MainCanvas.Children.Add(focus);
                Canvas.SetLeft(focus, rect.startX);
                Canvas.SetTop(focus, rect.startY);
                rect.selection = focus;
            }
            GlobalProperties.isShapeSelected = true;            
        }
    }

    class RectangleCreator : ICreator
    {
        public Shapes FactoryMethod(string Name,
            int startX, int startY, int finishX, int finishY)
        {
            return new Rectangles(Name, startX, startY, finishX, finishY);
        }
    }
}
