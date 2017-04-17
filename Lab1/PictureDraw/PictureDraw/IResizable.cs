using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PictureDraw
{
    public interface IResizable
    {
        void SetResizeAngle(object sender, MouseEventArgs e);
        void SetAnglesAction(Shapes rect);
        void StopResizeShape(object sender, MouseEventArgs e);        
    }
}
