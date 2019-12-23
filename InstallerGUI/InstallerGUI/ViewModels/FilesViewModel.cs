using InstallerGUI.Contracts;
using InstallerGUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace InstallerGUI.ViewModels
{
    public class FilesViewModel : BaseViewModel, IHandleFiles, IGetDataToNsi, ILoadFileHandler
    {
        private IHoldGeneralInformation _generalInformation;

        public ObservableCollection<string> SelectedFiles { get; set; }

        public ICommand RemoveFileCommand { get; set; }

        public FilesViewModel(IHoldGeneralInformation generalInformation)
        {
            _generalInformation = generalInformation;
            SelectedFiles = new ObservableCollection<string>();
            RemoveFileCommand = new CommandAction<string>(RemoveFileCommandAction);
        }

        public void RemoveFileCommandAction(string parameter)
        {
            SelectedFiles.Remove(parameter);
        }

        public void HandleFiles(string[] safeFileNames)
        {
            foreach (var filename in safeFileNames)
            {
                SelectedFiles.Add(filename);
            }
        }

        public string GetDataToNsi()
        {
            var sb = new StringBuilder();

            sb.Append("; The stuff to install" + Environment.NewLine);
            sb.Append("Section \"" + _generalInformation.ApplcationName + " files to be installed (required)\"" + Environment.NewLine);
            sb.Append("     SectionIn RO" + Environment.NewLine);
            sb.Append("     SetOutPath $INSTDIR" + Environment.NewLine);

            foreach (var filename in SelectedFiles)
            {
                sb.Append("     File \"" + filename + "\"" + Environment.NewLine);
            }

            sb.Append("     WriteUninstaller \"uninstall.exe\"" + Environment.NewLine);
            sb.Append("SectionEnd" + Environment.NewLine + Environment.NewLine);

            return sb.ToString();
        }

        public void Load(IEnumerable<string> lines)
        {
            foreach (var line in lines.Where(x => x.Contains("File ") && !x.Contains("OutFile")))
            {
                var temp = line.Replace("File \"", "").Replace("\"", "");
                SelectedFiles.Add(temp);
            }
        }
    }
}