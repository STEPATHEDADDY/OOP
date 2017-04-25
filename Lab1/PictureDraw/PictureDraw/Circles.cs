using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;
using YAXLib;

namespace PictureDraw
{    
    public class Circles : Shapes, ISelectable, IMovable, IResizable, IEditable
    {
        [YAXSerializableField]
        public double Radius { get; set; }

        public Circles() { }

        public override void AfterDesirialization()
        {
            MouseDown += SelectShape;
            MouseDown += SetDragPoint;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //TODO : THICKNESS
            drawingContext.DrawEllipse(new SolidColorBrush(ColorFill),
                new Pen(new SolidColorBrush(ColorStroke), ThicknessBorder),
                new Point(Radius, Radius), Radius, Radius);            
        }

        public Circles(string name, Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double ThicknessBorder) : base(
                name, colorFill, colorStroke, ThicknessBorder)
        {
            //finish not initialize             
            this.startPoint = new Point(Math.Min(startPoint.X, finishPoint.X), Math.Min(startPoint.Y, finishPoint.Y));
            this.finishPoint = new Point(Math.Max(startPoint.X, finishPoint.X), Math.Max(startPoint.Y, finishPoint.Y));
            if (finishPoint.X < startPoint.X && finishPoint.Y < startPoint.Y) //LEFTTOP
            {
                if (this.finishPoint.X - this.startPoint.X > this.finishPoint.Y - this.startPoint.Y)
                {
                    this.startPoint = new Point(this.finishPoint.X - this.finishPoint.Y + this.startPoint.Y,
                        this.startPoint.Y);
                }
                else
                {
                    this.startPoint = new Point(this.startPoint.X,
                        this.finishPoint.Y - this.finishPoint.X + this.startPoint.X);
                }
            }
            if (finishPoint.X > startPoint.X && finishPoint.Y < startPoint.Y) //RIGHTTOP
            {
                if (this.finishPoint.X - this.startPoint.X > this.finishPoint.Y - this.startPoint.Y)
                {
                    this.finishPoint = new Point(this.startPoint.X + this.finishPoint.Y - this.startPoint.Y,
                        this.finishPoint.Y);
                }
                else
                {
                    this.startPoint = new Point(this.startPoint.X,
                        this.finishPoint.Y - this.finishPoint.X + this.startPoint.X);
                }
            }
            if (finishPoint.X < startPoint.X && finishPoint.Y > startPoint.Y) //LEFTBOT
            {
                if (this.finishPoint.X - this.startPoint.X > this.finishPoint.Y - this.startPoint.Y)
                {
                    this.startPoint = new Point(this.finishPoint.X - this.finishPoint.Y + this.startPoint.Y,
                        this.startPoint.Y);
                }
                else
                {
                    this.finishPoint = new Point(this.finishPoint.X,
                        this.finishPoint.Y + this.finishPoint.X - this.startPoint.X);
                }
            }
            Width = this.finishPoint.X - this.startPoint.X;
            Height = this.finishPoint.Y - this.startPoint.Y;
            Radius = Width < Height ? Width / 2 : Height / 2;
            Width = Radius*2;
            Height = Radius*2;        
            MouseDown += SelectShape;            
            MouseDown += SetDragPoint;
            MouseDown += ShowProperties;
//            MouseMove += MovingShape;
//            MouseUp += StopMovingShape;
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
                var circle = (Circles)sender;
                if (GlobalProperties.selectedShape != null)
                {
                    RemoveSelection(GlobalProperties.selectedShape);
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

        private Rectangle GetFocusFrame(Shapes circle, double frameSize)
        {
            Rectangle focus = new Rectangle();
            focus.Stroke = new SolidColorBrush(Colors.SlateBlue);
            focus.StrokeDashArray = new DoubleCollection(new List<double> { 5, 1 });
            focus.StrokeThickness = 2.0;
            focus.Width = circle.Width + frameSize;
            focus.Height = circle.Height + frameSize;
            GlobalProperties.MainCanvas.Children.Add(focus);
            Canvas.SetLeft(focus, circle.startPoint.X - frameSize / 2);
            Canvas.SetTop(focus, circle.startPoint.Y - frameSize / 2);
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
            var rect = (Circles)sender;
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
                //TODO : CAN REWRITE WITHOUT GLOBAL VARIABLE
                if (e.LeftButton == MouseButtonState.Pressed && CommonMethods.CheckType(GlobalProperties.selectedShape, typeof(Circles)))
                {
                    var circle = (Circles)GlobalProperties.selectedShape;
                    GlobalProperties.selectedShape.Opacity = 0.7;
                    if (!Double.IsNaN(circle.dragPoint.X))
                    {
                        var currentMousePosition = e.GetPosition(GlobalProperties.MainCanvas);
                        var offset = new Point(circle.startPoint.X + (currentMousePosition.X - circle.dragPoint.X),
                            circle.startPoint.Y + (currentMousePosition.Y - circle.dragPoint.Y));
                        ChangePosition(offset, circle, GlobalProperties.frameSize, currentMousePosition);
                    }
                }
            }
        }

        private static void ChangePosition(Point offset, Circles shape, double frameSize, Point mousePosition)
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

        public void StopMovingShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                GlobalProperties.selectedShape.Opacity = 1;
                GlobalProperties.selectedShape.finishPoint = new Point(GlobalProperties.selectedShape.startPoint.X +
                        GlobalProperties.selectedShape.Width, GlobalProperties.selectedShape.startPoint.Y + GlobalProperties.selectedShape.Height);
                GlobalProperties.SecondaryCanvas.Children.Remove(GlobalProperties.ResizeCanvas);
                GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.SecondaryCanvas);
//                GlobalProperties.ShapesList.AllShapes.Remove(GlobalProperties.selectedShape);
//                GlobalProperties.ShapesList.AllShapes.Add(GlobalProperties.selectedShape);
            }
        }

        public void SetAnglesAction(Shapes circle)
        {
            foreach (var angle in circle.AnglesBorder.Values)
            {
                angle.MouseDown += SetResizeAngle;
//                angle.MouseUp += StopResizeShape;
            }
//            circle.AnglesBorder["leftTop"].MouseMove += ResizeAngles;
//            //TODO : MAKE ALL EVENTS
//            circle.AnglesBorder["rightTop"].MouseMove += ResizeAngles;
//            circle.AnglesBorder["rightBottom"].MouseMove += ResizeAngles;
//            circle.AnglesBorder["leftBottom"].MouseMove += ResizeAngles;
        }

        public void SetResizeAngle(object sender, MouseEventArgs e)
        {
            GlobalProperties.selectedAnglePoint = e.GetPosition(GlobalProperties.MainCanvas);
            var angle = (Rectangle) sender;
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
            //            GlobalProperties.ResizeCanvas = new Rectangle { Width = angle.Width * 2, Height = angle.Height * 2, Fill = Brushes.Aqua };
            //            GlobalProperties.MainCanvas.Children.Add(GlobalProperties.ResizeCanvas);
            //            Canvas.SetLeft(GlobalProperties.ResizeCanvas, Canvas.GetLeft(angle));
            //            Canvas.SetTop(GlobalProperties.ResizeCanvas, Canvas.GetTop(angle));
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
                Point start = new Point(), finish = new Point();               
                if (GlobalProperties.selectedShape.Width + sizeConsts[angle][0] * offset.X > GlobalProperties.MinShapeSize &&
                    GlobalProperties.selectedShape.Height + sizeConsts[angle][1] * offset.Y > GlobalProperties.MinShapeSize)
                {
                    var anglePosition = GlobalProperties.selectedShape.AnglesBorder.First(f => Equals(f.Value, angle));
                    if (Equals(anglePosition.Value, GlobalProperties.selectedShape.AnglesBorder["rightBottom"]))
                    {
                        start =
                            new Point(GlobalProperties.selectedShape.startPoint.X,
                                GlobalProperties.selectedShape.startPoint.Y);
                        finish =
                            new Point(GlobalProperties.selectedShape.finishPoint.X + offset.X,
                                GlobalProperties.selectedShape.finishPoint.Y + offset.Y);
                    }
                    if (Equals(anglePosition.Value, GlobalProperties.selectedShape.AnglesBorder["rightTop"]))
                    {
                        start =
                            new Point(GlobalProperties.selectedShape.startPoint.X,
                                GlobalProperties.selectedShape.finishPoint.Y);
                        finish =
                            new Point(GlobalProperties.selectedShape.finishPoint.X + offset.X,
                                GlobalProperties.selectedShape.startPoint.Y + offset.Y);
                    }
                    //TODO : NEED TO ADD ALL SITUATIONS
                    if (Equals(anglePosition.Value, GlobalProperties.selectedShape.AnglesBorder["leftTop"]))
                    {
                        start =
                            new Point(GlobalProperties.selectedShape.finishPoint.X,
                                GlobalProperties.selectedShape.finishPoint.Y);
                        finish =
                            new Point(GlobalProperties.selectedShape.startPoint.X + offset.X,
                                GlobalProperties.selectedShape.startPoint.Y + offset.Y);
                    }
                    if (Equals(anglePosition.Value, GlobalProperties.selectedShape.AnglesBorder["leftBottom"]))
                    {
                        start =
                            new Point(GlobalProperties.selectedShape.finishPoint.X,
                                GlobalProperties.selectedShape.startPoint.Y);
                        finish =
                            new Point(GlobalProperties.selectedShape.startPoint.X + offset.X,
                                GlobalProperties.selectedShape.finishPoint.Y + offset.Y);
                    }
                    Debug.WriteLine($"{start} {finish}");                 
                    RecreateShape(e, start, finish);
                    GlobalProperties.selectedAngle = GlobalProperties.selectedShape.AnglesBorder[angleName];
                }
            }
        }

        private void RecreateShape(MouseEventArgs e, Point start, Point finish)
        {            
            var type = GlobalProperties.selectedShape.GetType().Name;
            GlobalProperties.currentShape = CommonMethods.creators[type];
            GlobalProperties.MainCanvas.Children.Remove(GlobalProperties.selectedShape);
            RemoveSelection(GlobalProperties.selectedShape);                       
            Shapes shape = GlobalProperties.currentShape.FactoryMethod("Default", start, finish,
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
            GlobalProperties.selectedAngle = null;
            GlobalProperties.selectedShape.finishPoint = new Point(GlobalProperties.selectedShape.startPoint.X + GlobalProperties.selectedShape.Width, 
                GlobalProperties.selectedShape.startPoint.Y + GlobalProperties.selectedShape.Height);
        }

        public void ShowProperties(object sender, MouseEventArgs e)
        {
            var rect = (Circles)sender;
            GlobalProperties.PropertiesPanel.Visibility = Visibility.Visible;
            GlobalProperties.FillSelected.SelectedColor = rect.ColorFill;
            GlobalProperties.BorderSelected.SelectedColor = rect.ColorStroke;
        }
    }
    
    class CircleCreator : ICreator
    {
        public Shapes FactoryMethod(string Name,
            Point startPoint, Point finishPoint, Color colorFill, Color colorStroke, double ThicknessBorder)
        {
            return new Circles(Name, startPoint, finishPoint, colorFill, colorStroke, ThicknessBorder);
        }
    }
}
