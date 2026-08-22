using System.Linq;
using System.Collections.ObjectModel;
using MetadataBridge.Utils;

namespace MetadataBridge.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private string title = "MetadataBridge";
        private ObservableCollection<ImageViewModel> imageViewModels = new ();
        private ImageViewModel? selectedImage;

        public MainWindowViewModel()
        {
            AppLogger.Info("MainWindowViewModel created");
            #if DEBUG
            LoadDebugImages();
            JsonExtractor.ExportWorkflow(ImageViewModels[0].FileInfo.FullName);
            #endif
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

        #if DEBUG
        private void LoadDebugImages()
        {
            try
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var targetDir = System.IO.Path.Combine(desktopPath, "myFiles", "Tests", "MetadataBridge", "images");

                if (System.IO.Directory.Exists(targetDir))
                {
                    var files = System.IO.Directory.GetFiles(targetDir, "*.*")
                        .Where(f => f.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                                    || f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                                    || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase));

                    foreach (var file in files)
                    {
                        ImageViewModels.Add(new ImageViewModel(file));
                    }

                    if (ImageViewModels.Count > 0)
                    {
                        SelectedImage = ImageViewModels[0];
                    }

                    AppLogger.Info($"Loaded {ImageViewModels.Count} debug images from {targetDir}");
                }
                else
                {
                    AppLogger.Warn($"Debug image directory not found: {targetDir}");
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("Error loading debug images", ex);
            }
        }
        #endif

        public ObservableCollection<ImageViewModel> ImageViewModels
        {
            get => imageViewModels;
            set => SetProperty(ref imageViewModels, value);
        }

        public ImageViewModel? SelectedImage { get => selectedImage; set => SetProperty(ref selectedImage, value); }
    }
}