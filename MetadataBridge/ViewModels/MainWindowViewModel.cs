using System.Collections.ObjectModel;
using MetadataBridge.Utils;

namespace MetadataBridge.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private string title = "MetadataBridge";
        private ObservableCollection<ImageViewModel> imageViewModels = new ();

        public MainWindowViewModel()
        {
            AppLogger.Info("MainWindowViewModel created");
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public ObservableCollection<ImageViewModel> ImageViewModels
        {
            get => imageViewModels;
            set => SetProperty(ref imageViewModels, value);
        }
    }
}