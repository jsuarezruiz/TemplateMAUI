namespace TemplateMAUI.Controls
{
    public class PropertyItem : BindableObject
    {
        public static readonly BindableProperty CategoryProperty =
            BindableProperty.Create(nameof(Category), typeof(string), typeof(PropertyItem), string.Empty);

        public string Category
        {
            get => (string)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }

        public static readonly BindableProperty DisplayNameProperty =
            BindableProperty.Create(nameof(DisplayName), typeof(string), typeof(PropertyItem), string.Empty);

        public string DisplayName
        {
            get => (string)GetValue(DisplayNameProperty);
            set => SetValue(DisplayNameProperty, value);
        }

        public static readonly BindableProperty DescriptionProperty =
            BindableProperty.Create(nameof(Description), typeof(string), typeof(PropertyItem), string.Empty);

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly BindableProperty IsReadOnlyProperty =
            BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(PropertyItem), default(bool));

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }
        
        public static readonly BindableProperty ValueProperty = 
            BindableProperty.Create(nameof(Value), typeof(object), typeof(PropertyItem), default(object));

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly BindableProperty DefaultValueProperty =       
            BindableProperty.Create(nameof(DefaultValue), typeof(object), typeof(PropertyItem), default(object));

        public object DefaultValue
        {
            get => GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        public static readonly BindableProperty EditorProperty =
            BindableProperty.Create(nameof(Editor), typeof(PropertyEditorBase), typeof(PropertyItem), default(PropertyEditorBase));

        public PropertyEditorBase Editor
        {
            get => (PropertyEditorBase)GetValue(EditorProperty);
            set => SetValue(EditorProperty, value);
        }

        public static readonly BindableProperty EditorElementProperty =
            BindableProperty.Create(nameof(EditorElement), typeof(View), typeof(PropertyItem), default(View));

        public View EditorElement
        {
            get => (View)GetValue(EditorElementProperty);
            set => SetValue(EditorElementProperty, value);
        }

        public static readonly BindableProperty PropertyNameProperty =
            BindableProperty.Create(nameof(PropertyName), typeof(string), typeof(PropertyItem), string.Empty);

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static readonly BindableProperty PropertyTypeProperty =      
            BindableProperty.Create(nameof(PropertyType), typeof(Type), typeof(PropertyItem), default(Type));

        public Type PropertyType
        {
            get => (Type)GetValue(PropertyTypeProperty);
            set => SetValue(PropertyTypeProperty, value);
        }

        public virtual void Initialize()
        {
            if (Editor is null) 
                return;

            EditorElement = Editor.CreateView(this);
            EditorElement.BindingContext = Value;
            Editor.CreateBinding(this, EditorElement);
        }
    }
}