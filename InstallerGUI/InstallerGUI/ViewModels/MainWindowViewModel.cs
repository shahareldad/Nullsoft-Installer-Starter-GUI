using InstallerGUI.Contracts;
using InstallerGUI.Infrastructure;
using System;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace InstallerGUI.ViewModels
{
    public delegate string GetFilenameToSaveHandler();

    public class MainWindowViewModel : BaseViewModel, IRequestOpenFileExplorer
    {
        public event GetFilenameToSaveHandler GetFilenameToSaveEvent;

        public GeneralViewModel GeneralViewModel { get; set; }

        public FilesViewModel FilesViewModel { get; set; }

        public RegistryViewModel RegistryViewModel { get; set; }

        public PagesViewModel PagesViewModel { get; set; }

        public SectionsViewModel SectionsViewModel { get; set; }

        public ICommand CreateNsiFileCommand { get; set; }

        public MainWindowViewModel()
        {
            GeneralViewModel = new GeneralViewModel();
            FilesViewModel = new FilesViewModel(GeneralViewModel);
            RegistryViewModel = new RegistryViewModel();
            PagesViewModel = new PagesViewModel();
            SectionsViewModel = new SectionsViewModel(FilesViewModel, RegistryViewModel);

            CreateNsiFileCommand = new CommandAction(CreateNsiFileCommandAction);
        }

        private void CreateNsiFileCommandAction()
        {
            var sb = new StringBuilder();

            if (RegistryViewModel.RegistrySectionNeeded)
            {
                sb.Append("!include Registry.nsh" + Environment.NewLine + Environment.NewLine);
            }

            sb.Append(GeneralViewModel.GetDataToNsi());
            sb.Append(PagesViewModel.GetDataToNsi());
            sb.Append(SectionsViewModel.GetDataToNsi());

            var filename = GetFilenameToSaveEvent?.Invoke();

            if (!string.IsNullOrWhiteSpace(filename))
            {
                File.WriteAllText(filename, sb.ToString());
            }
        }
    }
}