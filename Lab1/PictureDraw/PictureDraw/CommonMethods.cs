using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PictureDraw
{
    public static class CommonMethods
    {        
        private static ImportModules importInstance = new ImportModules();
        public static Dictionary<string, ICreator> creatorsNames = new Dictionary<string, ICreator>();        
        public static Dictionary<Type, ICreator> creatorsShapes = new Dictionary<Type, ICreator>();        

        public static void getCreatorsNames(MainWindow mainInstance)
        {
            creatorsNames = importInstance.getCreatorsNames(mainInstance, ref creatorsShapes);
        }

        public static void getCreatorsShapes()
        {
            creatorsShapes = importInstance.getCreatorsShapes();
        }

        public static bool CheckType(Shapes instance, Type possibleType)
        {            
            var isInstanceType = possibleType.IsInstanceOfType(instance);
            return isInstanceType;
        }


        public static void RemoveSelection(Shapes shape)
        {
            GlobalProperties.MainCanvas.Children.Remove(shape.Selection);
            shape.Selection = null;
            GlobalProperties.PropertiesPanel.Visibility = Visibility.Hidden;
            foreach (var angle in shape.AnglesBorder.Values)
            {
                GlobalProperties.MainCanvas.Children.Remove(angle);
            }
        }
    }    
}
