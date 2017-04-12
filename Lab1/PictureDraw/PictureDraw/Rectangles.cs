using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
                if (GlobalProperties.selectedShape != null)
                {
                    GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape.selection);
                    foreach (var angle in GlobalProperties.AnglesBorder)
                    {
                        GlobalProperties.MainCanvas.Children.Remove(angle);
                    }
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
                foreach (var angle in GlobalProperties.AnglesBorder)
                {
                    var s = Canvas.GetLeft(angle);
                    Canvas.SetLeft(angle, Canvas.GetLeft(angle) + (GlobalProperties.currentMousePoint.X - GlobalProperties.selectedShapePoint.X));
                    Canvas.SetTop(angle, Canvas.GetTop(angle) + (GlobalProperties.currentMousePoint.Y - GlobalProperties.selectedShapePoint.Y));
                }
                GlobalProperties.selectedShape.startX = (float)(offset.X);
                GlobalProperties.selectedShape.startY = (float)(offset.Y);
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

        private Rectangle getAngle(float x, float y, float width, float height)
        {
            var angle = new Rectangle { Width = width, Height = height, Fill = Brushes.Black };
            angle.MouseDown += setResizeAngle;
            angle.MouseMove += resizeShape;
            angle.MouseUp += stopResizeShape;
            GlobalProperties.MainCanvas.Children.Add(angle);
            Canvas.SetLeft(angle, x);
            Canvas.SetTop(angle, y);
            return angle;
        }

        public void setResizeAngle(object sender, MouseEventArgs e)
        {
            GlobalProperties.selectedAnglePoint = e.GetPosition(GlobalProperties.MainCanvas);
            GlobalProperties.selectedAngle = (Rectangle)sender;
            GlobalProperties.isAngleSelected = true;
        }

        public void resizeShape(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var angle = (Rectangle)sender;
                GlobalProperties.currentMousePoint = e.GetPosition(GlobalProperties.MainCanvas);
                var offset = new Point(Canvas.GetLeft(angle) + (GlobalProperties.currentMousePoint.X - GlobalProperties.selectedAnglePoint.X),
                    Canvas.GetTop(angle) + (GlobalProperties.currentMousePoint.Y - GlobalProperties.selectedAnglePoint.Y));
                Canvas.SetLeft(GlobalProperties.selectedAngle, offset.X);
                Canvas.SetTop(GlobalProperties.selectedAngle, offset.Y);
                var offsetShape = new Point(GlobalProperties.currentMousePoint.X - GlobalProperties.selectedAnglePoint.X, GlobalProperties.currentMousePoint.Y -
                                   GlobalProperties.selectedAnglePoint.Y);
                GlobalProperties.selectedShape.startX = (float)(GlobalProperties.selectedShape.startX +
                                                        offsetShape.X);
                GlobalProperties.selectedShape.startY = (float)(GlobalProperties.selectedShape.startY +
                                        offsetShape.Y);
                GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape);
                Debug.WriteLine($"{GlobalProperties.MainCanvas.Children.Count}");
                Shapes shape = GlobalProperties.currentShape.FactoryMethod("Default",
                    GlobalProperties.selectedShape.startX, GlobalProperties.selectedShape.startY,
                    GlobalProperties.selectedShape.finishX, GlobalProperties.selectedShape.finishY,
                    Colors.Black, GlobalProperties.selectedShape.ColorStroke);
                shape.Draw();
                GlobalProperties.selectedShape = shape;
                GlobalProperties.selectedAnglePoint = new Point(GlobalProperties.currentMousePoint.X, GlobalProperties.currentMousePoint.Y);
            }
        }

        public void stopResizeShape(object sender, MouseEventArgs e)
        {
        }

        private List<Rectangle> getFocusAngles(float x, float y, float width, float height)
        {
            const float SIZE_ANGLES = 6;
            var result = new List<Rectangle>();
            var leftTopAngle = getAngle(x - (GlobalProperties.frameSize + SIZE_ANGLES) / 2,
                y - (GlobalProperties.frameSize + SIZE_ANGLES) / 2, SIZE_ANGLES, SIZE_ANGLES);
            var rightTopAngle = getAngle(x + width + SIZE_ANGLES / 2,
                y - (GlobalProperties.frameSize + SIZE_ANGLES) / 2, SIZE_ANGLES, SIZE_ANGLES);
            var rightBottomAngle = getAngle(x + width + SIZE_ANGLES / 2,
                y + height + SIZE_ANGLES / 2, SIZE_ANGLES, SIZE_ANGLES);
            var leftBottomAngle = getAngle(x - (GlobalProperties.frameSize + SIZE_ANGLES) / 2,
                y + height + SIZE_ANGLES / 2, SIZE_ANGLES, SIZE_ANGLES);
            result.Add(leftTopAngle);
            result.Add(rightTopAngle);
            result.Add(rightBottomAngle);
            result.Add(leftBottomAngle);
            return result;
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
            GlobalProperties.AnglesBorder = getFocusAngles(x, y, width, height);
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