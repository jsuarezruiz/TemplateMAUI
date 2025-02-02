using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The ProgressButton is a custom templated control designed to represent a button with integrated progress indication functionality. 
    /// It allows you to display content within the button while showing progress, making it useful for scenarios where users need visual feedback on ongoing tasks, such as loading or submitting.
    /// </summary>
    [ContentProperty(nameof(Content))]
    public class ProgressButton : TemplatedView, IButton
    {
        const string ElementFeedback = "PART_Feedback";
        const string ElementContainer = "PART_Container"; 
        const string ElementActivityIndicator = "PART_ActivityIndicator";

        FeedbackView _feedbackView;
        Border _container;
        ActivityIndicator _activityIndicator;

        TapGestureRecognizer _tapGestureRecognizer;
        PointerGestureRecognizer _pointerGestureRecognizer;

        ButtonVisualState _visualState;

        public static new readonly BindableProperty BackgroundProperty = ButtonBase.BackgroundProperty;

        public new Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public static readonly BindableProperty BorderBrushProperty = ButtonBase.BorderBrushProperty;

        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public static readonly BindableProperty BorderThicknessProperty = ButtonBase.BorderThicknessProperty;

        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(ProgressButton), new CornerRadius(6d),
                propertyChanged: OnCornerRadiusChanged);

        static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ProgressButton).UpdateCornerRadius();
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = ButtonBase.TextColorProperty;

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static BindableProperty FontSizeProperty = ButtonBase.FontSizeProperty;

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        public static BindableProperty FontFamilyProperty = ButtonBase.FontFamilyProperty;

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly BindableProperty RippleColorProperty = ButtonBase.RippleColorProperty;

        public Color RippleColor
        {
            get => (Color)GetValue(RippleColorProperty);
            set => SetValue(RippleColorProperty, value);
        }

        public static readonly BindableProperty ProgressColorProperty =
            BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(ProgressButton));

        public Color ProgressColor
        {
            get => (Color)GetValue(ProgressColorProperty);
            set => SetValue(ProgressColorProperty, value);
        }

        public static readonly BindableProperty ContentProperty = ButtonBase.ContentProperty;

        public object Content
        {
            get
            {
                var value = GetValue(ContentProperty);

                if (value is not null && value.GetType() == typeof(string))
                {
                    return ContentAsString(value);
                }

                return value;
            }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly BindableProperty IsBusyProperty =
            BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(ProgressButton), false,
                propertyChanged: OnIsBusyChanged);

        static void OnIsBusyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ProgressButton)?.UpdateIsBusy();
        }

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ProgressButton));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ProgressButton));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set { SetValue(CommandParameterProperty, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ButtonVisualState ButtonVisualState
        {
            get => _visualState;
            set
            {
                _visualState = value;
                ChangeVisualState();
            }
        }

        public event EventHandler Pressed;
        public event EventHandler Released;
        public event EventHandler PointerEntered;
        public event EventHandler PointerExited;
        public event EventHandler Clicked;

        public View ContentAsString(object content)
        {
            return new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontFamily = FontFamily,
                FontSize = FontSize,
                TextColor = TextColor,
                Text = content?.ToString()
            };
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _feedbackView = GetTemplateChild(ElementFeedback) as FeedbackView;
            _container = GetTemplateChild(ElementContainer) as Border;
            _activityIndicator = GetTemplateChild(ElementActivityIndicator) as ActivityIndicator;

            _tapGestureRecognizer = new TapGestureRecognizer();
            _pointerGestureRecognizer = new PointerGestureRecognizer();

            UpdateIsEnabled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        protected override void ChangeVisualState()
        {
            string state;

            switch (ButtonVisualState)
            {
                case ButtonVisualState.Normal:
                    state = "Normal";
                    break;
                case ButtonVisualState.MouseOver:
                    state = "MouseOver";
                    break;
                case ButtonVisualState.Pressed:
                    state = "Pressed";
                    break;
                case ButtonVisualState.Disabled:
                    state = "Disabled";
                    break;
                default:
                    state = "Normal";
                    break;
            }

            VisualStateManager.GoToState(this, state);
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                _tapGestureRecognizer.Tapped += OnButtonTapped;
                _container.GestureRecognizers.Add(_tapGestureRecognizer);

                _pointerGestureRecognizer.PointerEntered += OnButtonPointerEntered;
                _pointerGestureRecognizer.PointerPressed += OnButtonPointerPressed;
                _pointerGestureRecognizer.PointerMoved += OnButtonPointerMoved;
                _pointerGestureRecognizer.PointerExited += OnButtonPointerExited;
                _pointerGestureRecognizer.PointerReleased += OnButtonPointerReleased;
                _container.GestureRecognizers.Add(_pointerGestureRecognizer);
            }
            else
            {
                if (_tapGestureRecognizer is not null)
                {
                    _tapGestureRecognizer.Tapped -= OnButtonTapped;
                    _container.GestureRecognizers.Remove(_tapGestureRecognizer);
                }

                if (_pointerGestureRecognizer is not null)
                {
                    _pointerGestureRecognizer.PointerEntered -= OnButtonPointerEntered;
                    _pointerGestureRecognizer.PointerPressed -= OnButtonPointerPressed;
                    _pointerGestureRecognizer.PointerMoved -= OnButtonPointerMoved;
                    _pointerGestureRecognizer.PointerExited -= OnButtonPointerExited;
                    _pointerGestureRecognizer.PointerReleased -= OnButtonPointerReleased;
                    _container.GestureRecognizers.Remove(_pointerGestureRecognizer);
                }
            }

            ButtonVisualState = IsEnabled ? ButtonVisualState.Normal : ButtonVisualState.Disabled;
        }

        async void UpdateIsBusy()
        {
            if (_activityIndicator is null)
                return;

            if (IsBusy)
            {
                _container.IsEnabled = false;
                _activityIndicator.IsRunning = true;
                await _activityIndicator.FadeTo(1);
            }
            else
            {
                _container.IsEnabled = true;
                _activityIndicator.IsRunning = false;
                await _activityIndicator.FadeTo(0);
            }
        }

        void UpdateCornerRadius()
        {
            if (_feedbackView is not null)
                _feedbackView.CornerRadius = CornerRadius;

            if (_container is not null && _container.StrokeShape is RoundRectangle containerStrokeShape)
                containerStrokeShape.CornerRadius = CornerRadius;
        }

        void OnButtonTapped(object sender, TappedEventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);

            if (Command is not null && Command.CanExecute(CommandParameter))
                Command.Execute(null);
        }

        void OnButtonPointerEntered(object sender, PointerEventArgs e)
        {
            PointerEntered?.Invoke(this, EventArgs.Empty);
        }

        void OnButtonPointerPressed(object sender, PointerEventArgs e)
        {
            UpdateVisualState(ButtonVisualState.Pressed);
            Pressed?.Invoke(this, EventArgs.Empty);
        }

        void OnButtonPointerMoved(object sender, PointerEventArgs e)
        {
            UpdateVisualState(ButtonVisualState.MouseOver);
        }

        void OnButtonPointerExited(object sender, PointerEventArgs e)
        {
            UpdateVisualState(ButtonVisualState.Normal);
            PointerExited?.Invoke(this, EventArgs.Empty);
        }

        void OnButtonPointerReleased(object sender, PointerEventArgs e)
        {
            Released?.Invoke(this, EventArgs.Empty);
        }

        void UpdateVisualState(ButtonVisualState visualState)
        {
            if (!IsEnabled)
                return;

            ButtonVisualState = visualState;
        }
    }
}