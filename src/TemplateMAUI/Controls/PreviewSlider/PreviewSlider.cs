using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TemplateMAUI.Controls
{
    /// <summary>
    /// Represents a Slider control that provides a preview of its value.
    /// </summary>
    public class PreviewSlider : TemplatedView, ISlider
    {
        const string ElementPreview = "PART_Preview";
        const string ElementTrackBackground = "PART_TrackBackground";
        const string ElementTrack = "PART_Track";
        const string ElementThumb = "PART_Thumb";

        View _preview;
        View _trackBackground;
        View _track;
        ContentView _thumb;

        double _previousPosition;
        double? _previousVal = null;
        bool _previewIsVisible;

        PanGestureRecognizer _panGestureRecognizer;
        PointerGestureRecognizer _pointerGestureRecognizer;

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        double ThumbHalfWidth => (_thumb?.Width ?? 0) / 2;

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(double), typeof(PreviewSlider), 0.0d, propertyChanged: OnValueChanged);

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PreviewSlider previewSlider)
                previewSlider.UpdateValue();
        }

        internal static readonly BindableProperty PreviewValueProperty =
            BindableProperty.Create(nameof(PreviewValue), typeof(double), typeof(PreviewSlider), 0.0d);

        public double PreviewValue
        {
            get => (double)GetValue(PreviewValueProperty);
            private set => SetValue(PreviewValueProperty, value);
        }

        public static readonly BindableProperty MinimumProperty = SliderBase.MinimumProperty;

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly BindableProperty MaximumProperty = SliderBase.MaximumProperty;

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly BindableProperty TrackColorProperty =
            BindableProperty.Create(nameof(TrackColor), typeof(Color), typeof(PreviewSlider), Colors.LightGray);

        public Color TrackColor
        {
            get => (Color)GetValue(TrackColorProperty);
            set => SetValue(TrackColorProperty, value);
        }

        public static readonly BindableProperty ThumbColorProperty =
            BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(PreviewSlider), Colors.Gray);

        public Color ThumbColor
        {
            get => (Color)GetValue(ThumbColorProperty);
            set => SetValue(ThumbColorProperty, value);
        }

        public static readonly BindableProperty PreviewColorProperty =
            BindableProperty.Create(nameof(PreviewColor), typeof(Color), typeof(PreviewSlider), Colors.LightGray);

        public Color PreviewColor
        {
            get => (Color)GetValue(PreviewColorProperty);
            set => SetValue(PreviewColorProperty, value);
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(PreviewSlider));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(PreviewSlider), 10.0);

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontFamilyProperty =
           BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(PreviewSlider), string.Empty);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(PreviewSlider), FontAttributes.None);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }

        public static BindableProperty PreviewAnimationProperty =
            BindableProperty.Create(nameof(PreviewAnimation), typeof(IPreviewSliderAnimation), typeof(PreviewSlider), new PreviewSliderAnimation());

        public IPreviewSliderAnimation PreviewAnimation
        {
            get { return (IPreviewSliderAnimation)GetValue(PreviewAnimationProperty); }
            set { SetValue(PreviewAnimationProperty, value); }
        }

        public static readonly BindableProperty PreviewPositionProperty =
            BindableProperty.Create(nameof(PreviewPosition), typeof(PreviewSliderPosition), typeof(PreviewSlider), PreviewSliderPosition.Top, propertyChanged: OnPreviewPositionChanged);

        public PreviewSliderPosition PreviewPosition
        {
            get => (PreviewSliderPosition)GetValue(PreviewPositionProperty);
            set => SetValue(PreviewPositionProperty, value);
        }

        static void OnPreviewPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PreviewSlider previewSlider)
                previewSlider.UpdatePreviewPosition();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _preview = GetTemplateChild(ElementPreview) as View;
            _trackBackground = GetTemplateChild(ElementTrackBackground) as View;
            _track = GetTemplateChild(ElementTrack) as View;
            _thumb = GetTemplateChild(ElementThumb) as ContentView;

            _panGestureRecognizer = new PanGestureRecognizer();
            _pointerGestureRecognizer = new PointerGestureRecognizer();

            UpdateIsEnabled();
            UpdatePreviewPosition();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            UpdateValue(true);
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                _panGestureRecognizer = new PanGestureRecognizer();
                _panGestureRecognizer.PanUpdated += OnThumbPanUpdated;
                _thumb.GestureRecognizers.Add(_panGestureRecognizer);

                _pointerGestureRecognizer.PointerEntered += OnPreviewSliderPointerEntered;
                _pointerGestureRecognizer.PointerMoved += OnPreviewSliderPointerMoved;
                _pointerGestureRecognizer.PointerExited += OnPreviewSliderPointerExited;
                _trackBackground.GestureRecognizers.Add(_pointerGestureRecognizer);
            }
            else
            {
                if (_panGestureRecognizer is not null)
                {
                    _panGestureRecognizer.PanUpdated -= OnThumbPanUpdated;
                    _thumb.GestureRecognizers.Remove(_panGestureRecognizer);
                }

                if (_pointerGestureRecognizer is not null)
                {
                    _pointerGestureRecognizer.PointerEntered -= OnPreviewSliderPointerEntered;
                    _pointerGestureRecognizer.PointerMoved -= OnPreviewSliderPointerMoved;
                    _pointerGestureRecognizer.PointerExited -= OnPreviewSliderPointerExited;
                    _trackBackground.GestureRecognizers.Remove(_pointerGestureRecognizer);
                }
            }
        }

        void UpdateValue(bool isNecessary = false)
        {
            var min = Minimum;
            var max = Maximum;
            var val = Value;
            var valChecked = CheckValueByRange(val, min, max);
            if (valChecked != val)
            {
                Value = PreviewValue = valChecked;
                return;
            }
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(valChecked));

            if (_trackBackground is null)
                return;

            if (!isNecessary && _previousVal == val)
                return;

            var half = ThumbHalfWidth;

            var width = _trackBackground.Width > 0 ? _trackBackground.Width : Width;
            var thumbCenterPosition = ConvertRangeValue(val, min, max, half, width - half);
            SetThumbPosition(thumbCenterPosition - half, thumbCenterPosition, val);
        }

        void OnThumbPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _previousPosition = e.TotalX;

                    if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.WinUI)
                        _previousPosition += _thumb.TranslationX;
                    break;
                case GestureStatus.Running:
                    _previewIsVisible = true;

                    var totalX = _previousPosition + e.TotalX;
                    var half = ThumbHalfWidth;
                    var maxPosition = _trackBackground.Width - half;

                    if (DeviceInfo.Platform == DevicePlatform.Android)
                        totalX += _thumb.TranslationX;

                    var thumbCenterPosition = CheckValueByRange(totalX + half, half, maxPosition);
                    var val = ConvertRangeValue(thumbCenterPosition, half, maxPosition, Minimum, Maximum);
                    SetThumbPosition(thumbCenterPosition - half, thumbCenterPosition, val);
                    Value = PreviewValue = val;
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }

        async void OnPreviewSliderPointerEntered(object sender, PointerEventArgs e)
        {
            _previewIsVisible = true;
            await UpdatePreviewVisibilityAsync();
        }

        void OnPreviewSliderPointerMoved(object sender, PointerEventArgs e)
        {
            var previewPosition = e.GetPosition(_trackBackground);

            if (previewPosition is null)
                return;

            var positionX = previewPosition.Value.X;

            var previewWidth = _preview.Width;

            if (positionX < previewWidth)
                positionX = previewWidth;

            if (positionX > _trackBackground.Width - previewWidth)
                positionX = _trackBackground.Width - previewWidth;

            _preview.TranslationX = positionX;

            var previewValue = positionX * Maximum / _trackBackground.Width;
            PreviewValue = previewValue;
        }

        async void OnPreviewSliderPointerExited(object sender, PointerEventArgs e)
        {
            _previewIsVisible = false;
            await UpdatePreviewVisibilityAsync();
        }

        double ConvertRangeValue(double oldVal, double oldMin, double oldMax, double min, double max)
        {
            var relativeValue = (oldVal - oldMin) / (oldMax - oldMin);
            return min + (max - min) * relativeValue;
        }

        double CheckValueByRange(double val, double min, double max)
            => val <= min ? min : val >= max ? max : val;

        void SetThumbPosition(double thumbPosition, double progressWidth, double val)
        {
            _previousVal = val;
            _thumb.TranslationX = _preview.TranslationX = thumbPosition;
        }

        async Task UpdatePreviewVisibilityAsync()
        {
            if (_preview is null)
                return;

            if (_preview.IsVisible == _previewIsVisible)
                return;

            if (_previewIsVisible)
            {
                _preview.IsVisible = true;
                await PreviewAnimation.OnAppearing(_preview);
            }
            else
            {
                await PreviewAnimation.OnDisappering(_preview);
                _preview.IsVisible = false;
            }
        }

        void UpdatePreviewPosition()
        {
            if (_trackBackground is null)
                return;

            if (PreviewSliderPosition.Top == PreviewPosition)
            {
                Application.Current.Resources.TryGetValue("TopPreviewSliderControlTemplate", out var topPreviewSlider);
                
                if (topPreviewSlider is ControlTemplate topPreviewSliderControlTemplate)
                    ControlTemplate = topPreviewSliderControlTemplate;
            }
            else 
            {
                Application.Current.Resources.TryGetValue("BottomPreviewSliderControlTemplate", out var bottomPreviewSlider);

                if (bottomPreviewSlider is ControlTemplate bottomPreviewSliderControlTemplate)
                    ControlTemplate = bottomPreviewSliderControlTemplate;
            }
        }
    }
}