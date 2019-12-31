using System.Collections.Generic;

namespace InstallerGUI.Contracts
{
    public interface IHandleNsiData
    {
        string GetInstallDataToNsi();

        string GetUninstallDataToNsi();

        void LoadDataFromNsi(IEnumerable<string> lines);
    }
}