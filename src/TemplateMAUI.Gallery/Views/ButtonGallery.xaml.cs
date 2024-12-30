namespace TemplateMAUI.Gallery.Views;

public partial class ButtonGallery : ContentPage
{
	public ButtonGallery()
	{
		InitializeComponent();
	}

    void OnButtonClicked(object sender, EventArgs e)
    {
		DisplayAlert("Button Clicked", "Hello from the Button!", "OK");
    }
}