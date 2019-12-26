using InstallerGUI.Contracts;
using InstallerGUI.Infrastructure;
using InstallerGUI.Models;
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

        public ObservableCollection<InstallationFile> SelectedFiles { get; set; }

        public ICommand RemoveFileCommand { get; set; }

        public FilesViewModel(IHoldGeneralInformation generalInformation)
        {
            _generalInformation = generalInformation;
            SelectedFiles = new ObservableCollection<InstallationFile>();
            RemoveFileCommand = new CommandAction<InstallationFile>(RemoveFileCommandAction);
        }

        public void RemoveFileCommandAction(InstallationFile parameter)
        {
            SelectedFiles.Remove(parameter);
        }

        public void HandleFiles(string[] safeFileNames)
        {
            foreach (var filename in safeFileNames)
            {
                SelectedFiles.Add(new InstallationFile
                {
                    SourceFullPath = filename,
                    DestinationFolder = "$INSTDIR"
                });
            }
        }

        public string GetDataToNsi()
        {
            var sb = new StringBuilder();

            sb.Append("; The stuff to install" + Environment.NewLine);
            sb.Append("Section \"" + _generalInformation.ApplcationName + " files to be installed (required)\"" + Environment.NewLine);
            sb.Append("     SectionIn RO" + Environment.NewLine);

            var groupBy = SelectedFiles.GroupBy(x => x.DestinationFolder);
            foreach (var item in groupBy)
            {
                sb.Append("     SetOutPath \"" + item.Key + "\"" + Environment.NewLine);
                foreach (var file in item)
                {
                    sb.Append("     File \"" + file.SourceFullPath + "\"" + Environment.NewLine);
                }
            }

            sb.Append("     WriteUninstaller \"uninstall.exe\"" + Environment.NewLine);
            sb.Append("SectionEnd" + Environment.NewLine + Environment.NewLine);

            return sb.ToString();
        }

        public void Load(IEnumerable<string> lines)
        {
            SelectedFiles.Clear();

            var filteredLines = lines.Where(x => (x.ToLower().Contains("file ") || x.ToLower().Contains("setoutpath ")) &&
                                    !x.ToLower().Contains("outfile") && !x.ToLower().Contains("the file to write") &&
                                    !x.ToLower().Contains("!define"));

            var currentOutputPath = string.Empty;
            foreach (var line in filteredLines)
            {
                if (line.ToLower().Contains("setoutpath "))
                {
                    currentOutputPath = line.ToLower().Replace("setoutpath ", "").Replace("\"", "").Trim();
                }
                if (line.ToLower().Contains("file "))
                {
                    var temp = line.ToLower().Replace("file ", "").Replace("\"", "").Trim();
                    SelectedFiles.Add(new InstallationFile
                    {
                        SourceFullPath = temp,
                        DestinationFolder = currentOutputPath
                    });
                }
            }
        }
    }
}