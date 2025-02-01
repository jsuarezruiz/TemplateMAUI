using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TemplateMAUI.Controls
{
    [ContentProperty(nameof(Content))]
    public class ToggleButton : TemplatedView, IButton
    {
        const string ElementContainer = "PART_Container";

        Border _container;

        TapGestureRecognizer _tapGestureRecognizer;
        PointerGestureRecognizer _pointerGestureRecognizer;

        bool _isMouseOver;

        public static readonly BindableProperty IsToggledProperty =
            BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(ToggleButton), false,
                propertyChanged: OnIsToggledChanged);

        static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ToggleButton).UpdateIsToggled();
        }

        public bool IsToggled
        {
            get => (bool)GetValue(IsToggledProperty);
            set => SetValue(IsToggledProperty, value);
        }

        public static new readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(nameof(Background), typeof(Brush), typeof(ToggleButton), null, 
                propertyChanged: OnToggleButtonPropertyChanged);

        public new Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }
    
        public static readonly BindableProperty ToggledBackgroundProperty =
            BindableProperty.Create(nameof(ToggledBackground), typeof(Brush), typeof(ToggleButton), null);

        public Brush ToggledBackground
        {
            get => (Brush)GetValue(ToggledBackgroundProperty);
            set { SetValue(ToggledBackgroundProperty, value); }
        }

        public static readonly BindableProperty BorderBrushProperty =
            BindableProperty.Create(nameof(BorderBrush), typeof(Brush), typeof(ToggleButton), null,
                propertyChanged: OnToggleButtonPropertyChanged);

        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }
       
        public static readonly BindableProperty ToggledBorderBrushProperty =
            BindableProperty.Create(nameof(ToggledBorderBrush), typeof(Brush), typeof(ToggleButton), null);

        public Brush ToggledBorderBrush
        {
            get => (Brush)GetValue(ToggledBorderBrushProperty);
            set { SetValue(ToggledBorderBrushProperty, value); }
        }

        public static readonly BindableProperty BorderThicknessProperty = ButtonBase.BorderThicknessProperty;

        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty = 
            BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(ToggleButton), 6.0d,
                propertyChanged: OnCornerRadiusChanged);

        static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ToggleButton).UpdateCornerRadius();
        }

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = ButtonBase.TextColorProperty;

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty ToggledTextColorProperty =
            BindableProperty.Create(nameof(ToggledTextColor), typeof(Color), typeof(ToggleButton), Colors.Transparent);

        public Color ToggledTextColor
        {
            get => (Color)GetValue(ToggledTextColorProperty);
            set { SetValue(ToggledTextColorProperty, value); }
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
            get => GetValue(CommandParameterProperty);
            set { SetValue(CommandParameterProperty, value); }
        }

        internal static readonly BindablePropertyKey CurrentBackgroundPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentBackground), typeof(Brush), typeof(ToggleButton), null);

        public static readonly BindableProperty CurrentBackgroundProperty = CurrentBackgroundPropertyKey.BindableProperty;

        public Brush CurrentBackground
        {
            get
            {
                return (Brush)GetValue(CurrentBackgroundProperty);
            }
            private set
            {
                SetValue(CurrentBackgroundPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentBorderBrushPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentBorderBrush), typeof(Brush), typeof(ToggleButton), null);

        public static readonly BindableProperty CurrentBorderBrushProperty = CurrentBorderBrushPropertyKey.BindableProperty;

        public Brush CurrentBorderBrush
        {
            get
            {
                return (Brush)GetValue(CurrentBorderBrushProperty);
            }
            private set
            {
                SetValue(CurrentBorderBrushPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentTextColorPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentTextColor), typeof(Color), typeof(ToggleButton), Colors.Transparent);

        public static readonly BindableProperty CurrentTextColorProperty = CurrentTextColorPropertyKey.BindableProperty;

        public Color CurrentTextColor
        {
            get
            {
                return (Color)GetValue(CurrentTextColorProperty);
            }
            private set
            {
                SetValue(CurrentTextColorPropertyKey, value);
            }
        }

        static void OnToggleButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ToggleButton)?.UpdateToggleButton();
        }

        internal bool IsMouseHover => _isMouseOver;

        public event EventHandler<IsToggledEventArgs> IsToggledChanged;

        public View ContentAsString(object content)
        {
            return new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontFamily = FontFamily,
                FontSize = FontSize,
                TextColor = CurrentTextColor,
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
            if (IsToggled)
            {
                if (!IsEnabled)
                    VisualStateManager.GoToState(this, ToggleButtonVisualState.ToggledDisabled.ToString());            
                else if (IsMouseHover)
                    VisualStateManager.GoToState(this, ToggleButtonVisualState.MouseOver.ToString());
                else
                    VisualStateManager.GoToState(this, ToggleButtonVisualState.Toggled.ToString());
            }
            else
            {
                if (!IsEnabled)
                    VisualStateManager.GoToState(this, ToggleButtonVisualState.Disabled.ToString());
                else if (IsMouseHover)
                    VisualStateManager.GoToState(this, ToggleButtonVisualState.MouseOver.ToString());
                else
                    VisualStateManager.GoToState(this, ToggleButtonVisualState.Normal.ToString());
            }    
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                _tapGestureRecognizer.Tapped += OnToggleButtonTapped;
                _container.GestureRecognizers.Add(_tapGestureRecognizer);

                _pointerGestureRecognizer.PointerEntered += OnToggleButtonPointerEntered;
                _pointerGestureRecognizer.PointerExited += OnToggleButtonHandlePointerExited;
                _container.GestureRecognizers.Add(_pointerGestureRecognizer);
            }
            else
            {
                if (_tapGestureRecognizer is not null)
                {
                    _tapGestureRecognizer.Tapped -= OnToggleButtonTapped;
                    _container.GestureRecognizers.Remove(_tapGestureRecognizer);
                }

                if (_pointerGestureRecognizer is not null)
                {
                    _pointerGestureRecognizer.PointerEntered -= OnToggleButtonPointerEntered;
                    _pointerGestureRecognizer.PointerExited -= OnToggleButtonHandlePointerExited;
                    _container.GestureRecognizers.Remove(_pointerGestureRecognizer);
                }
            }

            ChangeVisualState();
        }

        void UpdateIsToggled()
        {
            RaiseIsToggledChanged(IsToggled);
            UpdateToggleButton();
        }

        void UpdateCornerRadius()
        {
            if (_container is not null && _container.StrokeShape is RoundRectangle strokeShape)
            {
                strokeShape.CornerRadius = new CornerRadius(CornerRadius);
            }
        }

        void UpdateToggleButton()
        {
            CurrentBackground = !IsToggled ? Background : ToggledBackground;
            CurrentBorderBrush = !IsToggled ? BorderBrush : ToggledBorderBrush;
            CurrentTextColor = !IsToggled ? TextColor : ToggledTextColor;
        }

        void RaiseIsToggledChanged(bool isToggled)
        {
            IsToggledChanged?.Invoke(this, new IsToggledEventArgs(isToggled));
            ChangeVisualState();
        }

        void OnToggleButtonTapped(object sender, TappedEventArgs e)
        {
            IsToggled = !IsToggled;

            if (Command is not null && Command.CanExecute(CommandParameter))
                Command.Execute(null);
        }

        void OnToggleButtonPointerEntered(object sender, PointerEventArgs e)
        {
            _isMouseOver = true;
            ChangeVisualState();
        }

        void OnToggleButtonHandlePointerExited(object sender, PointerEventArgs e)
        {
            _isMouseOver = false;
            ChangeVisualState();
        }
    }
}