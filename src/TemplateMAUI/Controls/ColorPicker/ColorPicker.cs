﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Platform;
using System.Runtime.CompilerServices;
using TemplateMAUI.Platforms;

namespace TemplateMAUI.Controls
{
    public class ColorPicker : TemplatedView
    {
        const string ElementGradient = "PART_Gradient";
        const string ElementThumb = "PART_Thumb";
        const string ElementSliderColor = "PART_SliderColor";
        const string ElementSliderOpacity = "PART_SliderOpacity";

        Border _gradientBackground;
        Shape _thumb;
        Slider _sliderColor;
        Slider _sliderOpacity;

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

            _gradientBackground = GetTemplateChild(ElementGradient) as Border;
            _thumb = GetTemplateChild(ElementThumb) as Shape;
            _sliderColor = GetTemplateChild(ElementSliderColor) as Slider;
            _sliderOpacity = GetTemplateChild(ElementSliderOpacity) as Slider;

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

        void OnSliderColorValueChanged(object sender, ValueChangedEventArgs e)
        {
            UpdateSelectedColor();
        }

        void OnSliderOpacityValueChanged(object sender, ValueChangedEventArgs e)
        {
            UpdateSelectedColor();
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += OnThumbPanUpdated;
                _gradientBackground.GestureRecognizers.Add(panGestureRecognizer);
            }
            else
            {
                _gradientBackground.GestureRecognizers.Clear();
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

                    SetThumbPosition(x, y);
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    _thumb.Scale = 1.0;

                    var finalX = _thumb.TranslationX + _gradientBackground.Width / 2;
                    var finalY = _thumb.TranslationY + _gradientBackground.Height / 2;
                    var selectedColor = await GetGradientBackgroundSelectedColorAsync(finalX, finalY);
                                       
                    SelectedColor = selectedColor;
                    break;
            }
        }

        void UpdateSelectedColor()
        {
            if (Application.Current.Resources["ColorPickerRainbowBrush"] is not LinearGradientBrush colorPickerRainbowBrush)
                return;

            var value = _sliderColor.Value / 100;
            var color = colorPickerRainbowBrush.GradientStops.FirstOrDefault(gs => gs.Offset > value).Color;
            var colorWithAlpha = color.WithAlpha((float)_sliderOpacity.Value / 255);

            SelectedColor = colorWithAlpha;

            UpdateGradientBackground();
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

        async Task<Color> GetGradientBackgroundSelectedColorAsync(double x, double y)
        {
            Color color = await _gradientBackground.ColorAtPoint(x, y);

            return color;
        }
    }
}