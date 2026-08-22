using System.Collections.ObjectModel;
using System.IO;
using MetadataBridge.Utils;

namespace MetadataBridge.ViewModels
{
    public class ImageViewModel : BindableBase
    {
        private FileInfo fileInfo;
        private ObservableCollection<FileInfo> jsonFiles = new ();

        public ImageViewModel(string info)
        {
            FileInfo = new FileInfo(info);
            var outputPath = JsonExtractor.ExportWorkflow(FileInfo.FullName);
            JsonFiles.Add(outputPath);
            var success = ComfyWorkflowHelper.FixSeedRandomize(outputPath.FullName);
            Console.WriteLine(success);
        }

        public FileInfo FileInfo { get => fileInfo; set => SetProperty(ref fileInfo, value); }

        public ObservableCollection<FileInfo> JsonFiles
        {
            get => jsonFiles;
            set => SetProperty(ref jsonFiles, value);
        }
    }
}