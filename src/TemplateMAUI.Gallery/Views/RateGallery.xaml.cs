using ValueChangedEventArgs = TemplateMAUI.Controls.ValueChangedEventArgs;

namespace TemplateMAUI.Gallery.Views
{
    public partial class RateGallery : TabbedPage
    {
        public RateGallery()
        {
            InitializeComponent();
        }

        void OnRateValueChanged(object sender, ValueChangedEventArgs e)
        {
            DisplayAlert("ValueChanged", $"The value is {e.Value}", "Ok");
        }
    }
}