namespace TemplateMAUI.Gallery.Views;

public partial class ProgressButtonGallery : TabbedPage
{
	public ProgressButtonGallery()
	{
		InitializeComponent();
	}

    async void OnProgressButtonClicked(object sender, EventArgs e)
    {
        ProgressButton1.IsBusy = 
            ProgressButton2.IsBusy =
            GradientProgressButton.IsBusy =
            CornerRadiusProgressButton.IsBusy = true;
        await Task.Delay(5000);
        ProgressButton1.IsBusy = 
            ProgressButton2.IsBusy =
            GradientProgressButton.IsBusy =
            CornerRadiusProgressButton.IsBusy = false;
    }
}