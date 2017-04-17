using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureDraw
{
    static class CommonMethods
    {
        public static Dictionary<string, ICreator> creators = new Dictionary<string, ICreator>
            {
                { "Circles", new CircleCreator() },
//                { "Line", new LineCreator() },
                { "Rectangles", new RectangleCreator() },
//                { "Square", new SquareCreator() },
//                { "Tetragon", new TetragonCreator() },
                { "Triangles", new TriangleCreator() },
            };

        public static bool CheckType(Shapes instance, Type possibleType)
        {            
            var isInstanceType = possibleType.IsInstanceOfType(instance);
            return isInstanceType;
        }
    }
}
