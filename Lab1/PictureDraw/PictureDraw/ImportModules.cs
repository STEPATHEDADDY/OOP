using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace PictureDraw
{
    class ImportModules
    {
        private string rootPath { get; set; }
        public List<Assembly> modules { get; set; }


        public Dictionary<string, ICreator> getCreatorsNames(MainWindow mainInstance, List<ICreator> creators)
        {
            Dictionary<string, ICreator> result = new Dictionary<string, ICreator>();
            var topMargin = 10.0;
            foreach (var creator in creators)
            {
                var name = creator.ToString();
                result[name] = creator;
                Button myButton = new Button
                {
                    Name = name,
                    Content = name,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(10, topMargin, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 112,
                    Foreground = Brushes.White,
                    BorderBrush = Brushes.White,
                    Background = new SolidColorBrush(Color.FromRgb(11, 0, 41))                    
                };
                myButton.Click += mainInstance.buttonShape_Click;
                GlobalProperties.MainGrid.Children.Add(myButton);
                topMargin += 24;
            }
            return result;
        }

        public Dictionary<Type, ICreator> getCreatorsShapes()
        {
            Dictionary<Type, ICreator> result = new Dictionary<Type, ICreator>();
            foreach (var module in modules)
            {
                Type shapeType = null;
                Type shapeCreatorType = null;
                foreach (var type in module.GetTypes())
                {
                    if (typeof(Shapes).IsAssignableFrom(type))
                    {
                        shapeType = type;
                    }
                    if (typeof(ICreator).IsAssignableFrom(type))
                    {
                        shapeCreatorType = type;
                    }
                }
                if (shapeType != null && shapeCreatorType != null)
                {
                    result[shapeType] = Activator.CreateInstance(shapeCreatorType) as ICreator;
                }
            }
            return result;
        }

        public ImportModules()
        {
//            TODO: WORK PATH BELOW
            rootPath = @"D:\OOP\OOP\Lab1\PictureDraw\Modules";
            modules = new List<Assembly>();            
            var directory = new DirectoryInfo(rootPath);

            foreach (var file in directory.GetFiles("*.dll"))
            {
                modules.Add(Assembly.LoadFrom(file.FullName));
            }         
        }
    }
}
