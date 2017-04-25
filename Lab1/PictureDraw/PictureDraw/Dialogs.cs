using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Microsoft.Win32;
using YAXLib;

namespace PictureDraw
{
    public static class Dialogs
    {
//        static XmlSerializer formatter = new XmlSerializer(typeof(List<Shapes>));
                    
//        public static List<Shapes> OpenFile()
//        {
//            OpenFileDialog openFileDialog = new OpenFileDialog();
//            List<Shapes> ListShapes = null;
//            if (openFileDialog.ShowDialog() == true)
//            {
//                GlobalProperties.MainCanvas.Children.RemoveRange(0, GlobalProperties.MainCanvas.Children.Count - 1);
//                GlobalProperties.MainCanvas.Children.Clear();
//                Rectangle rectCanvas = new Rectangle();
//                rectCanvas.StrokeThickness = 2;
//                rectCanvas.Fill = new SolidColorBrush(Color.FromArgb(100, 0xFF, 0xFF, 0xFF));
//                rectCanvas.Width = GlobalProperties.MainCanvas.ActualWidth;
//                rectCanvas.Height = GlobalProperties.MainCanvas.ActualHeight;
//                GlobalProperties.MainCanvas.Children.Add(rectCanvas);
//                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
//                {
//                   ListShapes = (List<Shapes>)formatter.Deserialize(fs);
//                }
//                foreach (var shape in ListShapes)
//                {
//                    shape.Draw();
//                }
//            }
//            return ListShapes;
//        }

        public static void SaveFile(List<Shapes> listShapes)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {                
                YAXSerializer serializer = new YAXSerializer(typeof(Shapes));
                string xmlResult = String.Empty;
                foreach (var shape in listShapes)
                {
                    xmlResult += serializer.Serialize(shape);
                    //                    Debug.WriteLine(xmlResult);
                }
                System.IO.File.WriteAllText(saveFileDialog.FileName, xmlResult);
                //                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                //                {
                //                    formatter.Serialize(fs, listShapes);
                //                }
            }
        }
    }
}
