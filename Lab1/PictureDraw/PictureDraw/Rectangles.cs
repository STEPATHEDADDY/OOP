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
            
            drawingContext.DrawRectangle(new SolidColorBrush(ColorFill), 
                new Pen(new SolidColorBrush(ColorStroke), GlobalProperties.Thickness),
                new Rect(0, 0, Width, Height));
        }

        public Rectangles(string name,
            float startX, float startY, float finishX, float finishY, Color colorFill, Color colorStroke) : base(
                name, colorFill, colorStroke)
        {
            this.startX = Math.Min(startX, finishX);
            this.startY = Math.Min(startY, finishY);
            this.finishX = Math.Max(startX, finishX);            
            this.finishY = Math.Max(startY, finishY);                   
            Width = this.finishX - this.startX;
            Height = this.finishY - this.startY;       
            MouseDown += selectShape;
            MouseDown += setCurrentShape;
            MouseMove += movingCurrentShape;
            MouseUp += stopMovingCurrentShape;
        }

//        private Rectangle getCbColor(ComboBox cb, SolidColorBrush color)
//        {
//            Rectangle rect = null;
//            foreach (var item in cb.Items)
//            {
//                var temp = (Rectangle)item;
//                var colorRect = (SolidColorBrush)temp.Fill;
//                if (Equals(colorRect, color))
//                {
//                    rect = temp;
//                }
//            }
//            return rect;
//        }

        private void showPropertiesPanel()
        {
            GlobalProperties.PropertiesPanel.Visibility = Visibility.Visible;
            GlobalProperties.FillSelected.SelectedColor = GlobalProperties.selectedShape.ColorFill;
            GlobalProperties.BorderSelected.SelectedColor = GlobalProperties.selectedShape.ColorStroke;
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
                var focusFrame = getFocusFrame(rect.Width, rect.Height, GlobalProperties.MainCanvas, rect.startX,
                    rect.startY);
                rect.selection = focusFrame;                
                GlobalProperties.isShapeSelected = true;
                showPropertiesPanel();
            }            
        }

        public void setCurrentShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.isDraw)
            {
                GlobalProperties.selectedShapePoint = e.GetPosition(GlobalProperties.MainCanvas);
            }
        }

        public void movingCurrentShape(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && GlobalProperties.selectedShape != null && !GlobalProperties.isDraw)
            {                                                            
                GlobalProperties.currentMousePoint = e.GetPosition(GlobalProperties.MainCanvas);
                var offset = new Point(GlobalProperties.selectedShape.startX + (GlobalProperties.currentMousePoint.X - GlobalProperties.selectedShapePoint.X),
                    GlobalProperties.selectedShape.startY + (GlobalProperties.currentMousePoint.Y - GlobalProperties.selectedShapePoint.Y));  
                Canvas.SetLeft(GlobalProperties.selectedShape, offset.X);
                Canvas.SetTop(GlobalProperties.selectedShape, offset.Y);
                Canvas.SetLeft(GlobalProperties.selectedShape.selection, offset.X - GlobalProperties.frameSize / 2);
                Canvas.SetTop(GlobalProperties.selectedShape.selection, offset.Y - GlobalProperties.frameSize / 2);
                GlobalProperties.selectedShape.startX = (float)(offset.X);
                GlobalProperties.selectedShape.startY = (float)(offset.Y);
                //TODO FINISH POINTS
                GlobalProperties.selectedShapePoint = new Point(GlobalProperties.currentMousePoint.X, GlobalProperties.currentMousePoint.Y);
            }
        }

        public void stopMovingCurrentShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.isDraw && GlobalProperties.selectedShape != null)
            {
                GlobalProperties.selectedShape.finishX = GlobalProperties.selectedShape.startX +
                                                        GlobalProperties.selectedShape.Width;
                GlobalProperties.selectedShape.finishY = GlobalProperties.selectedShape.startY +
                                                         GlobalProperties.selectedShape.Height;
            }            
        }

        public override void Draw()
        {            
            Canvas.SetLeft(this, startX);
            Canvas.SetTop(this, startY);
            GlobalProperties.MainCanvas.Children.Add(this);            
        }

        private Rectangle getFocusFrame(float width, float height, Canvas canvas, float x, float y)
        {
            Rectangle focus = new Rectangle();
            focus.Stroke = new SolidColorBrush(Colors.SlateBlue);
            focus.StrokeDashArray = new DoubleCollection(new List<double> { 5, 1 });
            focus.StrokeThickness = 2.0;
            focus.Width = width + GlobalProperties.frameSize;
            focus.Height = height + GlobalProperties.frameSize;
            GlobalProperties.MainCanvas.Children.Add(focus);
            Canvas.SetLeft(focus, x - GlobalProperties.frameSize / 2);
            Canvas.SetTop(focus, y - GlobalProperties.frameSize / 2);
            return focus;
        }
    }

    class RectangleCreator : ICreator
    {
        public Shapes FactoryMethod(string Name,
            float startX, float startY, float finishX, float finishY, Color colorFill, Color colorStroke)
        {
            return new Rectangles(Name, startX, startY, finishX, finishY, colorFill, colorStroke);
        }
    }
}
