using InstallerGUI.Contracts;
using System;
using System.Text;

namespace InstallerGUI.ViewModels
{
    public class SectionsViewModel : BaseViewModel, IGetDataToNsi
    {
        private IHandleFiles _fileHandler;
        private IHandleRegistry _registryHandler;

        public SectionsViewModel(IHandleFiles fileHandler, IHandleRegistry registryHandler)
        {
            _fileHandler = fileHandler;
            _registryHandler = registryHandler;
        }

        public string GetDataToNsi()
        {
            var sb = new StringBuilder();

            sb.Append((_fileHandler as IGetDataToNsi).GetDataToNsi());
            sb.Append((_registryHandler as IGetDataToNsi).GetDataToNsi());

            sb.Append("; Uninstaller" + Environment.NewLine);
            sb.Append("Section \"Uninstall\"" + Environment.NewLine);
            sb.Append(@"    Delete $INSTDIR\*.*" + Environment.NewLine);
            sb.Append("     RMDir \"$INSTDIR\"" + Environment.NewLine);
            sb.Append("SectionEnd" + Environment.NewLine + Environment.NewLine);

            return sb.ToString();
        }
    }
}