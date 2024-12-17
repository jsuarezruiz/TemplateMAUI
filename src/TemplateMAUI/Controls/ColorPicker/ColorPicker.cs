using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using TemplateMAUI.Platforms;

namespace TemplateMAUI.Controls
{
    public class ColorPicker : TemplatedView
    {
        const string ElementGradientContainer = "PART_GradientContainer";
        const string ElementGradientBackgroundLayer1 = "PART_GradientBackgroundLayer1";
        const string ElementGradientBackgroundLayer2 = "PART_GradientBackgroundLayer2";
        const string ElementGradient = "PART_Gradient";
        const string ElementThumb = "PART_Thumb";
        const string ElementSliderColor = "PART_SliderColor";
        const string ElementSliderOpacity = "PART_SliderOpacity";
        const string ElementHexLayout = "PART_Hex";
        const string ElementRgbaLayout = "PART_Rgba";

        Grid _gradientContainer;
        Border _gradientBackgroundLayer1;
        Border _gradientBackgroundLayer2;
        Border _gradientBackground;
        Shape _thumb;
        Slider _sliderColor;
        Slider _sliderOpacity;
        Layout _hexLayout;
        Layout _rgbaLayout;

        double _previousX;
        double _previousY;

        double ThumbHalfWidth => (_thumb?.Width ?? 0) / 2;

        public static readonly BindableProperty SelectedColorProperty =
            BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(ColorPicker), Colors.White);

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set { SetValue(SelectedColorProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            if (_sliderColor is not null)
            {
                _sliderColor.ValueChanged -= OnSliderColorValueChanged;
            }

            if (_sliderOpacity is not null)
            {
                _sliderOpacity.ValueChanged -= OnSliderOpacityValueChanged;
            }

            base.OnApplyTemplate();

            _gradientContainer = GetTemplateChild(ElementGradientContainer) as Grid;
            _gradientBackgroundLayer1 = GetTemplateChild(ElementGradientBackgroundLayer1) as Border;
            _gradientBackgroundLayer2 = GetTemplateChild(ElementGradientBackgroundLayer2) as Border;
            _gradientBackground = GetTemplateChild(ElementGradient) as Border;
            _thumb = GetTemplateChild(ElementThumb) as Shape;
            _sliderColor = GetTemplateChild(ElementSliderColor) as Slider;
            _sliderOpacity = GetTemplateChild(ElementSliderOpacity) as Slider;
            _hexLayout = GetTemplateChild(ElementHexLayout) as Layout;
            _rgbaLayout = GetTemplateChild(ElementRgbaLayout) as Layout;

            if (_sliderColor is not null)
            {
                _sliderColor.ValueChanged += OnSliderColorValueChanged;
            }

            if (_sliderOpacity is not null)
            {
                _sliderOpacity.ValueChanged += OnSliderOpacityValueChanged;
            }

            UpdateGradientBackground();
            UpdateIsEnabled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void UpdateGradientBackground()
        {
            _gradientBackground.Background = null;

            var gradientStopCollection = new GradientStopCollection
            {
                new GradientStop(Colors.Transparent, 0),
                new GradientStop(SelectedColor, 1)
            };

            var gradientBackground = new LinearGradientBrush(
                gradientStopCollection,
                new Point(0, 0),
                new Point(1, 0));

            _gradientBackground.Background = gradientBackground;
        }

        async void OnSliderColorValueChanged(object sender, ValueChangedEventArgs e)
        {
            await UpdateSelectedColorFromSliderAsync();
        }

        async void OnSliderOpacityValueChanged(object sender, ValueChangedEventArgs e)
        {
            var opacity = e.Value / 255;
            _gradientBackgroundLayer1.Opacity = _gradientBackgroundLayer2.Opacity = opacity;

            await UpdateSelectedColorFromSliderAsync();
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += OnThumbPanUpdated;
                _gradientBackground.GestureRecognizers.Add(panGestureRecognizer);

                var hexTapGestureRecognizer = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 2
                };
                hexTapGestureRecognizer.Tapped += OnHexTapped;
                _hexLayout.GestureRecognizers.Add(hexTapGestureRecognizer);

                var rgbaTapGestureRecognizer = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 2
                };
                rgbaTapGestureRecognizer.Tapped += OnRgbaTapped;
                _rgbaLayout.GestureRecognizers.Add(rgbaTapGestureRecognizer);
            }
            else
            {
                _gradientBackground.GestureRecognizers.Clear();
                _hexLayout.GestureRecognizers.Clear();
                _rgbaLayout.GestureRecognizers.Clear();
            }
        }

        async void OnThumbPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _previousX = e.TotalX;
                    _previousX = e.TotalY;

                    if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        _previousX += _thumb.TranslationX;
                        _previousY += _thumb.TranslationY;
                    }

                    _thumb.Scale = 1.2;
                    break;
                case GestureStatus.Running:
                    var x = _previousX + e.TotalX;
                    var y = _previousY + e.TotalY;

                    Console.WriteLine($"X: {x}, Y: {y}");

                    SetThumbPosition(x, y);

                    var positionX = x + _gradientBackground.Width / 2;
                    var positionY = y + _gradientBackground.Height / 2;

                    await UpdateSelectedColorFromGradientThumbAsync(positionX, positionY);
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    _thumb.Scale = 1.0;
                    break;
            }
        }

        async void OnHexTapped(object sender, EventArgs args)
        {
            await Clipboard.Default.SetTextAsync(SelectedColor.ToHex());
        }

        async void OnRgbaTapped(object sender, EventArgs args)
        {
            SelectedColor.ToRgba(out byte r, out byte g, out byte b, out byte a);

            await Clipboard.Default.SetTextAsync($"{r} {g} {b} {a}");
        }

        async Task UpdateSelectedColorFromSliderAsync()
        {
            try
            {
                Color color = await _sliderColor.ColorAtPoint(_sliderColor.Value, _sliderColor.Height / 2);

                Color colorWithAlpha = color.WithAlpha((float)_sliderOpacity.Value / 255);

                SelectedColor = colorWithAlpha;

                UpdateGradientBackground();
            }
            catch
            {
                // Invalid color point
            }
        }

        void SetThumbPosition(double x, double y)
        {
            if (_thumb is null)
                return;

            var half = ThumbHalfWidth;

            var halfHeight = _gradientBackground.Height / 2;
            var halfWidth = _gradientBackground.Width / 2;

            var minX = (_gradientBackground.Bounds.Left - halfWidth) + half;

            if (x < minX)
                x = minX;

            var maxX = _gradientBackground.Width - (halfWidth + half);

            if (x > maxX)
                x = maxX;

            var minY = (_gradientBackground.Bounds.Top - halfHeight) + half;

            if (y < minY)
                y = minY;

            var maxY = _gradientBackground.Height - (halfHeight + half);

            if (y > maxY)
                y = maxY;

            _thumb.TranslationX = x;
            _thumb.TranslationY = y;
        }

        async Task UpdateSelectedColorFromGradientThumbAsync(double x, double y)
        {
            try
            {
                Color color = await _gradientContainer.ColorAtPoint(x, y);

                Color colorWithAlpha = color.WithAlpha((float)_sliderOpacity.Value / 255);

                SelectedColor = colorWithAlpha;
            }
            catch
            {
                // Invalid color point
            }
        }
    }
}