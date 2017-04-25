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

namespace PictureDraw
{
    [Serializable]
    public class Tetragons : Shapes, ISelectable, IMovable, IResizable, IEditable
    {
        private double X1 { get; set; }
        private double Y1 { get; set; }
        private double X2 { get; set; }
        private double Y2 { get; set; }
        private double X3 { get; set; }
        private double Y3 { get; set; }
        private double X4 { get; set; }
        private double Y4 { get; set; }

        public Tetragons() { }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(new Point(X1, Y1), true, true);
                PointCollection points = new PointCollection
                {
                    new Point(X2, Y2),
                    new Point(X3, Y3),
                    new Point(X4, Y4),
                };
                geometryContext.PolyLineTo(points, true, true);
            }
            drawingContext.DrawGeometry(new SolidColorBrush(ColorFill),
                    new Pen(new SolidColorBrush(ColorStroke), ThicknessBorder), streamGeometry);
        }

        public Tetragons(string name, Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double ThicknessBorder) : base(
                name, colorFill, colorStroke, ThicknessBorder)
        {
            //finish not initialize 
            this.startPoint = new Point(Math.Min(startPoint.X, finishPoint.X), Math.Min(startPoint.Y, finishPoint.Y));
            this.finishPoint = new Point(Math.Max(startPoint.X, finishPoint.X), Math.Max(startPoint.Y, finishPoint.Y));
            Width = this.finishPoint.X - this.startPoint.X;
            Height = this.finishPoint.Y - this.startPoint.Y;
            X1 = 0;
            Y1 = Height;
            X2 = Width / 3;
            Y2 = 0;
            X3 = 2 * X2;
            Y3 = 0;
            X4 = Width;
            Y4 = Height;
            MouseDown += SelectShape;
            MouseDown += SetDragPoint;
            MouseDown += ShowProperties;
        }

        public override void Draw()
        {
            Canvas.SetLeft(this, startPoint.X);
            Canvas.SetTop(this, startPoint.Y);
            GlobalProperties.MainCanvas.Children.Add(this);
        }
        public void SelectShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                var triangle = (Tetragons)sender;
                if (GlobalProperties.selectedShape != null)
                {
                    RemoveSelection(GlobalProperties.selectedShape);
                }
                triangle.Selection = GetFocusFrame(triangle, GlobalProperties.frameSize);
                GlobalProperties.selectedShape = triangle;
                GlobalProperties.drawShape = triangle;
                triangle.AnglesBorder = GetFocusAngles(triangle, GlobalProperties.frameSize);
                if (CommonMethods.CheckType(triangle, typeof(IResizable)))
                {
                    SetAnglesAction(triangle);
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

        private Rectangle GetFocusFrame(Shapes triangle, double frameSize)
        {
            Rectangle focus = new Rectangle();
            focus.Stroke = new SolidColorBrush(Colors.SlateBlue);
            focus.StrokeDashArray = new DoubleCollection(new List<double> { 5, 1 });
            focus.StrokeThickness = 2.0;
            focus.Width = triangle.Width + frameSize;
            focus.Height = triangle.Height + frameSize;
            GlobalProperties.MainCanvas.Children.Add(focus);
            Canvas.SetLeft(focus, triangle.startPoint.X - frameSize / 2);
            Canvas.SetTop(focus, triangle.startPoint.Y - frameSize / 2);
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
            //TODO : REFLECTION CAST WITHOUT TYPE
            var rect = (Tetragons)sender;
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
                if (e.LeftButton == MouseButtonState.Pressed && CommonMethods.CheckType(GlobalProperties.selectedShape, typeof(Tetragons)))
                {
                    var triangle = (Tetragons)GlobalProperties.selectedShape;
                    GlobalProperties.selectedShape.Opacity = 0.7;
                    if (!Double.IsNaN(triangle.dragPoint.X))
                    {
                        var currentMousePosition = e.GetPosition(GlobalProperties.MainCanvas);
                        var offset = new Point(triangle.startPoint.X + (currentMousePosition.X - triangle.dragPoint.X),
                            triangle.startPoint.Y + (currentMousePosition.Y - triangle.dragPoint.Y));
                        ChangePosition(offset, triangle, GlobalProperties.frameSize, currentMousePosition);
                    }
                }
            }
        }

        private static void ChangePosition(Point offset, Tetragons shape, double frameSize, Point mousePosition)
        {
            Canvas.SetLeft(shape, offset.X);
            Canvas.SetTop(shape, offset.Y);
            Canvas.SetLeft(shape.Selection, offset.X - frameSize / 2);
            Canvas.SetTop(shape.Selection, offset.Y - frameSize / 2);
            foreach (var angle in shape.AnglesBorder.Values)
            {
                Canvas.SetLeft(angle,
                    Canvas.GetLeft(angle) + (mousePosition.X - shape.dragPoint.X));
                Canvas.SetTop(angle,
                    Canvas.GetTop(angle) + (mousePosition.Y - shape.dragPoint.Y));
            }
            shape.startPoint = new Point(offset.X, offset.Y);
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
            Shapes shape = GlobalProperties.currentShape.FactoryMethod("Default",
                GlobalProperties.selectedShape.startPoint, GlobalProperties.selectedShape.finishPoint,
                GlobalProperties.selectedShape.ColorFill, GlobalProperties.selectedShape.ColorStroke,
                GlobalProperties.selectedShape.ThicknessBorder);
            shape.Draw();
            shape.Selection = GetFocusFrame(shape, GlobalProperties.frameSize);
            shape.AnglesBorder = GetFocusAngles(shape, GlobalProperties.frameSize);
            SetAnglesAction(shape);
            //TODO : MAKE ALL FIELDS AS WHEN WE CHANGING COLORS
            shape.dragPoint = new Point(Double.NaN, Double.NaN);
            GlobalProperties.selectedShape = shape;
            GlobalProperties.selectedAnglePoint = new Point(e.GetPosition(GlobalProperties.MainCanvas).X, e.GetPosition(GlobalProperties.MainCanvas).Y);

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
            var rect = (Tetragons)sender;
            GlobalProperties.PropertiesPanel.Visibility = Visibility.Visible;
            GlobalProperties.FillSelected.SelectedColor = rect.ColorFill;
            GlobalProperties.BorderSelected.SelectedColor = rect.ColorStroke;
        }
    }
    
    class TetragonCreator : ICreator
    {
        public Shapes FactoryMethod(string Name,
            Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double ThicknessBorder)
        {
            return new Tetragons(Name, startPoint, finishPoint, colorFill, colorStroke, ThicknessBorder);
        }
    }
}
