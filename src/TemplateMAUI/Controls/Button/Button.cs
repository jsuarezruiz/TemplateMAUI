using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The Button is a custom templated control that implements the IButton interface. 
    /// It represents a button control with customizable content, appearance, and behavior, making it a versatile component for user interaction in your application.
    /// </summary>
    [ContentProperty(nameof(Content))]
    public class Button : TemplatedView, IButton
    {
        const string ElementContainer = "PART_Container";

        Border _container;

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

        public static readonly BindableProperty CommandProperty = ButtonBase.CommandProperty;

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty = ButtonBase.CommandParameterProperty;

        public object CommandParameter
        {
            get => GetValue(CommandProperty);
            set { SetValue(CommandProperty, value); }
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

            _container = GetTemplateChild(ElementContainer) as Border;

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

                _pointerGestureRecognizer.PointerPressed += OnButtonPointerPressed;
                _pointerGestureRecognizer.PointerMoved += OnButtonPointerMoved;
                _pointerGestureRecognizer.PointerExited += OnButtonHandlePointerExited;
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
                    _pointerGestureRecognizer.PointerPressed -= OnButtonPointerPressed;
                    _pointerGestureRecognizer.PointerMoved -= OnButtonPointerMoved;
                    _pointerGestureRecognizer.PointerExited -= OnButtonHandlePointerExited;
                    _pointerGestureRecognizer.PointerReleased -= OnButtonPointerReleased;
                    _container.GestureRecognizers.Remove(_pointerGestureRecognizer);
                }
            }

            ButtonVisualState = IsEnabled ? ButtonVisualState.Normal : ButtonVisualState.Disabled;
        }

        void OnButtonTapped(object sender, TappedEventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);

            if (Command is not null && Command.CanExecute(CommandParameter))
                Command.Execute(null);
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

        void OnButtonHandlePointerExited(object sender, PointerEventArgs e)
        {
            UpdateVisualState(ButtonVisualState.Normal);
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