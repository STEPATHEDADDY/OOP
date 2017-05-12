using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using PictureDraw;

namespace LinesModule
{
    public class LinesModule : Shapes, ISelectable, IMovable, IResizable, IEditable
    {
        public LinesModule() { }

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
            drawingContext.DrawLine(new Pen(new SolidColorBrush(ColorStroke), ThicknessBorder),
                new Point(0, 0), new Point(finishPoint.X - startPoint.X, finishPoint.Y - startPoint.Y));
        }

        public LinesModule(string name, Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double thicknessBorder) : base(
                name, colorFill, colorStroke, thicknessBorder)
        {
            this.startPoint = startPoint;
            this.finishPoint = finishPoint;
            Width = Math.Abs(this.finishPoint.X - this.startPoint.X);
            Height = Math.Abs(this.finishPoint.Y - this.startPoint.Y);
            SetEvents();
        }

        public void SelectShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                var rect = (Shapes)sender;
                if (GlobalProperties.selectedShape != null)
                {
                    CommonMethods.RemoveSelection(GlobalProperties.selectedShape);
                }
                GlobalProperties.selectedShape = rect;
                GlobalProperties.drawShape = rect;
                rect.AnglesBorder = GetFocusAngles(rect);
                if (CommonMethods.CheckType(rect, typeof(IResizable)))
                {
                    SetAnglesAction(rect);
                }
            }
        }

        private Dictionary<string, Rectangle> GetFocusAngles(Shapes shape)
        {
            const double SIZE_ANGLES = 6;
            var firstPoint = new Point(shape.startPoint.X, shape.startPoint.Y);
            var secondPoint = new Point(shape.finishPoint.X, shape.finishPoint.Y);
            var firstAngle = GetAngle(firstPoint, SIZE_ANGLES);
            var secondAngle = GetAngle(secondPoint, SIZE_ANGLES);
            var result = new Dictionary<string, Rectangle>
            {
                {"first", firstAngle },
                {"second", secondAngle }
            };
            return result;
        }

        public void SetDragPoint(object sender, MouseEventArgs e)
        {
            var rect = (LinesModule)sender;
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
                if (e.LeftButton == MouseButtonState.Pressed && CommonMethods.CheckType(GlobalProperties.selectedShape, typeof(LinesModule)))
                {
                    var rect = (LinesModule)GlobalProperties.selectedShape;
                    GlobalProperties.selectedShape.Opacity = GlobalProperties.Opacity;
                    if (!double.IsNaN(rect.dragPoint.X))
                    {
                        var currentMousePosition = e.GetPosition(GlobalProperties.MainCanvas);
                        var offset = new Point((currentMousePosition.X - rect.dragPoint.X),
                            (currentMousePosition.Y - rect.dragPoint.Y));
                        ChangePosition(offset, rect, currentMousePosition);
                    }
                }
            }
        }

        private static void ChangePosition(Point offset, LinesModule shape, Point mousePosition)
        {
            Canvas.SetLeft(shape, shape.startPoint.X + offset.X);
            Canvas.SetTop(shape, shape.startPoint.Y + offset.Y);
            foreach (var angle in shape.AnglesBorder.Values)
            {
                Canvas.SetLeft(angle,
                    Canvas.GetLeft(angle) + (mousePosition.X - shape.dragPoint.X));
                Canvas.SetTop(angle,
                    Canvas.GetTop(angle) + (mousePosition.Y - shape.dragPoint.Y));
            }
            shape.startPoint = new Point(offset.X + shape.startPoint.X, offset.Y + shape.startPoint.Y);
            shape.finishPoint = new Point(offset.X + shape.finishPoint.X, offset.Y + shape.finishPoint.Y);
            shape.dragPoint = new Point(mousePosition.X,
                mousePosition.Y);
        }

        public void SetAnglesAction(Shapes circle)
        {
            foreach (var angle in circle.AnglesBorder.Values)
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
                var angleName =
                    GlobalProperties.selectedShape.AnglesBorder.Keys.First(
                        k => Equals(GlobalProperties.selectedShape.AnglesBorder[k], GlobalProperties.selectedAngle));
                var offset = e.GetPosition(GlobalProperties.MainCanvas);
                if (Equals(GlobalProperties.selectedShape.AnglesBorder["first"], GlobalProperties.selectedAngle))
                {
                    GlobalProperties.selectedShape.startPoint = new Point(offset.X, offset.Y);
                }
                if (Equals(GlobalProperties.selectedShape.AnglesBorder["second"], GlobalProperties.selectedAngle))
                {
                    GlobalProperties.selectedShape.finishPoint = new Point(offset.X, offset.Y);
                }
                RecreateShape();
                GlobalProperties.selectedAngle = GlobalProperties.selectedShape.AnglesBorder[angleName];
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
            shape.AnglesBorder = GetFocusAngles(shape);
            SetAnglesAction(shape);
            shape.dragPoint = new Point(double.NaN, double.NaN);
            GlobalProperties.ShapesList.AllShapes.Remove(GlobalProperties.selectedShape);
            GlobalProperties.selectedShape = shape;
            GlobalProperties.ShapesList.AllShapes.Add(shape);
        }

        public new void StopResizeShape(object sender, MouseEventArgs e)
        {
            var secondaryCanvas = (Canvas)sender;
            GlobalProperties.MainCanvas.Children.Remove(secondaryCanvas);
        }
    }

    internal class LineModuleCreator : ICreator
    {
        public Shapes Create(string name,
            Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double thicknessBorder)
        {
            return new LinesModule(name, startPoint, finishPoint, colorFill, colorStroke, thicknessBorder);
        }

        public override string ToString()
        {
            return "Lines";
        }
    }
}
