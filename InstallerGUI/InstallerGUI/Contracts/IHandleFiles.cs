namespace InstallerGUI.Contracts
{
    public interface IHandleFiles : IGetDataToNsi
    {
        void HandleFiles(string[] safeFileNames);
    }
}