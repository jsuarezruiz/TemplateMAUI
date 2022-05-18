using System.Collections.ObjectModel;
using System.Windows.Input;
using TemplateMAUI.Gallery.Models;
using TemplateMAUI.Gallery.Services;

namespace TemplateMAUI.Gallery.ViewModels
{
    public class MainViewModel : BindableObject
    {
        ObservableCollection<GalleryItem> _trending;
        ObservableCollection<GalleryItem> _gallery;

        public MainViewModel()
        {
            LoadData();
        }

        public ObservableCollection<GalleryItem> Trending
        {
            get { return _trending; }
            set
            {
                _trending = value;
                OnPropertyChanged();
            }
        }

        public ICommand TrendingCommand => new Command<GalleryItem>(NavigateToGallery);

        public ObservableCollection<GalleryItem> Gallery
        {
            get { return _gallery; }
            set
            {
                _gallery = value;
                OnPropertyChanged();
            }
        }

        public ICommand GalleryCommand => new Command<GalleryItem>(NavigateToGallery);

        public ICommand GitHubCommand => new Command(OpenGitHubCommand);

        void LoadData()
        {
            Trending = new ObservableCollection<GalleryItem>
            {
                new GalleryItem { Title = "Rate", SubTitle = "Allows users to select a rating value from a group of visual symbols like stars.", Icon = "rate.png", Color = Colors.DarkTurquoise },
             };

            Gallery = new ObservableCollection<GalleryItem>
            {
                new GalleryItem { Title = "Rate", SubTitle = "Allows users to select a rating value from a group of visual symbols like stars.", Icon = "rate.png", Color = Colors.DarkTurquoise },
            };
        }

        void OpenGitHubCommand()
        {
            Browser.OpenAsync("https://github.com/jsuarezruiz/TemplateMAUI", BrowserLaunchMode.External);
        }

        async void NavigateToGallery(GalleryItem galleryItem)
        {
            Type type = Type.GetType($"TemplateMAUI.Gallery.Views.{galleryItem.Title}Gallery");

            if (type != null)
            {
                if (Activator.CreateInstance(type) is Page page)
                {
                    if (galleryItem.Status == GalleryItemStatus.InProgress)
                        await DialogService.Instance.ShowInfoAlert("Work in progress", "This control is in progress and may present visual or functional errors.");

                    await NavigationService.Instance.NavigateAsync(page);
                }
            }
        }
    }
}