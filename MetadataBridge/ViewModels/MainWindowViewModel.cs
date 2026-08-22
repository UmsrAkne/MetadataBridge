using MetadataBridge.Utils;

namespace MetadataBridge.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private string title = "MetadataBridge";

        public MainWindowViewModel()
        {
            AppLogger.Info("MainWindowViewModel created");
        }

        public string Title { get => title; set => SetProperty(ref title, value); }
    }
}