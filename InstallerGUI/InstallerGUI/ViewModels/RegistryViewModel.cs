using InstallerGUI.Contracts;
using InstallerGUI.Infrastructure;
using InstallerGUI.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace InstallerGUI.ViewModels
{
    public class RegistryViewModel : BaseViewModel, IHandleRegistry, IGetDataToNsi
    {
        public ICommand AddNewRegistryKeyCommand { get; set; }

        public string SectionName { get; set; }

        public string Path { get; set; }

        public string KeyName { get; set; }

        public string KeyValue { get; set; }

        public string KeyType { get; set; }

        public bool RegistrySectionNeeded { get { return RegistryKeysToAdd.Any(); } }

        public ObservableCollection<RegistryModel> RegistryKeysToAdd { get; set; }

        public RegistryViewModel()
        {
            AddNewRegistryKeyCommand = new CommandAction(AddNewRegistryKeyCommandAction);
            RegistryKeysToAdd = new ObservableCollection<RegistryModel>();
        }

        public string GetDataToNsi()
        {
            var sb = new StringBuilder();

            sb.Append("; Registry section" + Environment.NewLine);
            sb.Append("Section \"Registry (required)\"" + Environment.NewLine);
            sb.Append("     SectionIn RO" + Environment.NewLine);

            foreach (var key in RegistryKeysToAdd)
            {
                sb.Append("     ${registry::CreateKey} \"" + key.RegistrySectionLong + "\\" + key.RegistryPathToKey + "\"" + Environment.NewLine);
                sb.Append("     ${registry::Write} \"" + key.RegistrySectionLong + "\\" + key.RegistryPathToKey + "\" \"" + key.RegistryKeyName + "\" \"" + key.RegistryKeyValue + "\" \"" + key.RegistryKeyType + "\" $R0" + Environment.NewLine);
            }

            sb.Append("SectionEnd" + Environment.NewLine + Environment.NewLine);

            return sb.ToString();
        }

        private void AddNewRegistryKeyCommandAction()
        {
            var item = new RegistryModel
            {
                RegistrySectionLong = SectionName,
                RegistrySectionShort = ConvertSectionNameLongToShort(SectionName),
                RegistryPathToKey = Path,
                RegistryKeyName = KeyName,
                RegistryKeyValue = KeyValue,
                RegistryKeyType = ConvertKeyType(KeyType)
            };
            RegistryKeysToAdd.Add(item);
        }

        private string ConvertKeyType(string keyType)
        {
            switch (keyType.Replace("System.Windows.Controls.ComboBoxItem: ", ""))
            {
                case "String":
                    return "REG_SZ";
                case "Binary":
                    return "REG_BINARY";
                case "DWord":
                    return "REG_DWORD";
                case "QWord":
                    return "REG_QWORD";
                case "Multi String":
                    return "REG_MULTI_SZ";
                case "Expandable String":
                    return "REG_EXPAND_SZ";
                default:
                    return string.Empty;
            }
        }

        private string ConvertSectionNameLongToShort(string sectionName)
        {
            switch (sectionName.Replace("System.Windows.Controls.ComboBoxItem: ", ""))
            {
                case "HKEY_CLASSES_ROOT":
                    return "HKCR";
                case "HKEY_CURRENT_USER":
                    return "HKCU";
                case "HKEY_LOCAL_MACHINE":
                    return "HKLM";
                case "HKEY_USERS":
                    return "HKU";
                case "HKEY_CURRENT_CONFIG":
                    return "HKCC";
                default:
                    return string.Empty;
            }
        }

        public string GetDataToUninstallSection()
        {
            var sb = new StringBuilder();

            sb.Append("     ; Remove registry keys" + Environment.NewLine);
            foreach (var key in RegistryKeysToAdd)
            {
                sb.Append("     DeleteRegKey " + key.RegistrySectionShort + " \"" + key.RegistryKeyName + "\"" + Environment.NewLine);
            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
    }
}