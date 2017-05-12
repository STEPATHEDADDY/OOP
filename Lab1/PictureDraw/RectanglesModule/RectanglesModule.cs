using System;
using System.Collections.Generic;
using System.Linq;
using PictureDraw;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;


namespace RectanglesModule
{
    internal class RectanglesModule : Shapes, ISelectable, IMovable, IResizable, IEditable
    {
        public RectanglesModule() { }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(new SolidColorBrush(ColorFill),
                new Pen(new SolidColorBrush(ColorStroke), ThicknessBorder),
                new Rect(0, 0, Width, Height));
        }

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

        public RectanglesModule(string name, Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double thicknessBorder) : base(
                name, colorFill, colorStroke, thicknessBorder)
        {
            this.startPoint = startPoint;
            this.finishPoint = finishPoint;
            Width = this.finishPoint.X - this.startPoint.X;
            Height = this.finishPoint.Y - this.startPoint.Y;
            SetEvents();
        }

        public void SelectShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                var rect = (RectanglesModule)sender;
                if (GlobalProperties.selectedShape != null)
                {
                    CommonMethods.RemoveSelection(GlobalProperties.selectedShape);
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
        
        public void SetDragPoint(object sender, MouseEventArgs e)
        {
            var rect = (RectanglesModule)sender;
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
                Panel.SetZIndex(secondaryCanvas, 99);
                Canvas.SetTop(secondaryCanvas, 0);
            }
        }
        
        public void MovingShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                if (e.LeftButton == MouseButtonState.Pressed && CommonMethods.CheckType(GlobalProperties.selectedShape, typeof(RectanglesModule)))
                {
                    var rect = (RectanglesModule)GlobalProperties.selectedShape;
                    GlobalProperties.selectedShape.Opacity = GlobalProperties.Opacity;
                    if (!double.IsNaN(rect.dragPoint.X))
                    {
                        var currentMousePosition = e.GetPosition(GlobalProperties.MainCanvas);
                        var offset = new Point(rect.startPoint.X + (currentMousePosition.X - rect.dragPoint.X),
                            rect.startPoint.Y + (currentMousePosition.Y - rect.dragPoint.Y));
                        ChangePosition(offset, rect, GlobalProperties.frameSize, currentMousePosition);                        
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
            GlobalProperties.selectedAngle = (Rectangle)sender;
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
            shape.dragPoint = new Point(double.NaN, Double.NaN);
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

    internal class RectanglesModuleCreator : ICreator
    {
        public Shapes Create(string name,
            Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double thicknessBorder)
        {
            var start = new Point(Math.Min(startPoint.X, finishPoint.X), Math.Min(startPoint.Y, finishPoint.Y));
            var finish = new Point(Math.Max(startPoint.X, finishPoint.X), Math.Max(startPoint.Y, finishPoint.Y));
            return new RectanglesModule(name, start, finish, colorFill, colorStroke, thicknessBorder);
        }

        public override string ToString()
        {
            return "Rectangles";
        }
    }
}
