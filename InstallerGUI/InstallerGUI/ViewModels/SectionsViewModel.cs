using InstallerGUI.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstallerGUI.ViewModels
{
    public class SectionsViewModel : BaseViewModel, IHandleNsiData
    {
        private IHandleFiles _fileHandler;
        private IHandleRegistry _registryHandler;

        public SectionsViewModel(IHandleFiles fileHandler, IHandleRegistry registryHandler)
        {
            _fileHandler = fileHandler;
            _registryHandler = registryHandler;
        }

        public string GetInstallDataToNsi()
        {
            var sb = new StringBuilder();

            sb.Append(_fileHandler.GetInstallDataToNsi());
            sb.Append(_registryHandler.GetInstallDataToNsi());

            sb.Append("; Uninstaller" + Environment.NewLine);
            sb.Append("Section \"Uninstall\"" + Environment.NewLine);
            sb.Append(_registryHandler.GetUninstallDataToNsi());
            sb.Append(_fileHandler.GetUninstallDataToNsi());
            sb.Append("SectionEnd" + Environment.NewLine + Environment.NewLine);

            return sb.ToString();
        }

        public string GetUninstallDataToNsi()
        {
            return string.Empty;
        }

        public void LoadDataFromNsi(IEnumerable<string> lines)
        {
        }
    }
}