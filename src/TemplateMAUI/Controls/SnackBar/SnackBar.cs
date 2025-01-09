﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The SnackBar is a custom templated control designed to display brief messages or notifications to users. 
    /// SnackBars typically appear at the bottom of the screen and provide feedback about an operation or offer a quick action for the user to perform.
    /// </summary>
    // TODO: Create a SnackBarManager to allow have methods that can show a SnackBar without adding the SnackBar to the page hierarchy.
    public class SnackBar : TemplatedView
    {
        const string DefaultActionText = "Close";
        const int AutoCloseDuration = 2750;

        const string ElementContainer = "PART_Container";
        const string ElementText = "PART_Text";
        const string ElementAction = "PART_Action";

        Grid _container;
        Label _text;
        Microsoft.Maui.Controls.Button _action;
        SnackBarTimer _timer;

        public static readonly BindableProperty IsOpenProperty =
            BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(SnackBar), false,
                propertyChanged: IsOpenChanged);

        static void IsOpenChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SnackBar)?.UpdateIsOpen();
        }

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set { SetValue(IsOpenProperty, value); }
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SnackBar), Colors.Black);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SnackBar), Colors.White);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty ActionTextColorProperty =
            BindableProperty.Create(nameof(ActionTextColor), typeof(Color), typeof(SnackBar), Colors.White);

        public Color ActionTextColor
        {
            get => (Color)GetValue(ActionTextColorProperty);
            set { SetValue(ActionTextColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(SnackBar), Colors.Transparent);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty MessageProperty =
            BindableProperty.Create(nameof(Message), typeof(string), typeof(SnackBar), string.Empty);

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set { SetValue(MessageProperty, value); }
        }

        public static readonly BindableProperty ActionTextProperty =
            BindableProperty.Create(nameof(ActionText), typeof(string), typeof(SnackBar), DefaultActionText);

        public string ActionText
        {
            get => (string)GetValue(ActionTextProperty);
            set { SetValue(ActionTextProperty, value); }
        }

        public static BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SnackBar), 12.0);

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(SnackBar), string.Empty);

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(SnackBar), 0.0d);

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static BindableProperty AnimationProperty =
            BindableProperty.Create(nameof(Animation), typeof(ISnackBarAnimation), typeof(SnackBar), new SnackBarAnimation());

        public ISnackBarAnimation Animation
        {
            get { return (ISnackBarAnimation)GetValue(AnimationProperty); }
            set { SetValue(AnimationProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_action is not null)
                _action.Clicked -= OnActionClicked;

            _container = GetTemplateChild(ElementContainer) as Grid;
            _text = GetTemplateChild(ElementText) as Label; 
            _action = GetTemplateChild(ElementAction) as Microsoft.Maui.Controls.Button;

            if (_action is not null)
                _action.Clicked += OnActionClicked;

            UpdateIsOpen();
            UpdateIsEnabled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
            else if (propertyName == FlowDirectionProperty.PropertyName)
                UpdateFlowDirection();
        }

        public void Open()
        {
            IsOpen = true;

            _timer?.Stop();

            Animation.OnOpen(this);
        }

        public void Close()
        {
            IsOpen = false;
            Animation.OnClose(this);
        }

        void UpdateIsOpen()
        {
            if (IsOpen)
            {
                Open();

                if (_timer == null)
                {
                    _timer = new SnackBarTimer(TimeSpan.FromMilliseconds(AutoCloseDuration), AutoCloseSnackBar);
                    _timer.Start();
                }
                else
                {
                    _timer.Stop();
                    _timer.Start();
                }
            }
            else
            {
                Close();
            }
        }

        void UpdateIsEnabled()
        {
            _action.IsEnabled = IsEnabled;
        }

        void UpdateFlowDirection()
        {
            _container.FlowDirection = FlowDirection;
        }

        void AutoCloseSnackBar()
        {
            Close();
        }

        void OnActionClicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}