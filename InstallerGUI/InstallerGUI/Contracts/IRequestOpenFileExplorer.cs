using InstallerGUI.ViewModels;

namespace InstallerGUI.Contracts
{
    public interface IRequestOpenFileExplorer
    {
        event GetFilenameToSaveHandler GetFilenameToSaveEvent;

        event GetFilenameToSaveHandler GetFilenameToLoadEvent;
    }
}