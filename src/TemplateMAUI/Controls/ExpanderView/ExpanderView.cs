using System.Runtime.CompilerServices;

namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The ExpanderView class is a custom templated control designed to represent an expandable section of content within a user interface. 
    /// It provides the functionality to show or hide additional content when the header is clicked, making it useful for creating collapsible panels or accordion-style interfaces.
    /// </summary>
    public class ExpanderView : TemplatedView
    {
        const string ElementContainer = "PART_Container";
        const string ElementHeader = "PART_Header";
        const string ElementContent = "PART_Content";

        Grid _container;
        ContentView _header;
        ContentView _content;

        public static readonly BindableProperty HeaderProperty =
            BindableProperty.Create(nameof(Header), typeof(View), typeof(ExpanderView), null);

        public View Header
        {
            get => (View)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(ExpanderView), null);

        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly BindableProperty IsExpandedProperty =
            BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(ExpanderView), true,
                propertyChanged: OnIsExpandedChanged);

        static async void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            await (bindable as ExpanderView)?.UpdateIsExpandedAsync();
        }

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public static BindableProperty ExpanderAnimationProperty = 
            BindableProperty.Create(nameof(ExpanderAnimation), typeof(IExpanderAnimation), typeof(ExpanderView), new ExpanderAnimation());

        public IExpanderAnimation ExpanderAnimation
        {
            get { return (IExpanderAnimation)GetValue(ExpanderAnimationProperty); }
            set { SetValue(ExpanderAnimationProperty, value); }
        }

        public static readonly BindableProperty ExpandDirectionProperty =
           BindableProperty.Create(nameof(ExpandDirection), typeof(ExpandDirection), typeof(ExpanderView), ExpandDirection.Down,
               propertyChanged: OnExpandDirectionChanged);

        static void OnExpandDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ExpanderView)?.UpdateExpandDirection();
        }

        public ExpandDirection ExpandDirection
        {
            get => (ExpandDirection)GetValue(ExpandDirectionProperty);
            set => SetValue(ExpandDirectionProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = GetTemplateChild(ElementContainer) as Grid;
            _header = GetTemplateChild(ElementHeader) as ContentView;
            _content = GetTemplateChild(ElementContent) as ContentView;

            UpdateIsEnabled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        async Task UpdateIsExpandedAsync()
        {
            if (IsExpanded)
            {
                _content.IsVisible = true;

                await ExpanderAnimation.OnExpand(_content);

            }
            else
            {
                await ExpanderAnimation.OnCollapse(_content);

                _content.IsVisible = false;
            }
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                var headerTapGestureRecognizer = new TapGestureRecognizer();
                headerTapGestureRecognizer.Tapped += OnHeaderTapped;
                _header.GestureRecognizers.Add(headerTapGestureRecognizer);
            }
            else
            {
                _header.GestureRecognizers.Clear();
            }
        }

        void UpdateExpandDirection()
        {
            if (_container is null)
                return;
            
            _container.Children.Remove(_header);
            _container.Children.Remove(_content);

            _container.ColumnDefinitions.Clear();
            _container.RowDefinitions.Clear();

            switch (ExpandDirection)
            {
                case ExpandDirection.Down:
                    _container.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                    _container.RowDefinitions.Add(new RowDefinition(GridLength.Star));

                    _container.Children.Add(_header);
                    Grid.SetRow(_header, 0);
                    _container.Children.Add(_content);
                    Grid.SetRow(_content, 1);
                    break;
                case ExpandDirection.Up:
                    _container.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                    _container.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                
                    _container.Children.Add(_content);
                    Grid.SetRow(_content, 0);
                    _container.Children.Add(_header);
                    Grid.SetRow(_header, 1);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        void OnHeaderTapped(object sender, TappedEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }
    }
}