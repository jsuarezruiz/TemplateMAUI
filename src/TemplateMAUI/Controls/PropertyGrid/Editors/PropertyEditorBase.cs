namespace TemplateMAUI.Controls
{
    public abstract class PropertyEditorBase : BindableObject
    {
        public abstract View CreateView(PropertyItem propertyItem);
        public abstract BindableProperty GetBindableProperty();

        public virtual void CreateBinding(PropertyItem propertyItem, BindableObject element)
        {
            element.SetBinding
                (GetBindableProperty(),
                propertyItem.PropertyName,           
                GetBindingMode(propertyItem),     
                GetConverter(propertyItem));
        }

        public virtual BindingMode GetBindingMode(PropertyItem propertyItem) => propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;
        protected virtual IValueConverter GetConverter(PropertyItem propertyItem) => null;
    }
}
