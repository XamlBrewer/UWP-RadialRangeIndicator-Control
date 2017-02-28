using Windows.UI.Xaml.Controls;
using Mvvm.Services;
using XamlBrewer.Uwp.RadialRangeIndicatorClient;

namespace Mvvm
{
    internal class ShellViewModel : ViewModelBase
    {
        public ShellViewModel()
        {
            // Build the menus
            Menu.Add(new MenuItem() { Glyph = Icon.GetIcon("HomeIcon"), Text = "Main", NavigationDestination = typeof(MainPage) });
            Menu.Add(new MenuItem() { Glyph = Icon.GetIcon("GalleryIcon"), Text = "Gallery", NavigationDestination = typeof(GalleryPage) });
            Menu.Add(new MenuItem() { Glyph = Icon.GetIcon("SquaresIcon"), Text = "Squares", NavigationDestination = typeof(SquareOfSquaresPage) });

            SecondMenu.Add(new MenuItem() { Glyph = Icon.GetIcon("InfoIcon"), Text = "About", NavigationDestination = typeof(AboutPage) });
        }
    }
}
