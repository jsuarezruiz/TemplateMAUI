namespace TemplateMAUI.Gallery.Views;

public partial class ProgressButtonGallery : ContentPage
{
	public ProgressButtonGallery()
	{
		InitializeComponent();
	}

    async void OnProgressButtonClicked(object sender, EventArgs e)
    {
        ProgressButton1.IsBusy = ProgressButton2.IsBusy = true;
        await Task.Delay(5000);
        ProgressButton1.IsBusy = ProgressButton2.IsBusy = false;
    }
}