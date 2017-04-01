using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                new Rect(0, 0, Width, Height));
        }

        public Rectangles(string Name,
            float startX, float startY, float finishX, float finishY) : base(
                Name)
        {
            this.startX = Math.Min(startX, finishX);
            this.startY = Math.Min(startY, finishY);
            this.finishX = Math.Max(startX, finishX);            
            this.finishY = Math.Max(startY, finishY);            
            Width = Math.Abs(startX - finishX);
            Height = Math.Abs(startY - finishY);        
            MouseDown += selectShape;
            MouseDown += setCurrentShape;
            MouseMove += movingCurrentShape;
            MouseUp += stopMovingCurrentShape;
        }

        public void selectShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.isDraw)
            {
                var rect = (Rectangles)sender;
                var temp = e.GetPosition(GlobalProperties.drawShape);
                if (GlobalProperties.selectedShape != null)
                {
                    GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape.selection);
                }
                GlobalProperties.selectedShape = rect;
                var forusFrame = getFocuseFrame(rect.Width, rect.Height, GlobalProperties.MainCanvas, rect.startX,
                    rect.startY);
                rect.selection = forusFrame;
                Debug.WriteLine($"{GlobalProperties.selectedShape.startX} {GlobalProperties.selectedShape.startY}START");
                GlobalProperties.isShapeSelected = true;
            }            
        }

        public void setCurrentShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.isDraw)
            {
                //var rect = (Rectangles)sender;
                var temp = e.GetPosition(GlobalProperties.MainCanvas);
                //var point = new Point(GlobalProperties.selectedShape.startX, GlobalProperties.selectedShape.startY);
                GlobalProperties.selectedShapePoint = temp;
                //GlobalProperties.point1 = temp;
                //Canvas.SetLeft(GlobalProperties.selectedShape, GlobalProperties.selectedShape.startX + (GlobalProperties.point1.X - GlobalProperties.selectedShapePoint.X));
                //Canvas.SetTop(GlobalProperties.selectedShape, GlobalProperties.selectedShape.startY + (GlobalProperties.point1.Y - GlobalProperties.selectedShapePoint.Y));
                //GlobalProperties.selectedShape.startX = (float)(GlobalProperties.selectedShape.startX + (GlobalProperties.point1.X - GlobalProperties.selectedShapePoint.X));
                //GlobalProperties.selectedShape.startY = (float)(GlobalProperties.selectedShape.startY + (GlobalProperties.point1.Y - GlobalProperties.selectedShapePoint.Y));
                //GlobalProperties.selectedShapePoint = new Point(GlobalProperties.point1.X, GlobalProperties.point1.Y);
            }
        }

        public void movingCurrentShape(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && GlobalProperties.selectedShape != null && !GlobalProperties.isDraw)
            {                                                            
                GlobalProperties.point1 = e.GetPosition(GlobalProperties.MainCanvas);
                //var point4 = new Point(GlobalProperties.selectedShape.startX, GlobalProperties.selectedShape.startY);
                //var point2 = e.GetPosition(GlobalProperties.RectCanvas);
                //var point3 = new Point(Canvas.GetLeft(GlobalProperties.selectedShape), (float)(Canvas.GetTop(GlobalProperties.selectedShape)));
                //Debug.WriteLine($"{point1.X} {point1.Y} CANVAS");
                //Debug.WriteLine($"{point3.X} {point4.Y} CANVAS");
                //Debug.WriteLine($"{point2.X} {point2.Y} RECT");
                //Debug.WriteLine($"{point3.X} {point3.Y} SHAPE");
                //var pointRect = new Point((double)GlobalProperties.selectedShape.startX, (double)GlobalProperties.selectedShape.startY);
                //Canvas.SetLeft(GlobalProperties.selectedShape.selection, pointRect.X);
                //Canvas.SetTop(GlobalProperties.selectedShape.selection, pointRect.Y); 
                //var f = Canvas.GetLeft(GlobalProperties.selectedShape);     
                Canvas.SetLeft(GlobalProperties.selectedShape, GlobalProperties.selectedShape.startX + (GlobalProperties.point1.X - GlobalProperties.selectedShapePoint.X));
                Canvas.SetTop(GlobalProperties.selectedShape, GlobalProperties.selectedShape.startY + (GlobalProperties.point1.Y - GlobalProperties.selectedShapePoint.Y));
                GlobalProperties.selectedShape.startX = (float)(GlobalProperties.selectedShape.startX + (GlobalProperties.point1.X - GlobalProperties.selectedShapePoint.X));
                GlobalProperties.selectedShape.startY = (float)(GlobalProperties.selectedShape.startY + (GlobalProperties.point1.Y - GlobalProperties.selectedShapePoint.Y));
                GlobalProperties.selectedShapePoint = new Point(GlobalProperties.point1.X, GlobalProperties.point1.Y);
                //Debug.WriteLine($"{GlobalProperties.selectedShape.startX} {GlobalProperties.selectedShape.startY} START");
                //GlobalProperties.selectedShape.startX = (float)point1.X - GlobalProperties.selectedShape.Width;
                //GlobalProperties.selectedShape.startY = (float)point1.X - GlobalProperties.selectedShape.Width;

                //Debug.WriteLine($"{point1.X} {GlobalProperties.selectedShapePoint.X} {point1.Y} {GlobalProperties.selectedShapePoint.Y} FINISH");
                //Debug.WriteLine("-----");
            }
        }

        public void stopMovingCurrentShape(object sender, MouseEventArgs e)
        {
            //GlobalProperties.selectedShape = null;
            //if (!GlobalProperties.isDraw)
            //{
            //    var mousePoint = e.GetPosition(GlobalProperties.MainCanvas);
            //    var rect = (Rectangles)sender;
            //}            
        }

        public override void Draw()
        {            
            Canvas.SetLeft(this, GlobalProperties.startX);
            Canvas.SetTop(this, GlobalProperties.startY);
            GlobalProperties.MainCanvas.Children.Add(this);            
        }

        private Rectangle getFocuseFrame(float width, float height, Canvas canvas, float x, float y)
        {
            Rectangle focus = new Rectangle();
            focus.Stroke = new SolidColorBrush(Colors.SlateBlue);
            focus.StrokeDashArray = new DoubleCollection(new List<double> { 5, 1 });
            focus.StrokeThickness = 2.0;
            focus.Width = width;
            focus.Height = height;
            GlobalProperties.MainCanvas.Children.Add(focus);
            Canvas.SetLeft(focus, x);
            Canvas.SetTop(focus, y);
            return focus;
        }
    }

    class RectangleCreator : ICreator
    {
        public Shapes FactoryMethod(string Name,
            float startX, float startY, float finishX, float finishY)
        {
            return new Rectangles(Name, startX, startY, finishX, finishY);
        }
    }
}
