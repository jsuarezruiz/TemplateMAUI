using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TemplateMAUI.Gallery.Views
{
    public partial class PropertyGridGallery : ContentPage
    {
        public PropertyGridGallery()
        {
            InitializeComponent();

            Model = new PropertyGridModel
            {
                String = "String",
                Enum = PropertyEnum.Value1,
                Boolean = true,
                Integer = 14,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            };

            BindingContext = this;
        }

        public static readonly BindableProperty ModelProperty =
         BindableProperty.Create(nameof(Model), typeof(PropertyGridModel), typeof(PropertyGridGallery), default(PropertyGridModel));

   
        public PropertyGridModel Model
        {
            get => (PropertyGridModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }
    }

    public class PropertyGridModel : INotifyPropertyChanged
    {
        string _string;
        int _integer;
        bool _boolean;
        PropertyEnum _enum;
        HorizontalAlignment _horizontalAlignment;
        VerticalAlignment _verticalAlignment;

        public string String 
        { 
            get 
            { 
                return _string; 
            } 
            set
            {
                _string = value;
                OnPropertyChanged();
            }
        }

        public int Integer
        {
            get
            {
                return _integer;
            }
            set
            {
                _integer = value;
                OnPropertyChanged();
            }
        }

        public bool Boolean
        {
            get
            {
                return _boolean;
            }
            set
            {
                _boolean = value;
                OnPropertyChanged();
            }
        }

        public PropertyEnum Enum
        {
            get
            {
                return _enum;
            }
            set
            {
                _enum = value;
                OnPropertyChanged();
            }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return _horizontalAlignment;
            }
            set
            {
                _horizontalAlignment = value;
                OnPropertyChanged();
            }
        }

        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return _verticalAlignment;
            }
            set
            {
                _verticalAlignment = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged; 
        
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public enum PropertyEnum
    {
        Value1,
        Value2
    }
}