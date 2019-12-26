using InstallerGUI.Infrastructure;

namespace InstallerGUI.Models
{
    public class InstallationFile : INPC
    {
        private string _destinationFolder;
        private string _sourceFullPath;

        public string SourceFullPath
        {
            get { return _sourceFullPath; }
            set
            {
                _sourceFullPath = value;
                OnPropertyChanged(nameof(SourceFullPath));
            }
        }

        public string DestinationFolder
        {
            get { return _destinationFolder; }
            set
            {
                _destinationFolder = value;
                OnPropertyChanged(nameof(DestinationFolder));
            }
        }
    }
}
