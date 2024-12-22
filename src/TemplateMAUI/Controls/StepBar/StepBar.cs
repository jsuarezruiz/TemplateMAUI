namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The StepBar is a custom templated control designed to represent a step bar or progress indicator within a user interface. 
    /// It provides the functionality to display a series of steps, indicating the user's progress through a multi-step process.
    /// </summary>
    [ContentProperty(nameof(Items))]
    public class StepBar : TemplatedView
    {
        const string ElementProgressBar = "PART_ProgressBar";
        const string ElementItems = "PART_Container";

        ProgressBar _progress;
        Layout _container;

        public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(StepBarItems), typeof(StepBar), new StepBarItems(),
            propertyChanged: OnItemsChanged);

        static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as StepBar)?.UpdateStepItems();
        }

        public StepBarItems Items
        {
            get => (StepBarItems)GetValue(ItemsProperty);
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly BindableProperty StepIndexProperty =
            BindableProperty.Create(nameof(Index), typeof(int), typeof(StepBar), 0, coerceValue: CoerceStepIndex,
                propertyChanged: OnStepIndexChanged);

        static object CoerceStepIndex(BindableObject bindable, object value)
        {
            var ctl = (StepBar)bindable;
            int stepIndex = (int)value;

            if (ctl.Items.Count == 0 && stepIndex > 0)
                return 0;

            return stepIndex < 0
                ? 0
                : stepIndex >= ctl.Items.Count
                    ? ctl.Items.Count == 0 ? 0 : ctl.Items.Count - 1
                    : value;
        }

        static void OnStepIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var ctl = (StepBar)bindable;
            ctl.OnStepIndexChanged((int)newValue);
        }

        void OnStepIndexChanged(int stepIndex)
        {
            UpdateStepItemsStatus(stepIndex);

            double progress = (double)(decimal.Divide(1, Items.Count - 1) * stepIndex);
            _progress.Progress = progress;

            StepIndexChanged?.Invoke(this, new SelectedIndexEventArgs(stepIndex));
        }

        void UpdateStepItemsStatus(int stepIndex)
        {
            if (_container is null)
                return;

            for (int i = 0; i < stepIndex; i++)
            {
                if (_container.Children[i] is StepBarItem stepItemFinished)
                {
                    stepItemFinished.Status = StepStatus.Complete;
                }
            }

            for (int i = stepIndex + 1; i < Items.Count; i++)
            {
                if (_container.Children[i] is StepBarItem stepItemFinished)
                {
                    stepItemFinished.Status = StepStatus.NotStarted;
                }
            }

            if (_container.Children[stepIndex] is StepBarItem stepItemSelected)
            {
                stepItemSelected.Status = StepStatus.InProgress;
            }
        }

        public int StepIndex
        {
            get => (int)GetValue(StepIndexProperty);
            set => SetValue(StepIndexProperty, value);
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(nameof(Color), typeof(Color), typeof(StepBar), Colors.Transparent);

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set { SetValue(ColorProperty, value); }
        }

        public static readonly BindableProperty ColorSelectedProperty =
            BindableProperty.Create(nameof(ColorSelected), typeof(Color), typeof(StepBar), Colors.Transparent);

        public Color ColorSelected
        {
            get => (Color)GetValue(ColorSelectedProperty);
            set { SetValue(ColorSelectedProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
           BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(StepBar), Colors.Transparent);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextColorSelectedProperty =
            BindableProperty.Create(nameof(TextColorSelected), typeof(Color), typeof(StepBar), Colors.Transparent);

        public Color TextColorSelected
        {
            get => (Color)GetValue(TextColorSelectedProperty);
            set { SetValue(TextColorSelectedProperty, value); }
        }
        
        public event EventHandler<SelectedIndexEventArgs> StepIndexChanged;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_container is not null)
                _container.SizeChanged -= OnContainerSizeChanged;

            _container = GetTemplateChild(ElementItems) as Layout;
            _progress = GetTemplateChild(ElementProgressBar) as ProgressBar;

            if (_container is not null)
                _container.SizeChanged += OnContainerSizeChanged;
        }

        public void Next() => StepIndex++;

        public void Prev() => StepIndex--;

        void UpdateStepItems()
        {
            if (Items is null || Items.Count == 0)
                return;

            if (_container is not Grid gridContainer)
                return;

            gridContainer.Children.Clear();

            int index = 1;
            foreach (var item in Items)
            {
                if (!gridContainer.Children.Contains(item))
                {
                    UpdateStepBarItem(item, index);

                    gridContainer.ColumnDefinitions.Add(new ColumnDefinition());
                    gridContainer.Children.Add(item);
                    Grid.SetColumn(item, index - 1);

                    index++;
                }
            }

            UpdateItemsLayout();
            UpdateStepItemsStatus(StepIndex);
        }

        void UpdateStepBarItem(StepBarItem stepBarItem, int index)
        {
            if (stepBarItem == null)
                return;

            stepBarItem.Index = index;

            if (stepBarItem.Color == Colors.Transparent)
                stepBarItem.Color = Color;

            if (stepBarItem.ColorSelected == Colors.Transparent)
                stepBarItem.ColorSelected = ColorSelected;

            if (stepBarItem.TextColor == Colors.Transparent)
                stepBarItem.TextColor = TextColor;

            if (stepBarItem.TextColorSelected == Colors.Transparent)
                stepBarItem.TextColorSelected = TextColorSelected;
        }

        void UpdateItemsLayout()
        {
            if (Items is null || Items.Count == 0)
                return;

            if (_container.Width <= 0 || _container.Height <= 0)
                return;

            var stepWidth = _container.Width / Items.Count;

            _progress.Margin = new Thickness(stepWidth / 2, 0);
        }

        void OnContainerSizeChanged(object sender, EventArgs e)
        {
            UpdateItemsLayout();
        }
    }
}