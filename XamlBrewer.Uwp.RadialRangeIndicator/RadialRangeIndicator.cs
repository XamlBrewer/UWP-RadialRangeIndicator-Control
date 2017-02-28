using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace XamlBrewer.Uwp.Controls
{
    /// <summary>
    /// A Radial Range Indicator Control.
    /// </summary>
    //// All calculations are for a 200x200 square. The ViewBox control will do the rest.
    [TemplatePart(Name = ContainerPartName, Type = typeof(Grid))]
    [TemplatePart(Name = ScalePartName, Type = typeof(Path))]
    [TemplatePart(Name = RangePartName, Type = typeof(Path))]
    [TemplatePart(Name = TextPartName, Type = typeof(TextBlock))]
    public class RadialRangeIndicator : Control
    {
        /// <summary>
        /// Identifies the Minimum dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(RadialRangeIndicator), new PropertyMetadata(0.0, OnScaleChanged));

        /// <summary>
        /// Identifies the Maximum dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(RadialRangeIndicator), new PropertyMetadata(100.0, OnScaleChanged));

        /// <summary>
        /// Identifies the MinRangeValue dependency property.
        /// </summary>
        public static readonly DependencyProperty MinRangeValueProperty =
            DependencyProperty.Register(nameof(MinRangeValue), typeof(double), typeof(RadialRangeIndicator), new PropertyMetadata(40.0, OnValueChanged));

        /// <summary>
        /// Identifies the MaxRangeValue dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxRangeValueProperty =
            DependencyProperty.Register(nameof(MaxRangeValue), typeof(double), typeof(RadialRangeIndicator), new PropertyMetadata(60.0, OnValueChanged));


        /// <summary>
        /// Identifies the optional StepSize property.
        /// </summary>
        public static readonly DependencyProperty StepSizeProperty =
            DependencyProperty.Register(nameof(StepSize), typeof(double), typeof(RadialRangeIndicator), new PropertyMetadata(0.0));

        /// <summary>
        /// Identifies the ScaleWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleWidthProperty =
            DependencyProperty.Register(nameof(ScaleWidth), typeof(double), typeof(RadialRangeIndicator), new PropertyMetadata(25.0, OnScaleChanged));

        /// <summary>
        /// Identifies the RangeBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeBrushProperty =
            DependencyProperty.Register(nameof(RangeBrush), typeof(Brush), typeof(RadialRangeIndicator), new PropertyMetadata(new SolidColorBrush(Colors.Orange), OnValueChanged));

        /// <summary>
        /// Identifies the TrailStartCap dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeStartCapProperty =
            DependencyProperty.Register(nameof(RangeStartCap), typeof(PenLineCap), typeof(RadialRangeIndicator), new PropertyMetadata(PenLineCap.Round, OnValueChanged));

        /// <summary>
        /// Identifies the TrailEndCap dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeEndCapProperty =
            DependencyProperty.Register(nameof(RangeEndCap), typeof(PenLineCap), typeof(RadialRangeIndicator), new PropertyMetadata(PenLineCap.Round, OnValueChanged));

        /// <summary>
        /// Identifies the ScaleBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleBrushProperty =
            DependencyProperty.Register(nameof(ScaleBrush), typeof(Brush), typeof(RadialRangeIndicator), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray), OnScaleChanged));

        /// <summary>
        /// Identifies the ScaleStartCap dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleStartCapProperty =
            DependencyProperty.Register(nameof(ScaleStartCap), typeof(PenLineCap), typeof(RadialRangeIndicator), new PropertyMetadata(PenLineCap.Round, OnScaleChanged));

        /// <summary>
        /// Identifies the ScaleEndCap dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleEndCapProperty =
            DependencyProperty.Register(nameof(ScaleEndCap), typeof(PenLineCap), typeof(RadialRangeIndicator), new PropertyMetadata(PenLineCap.Round, OnScaleChanged));

        /// <summary>
        /// Identifies the ValueBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TextBrushProperty =
            DependencyProperty.Register(nameof(TextBrush), typeof(Brush), typeof(RadialRangeIndicator), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// Identifies the TextStringFormat dependency property.
        /// </summary>
        public static readonly DependencyProperty TextStringFormatProperty =
            DependencyProperty.Register(nameof(TextStringFormat), typeof(string), typeof(RadialRangeIndicator), new PropertyMetadata("{0} - {1}"));

        /// <summary>
        /// Identifies the MinAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty MinAngleProperty =
            DependencyProperty.Register(nameof(MinAngle), typeof(int), typeof(RadialRangeIndicator), new PropertyMetadata(0, OnScaleChanged));

        /// <summary>
        /// Identifies the MaxAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxAngleProperty =
            DependencyProperty.Register(nameof(MaxAngle), typeof(int), typeof(RadialRangeIndicator), new PropertyMetadata(360, OnScaleChanged));

        /// <summary>
        /// Identifies the MinRangeValueAngle dependency property.
        /// </summary>
        protected static readonly DependencyProperty MinRangeValueAngleProperty =
            DependencyProperty.Register(nameof(MinRangeValueAngle), typeof(double), typeof(RadialRangeIndicator), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the MaxRangeValueAngle dependency property.
        /// </summary>
        protected static readonly DependencyProperty MaxRangeValueAngleProperty =
            DependencyProperty.Register(nameof(MaxRangeValueAngle), typeof(double), typeof(RadialRangeIndicator), new PropertyMetadata(null));


        // Template Parts.
        private const string ContainerPartName = "PART_Container";
        private const string ScalePartName = "PART_Scale";
        private const string RangePartName = "PART_Range";
        private const string TextPartName = "PART_Text";

        // For convenience.
        private const double Degrees2Radians = Math.PI / 180;
        private const double Radius = 100;

        private double _normalizedMinAngle;
        private double _normalizedMaxAngle;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialRangeIndicator"/> class.
        /// </summary>
        public RadialRangeIndicator()
        {
            DefaultStyleKey = typeof(RadialRangeIndicator);
        }

        /// <summary>
        /// Gets or sets the rounding interval for the Value.
        /// </summary>
        public double StepSize
        {
            get { return (double)GetValue(StepSizeProperty); }
            set { SetValue(StepSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the scale, in percentage of the radius.
        /// </summary>
        public double ScaleWidth
        {
            get { return (double)GetValue(ScaleWidthProperty); }
            set { SetValue(ScaleWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum value of the scale.
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value of the scale.
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum value for the range.
        /// </summary>
        public double MinRangeValue
        {
            get { return (double)GetValue(MinRangeValueProperty); }
            set { SetValue(MinRangeValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value for the range.
        /// </summary>
        public double MaxRangeValue
        {
            get { return (double)GetValue(MaxRangeValueProperty); }
            set { SetValue(MaxRangeValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the range brush.
        /// </summary>
        public Brush RangeBrush
        {
            get { return (Brush)GetValue(RangeBrushProperty); }
            set { SetValue(RangeBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the StrokeStartCap for the Range.
        /// </summary>
        public PenLineCap RangeStartCap
        {
            get { return (PenLineCap)GetValue(RangeStartCapProperty); }
            set { SetValue(RangeStartCapProperty, value); }
        }

        /// <summary>
        /// Gets or sets the StrokeEndCap for the Range.
        /// </summary>
        public PenLineCap RangeEndCap
        {
            get { return (PenLineCap)GetValue(RangeEndCapProperty); }
            set { SetValue(RangeEndCapProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale brush.
        /// </summary>
        public Brush ScaleBrush
        {
            get { return (Brush)GetValue(ScaleBrushProperty); }
            set { SetValue(ScaleBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the StrokeStartCap for the Scale.
        /// </summary>
        public PenLineCap ScaleStartCap
        {
            get { return (PenLineCap)GetValue(ScaleStartCapProperty); }
            set { SetValue(ScaleStartCapProperty, value); }
        }

        /// <summary>
        /// Gets or sets the StrokeEndCap for the Scale.
        /// </summary>
        public PenLineCap ScaleEndCap
        {
            get { return (PenLineCap)GetValue(ScaleEndCapProperty); }
            set { SetValue(ScaleEndCapProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush for the displayed value.
        /// </summary>
        public Brush TextBrush
        {
            get { return (Brush)GetValue(TextBrushProperty); }
            set { SetValue(TextBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text string format.
        /// </summary>
        public string TextStringFormat
        {
            get { return (string)GetValue(TextStringFormatProperty); }
            set { SetValue(TextStringFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the start angle of the scale, which corresponds with the Minimum value, in degrees.
        /// </summary>
        public int MinAngle
        {
            get { return (int)GetValue(MinAngleProperty); }
            set { SetValue(MinAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the end angle of the scale, which corresponds with the Maximum value, in degrees.
        /// </summary>
        public int MaxAngle
        {
            get { return (int)GetValue(MaxAngleProperty); }
            set { SetValue(MaxAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle for the minimum value of the range (between MinAngle and MaxAngle). 
        /// </summary>
        protected double MinRangeValueAngle
        {
            get { return (double)GetValue(MinRangeValueAngleProperty); }
            set { SetValue(MinRangeValueAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle for the maximum value of the range (between MinAngle and MaxAngle). 
        /// </summary>
        protected double MaxRangeValueAngle
        {
            get { return (double)GetValue(MaxRangeValueAngleProperty); }
            set { SetValue(MaxRangeValueAngleProperty, value); }
        }

        /// <summary>
        /// Gets the normalized minimum angle.
        /// </summary>
        /// <value>The minimum angle in the range from -180 to 180.</value>
        protected double NormalizedMinAngle => _normalizedMinAngle;

        /// <summary>
        /// Gets the normalized maximum angle.
        /// </summary>
        /// <value>The maximum angle in the range from 180 to 540.</value>
        protected double NormalizedMaxAngle => _normalizedMaxAngle;

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            OnScaleChanged(this);

            base.OnApplyTemplate();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnValueChanged(d);
        }

        private static void OnValueChanged(DependencyObject d)
        {
            var radialRangeIndicator = (RadialRangeIndicator)d;
            if (!double.IsNaN(radialRangeIndicator.MaxRangeValue))
            {
                if (radialRangeIndicator.StepSize > 0)
                {
                    radialRangeIndicator.MaxRangeValue = radialRangeIndicator.RoundToMultiple(radialRangeIndicator.MaxRangeValue, radialRangeIndicator.StepSize);
                }

                var middleOfScale = Radius - (radialRangeIndicator.ScaleWidth / 2);
                var valueText = radialRangeIndicator.GetTemplateChild(TextPartName) as TextBlock;
                radialRangeIndicator.MinRangeValueAngle = radialRangeIndicator.ValueToAngle(radialRangeIndicator.MinRangeValue);
                radialRangeIndicator.MaxRangeValueAngle = radialRangeIndicator.ValueToAngle(radialRangeIndicator.MaxRangeValue);

                // Range
                var range = radialRangeIndicator.GetTemplateChild(RangePartName) as Path;
                if (range != null)
                {
                    if (radialRangeIndicator.MaxRangeValueAngle > radialRangeIndicator.MinRangeValueAngle)
                    {
                        range.Visibility = Visibility.Visible;

                        if (radialRangeIndicator.MaxRangeValueAngle - radialRangeIndicator.NormalizedMinAngle == 360)
                        {
                            // Draw full circle.
                            var eg = new EllipseGeometry
                            {
                                Center = new Point(Radius, Radius),
                                RadiusX = Radius - (radialRangeIndicator.ScaleWidth / 2)
                            };

                            eg.RadiusY = eg.RadiusX;
                            range.Data = eg;
                        }
                        else
                        {
                            range.StrokeStartLineCap = radialRangeIndicator.RangeStartCap;
                            range.StrokeEndLineCap = radialRangeIndicator.RangeEndCap;

                            // Draw arc.
                            var pg = new PathGeometry();
                            var pf = new PathFigure
                            {
                                IsClosed = false,
                                StartPoint = radialRangeIndicator.ScalePoint(radialRangeIndicator.MinRangeValueAngle, middleOfScale)
                            };

                            var seg = new ArcSegment
                            {
                                SweepDirection = SweepDirection.Clockwise,
                                IsLargeArc = radialRangeIndicator.MaxRangeValueAngle > (180 + radialRangeIndicator.MinRangeValueAngle),
                                Size = new Size(middleOfScale, middleOfScale),
                                Point =
                                    radialRangeIndicator.ScalePoint(
                                        Math.Min(radialRangeIndicator.MaxRangeValueAngle, radialRangeIndicator.NormalizedMaxAngle), middleOfScale)
                            };

                            pf.Segments.Add(seg);
                            pg.Figures.Add(pf);
                            range.Data = pg;
                        }
                    }
                    else
                    {
                        range.Visibility = Visibility.Collapsed;
                    }
                }

                // Value Text
                if (valueText != null)
                {
                    valueText.Text = string.Format(radialRangeIndicator.TextStringFormat, radialRangeIndicator.MinRangeValue, radialRangeIndicator.MaxRangeValue);
                }
            }
        }

        private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnScaleChanged(d);
        }

        private static void OnScaleChanged(DependencyObject d)
        {
            var RadialRangeIndicator = (RadialRangeIndicator)d;

            RadialRangeIndicator.UpdateNormalizedAngles();

            var scale = RadialRangeIndicator.GetTemplateChild(ScalePartName) as Path;
            if (scale != null)
            {
                if (RadialRangeIndicator.NormalizedMaxAngle - RadialRangeIndicator.NormalizedMinAngle == 360)
                {
                    // Draw full circle.
                    var eg = new EllipseGeometry
                    {
                        Center = new Point(Radius, Radius),
                        RadiusX = Radius - (RadialRangeIndicator.ScaleWidth / 2)
                    };

                    eg.RadiusY = eg.RadiusX;
                    scale.Data = eg;
                }
                else
                {
                    scale.StrokeStartLineCap = RadialRangeIndicator.ScaleStartCap;
                    scale.StrokeEndLineCap = RadialRangeIndicator.ScaleEndCap;

                    // Draw arc.
                    var pg = new PathGeometry();
                    var pf = new PathFigure { IsClosed = false };
                    var middleOfScale = Radius - (RadialRangeIndicator.ScaleWidth / 2);
                    pf.StartPoint = RadialRangeIndicator.ScalePoint(RadialRangeIndicator.NormalizedMinAngle, middleOfScale);
                    var seg = new ArcSegment
                    {
                        SweepDirection = SweepDirection.Clockwise,
                        IsLargeArc = RadialRangeIndicator.NormalizedMaxAngle > (RadialRangeIndicator.NormalizedMinAngle + 180),
                        Size = new Size(middleOfScale, middleOfScale),
                        Point = RadialRangeIndicator.ScalePoint(RadialRangeIndicator.NormalizedMaxAngle, middleOfScale)
                    };

                    pf.Segments.Add(seg);
                    pg.Figures.Add(pf);
                    scale.Data = pg;
                }
            }

            OnValueChanged(RadialRangeIndicator);
        }

        private void UpdateNormalizedAngles()
        {
            var result = Mod(MinAngle, 360);

            if (result >= 180)
            {
                result = result - 360;
            }

            _normalizedMinAngle = result;

            result = Mod(MaxAngle, 360);

            if (result < 180)
            {
                result = result + 360;
            }

            if (result > NormalizedMinAngle + 360)
            {
                result = result - 360;
            }

            _normalizedMaxAngle = result;
        }

        private Point ScalePoint(double angle, double middleOfScale)
        {
            return new Point(Radius + (Math.Sin(Degrees2Radians * angle) * middleOfScale), Radius - (Math.Cos(Degrees2Radians * angle) * middleOfScale));
        }

        private double ValueToAngle(double value)
        {
            // Off-scale on the left.
            if (value < Minimum)
            {
                return MinAngle;
            }

            // Off-scale on the right.
            if (value > Maximum)
            {
                return MaxAngle;
            }

            return ((value - Minimum) / (Maximum - Minimum) * (NormalizedMaxAngle - NormalizedMinAngle)) + NormalizedMinAngle;
        }

        private double RoundToMultiple(double number, double multiple)
        {
            var modulo = number % multiple;
            if ((multiple - modulo) <= modulo)
            {
                modulo = multiple - modulo;
            }
            else
            {
                modulo *= -1;
            }

            return number + modulo;
        }

        private static double Mod(double number, double divider)
        {
            var result = number % divider;
            result = result < 0 ? result + divider : result;
            return result;
        }
    }
}
