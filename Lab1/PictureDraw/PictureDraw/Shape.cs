using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PictureDraw
{
    abstract class Shape
    {
        public string Color { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Dictionary<string, int> Size { get; set; }
        public Dictionary<string, int> Coordinates { get; set; }
        public Grid mainGrid { get; set; }

        public Shape(string Color, int Width, int Height, Grid mainGrid)
        {
            Size = new Dictionary<string, int>();
            Coordinates = new Dictionary<string, int>();        
            this.Color = Color;
            this.mainGrid = mainGrid;
            Size.Add("Width", Width);
            Size.Add("Height", Height);
        }

        public abstract void Draw(int x, int y);
    }
}
