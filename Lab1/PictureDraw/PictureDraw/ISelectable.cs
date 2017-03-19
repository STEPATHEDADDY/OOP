using System.Windows;
using System.Windows.Input;

namespace PictureDraw
{
    public interface ISelectable
    {
        void selectShape(object sender, MouseEventArgs e);        
    }
}