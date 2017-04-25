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
using YAXLib;

namespace PictureDraw
{
    public class Lines : Shapes, ISelectable, IMovable, IResizable, IEditable
    {
        [YAXSerializableField]    
        private double X1 { get; set; }
        [YAXSerializableField]
        private double Y1 { get; set; }
        [YAXSerializableField]
        private double X2 { get; set; }
        [YAXSerializableField]
        private double Y2 { get; set; }

        public Lines() { }

        public override void AfterDesirialization()
        {
            MouseDown += SelectShape;
            MouseDown += SetDragPoint;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //TODO : THICKNESS            
            drawingContext.DrawLine(new Pen(new SolidColorBrush(ColorStroke), ThicknessBorder),
                new Point(0, 0), new Point(X2 - X1, Y2 - Y1));
        }

        public Lines(string name, Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double ThicknessBorder) : base(
                name, colorFill, colorStroke, ThicknessBorder)
        {
//            this.startPoint = startPoint;
//            this.finishPoint = finishPoint;
            this.startPoint = new Point(Math.Min(startPoint.X, finishPoint.X), Math.Min(startPoint.Y, finishPoint.Y));
            this.finishPoint = new Point(Math.Max(startPoint.X, finishPoint.X), Math.Max(startPoint.Y, finishPoint.Y));
            GlobalProperties.startPoint = startPoint;                      
            GlobalProperties.finishPoint = finishPoint;                      
            Width = Math.Abs(this.finishPoint.X - this.startPoint.X);
            Height = Math.Abs(this.finishPoint.Y - this.startPoint.Y);
            X1 = startPoint.X;
            Y1 = startPoint.Y;
            X2 = finishPoint.X;
            Y2 = finishPoint.Y;
            MouseDown += SelectShape;
            MouseDown += SetDragPoint;
            if (CommonMethods.CheckType(this, typeof(IEditable)))
            {
                MouseDown += ShowProperties;
            }
//            MouseMove += MovingShape;
//            MouseUp += StopMovingShape;
        }

        public override void Draw()
        {
            Canvas.SetLeft(this, X1);
            Canvas.SetTop(this, Y1);
            Debug.WriteLine($"{X1} {Y1}");
            GlobalProperties.MainCanvas.Children.Add(this);
        }

        public void SelectShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                var rect = (Lines)sender;
                if (GlobalProperties.selectedShape != null)
                {
                    RemoveSelection(GlobalProperties.selectedShape);
                }
                rect.Selection = GetFocusFrame(rect, GlobalProperties.frameSize);
                GlobalProperties.selectedShape = rect;
                GlobalProperties.drawShape = rect;
                rect.AnglesBorder = GetFocusAngles(rect, GlobalProperties.frameSize);
                if (CommonMethods.CheckType(rect, typeof(IResizable)))
                {
                    SetAnglesAction(rect);
                }
            }
        }

        public static void RemoveSelection(Shapes shape)
        {
            GlobalProperties.MainCanvas.Children.Remove(shape.Selection);
            shape.Selection = null;
            GlobalProperties.PropertiesPanel.Visibility = Visibility.Hidden;
            //TODO : MAYBE NEED TO REWRITE
            foreach (var angle in shape.AnglesBorder.Values)
            {
                GlobalProperties.MainCanvas.Children.Remove(angle);
            }
        }

        private Rectangle GetFocusFrame(Shapes rect, double frameSize)
        {
            Rectangle focus = new Rectangle();
            focus.Stroke = new SolidColorBrush(Colors.SlateBlue);
            focus.StrokeDashArray = new DoubleCollection(new List<double> { 5, 1 });
            focus.StrokeThickness = 2.0;
            focus.Width = rect.Width + frameSize;
            focus.Height = rect.Height + frameSize;
            GlobalProperties.MainCanvas.Children.Add(focus);

//            var tempPoint = new Point(Math.Min(rect.startPoint.X, rect.finishPoint.X), Math.Min(rect.startPoint.Y, rect.finishPoint.Y));
            //            new Point(Math.Min(startPoint.X, finishPoint.X), Math.Min(startPoint.Y, finishPoint.Y));
//            new Point(Math.Max(startPoint.X, finishPoint.X), Math.Max(startPoint.Y, finishPoint.Y));
            Canvas.SetLeft(focus, rect.startPoint.X - frameSize / 2);
            Canvas.SetTop(focus, rect.startPoint.Y - frameSize / 2);
            return focus;
        }

        public Dictionary<string, Rectangle> GetFocusAngles(Shapes shape, double frameSize)
        {
            const double SIZE_ANGLES = 6;
            var leftTopPoint = new Point(shape.startPoint.X - (frameSize + SIZE_ANGLES) / 2, shape.startPoint.Y - (frameSize + SIZE_ANGLES) / 2);
            var rightBottomPoint = new Point(shape.startPoint.X + shape.Width + SIZE_ANGLES / 2, shape.startPoint.Y + shape.Height + SIZE_ANGLES / 2);
            var rightTopPoint = new Point(rightBottomPoint.X, leftTopPoint.Y);
            var leftBottomPoint = new Point(leftTopPoint.X, rightBottomPoint.Y);
            var leftTopAngle = GetAngle(leftTopPoint, SIZE_ANGLES);
            var rightTopAngle = GetAngle(rightTopPoint, SIZE_ANGLES);
            var rightBottomAngle = GetAngle(rightBottomPoint, SIZE_ANGLES);
            var leftBottomAngle = GetAngle(leftBottomPoint, SIZE_ANGLES);
            var result = new Dictionary<string, Rectangle>
            {
                {"leftTop", leftTopAngle },
                {"rightTop", rightTopAngle },
                {"rightBottom", rightBottomAngle },
                {"leftBottom", leftBottomAngle }
            };
            return result;
        }

        private Rectangle GetAngle(Point position, double size)
        {
            var angle = new Rectangle { Width = size, Height = size, Fill = Brushes.Black };
            GlobalProperties.MainCanvas.Children.Add(angle);
            Canvas.SetLeft(angle, position.X);
            Canvas.SetTop(angle, position.Y);
            return angle;
        }

        public void SetDragPoint(object sender, MouseEventArgs e)
        {
            var rect = (Lines)sender;
            if (!GlobalProperties.DrawModeOn)
            {
                rect.dragPoint = e.GetPosition(GlobalProperties.MainCanvas);
                GlobalProperties.SecondaryCanvas = new Canvas { Width = GlobalProperties.RectCanvas.Width, Height = GlobalProperties.RectCanvas.Height };
                GlobalProperties.ResizeCanvas = new Rectangle
                {
                    Width = GlobalProperties.RectCanvas.Width,
                    Height = GlobalProperties.RectCanvas.Height,
                    Fill = Brushes.AntiqueWhite,
                    Opacity = 0
                };
                GlobalProperties.SecondaryCanvas.MouseMove += MovingShape;
                GlobalProperties.SecondaryCanvas.MouseUp += StopMovingShape;
                GlobalProperties.SecondaryCanvas.Children.Add(GlobalProperties.ResizeCanvas);
                GlobalProperties.MainCanvas.Children.Add(GlobalProperties.SecondaryCanvas);
                Canvas.SetLeft(GlobalProperties.SecondaryCanvas, 0);
                Panel.SetZIndex(GlobalProperties.SecondaryCanvas, 99);
                Canvas.SetTop(GlobalProperties.SecondaryCanvas, 0);
            }
        }

        public void MovingShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                if (e.LeftButton == MouseButtonState.Pressed && CommonMethods.CheckType(GlobalProperties.selectedShape, typeof(Lines)))
                {
                    var rect = (Lines) GlobalProperties.selectedShape;
                    GlobalProperties.selectedShape.Opacity = 0.7;
                    if (!Double.IsNaN(rect.dragPoint.X))
                    {
                        var currentMousePosition = e.GetPosition(GlobalProperties.MainCanvas);
                        var offsetFrameBorder = new Point(GlobalProperties.selectedShape.startPoint.X + (currentMousePosition.X - rect.dragPoint.X),
                            GlobalProperties.selectedShape.startPoint.Y + (currentMousePosition.Y - rect.dragPoint.Y));
                        var offset = new Point((currentMousePosition.X - rect.dragPoint.X),
                            (currentMousePosition.Y - rect.dragPoint.Y));
                        ChangePosition(offset, offsetFrameBorder, rect, GlobalProperties.frameSize, currentMousePosition);
                    }
                }
            }
        }

        private static void ChangePosition(Point offset, Point offsetFrameBorder, Lines shape, double frameSize, Point mousePosition)
        {
            Canvas.SetLeft(shape, shape.X1 + offset.X);
            Canvas.SetTop(shape, shape.Y1 + offset.Y);
            Canvas.SetLeft(shape.Selection, offsetFrameBorder.X - frameSize / 2);
            Canvas.SetTop(shape.Selection, offsetFrameBorder.Y - frameSize / 2);
            foreach (var angle in shape.AnglesBorder.Values)
            {
                Canvas.SetLeft(angle,
                    Canvas.GetLeft(angle) + (mousePosition.X - shape.dragPoint.X));
                Canvas.SetTop(angle,
                    Canvas.GetTop(angle) + (mousePosition.Y - shape.dragPoint.Y));
            }
            GlobalProperties.selectedShape.startPoint = offsetFrameBorder;
            shape.X1 += offset.X;
            shape.Y1 += offset.Y;
            shape.X2 += offset.X;
            shape.Y2 += offset.Y;
            Debug.WriteLine($"CHANGE POSITION X1: {shape.X1} {shape.Y1}");
            Debug.WriteLine($"CHANGE POSITION X2: {shape.X2} {shape.Y2}");
            shape.dragPoint = new Point(mousePosition.X,
                mousePosition.Y);
        }

        //TODO : ADD NEW SHAPE TO LIST
        public void StopMovingShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                GlobalProperties.selectedShape.Opacity = 1;                                                  
                GlobalProperties.selectedShape.finishPoint = new Point(GlobalProperties.selectedShape.startPoint.X +
                        GlobalProperties.selectedShape.Width, GlobalProperties.selectedShape.startPoint.Y + GlobalProperties.selectedShape.Height);                
                GlobalProperties.SecondaryCanvas.Children.Remove(GlobalProperties.ResizeCanvas);
                GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.SecondaryCanvas);
            }
        }

        public void SetAnglesAction(Shapes rect)
        {
            foreach (var angle in rect.AnglesBorder.Values)
            {
                angle.MouseDown += SetResizeAngle;
                //                angle.MouseUp += StopResizeShape;
            }
            //            rect.AnglesBorder["leftTop"].MouseMove += ResizeAngles;
            //            //TODO : MAKE ALL EVENTS
            //            rect.AnglesBorder["rightTop"].MouseMove += ResizeAngles;
            //            rect.AnglesBorder["rightBottom"].MouseMove += ResizeAngles;
            //            rect.AnglesBorder["leftBottom"].MouseMove += ResizeAngles;
        }

        public void SetResizeAngle(object sender, MouseEventArgs e)
        {
            GlobalProperties.selectedAnglePoint = e.GetPosition(GlobalProperties.MainCanvas);
            var angle = (Rectangle)sender;
            GlobalProperties.selectedAngle = angle;
            GlobalProperties.SecondaryCanvas = new Canvas { Width = GlobalProperties.RectCanvas.Width, Height = GlobalProperties.RectCanvas.Height };
            GlobalProperties.ResizeCanvas = new Rectangle
            {
                Width = GlobalProperties.RectCanvas.Width,
                Height = GlobalProperties.RectCanvas.Height,
                Fill = Brushes.AntiqueWhite,
                Opacity = 0
            };
            GlobalProperties.SecondaryCanvas.MouseMove += ResizeAngles;
            GlobalProperties.SecondaryCanvas.MouseUp += StopResizeShape;
            GlobalProperties.SecondaryCanvas.Children.Add(GlobalProperties.ResizeCanvas);
            GlobalProperties.MainCanvas.Children.Add(GlobalProperties.SecondaryCanvas);
            Canvas.SetLeft(GlobalProperties.SecondaryCanvas, 0);
            Panel.SetZIndex(GlobalProperties.SecondaryCanvas, 99);
            Canvas.SetTop(GlobalProperties.SecondaryCanvas, 0);
        }

        public void ResizeAngles(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Dictionary<Rectangle, int[]> sizeConsts = new Dictionary<Rectangle, int[]>
                {
                    {GlobalProperties.selectedShape.AnglesBorder["leftTop"], new []{-1, -1} },
                    {GlobalProperties.selectedShape.AnglesBorder["rightTop"], new []{1, -1} },
                    {GlobalProperties.selectedShape.AnglesBorder["rightBottom"], new []{1, 1} },
                    {GlobalProperties.selectedShape.AnglesBorder["leftBottom"], new []{-1, 1} }
                };
                Dictionary<Rectangle, int[]> pointsConsts = new Dictionary<Rectangle, int[]>
                {
                    {GlobalProperties.selectedShape.AnglesBorder["leftTop"], new []{1, 1, 0, 0} },
                    {GlobalProperties.selectedShape.AnglesBorder["rightTop"], new []{0, 1, 1, 0} },
                    {GlobalProperties.selectedShape.AnglesBorder["rightBottom"], new []{0, 0, 1, 1} },
                    {GlobalProperties.selectedShape.AnglesBorder["leftBottom"], new []{1, 0, 0, 1} }
                };
                var angle = GlobalProperties.selectedAngle;
                var angleName =
                    GlobalProperties.selectedShape.AnglesBorder.Keys.First(
                        k => Equals(GlobalProperties.selectedShape.AnglesBorder[k], GlobalProperties.selectedAngle));
                var offset = GetOffset(e);
                if (GlobalProperties.selectedShape.Width + sizeConsts[angle][0] * offset.X > GlobalProperties.MinShapeSize &&
                    GlobalProperties.selectedShape.Height + sizeConsts[angle][1] * offset.Y > GlobalProperties.MinShapeSize)
                {
                    Canvas.SetLeft(angle, Canvas.GetLeft(angle) + offset.X);
                    Canvas.SetTop(angle, Canvas.GetTop(angle) + offset.Y);
                    GlobalProperties.selectedShape.startPoint =
                        new Point(GlobalProperties.selectedShape.startPoint.X + pointsConsts[angle][0] * offset.X,
                            GlobalProperties.selectedShape.startPoint.Y + pointsConsts[angle][1] * offset.Y);
                    GlobalProperties.selectedShape.finishPoint =
                        new Point(GlobalProperties.selectedShape.finishPoint.X + pointsConsts[angle][2] * offset.X,
                            GlobalProperties.selectedShape.finishPoint.Y + pointsConsts[angle][3] * offset.Y);
                    var line = (Lines) GlobalProperties.selectedShape;
                    line.X1 += pointsConsts[angle][0]*offset.X;
                    line.Y1 += pointsConsts[angle][1]*offset.Y;
                    line.X2 += pointsConsts[angle][2]*offset.X;
                    line.Y2 += pointsConsts[angle][3]*offset.Y;
                    Debug.WriteLine($"{angleName}");
                    RecreateShape(e);
                    GlobalProperties.selectedAngle = GlobalProperties.selectedShape.AnglesBorder[angleName];
                }
            }
        }

        private void RecreateShape(MouseEventArgs e)
        {
            var type = GlobalProperties.selectedShape.GetType().Name;
            GlobalProperties.currentShape = CommonMethods.creators[type];
            GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape);
            RemoveSelection(GlobalProperties.selectedShape);
            var line = (Lines) GlobalProperties.selectedShape;
            Shapes shape = GlobalProperties.currentShape.FactoryMethod("Default",
                new Point(line.X1, line.Y1), new Point(line.X2, line.Y2),
                GlobalProperties.selectedShape.ColorFill, GlobalProperties.selectedShape.ColorStroke,
                GlobalProperties.selectedShape.ThicknessBorder);
            shape.Draw();
            shape.Selection = GetFocusFrame(shape, GlobalProperties.frameSize);
            shape.AnglesBorder = GetFocusAngles(shape, GlobalProperties.frameSize);
            SetAnglesAction(shape);
            //TODO : MAKE ALL FIELDS AS WHEN WE CHANGING COLORS
            shape.dragPoint = new Point(Double.NaN, Double.NaN);
            GlobalProperties.ShapesList.AllShapes.Remove(GlobalProperties.selectedShape);
            GlobalProperties.selectedShape = shape;
            GlobalProperties.selectedAnglePoint = new Point(e.GetPosition(GlobalProperties.MainCanvas).X, e.GetPosition(GlobalProperties.MainCanvas).Y);
            GlobalProperties.ShapesList.AllShapes.Add(GlobalProperties.selectedShape);
        }

        private static Point GetOffset(MouseEventArgs e)
        {
            var currentMousePosition = e.GetPosition(GlobalProperties.MainCanvas);
            var offset = new Point(currentMousePosition.X - GlobalProperties.selectedAnglePoint.X,
                currentMousePosition.Y - GlobalProperties.selectedAnglePoint.Y);
            return offset;
        }

        public void StopResizeShape(object sender, MouseEventArgs e)
        {
            GlobalProperties.SecondaryCanvas.Children.Remove(GlobalProperties.ResizeCanvas);
            GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.SecondaryCanvas);
            GlobalProperties.selectedShape.finishPoint = new Point(GlobalProperties.selectedShape.startPoint.X + GlobalProperties.selectedShape.Width,
                GlobalProperties.selectedShape.startPoint.Y + GlobalProperties.selectedShape.Height);
        }

        public void ShowProperties(object sender, MouseEventArgs e)
        {
            var rect = (Lines)sender;
            GlobalProperties.PropertiesPanel.Visibility = Visibility.Visible;
            GlobalProperties.FillSelected.SelectedColor = rect.ColorFill;
            GlobalProperties.BorderSelected.SelectedColor = rect.ColorStroke;
        }
    }

    class LineCreator : ICreator
    {
        public Shapes FactoryMethod(string Name,
            Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double ThicknessBorder)
        {
            return new Lines(Name, startPoint, finishPoint, colorFill, colorStroke, ThicknessBorder);
        }
    }
}
