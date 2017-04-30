using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Microsoft.Win32;
using YAXLib;

namespace PictureDraw
{
    public static class Dialogs
    {
        public static void SaveFile(ListShapes listShapes)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {                
                YAXSerializer serializer = new YAXSerializer(typeof(ListShapes));
                string xmlResult = String.Empty;
                xmlResult = serializer.Serialize(listShapes);
                File.WriteAllText(saveFileDialog.FileName, xmlResult);
            }
        }

        public static ListShapes OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ListShapes allShapes = null;
            if (openFileDialog.ShowDialog() == true)
            {                
                YAXSerializer serializer = new YAXSerializer(typeof(ListShapes));
                string xmlResult = File.ReadAllText(openFileDialog.FileName);
                try
                {
                    allShapes = (ListShapes)serializer.Deserialize(xmlResult);
                    GlobalProperties.MainCanvas.Children.RemoveRange(1, GlobalProperties.MainCanvas.Children.Count - 1);
                    foreach (var shape in allShapes.AllShapes)
                    {
                        shape.SetEvents();
                        shape.Draw();
                    }
                }
                catch
                {
                    MessageBox.Show("Sorry, but your XML document was damaged. Please, fix it.");
                }
            }
            return allShapes;
        }
    }
}
