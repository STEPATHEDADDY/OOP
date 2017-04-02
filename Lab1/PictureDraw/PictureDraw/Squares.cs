//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Shapes;
//using System.Xml.Serialization;
//
//namespace PictureDraw
//{
//    [XmlInclude(typeof(Shapes))]
//    [Serializable]
//    public class Squares : Shapes
//    {
//        public Squares() { }
//
//        public Squares(string Name,
//            float startX, float startY, float finishX, float finishY): base(
//                Name)
//        {
//            //finish not initialize 
//            this.startX = Math.Min(startX, finishX);
//            this.startY = Math.Min(startY, finishY);
//            this.finishX = Math.Max(startX, finishX);
//            this.finishY = Math.Max(startY, finishY);
//            Width = Math.Abs(startX - finishX);
//            Height = Math.Abs(startY - finishY);
//            Width = Height = Width < Height ? Width : Height;
//        }
//
//        public override void Draw()
//        {
//            var square = new Rectangle();
//            square.Stroke = ColorStroke;
//            square.Fill = ColorFill;
//            square.Width = square.Height = Width;
//            GlobalProperties.MainCanvas.Children.Add(square);
//            Canvas.SetLeft(square, startX);
//            Canvas.SetTop(square, startY);
//        }
//    }
//    
//    class SquareCreator : ICreator
//    {
//        public Shapes FactoryMethod(string Name,
//            float startX, float startY, float finishX, float finishY)
//        {
//            return new Squares(Name, startX, startY, finishX, finishY);
//        }
//    }
//}
