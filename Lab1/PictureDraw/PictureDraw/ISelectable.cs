using System.Windows;
using System.Windows.Input;

namespace PictureDraw
{
    public interface ISelectable
    {
        void selectShape(object sender, MouseEventArgs e);        
        void setCurrentShape(object sender, MouseEventArgs e);
        void movingCurrentShape(object sender, MouseEventArgs e);
        void stopMovingCurrentShape(object sender, MouseEventArgs e);
    }
}