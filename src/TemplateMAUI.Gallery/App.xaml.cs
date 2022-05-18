using TemplateMAUI.Gallery.Views;

namespace TemplateMAUI.Gallery
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            TemplateMAUI.Init();

            MainPage = new CustomNavigationPage(new MainView());
        }
    }
}