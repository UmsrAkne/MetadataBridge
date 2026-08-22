using System.IO;

namespace MetadataBridge.ViewModels
{
    public class ImageViewModel : BindableBase
    {
        private FileInfo fileInfo;

        public ImageViewModel(string info)
        {
            FileInfo = new FileInfo(info);
        }

        public FileInfo FileInfo { get => fileInfo; set => SetProperty(ref fileInfo, value); }
    }
}