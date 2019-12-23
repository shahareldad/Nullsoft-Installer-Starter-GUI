using InstallerGUI.Contracts;
using InstallerGUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace InstallerGUI.ViewModels
{
    public delegate string GetFilenameToSaveHandler();

    public class MainWindowViewModel : BaseViewModel, IRequestOpenFileExplorer
    {
        public static IEnumerable<string> LoadFileLines;

        public event GetFilenameToSaveHandler GetFilenameToSaveEvent;

        public event GetFilenameToSaveHandler GetFilenameToLoadEvent;

        public GeneralViewModel GeneralViewModel { get; set; }

        public FilesViewModel FilesViewModel { get; set; }

        public RegistryViewModel RegistryViewModel { get; set; }

        public PagesViewModel PagesViewModel { get; set; }

        public SectionsViewModel SectionsViewModel { get; set; }

        public ICommand LoadNsiFileCommand { get; set; }

        public ICommand CreateNsiFileCommand { get; set; }

        public MainWindowViewModel()
        {
            GeneralViewModel = new GeneralViewModel();
            FilesViewModel = new FilesViewModel(GeneralViewModel);
            RegistryViewModel = new RegistryViewModel();
            PagesViewModel = new PagesViewModel();
            SectionsViewModel = new SectionsViewModel(FilesViewModel, RegistryViewModel);

            CreateNsiFileCommand = new CommandAction(CreateNsiFileCommandAction);

            LoadNsiFileCommand = new CommandAction(LoadNsiFileCommandAction);
        }

        private void LoadNsiFileCommandAction()
        {
            var filename = GetFilenameToLoadEvent?.Invoke();

            if (!File.Exists(filename)) return;

            var nsiContent = File.ReadAllLines(filename).ToList();
            int length = nsiContent.Count;
            for (int index = 0; index < length; index++)
            {
                nsiContent[index] = nsiContent[index].Replace("  ", " ");
                if (!nsiContent[index].EndsWith("\\")) continue;

                var lineLength = nsiContent[index].Length;
                nsiContent[index] = nsiContent[index].Remove(lineLength - 1);
                nsiContent[index] = string.Join("", nsiContent[index], nsiContent[index + 1]).Replace("  ", " ");

                nsiContent.RemoveAt(index + 1);
                length--;
            }
            LoadFileLines = nsiContent;
            GeneralViewModel.Load(LoadFileLines);
            FilesViewModel.Load(LoadFileLines);
            RegistryViewModel.Load(LoadFileLines);
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