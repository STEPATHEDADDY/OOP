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
using System.Xml;
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
            ListShapes allShapes = new ListShapes();
            if (openFileDialog.ShowDialog() == true)
            {                
                YAXSerializer serializer = new YAXSerializer(typeof(Shapes));                               
                StringReader sr = new StringReader(openFileDialog.FileName);                
                XmlReaderSettings set = new XmlReaderSettings();
                set.IgnoreWhitespace = true;
                XmlDocument doc = new XmlDocument();                
                var xtr = new XmlTextReader(sr.ReadToEnd());
                var lol = XmlReader.Create(xtr, set);
                lol.ReadToFollowing("Shapes");
                try
                {
                    while (lol.NodeType == XmlNodeType.Element && lol.Value != "AllShapes")
                    {
                        var l = lol.GetAttribute("yaxlib:realtype");
                        XmlReader temp = new XmlNodeReader(doc.ReadNode(lol));
                        if (CommonMethods.creatorsShapes.Keys.Any(p => p.FullName == l))
                        {
                            temp.Read();
                            var f = (Shapes) serializer.Deserialize(temp);
                            allShapes.AddShape(f);
                        }
                    }
                    if (allShapes.AllShapes.Count > 0)
                    {
                        GlobalProperties.MainCanvas.Children.RemoveRange(1,
                            GlobalProperties.MainCanvas.Children.Count - 1);
                        foreach (var shape in allShapes.AllShapes)
                        {
                            shape.SetEvents();
                            shape.Draw();
                        }
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Sorry, but your XML document had some troubles with data. Please, fix it.");
                }
                catch (XmlException)
                {
                    MessageBox.Show("Sorry, but your XML document was damaged. Please, fix it.");
                }
                finally
                {
                    lol.Close();
                    sr.Close();
                }
            }
                return allShapes;
        }
    }
}
