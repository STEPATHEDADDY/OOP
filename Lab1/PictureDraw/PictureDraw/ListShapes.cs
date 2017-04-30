using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace PictureDraw
{
    public class ListShapes
    {
        public List<Shapes> AllShapes { get; set; }
        public ListShapes()
        {
            AllShapes = new List<Shapes>();
        }
        
        [OnSerializing]        
        void Temp(Object sender)
        {
            Debug.WriteLine("OK");
        }

        public void AddShape(Shapes shape)
        {
            AllShapes.Add(shape);
        }
    }
}
