using System.Windows;
using System.Windows.Input;
using YAXLib;

namespace PictureDraw
{
    public interface ISelectable
    {
        void SelectShape(object sender, MouseEventArgs e);        
    }
}