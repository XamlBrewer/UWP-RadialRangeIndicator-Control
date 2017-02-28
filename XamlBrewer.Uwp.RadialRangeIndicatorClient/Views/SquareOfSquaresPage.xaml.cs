using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using XamlBrewer.Uwp.Controls;

namespace XamlBrewer.Uwp.RadialRangeIndicatorClient
{
    public sealed partial class SquareOfSquaresPage : Page
    {
        public SquareOfSquaresPage()
        {
            InitializeComponent();
            Loaded += SquareOfSquaresPage_Loaded;
        }

        private void SquareOfSquaresPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            foreach (var square in SquareOfSquares.Squares)
            {
                var min = random.Next(100);

                square.Content = new RadialRangeIndicator()
                {
                    Height = square.ActualHeight,
                    Width = square.ActualWidth,
                    ScaleBrush = new SolidColorBrush(square.RandomColor()),
                    RangeBrush = new SolidColorBrush(square.RandomColor()),
                    TextBrush = new SolidColorBrush(Colors.Silver),
                    ScaleWidth = 10 + random.Next(50),
                    MinRangeValue = min,
                    MaxRangeValue = random.Next(min, 100),
                    MinAngle = -150,
                    MaxAngle = 150,
                    ScaleStartCap = PenLineCap.Flat,
                    ScaleEndCap = PenLineCap.Flat,
                    RangeEndCap = PenLineCap.Flat,
                    RangeStartCap = PenLineCap.Flat
                };
            }
        }
    }
}
