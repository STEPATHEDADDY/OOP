using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using PictureDraw;
using YAXLib;


namespace TrianglesModule
{
    public class TrianglesModule : Shapes, ISelectable, IMovable, IResizable, IEditable
    {
        [YAXSerializableField]
        public double X1 { get; set; }
        [YAXSerializableField]
        public double Y1 { get; set; }
        [YAXSerializableField]
        public double X2 { get; set; }
        [YAXSerializableField]
        public double Y2 { get; set; }
        [YAXSerializableField]
        public double X3 { get; set; }
        [YAXSerializableField]
        public double Y3 { get; set; }

        public TrianglesModule() { }

        public sealed override void SetEvents()
        {
            if (CommonMethods.CheckType(this, typeof(ISelectable)))
            {
                MouseDown += SelectShape;
            }
            if (CommonMethods.CheckType(this, typeof(IMovable)))
            {
                MouseDown += SetDragPoint;
            }
            if (CommonMethods.CheckType(this, typeof(IEditable)))
            {
                MouseDown += ShowProperties;
            }
        }

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
                    new Point(X3, Y3)
                };
                geometryContext.PolyLineTo(points, true, true);
            }
            drawingContext.DrawGeometry(new SolidColorBrush(ColorFill),
                    new Pen(new SolidColorBrush(ColorStroke), ThicknessBorder), streamGeometry);
        }

        public TrianglesModule(string name, Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double thicknessBorder) : base(
                name, colorFill, colorStroke, thicknessBorder)
        {
            this.startPoint = startPoint;
            this.finishPoint = finishPoint;
            Width = this.finishPoint.X - this.startPoint.X;
            Height = this.finishPoint.Y - this.startPoint.Y;
            X1 = 0;
            Y1 = Height;
            X2 = Width / 2;
            Y2 = 0;
            X3 = Width;
            Y3 = Height;
            SetEvents();
        }

        public void SelectShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                var triangle = (TrianglesModule)sender;
                if (GlobalProperties.selectedShape != null)
                {
                    CommonMethods.RemoveSelection(GlobalProperties.selectedShape);
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

        public void SetDragPoint(object sender, MouseEventArgs e)
        {
            var rect = (TrianglesModule)sender;
            if (!GlobalProperties.DrawModeOn)
            {
                rect.dragPoint = e.GetPosition(GlobalProperties.MainCanvas);
                var secondaryCanvas = new Canvas { Width = GlobalProperties.RectCanvas.Width, Height = GlobalProperties.RectCanvas.Height };
                var secondaryRectCanvas = new Rectangle
                {
                    Width = GlobalProperties.RectCanvas.Width,
                    Height = GlobalProperties.RectCanvas.Height,
                    Fill = Brushes.AntiqueWhite,
                    Opacity = 0
                };
                secondaryCanvas.MouseMove += MovingShape;
                secondaryCanvas.MouseUp += StopMovingShape;
                secondaryCanvas.Children.Add(secondaryRectCanvas);
                GlobalProperties.MainCanvas.Children.Add(secondaryCanvas);
                Canvas.SetLeft(secondaryCanvas, 0);
                Panel.SetZIndex(secondaryCanvas, 99);
                Canvas.SetTop(secondaryCanvas, 0);
            }
        }

        public void MovingShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                if (e.LeftButton == MouseButtonState.Pressed && CommonMethods.CheckType(GlobalProperties.selectedShape, typeof(TrianglesModule)))
                {
                    var triangle = (TrianglesModule)GlobalProperties.selectedShape;
                    GlobalProperties.selectedShape.Opacity = GlobalProperties.Opacity;
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

        public void SetAnglesAction(Shapes rect)
        {
            foreach (var angle in rect.AnglesBorder.Values)
            {
                angle.MouseDown += SetResizeAngle;
            }
        }

        public void SetResizeAngle(object sender, MouseEventArgs e)
        {
            GlobalProperties.selectedAnglePoint = e.GetPosition(GlobalProperties.MainCanvas);
            var angle = (Rectangle)sender;
            GlobalProperties.selectedAngle = angle;
            var secondaryCanvas = new Canvas { Width = GlobalProperties.RectCanvas.Width, Height = GlobalProperties.RectCanvas.Height };
            var secondaryRectCanvas = new Rectangle
            {
                Width = GlobalProperties.RectCanvas.Width,
                Height = GlobalProperties.RectCanvas.Height,
                Fill = Brushes.AntiqueWhite,
                Opacity = 0
            };
            secondaryCanvas.MouseMove += ResizeAngles;
            secondaryCanvas.MouseUp += StopResizeShape;
            secondaryCanvas.Children.Add(secondaryRectCanvas);
            GlobalProperties.MainCanvas.Children.Add(secondaryCanvas);
            Canvas.SetLeft(secondaryCanvas, 0);
            Panel.SetZIndex(secondaryCanvas, 99);
            Canvas.SetTop(secondaryCanvas, 0);
        }

        private void ResizeAngles(object sender, MouseEventArgs e)
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
                var angleName =
                    GlobalProperties.selectedShape.AnglesBorder.Keys.First(
                        k => Equals(GlobalProperties.selectedShape.AnglesBorder[k], GlobalProperties.selectedAngle));
                var offset = GetOffset(e);
                if (GlobalProperties.selectedShape.Width + sizeConsts[GlobalProperties.selectedAngle][0] * offset.X > GlobalProperties.MinShapeSize &&
                    GlobalProperties.selectedShape.Height + sizeConsts[GlobalProperties.selectedAngle][1] * offset.Y > GlobalProperties.MinShapeSize)
                {
                    Canvas.SetLeft(GlobalProperties.selectedAngle, Canvas.GetLeft(GlobalProperties.selectedAngle) + offset.X);
                    Canvas.SetTop(GlobalProperties.selectedAngle, Canvas.GetTop(GlobalProperties.selectedAngle) + offset.Y);
                    GlobalProperties.selectedShape.startPoint =
                        new Point(GlobalProperties.selectedShape.startPoint.X + pointsConsts[GlobalProperties.selectedAngle][0] * offset.X,
                            GlobalProperties.selectedShape.startPoint.Y + pointsConsts[GlobalProperties.selectedAngle][1] * offset.Y);
                    GlobalProperties.selectedShape.finishPoint =
                        new Point(GlobalProperties.selectedShape.finishPoint.X + pointsConsts[GlobalProperties.selectedAngle][2] * offset.X,
                            GlobalProperties.selectedShape.finishPoint.Y + pointsConsts[GlobalProperties.selectedAngle][3] * offset.Y);
                    RecreateShape();
                    GlobalProperties.selectedAngle = GlobalProperties.selectedShape.AnglesBorder[angleName];
                    GlobalProperties.selectedAnglePoint = new Point(e.GetPosition(GlobalProperties.MainCanvas).X, e.GetPosition(GlobalProperties.MainCanvas).Y);
                }
            }
        }

        public override Shapes RecreateShape()
        {
            var type = GlobalProperties.selectedShape.GetType();
            GlobalProperties.currentShape = CommonMethods.creatorsShapes[type];
            GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape);
            CommonMethods.RemoveSelection(GlobalProperties.selectedShape);
            Shapes shape = GlobalProperties.currentShape.Create("Default",
                GlobalProperties.selectedShape.startPoint, GlobalProperties.selectedShape.finishPoint,
                GlobalProperties.selectedShape.ColorFill, GlobalProperties.selectedShape.ColorStroke,
                GlobalProperties.selectedShape.ThicknessBorder);
            shape.Draw();
            SetNewShapeProperties(shape);
            return shape;
        }

        private void SetNewShapeProperties(Shapes shape)
        {
            shape.Selection = GetFocusFrame(shape, GlobalProperties.frameSize);
            shape.AnglesBorder = GetFocusAngles(shape, GlobalProperties.frameSize);
            SetAnglesAction(shape);
            shape.dragPoint = new Point(double.NaN, double.NaN);
            GlobalProperties.ShapesList.AllShapes.Remove(GlobalProperties.selectedShape);
            GlobalProperties.selectedShape = shape;
            GlobalProperties.ShapesList.AllShapes.Add(shape);
        }

        private static Point GetOffset(MouseEventArgs e)
        {
            var currentMousePosition = e.GetPosition(GlobalProperties.MainCanvas);
            var offset = new Point(currentMousePosition.X - GlobalProperties.selectedAnglePoint.X,
                currentMousePosition.Y - GlobalProperties.selectedAnglePoint.Y);
            return offset;
        }
    }

    internal class TriangleModuleCreator : ICreator
    {
        public Shapes Create(string name,
            Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double thicknessBorder)
        {
            var start = new Point(Math.Min(startPoint.X, finishPoint.X), Math.Min(startPoint.Y, finishPoint.Y));
            var finish = new Point(Math.Max(startPoint.X, finishPoint.X), Math.Max(startPoint.Y, finishPoint.Y));
            return new TrianglesModule(name, start, finish, colorFill, colorStroke, thicknessBorder);
        }

        public override string ToString()
        {
            return "Triangles";
        }
    }
}
