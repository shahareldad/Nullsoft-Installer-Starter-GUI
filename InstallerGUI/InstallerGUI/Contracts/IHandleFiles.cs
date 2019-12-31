namespace InstallerGUI.Contracts
{
    public interface IHandleFiles : IHandleNsiData
    {
        void HandleFiles(string[] safeFileNames);
    }
}