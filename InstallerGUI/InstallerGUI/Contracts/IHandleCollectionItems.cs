namespace InstallerGUI.Contracts
{
    public interface IHandleCollectionItems
    {
        void AddEmptyItem();

        void RemoveItem(object model);
    }
}