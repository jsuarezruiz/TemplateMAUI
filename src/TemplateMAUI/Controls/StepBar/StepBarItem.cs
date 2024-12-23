namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The StepBarItem is a custom templated control designed to represent an individual step within a StepBar control. 
    /// It provides the visual representation and properties for each step, making it easy to customize and display step-by-step progress in a user interface.
    /// </summary>
    public class StepBarItem : TemplatedView
    {
        public static readonly BindableProperty IndexProperty =
            BindableProperty.Create(nameof(Index), typeof(int), typeof(StepBarItem), -1);

        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public static readonly BindableProperty StatusProperty =
            BindableProperty.Create(nameof(Status), typeof(StepStatus), typeof(StepBarItem), StepStatus.NotStarted,
                propertyChanged: OnStepBarItemPropertyChanged);

        public StepStatus Status
        {
            get => (StepStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public static readonly BindableProperty TextProperty =   
            BindableProperty.Create(nameof(Text), typeof(string), typeof(StepBarItem), string.Empty);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty ColorProperty =
           BindableProperty.Create(nameof(Color), typeof(Color), typeof(StepBarItem), Colors.Transparent,
               propertyChanged: OnStepBarItemPropertyChanged);

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set { SetValue(ColorProperty, value); }
        }

        public static readonly BindableProperty ColorSelectedProperty =
            BindableProperty.Create(nameof(ColorSelected), typeof(Color), typeof(StepBarItem), Colors.Transparent);

        public Color ColorSelected
        {
            get => (Color)GetValue(ColorSelectedProperty);
            set { SetValue(ColorSelectedProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
           BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(StepBarItem), Colors.Transparent,
               propertyChanged: OnStepBarItemPropertyChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextColorSelectedProperty =
            BindableProperty.Create(nameof(TextColorSelected), typeof(Color), typeof(StepBarItem), Colors.Transparent);

        public Color TextColorSelected
        {
            get => (Color)GetValue(TextColorSelectedProperty);
            set { SetValue(TextColorSelectedProperty, value); }
        }

        internal static readonly BindablePropertyKey CurrentColorPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentColor), typeof(Color), typeof(StepBarItem), Colors.Transparent);

        public static readonly BindableProperty CurrentColorProperty = CurrentColorPropertyKey.BindableProperty;

        public Color CurrentColor
        {
            get
            {
                return (Color)GetValue(CurrentColorProperty);
            }
            private set
            {
                SetValue(CurrentColorPropertyKey, value);
            }
        }

        internal static readonly BindablePropertyKey CurrentTextColorPropertyKey =  
            BindableProperty.CreateReadOnly(nameof(CurrentTextColor), typeof(Color), typeof(StepBarItem), Colors.Transparent);

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

        static void OnStepBarItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as StepBarItem)?.UpdateCurrent();
        }

        void UpdateCurrent()
        {
            bool isSelected = Status != StepStatus.NotStarted;
            CurrentColor = !isSelected || ColorSelected == Colors.Transparent ? Color : ColorSelected;
            CurrentTextColor = !isSelected || TextColorSelected == Colors.Transparent ? TextColor : TextColorSelected;
        }
    }
}