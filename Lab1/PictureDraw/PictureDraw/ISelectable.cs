using System.Windows;
using System.Windows.Input;

namespace PictureDraw
{
    public interface ISelectable
    {
        void SelectShape(object sender, MouseEventArgs e);        
    }
}