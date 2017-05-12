using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using YAXLib;

namespace PictureDraw
{       
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]    
    public abstract class Shapes : UIElement
    {
        [YAXSerializableField]
        public string Name { get; set; }
        [YAXSerializableField]    
        public Color ColorFill { get; set; }
        [YAXSerializableField]
        public Color ColorStroke { get; set; }  
        [YAXSerializableField]
        public Point startPoint { get; set; }
        [YAXSerializableField]
        public Point finishPoint { get; set; }        
        [YAXSerializableField]
        public double Width { get; set; }
        [YAXSerializableField]
        public double Height { get; set; }
        public UIElement Selection { get; set; }
        public Dictionary<string, Rectangle> AnglesBorder { get; set; }
        public Point dragPoint { get; set; }   
        [YAXSerializableField]
        public double ThicknessBorder { get; set; }               

        public Shapes() { }

        public Shapes(string Name, Color ColorFill, Color ColorStroke, double ThicknessBorder)
        {                        
            this.ColorFill = ColorFill;            
            this.ColorStroke = ColorStroke;
            this.Name = Name;
            this.ThicknessBorder = ThicknessBorder;
            Selection = null;
        }
        
//        public abstract void Draw();
        public abstract void SetEvents();
        public abstract Shapes RecreateShape();

        public void Draw()
        {
            Canvas.SetLeft(this, startPoint.X);
            Canvas.SetTop(this, startPoint.Y);
            GlobalProperties.MainCanvas.Children.Add(this);
        }


        public Rectangle GetFocusFrame(Shapes circle, double frameSize)
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

        public Rectangle GetAngle(Point position, double size)
        {
            var angle = new Rectangle { Width = size, Height = size, Fill = Brushes.Black };
            GlobalProperties.MainCanvas.Children.Add(angle);
            Canvas.SetLeft(angle, position.X);
            Canvas.SetTop(angle, position.Y);
            return angle;
        }

        public void StopMovingShape(object sender, MouseEventArgs e)
        {
            if (!GlobalProperties.DrawModeOn)
            {
                var secondaryCanvas = (Canvas)sender;
                GlobalProperties.selectedShape.Opacity = 1;
                GlobalProperties.selectedShape.finishPoint = new Point(GlobalProperties.selectedShape.startPoint.X +
                        GlobalProperties.selectedShape.Width, GlobalProperties.selectedShape.startPoint.Y + GlobalProperties.selectedShape.Height);
                GlobalProperties.MainCanvas.Children.Remove(secondaryCanvas);
            }
        }

        public static void ChangePosition(Point offset, Shapes shape, double frameSize, Point mousePosition)
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

        public void StopResizeShape(object sender, MouseEventArgs e)
        {
            var secondaryCanvas = (Canvas)sender;
            GlobalProperties.selectedShape.Opacity = 1;
            GlobalProperties.selectedShape.finishPoint = new Point(GlobalProperties.selectedShape.startPoint.X +
                    GlobalProperties.selectedShape.Width, GlobalProperties.selectedShape.startPoint.Y + GlobalProperties.selectedShape.Height);
            GlobalProperties.MainCanvas.Children.Remove(secondaryCanvas);
        }

        public void ShowProperties(object sender, MouseEventArgs e)
        {
            var rect = (Shapes)sender;
            GlobalProperties.PropertiesPanel.Visibility = Visibility.Visible;
            GlobalProperties.FillSelected.SelectedColor = rect.ColorFill;
            GlobalProperties.BorderSelected.SelectedColor = rect.ColorStroke;
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
    }

    public interface ICreator
    {
        Shapes Create(string name,
            Point startPoint, Point finisgPoint, Color colorFill, Color colorStroke, double thicknessBorder);
    }
}
