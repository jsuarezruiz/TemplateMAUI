using System.ComponentModel;

namespace TemplateMAUI.Controls
{
    public class PropertyResolver
    {
        static readonly Dictionary<Type, PropertyEditorType> TypeCodes = new()
        {
            [typeof(string)] = PropertyEditorType.String,
            [typeof(byte)] = PropertyEditorType.ByteNumber,
            [typeof(short)] = PropertyEditorType.Int16Number,
            [typeof(int)] = PropertyEditorType.Int32Number,
            [typeof(long)] = PropertyEditorType.Int64Number,
            [typeof(float)] = PropertyEditorType.SingleNumber,
            [typeof(double)] = PropertyEditorType.DoubleNumber,
            [typeof(bool)] = PropertyEditorType.Bool,
            [typeof(DateOnly)] = PropertyEditorType.Date,
            [typeof(DateTime)] = PropertyEditorType.Date,
            [typeof(TimeOnly)] = PropertyEditorType.Time,
        }; 
        
        public string ResolveCategory(PropertyDescriptor propertyDescriptor)
        {
            var categoryAttribute = propertyDescriptor.Attributes.OfType<CategoryAttribute>().FirstOrDefault();

            if(!string.IsNullOrEmpty(categoryAttribute?.Category))
            {
                return categoryAttribute.Category;
            }

            return string.Empty;
        }

        public string ResolveDisplayName(PropertyDescriptor propertyDescriptor)
        {
            var displayName = propertyDescriptor.DisplayName;

            if (string.IsNullOrEmpty(displayName))
            {
                displayName = propertyDescriptor.Name;
            }

            return displayName;
        }

        public string ResolveDescription(PropertyDescriptor propertyDescriptor) => propertyDescriptor.Description;
        public bool ResolveIsBrowsable(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsBrowsable;
        public bool ResolveIsLocalizable(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsLocalizable;
        public bool ResolveIsReadOnly(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsReadOnly;
      
        public object ResolveDefaultValue(PropertyDescriptor propertyDescriptor)
        {
            var defaultValueAttribute = propertyDescriptor.Attributes.OfType<DefaultValueAttribute>().FirstOrDefault();
            return defaultValueAttribute?.Value;
        }

        public PropertyEditorBase ResolveEditor(PropertyDescriptor propertyDescriptor)
        {
            var editorAttribute = propertyDescriptor.Attributes.OfType<EditorAttribute>().FirstOrDefault();
            var editor = editorAttribute == null || string.IsNullOrEmpty(editorAttribute.EditorTypeName)
                ? CreateDefaultEditor(propertyDescriptor.PropertyType)
                : CreateEditor(Type.GetType(editorAttribute.EditorTypeName));

            return editor;
        }

        public virtual PropertyEditorBase CreateDefaultEditor(Type type) =>
        TypeCodes.TryGetValue(type, out var editorType)
            ? editorType switch
            {
                PropertyEditorType.String => new StringPropertyEditor(),
                PropertyEditorType.ByteNumber => new NumberPropertyEditor(byte.MinValue, byte.MaxValue),
                PropertyEditorType.Int16Number => new NumberPropertyEditor(short.MinValue, short.MaxValue),
                PropertyEditorType.Int32Number => new NumberPropertyEditor(int.MinValue, int.MaxValue),
                PropertyEditorType.Int64Number => new NumberPropertyEditor(long.MinValue, long.MaxValue),
                PropertyEditorType.SingleNumber => new NumberPropertyEditor(float.MinValue, float.MaxValue),
                PropertyEditorType.DoubleNumber => new NumberPropertyEditor(double.MinValue, double.MaxValue),
                PropertyEditorType.Bool => new BoolPropertyEditor(),
                PropertyEditorType.Date => new DatePropertyEditor(),
                PropertyEditorType.Time => new TimePropertyEditor(),
                _ => new ReadOnlyPropertyEditor()
            }
            : type.IsSubclassOf(typeof(Enum))
                ? new EnumPropertyEditor()
                : new ReadOnlyPropertyEditor();

        public virtual PropertyEditorBase CreateEditor(Type type) => Activator.CreateInstance(type) as PropertyEditorBase ?? new ReadOnlyPropertyEditor();
    }
}