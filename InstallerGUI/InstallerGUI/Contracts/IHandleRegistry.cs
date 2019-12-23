namespace InstallerGUI.Contracts
{
    public interface IHandleRegistry : IGetDataToNsi
    {
        bool RegistrySectionNeeded { get; }

        string GetDataToUninstallSection();
    }
}