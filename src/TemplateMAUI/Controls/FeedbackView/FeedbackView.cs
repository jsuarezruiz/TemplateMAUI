using Microsoft.Maui.Animations;
using System.Runtime.CompilerServices;

namespace TemplateMAUI.Controls;

/// <summary>
/// The FeedbackView is a custom templated control designed to provide visual feedback to touch interactions. 
/// It allows you to wrap content and apply visual effects such as highlighting or ripple effects when the user interacts with the view.
/// </summary>
[ContentProperty(nameof(Content))]
public class FeedbackView : TemplatedView
{
    const string ElementContainer = "PART_Container";
    const string ElementGraphics = "PART_Graphics";

    Border _container;
    GraphicsView _graphicsView;

    readonly IAnimationManager _animationManager;
    Point _touchPoint;

    readonly HightlightEffect _hightlightEffect;
    readonly RippleEffect _rippleEffect;

    /// <summary>
    /// PointerGestureRecognizer isn’t working properly on Android for .NET 8 and 9 at the moment. Therefore, we’ve switched to TapGestureRecognizer to reliably capture click events. Link of Issue https://github.com/dotnet/maui/issues/20849
    /// </summary>
#if __ANDROID__
    public TapGestureRecognizer _pointerGestureRecognizer;
#else
    public PointerGestureRecognizer _pointerGestureRecognizer;
#endif
    public FeedbackView()
    {
        _hightlightEffect = new HightlightEffect();
        _rippleEffect = new RippleEffect();

#if __ANDROID__
        _animationManager = new AnimationManager(new PlatformTicker(new Microsoft.Maui.Platform.EnergySaverListenerManager()));
#else
        _animationManager = new AnimationManager(new PlatformTicker());
#endif

        UpdateFeedbackType();
    }

    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(nameof(Content), typeof(object), typeof(FeedbackView));

    public object Content
    {
        get { return GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    public static readonly BindableProperty FeedbackTypeProperty =
        BindableProperty.Create(nameof(FeedbackType), typeof(FeedbackType), typeof(FeedbackView), FeedbackType.Ripple,
            propertyChanged: OnFeedbackTypeChanged);

    static void OnFeedbackTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as FeedbackView)?.UpdateFeedbackType();
    }

    public FeedbackType FeedbackType
    {
        get { return (FeedbackType)GetValue(FeedbackTypeProperty); }
        set { SetValue(FeedbackTypeProperty, value); }
    }

    public static readonly BindableProperty RippleAnimationDurationProperty =
        BindableProperty.Create(nameof(RippleAnimationDuration), typeof(double), typeof(FeedbackView), 250d);

    public double RippleAnimationDuration
    {
        get => (double)GetValue(RippleAnimationDurationProperty);
        set => SetValue(RippleAnimationDurationProperty, value);
    }

    public static readonly BindableProperty RippleColorProperty =
        BindableProperty.Create(nameof(RippleColor), typeof(Color), typeof(FeedbackView), Colors.Gray,
            propertyChanged: OnRippleColorChanged);

    static void OnRippleColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as FeedbackView)?.UpdateRippleColor();
    }

    public Color RippleColor
    {
        get => (Color)GetValue(RippleColorProperty);
        set => SetValue(RippleColorProperty, value);
    }

    public static readonly BindableProperty HightlightAnimationDurationProperty =
        BindableProperty.Create(nameof(HightlightAnimationDuration), typeof(double), typeof(FeedbackView), 250d);

    public double HightlightAnimationDuration
    {
        get => (double)GetValue(HightlightAnimationDurationProperty);
        set => SetValue(HightlightAnimationDurationProperty, value);
    }

    public static readonly BindableProperty HightlightColorProperty =
        BindableProperty.Create(nameof(HightlightColor), typeof(Color), typeof(FeedbackView), Colors.Gray,
            propertyChanged: OnRippleColorChanged);

    static void OnHightlightColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as FeedbackView)?.UpdateHightlightColor();
    }

    public Color HightlightColor
    {
        get => (Color)GetValue(HightlightColorProperty);
        set => SetValue(HightlightColorProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _container = GetTemplateChild(ElementContainer) as Border;
        _graphicsView = GetTemplateChild(ElementGraphics) as GraphicsView;

#if __ANDROID__
        _pointerGestureRecognizer = new TapGestureRecognizer();
#else
        _pointerGestureRecognizer = new PointerGestureRecognizer();
#endif



        UpdateIsEnabled();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == IsEnabledProperty.PropertyName)
            UpdateIsEnabled();
    }

    void UpdateFeedbackType()
    {
        if (_graphicsView is null)
            return;

        switch (FeedbackType)
        {
            case FeedbackType.Hightlight:
                _graphicsView.Drawable = _hightlightEffect;
                break;
            case FeedbackType.Ripple:
                _graphicsView.Drawable = _rippleEffect;
                break;
        }
    }

    void UpdateRippleColor()
    {
        if (_rippleEffect is null)
            return;

        _rippleEffect.RippleColor = new SolidColorBrush(RippleColor);
    }

    void UpdateHightlightColor()
    {
        if (_hightlightEffect is null)
            return;

        _hightlightEffect.HightlightColor = HightlightColor;
    }

    void UpdateIsEnabled()
    {
#if __ANDROID__
        if (IsEnabled)
        {
            _pointerGestureRecognizer.Tapped += OnFeedbackViewPointerPressed;
            _container.GestureRecognizers.Add(_pointerGestureRecognizer);
        }
        else
        {
            if (_pointerGestureRecognizer is not null)
            {
                _pointerGestureRecognizer.Tapped -= OnFeedbackViewPointerPressed;
                _container.GestureRecognizers.Remove(_pointerGestureRecognizer);
            }
        }
#else
        if (IsEnabled)
        {
            _pointerGestureRecognizer.PointerPressed += OnFeedbackViewPointerPressed;
            _pointerGestureRecognizer.PointerReleased += OnFeedbackViewPointerReleased;
            _container.GestureRecognizers.Add(_pointerGestureRecognizer);
        }
        else
        {
            if (_pointerGestureRecognizer is not null)
            {
                _pointerGestureRecognizer.PointerPressed -= OnFeedbackViewPointerPressed;
                _pointerGestureRecognizer.PointerReleased -= OnFeedbackViewPointerReleased;
                _container.GestureRecognizers.Remove(_pointerGestureRecognizer);
            }
        }
#endif

    }

    void OnFeedbackViewPointerPressed(object? sender, TappedEventArgs e)
    {
        if (IsEnabled)
        {
            var point = e.GetPosition(_container);

            if (point is null)
                return;

            _touchPoint = point.Value;

            if (FeedbackType == FeedbackType.Hightlight)
                StartHightlightFeedback();
            else
                StartRippleFeedback();
        }
    }

    void OnFeedbackViewPointerPressed(object sender, PointerEventArgs e)
    {
        if (IsEnabled)
        {
            var point = e.GetPosition(_container);

            if (point is null)
                return;

            _touchPoint = point.Value;

            if (FeedbackType == FeedbackType.Hightlight)
                StartHightlightFeedback();
            else
                StartRippleFeedback();
        }
    }

    void OnFeedbackViewPointerReleased(object sender, PointerEventArgs e)
    {
        if (FeedbackType == FeedbackType.Hightlight)
            CancelHightlightFeedback();
        else
            CancelRippleFeedback();
    }

    void StartHightlightFeedback()
    {
        if (_hightlightEffect is null)
            return;

        Color startColor = Colors.Transparent;
        Color endColor = HightlightColor;

        var duration = TimeSpan.FromMilliseconds(HightlightAnimationDuration).TotalSeconds;
        _animationManager?.Add(new Microsoft.Maui.Animations.Animation(
            callback: (progress) =>
            {
                _hightlightEffect.HightlightColor = startColor.Lerp(endColor, progress);
                _graphicsView.Invalidate();
            },
            duration: duration,
            easing: Easing.SinInOut,
            finished: CancelHightlightFeedback));
    }

    void StartRippleFeedback()
    {
        if (_rippleEffect is null)
            return;

        if (DeviceInfo.Platform == DevicePlatform.WinUI &&
            ((_rippleEffect as IVisualElementController)?.EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
            _touchPoint = new Point(Width - _touchPoint.X, _touchPoint.Y);

        _rippleEffect.TouchPoint = _touchPoint;
        _rippleEffect.RippleColor = new SolidColorBrush(RippleColor);

        float startDiameter = 0f;
        float endDiameter;

        if (Width > 0 && Height > 0)
        {
            float width = (float)(_touchPoint.X > Width / 2 ? _touchPoint.X : Width - _touchPoint.X);
            float height = (float)(_touchPoint.Y > Height / 2 ? _touchPoint.Y : Height - _touchPoint.Y);
            endDiameter = (float)Math.Sqrt((width * width) + (height * height));
        }
        else
        {
            endDiameter = (float)Math.Sqrt((_touchPoint.X * _touchPoint.X) + (_touchPoint.Y * _touchPoint.Y));
        }

        var duration = TimeSpan.FromMilliseconds(RippleAnimationDuration).TotalSeconds;
        _animationManager?.Add(new Microsoft.Maui.Animations.Animation(
            callback: (progress) =>
            {
                _rippleEffect.Diameter = startDiameter.Lerp(endDiameter, progress);
                _graphicsView.Invalidate();
            },
            duration: duration,
            easing: Easing.SinInOut,
            finished: CancelRippleFeedback));
    }

    void CancelHightlightFeedback()
    {
        if (_hightlightEffect is not null)
        {
            _hightlightEffect.HightlightColor = Colors.Transparent;
            _graphicsView.Invalidate();
        }
    }

    void CancelRippleFeedback()
    {
        if (_rippleEffect is not null)
        {
            _rippleEffect.Diameter = 0f;
            _graphicsView.Invalidate();
        }
    }
}

