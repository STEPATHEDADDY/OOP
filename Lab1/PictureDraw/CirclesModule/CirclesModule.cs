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

namespace CirclesModule
{
    public class CirclesModule: Shapes, ISelectable, IMovable, IResizable
    {        
        [YAXSerializableField]
        public double Radius { get; set; }

        public CirclesModule() { }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawEllipse(new SolidColorBrush(ColorFill),
                new Pen(new SolidColorBrush(ColorStroke), ThicknessBorder),
                new Point(Radius, Radius), Radius, Radius);
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

        public CirclesModule(string name, Point startPoint, Point finishPoint, double radius, Color colorFill, Color colorStroke, double thicknessBorder) : base(
                name, colorFill, colorStroke, thicknessBorder)
        {
            this.startPoint = startPoint;
            this.finishPoint = finishPoint;
            Radius = radius;
            Width = radius * 2;
            Height = radius * 2;
            SetEvents();
        }

        public void SelectShape(object sender, MouseEventArgs e)
        {
            if (GlobalProperties.DrawModeOn) return;
            var circle = (Shapes)sender;
            if (GlobalProperties.selectedShape != null)
            {
                CommonMethods.RemoveSelection(GlobalProperties.selectedShape);
            }
            circle.Selection = GetFocusFrame(circle, GlobalProperties.frameSize);
            GlobalProperties.selectedShape = circle;
            GlobalProperties.drawShape = circle;
            circle.AnglesBorder = GetFocusAngles(circle, GlobalProperties.frameSize);
            if (CommonMethods.CheckType(circle, typeof(IResizable)))
            {
                SetAnglesAction(circle);
            }
        }

        public void SetDragPoint(object sender, MouseEventArgs e)
        {
            var rect = (CirclesModule)sender;
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
                if (e.LeftButton == MouseButtonState.Pressed && CommonMethods.CheckType(GlobalProperties.selectedShape, typeof(CirclesModule)))
                {
                    var circle = (CirclesModule)GlobalProperties.selectedShape;
                    GlobalProperties.selectedShape.Opacity = GlobalProperties.Opacity;
                    if (!double.IsNaN(circle.dragPoint.X))
                    {
                        var currentMousePosition = e.GetPosition(GlobalProperties.MainCanvas);
                        var offset = new Point(circle.startPoint.X + (currentMousePosition.X - circle.dragPoint.X),
                            circle.startPoint.Y + (currentMousePosition.Y - circle.dragPoint.Y));
                        ChangePosition(offset, circle, GlobalProperties.frameSize, currentMousePosition);
                    }
                }
            }
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
                Dictionary<Rectangle, int[]> sizeConsts = new Dictionary<Rectangle, int[]>
                {
                    {GlobalProperties.selectedShape.AnglesBorder["leftTop"], new []{-1, -1} },
                    {GlobalProperties.selectedShape.AnglesBorder["rightTop"], new []{1, -1} },
                    {GlobalProperties.selectedShape.AnglesBorder["rightBottom"], new []{1, 1} },
                    {GlobalProperties.selectedShape.AnglesBorder["leftBottom"], new []{-1, 1} }
                };
                var angleName =
                    GlobalProperties.selectedShape.AnglesBorder.Keys.First(
                        k => Equals(GlobalProperties.selectedShape.AnglesBorder[k], GlobalProperties.selectedAngle));
                var offset = e.GetPosition(GlobalProperties.MainCanvas);
                if ((GlobalProperties.selectedShape.Width + sizeConsts[GlobalProperties.selectedAngle][0] *
                    (offset.X - GlobalProperties.selectedShape.finishPoint.X) > GlobalProperties.MinShapeSize) &&
                   (GlobalProperties.selectedShape.Height + sizeConsts[GlobalProperties.selectedAngle][1] *
                   (offset.Y - GlobalProperties.selectedShape.finishPoint.Y) > GlobalProperties.MinShapeSize))
                {
                    var anglePosition = GlobalProperties.selectedShape.AnglesBorder.First(f => Equals(f.Value, GlobalProperties.selectedAngle));
                    if (Equals(anglePosition.Value, GlobalProperties.selectedShape.AnglesBorder["rightBottom"]))
                    {
                        GlobalProperties.selectedShape.startPoint =
                            new Point(GlobalProperties.selectedShape.startPoint.X,
                                GlobalProperties.selectedShape.startPoint.Y);
                    }
                    if (Equals(anglePosition.Value, GlobalProperties.selectedShape.AnglesBorder["rightTop"]))
                    {
                        GlobalProperties.selectedShape.startPoint =
                            new Point(GlobalProperties.selectedShape.startPoint.X,
                                GlobalProperties.selectedShape.finishPoint.Y);
                    }
                    if (Equals(anglePosition.Value, GlobalProperties.selectedShape.AnglesBorder["leftTop"]))
                    {
                        GlobalProperties.selectedShape.startPoint =
                            new Point(GlobalProperties.selectedShape.finishPoint.X,
                                GlobalProperties.selectedShape.finishPoint.Y);
                    }
                    if (Equals(anglePosition.Value, GlobalProperties.selectedShape.AnglesBorder["leftBottom"]))
                    {
                        GlobalProperties.selectedShape.startPoint =
                            new Point(GlobalProperties.selectedShape.finishPoint.X,
                                GlobalProperties.selectedShape.startPoint.Y);
                    }
                    GlobalProperties.selectedShape.finishPoint =
                        new Point(offset.X, offset.Y);
                    RecreateShape();
                    GlobalProperties.selectedAngle = GlobalProperties.selectedShape.AnglesBorder[angleName];
                }
            }
        }

        public override Shapes RecreateShape()
        {
            var type = GlobalProperties.selectedShape.GetType();
            GlobalProperties.currentShape = CommonMethods.creatorsShapes[type];
            GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape);
            CommonMethods.RemoveSelection(GlobalProperties.selectedShape);
            Shapes shape = GlobalProperties.currentShape.Create("Default", GlobalProperties.selectedShape.startPoint, GlobalProperties.selectedShape.finishPoint,
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
    }

    internal class CirclesModuleCreator : ICreator
    {
        public Shapes Create(string name,
            Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double thicknessBorder)
        {
            var width = Math.Abs(startPoint.X - finishPoint.X);
            var height = Math.Abs(startPoint.Y - finishPoint.Y);
            var radius = width < height ? width / 2 : height / 2;
            width = radius * 2;
            height = radius * 2;
            var start = new Point();
            if (finishPoint.X < startPoint.X && finishPoint.Y < startPoint.Y) //LEFTTOP
            {
                start = new Point(startPoint.X - width, startPoint.Y - height);
            }
            if (finishPoint.X > startPoint.X && finishPoint.Y < startPoint.Y) //RIGHTTOP
            {
                start = new Point(startPoint.X, startPoint.Y - height);
            }
            if (finishPoint.X < startPoint.X && finishPoint.Y > startPoint.Y) //LEFTBOT
            {
                start = new Point(startPoint.X - width, startPoint.Y);
            }
            if (finishPoint.X > startPoint.X && finishPoint.Y > startPoint.Y) //RIGHTBOT
            {
                start = startPoint;
            }
            var finish = new Point(start.X + width, start.Y + height);
            return new CirclesModule(name, start, finish, radius, colorFill, colorStroke, thicknessBorder);
        }

        public override string ToString()
        {
            return "Circles";
        }
    }
}
