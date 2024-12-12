using System.ComponentModel;
using System.Data;
using TemplateMAUI.Extensions;

namespace TemplateMAUI.Controls
{
    public class PropertyGrid : TemplatedView
    {
        const int SearchDelay = 500;

        const string ElementSearchBar = "PART_SearchBar";
        const string ElementItemsControl = "PART_Items";

        SearchBar _searchBar;
        CollectionView _itemsControl;

        public static readonly BindableProperty SelectedObjectProperty =
            BindableProperty.Create(nameof(SelectedObject), typeof(object), typeof(PropertyGrid), null,
                propertyChanged: OnSelectedObjectChanged);

        static void OnSelectedObjectChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var ctl = (PropertyGrid)bindable;
            ctl.OnSelectedObjectChanged(oldValue, newValue);
        }

        public object SelectedObject
        {
            get => GetValue(SelectedObjectProperty);
            set => SetValue(SelectedObjectProperty, value);
        }

        public event EventHandler SelectedObjectChanged;

        public virtual PropertyResolver PropertyResolver { get; } = new();

        protected override void OnApplyTemplate()
        {
            if (_searchBar != null)
            {
                _searchBar.TextChanged -= OnSearchBarTextChanged;
                _searchBar.SearchButtonPressed -= OnSearchButtonPressed;
            }

            base.OnApplyTemplate();

            _searchBar = GetTemplateChild(ElementSearchBar) as SearchBar;
            _itemsControl = GetTemplateChild(ElementItemsControl) as CollectionView;

            if (_searchBar is not null)
            {
                _searchBar.TextChanged += OnSearchBarTextChanged;
                _searchBar.SearchButtonPressed += OnSearchButtonPressed;
            }
        }

        protected virtual void OnSelectedObjectChanged(object oldValue, object newValue)
        {
            UpdateItems(newValue);
            SelectedObjectChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual PropertyItem CreatePropertyItem(PropertyDescriptor propertyDescriptor) => new()
        {
            Category = PropertyResolver.ResolveCategory(propertyDescriptor),
            DisplayName = PropertyResolver.ResolveDisplayName(propertyDescriptor),
            Description = PropertyResolver.ResolveDescription(propertyDescriptor),
            IsReadOnly = PropertyResolver.ResolveIsReadOnly(propertyDescriptor),
            Value = SelectedObject,
            DefaultValue = PropertyResolver.ResolveDefaultValue(propertyDescriptor),
            Editor = PropertyResolver.ResolveEditor(propertyDescriptor),
            PropertyName = propertyDescriptor.Name,
            PropertyType = propertyDescriptor.PropertyType,
        };

        void UpdateItems(object obj)
        {
            if (obj is null || _itemsControl is null) 
                return;

            var items = GetPropertyItems();

            if (_searchBar is not null)
            {
                string text = _searchBar.Text;
                bool searched = Search(text);

                if (searched)
                    return;
            }
            
            _itemsControl.ItemsSource = items;
        }

        IEnumerable<PropertyItem> GetPropertyItems()
        {  
            var items = TypeDescriptor.GetProperties(SelectedObject.GetType())
                .OfType<PropertyDescriptor>()
                .Where(PropertyResolver.ResolveIsBrowsable).Select(CreatePropertyItem)
                .Invoke(item => item.Initialize());

            return items;
        }

        bool Search(string text)
        {
            if (SelectedObject is null)
                return false;

            var items = GetPropertyItems();

            if (!string.IsNullOrEmpty(text))
                items = items.Where(item => item.DisplayName.Contains(text, StringComparison.InvariantCultureIgnoreCase));

            _itemsControl.ItemsSource = items;

            return true;
        }

        void Search()
        {
            string text = _searchBar.Text;
            Search(text);
        }

        CancellationTokenSource cancellationToken = new CancellationTokenSource();

        async void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Interlocked.Exchange(ref cancellationToken, new CancellationTokenSource()).Cancel();

                await Task.Delay(TimeSpan.FromMilliseconds(SearchDelay), cancellationToken.Token)
                    .ContinueWith(task => Search(),
                            CancellationToken.None,
                            TaskContinuationOptions.OnlyOnRanToCompletion,
                            TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch
            {

            }
        }

        void OnSearchButtonPressed(object sender, EventArgs e)
        {
            string text = _searchBar.Text;
            Search(text);
        }
    }
}
