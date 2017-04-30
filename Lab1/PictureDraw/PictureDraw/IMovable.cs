using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PictureDraw
{
    public interface IMovable
    {
        void SetDragPoint(object sender, MouseEventArgs e);
        void MovingShape(object sender, MouseEventArgs e);
        void StopMovingShape(object sender, MouseEventArgs e);
    }
}
