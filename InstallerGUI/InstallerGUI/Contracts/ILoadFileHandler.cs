using System.Collections.Generic;

namespace InstallerGUI.Contracts
{
    public interface ILoadFileHandler
    {
        void Load(IEnumerable<string> lines);
    }
}