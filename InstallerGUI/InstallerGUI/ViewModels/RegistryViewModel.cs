using InstallerGUI.Contracts;
using InstallerGUI.Infrastructure;
using InstallerGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace InstallerGUI.ViewModels
{
    public class RegistryViewModel : BaseViewModel, IHandleRegistry, IGetDataToNsi, ILoadFileHandler
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
            if (!RegistryKeysToAdd.Any()) return string.Empty;

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

        private string ConvertSectionNameShortToLong(string sectionName)
        {
            switch (sectionName.Replace("System.Windows.Controls.ComboBoxItem: ", ""))
            {
                case "HKCR":
                    return "HKEY_CLASSES_ROOT";

                case "HKCU":
                    return "HKEY_CURRENT_USER";

                case "HKLM":
                    return "HKEY_LOCAL_MACHINE";

                case "HKU":
                    return "HKEY_USERS";

                case "HKCC":
                    return "HKEY_CURRENT_CONFIG";

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
                sb.Append("     DeleteRegKey " + key.RegistrySectionShort + " \"" + key.RegistryPathToKey + "\"" + Environment.NewLine);
            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

        public void Load(IEnumerable<string> lines)
        {
            RegistryKeysToAdd.Clear();

            foreach (var line in lines.Where(x => x.Contains("${registry::") || x.Contains("WriteRegStr ")
                || x.Contains("WriteRegBin ") || x.Contains("WriteRegDWORD ")
                || x.Contains("WriteRegExpandStr ") || x.Contains("WriteRegMultiStr ")))
            {
                if (line.Contains("WriteRegBin "))
                {
                    ExtractParams(line, "WriteRegBin", out string section, out string path, out string key, out string value);
                    AddNewModelToCollection(section, path, key, value, "REG_BINARY");
                }
                if (line.Contains("WriteRegDWORD "))
                {
                    ExtractParams(line, "WriteRegDWORD", out string section, out string path, out string key, out string value);
                    AddNewModelToCollection(section, path, key, value, "REG_DWORD");
                }
                if (line.Contains("WriteRegStr "))
                {
                    ExtractParams(line, "WriteRegStr", out string section, out string path, out string key, out string value);
                    AddNewModelToCollection(section, path, key, value, "REG_SZ");
                }
                if (line.Contains("WriteRegExpandStr "))
                {
                    ExtractParams(line, "WriteRegExpandStr", out string section, out string path, out string key, out string value);
                    AddNewModelToCollection(section, path, key, value, "REG_EXPAND_SZ");
                }
                if (line.Contains("WriteRegMultiStr "))
                {
                    ExtractParams(line, "WriteRegMultiStr", out string section, out string path, out string key, out string value);
                    AddNewModelToCollection(section, path, key, value, "REG_MULTI_SZ");
                }
                if (line.Contains("${registry::CreateKey}"))
                {
                    var temp = line.Replace("${registry::CreateKey} \"", "").Replace("\"", "").Trim();
                    ExtractSectionPath(temp, out string section, out string path);
                    if (!RegistryKeysToAdd.Any(x => x.RegistryPathToKey.Equals(path) && x.RegistrySectionLong.Equals(section)))
                    {
                        RegistryKeysToAdd.Add(new RegistryModel
                        {
                            RegistrySectionLong = section,
                            RegistrySectionShort = ConvertSectionNameShortToLong(section),
                            RegistryPathToKey = path
                        });
                    }
                }
                if (line.Contains("${registry::Write}"))
                {
                    var temp = line.Replace("${registry::Write} \"", "").Trim();
                    var firstPart = temp.Substring(0, temp.IndexOf("\""));
                    ExtractSectionPath(firstPart, out string section, out string path);

                    var temp2 = temp.Replace(firstPart + "\" \"", "").Trim();
                    var key = temp2.Substring(0, temp2.IndexOf("\""));

                    var temp3 = temp2.Replace(key + "\" \"", "").Trim();
                    var value = temp3.Substring(0, temp3.IndexOf("\""));

                    var temp4 = temp3.Replace(value + "\" \"", "").Trim();
                    var type = temp4.Substring(0, temp4.IndexOf("\""));

                    AddNewModelToCollection(section, path, key, value, type);
                }
            }
        }

        private void ExtractParams(string line, string typeTitle, out string section, out string path, out string key, out string value)
        {
            var temp = line.Replace(typeTitle + " ", "").Trim();
            var sectionEndIndex = temp.IndexOf(' ');
            var sectionShort = temp.Substring(0, sectionEndIndex);
            section = ConvertSectionNameShortToLong(sectionShort);
            var temp2 = temp.Replace(sectionShort + " ", "");

            var parts = temp2.Split(new string[] { "\" \"" }, StringSplitOptions.RemoveEmptyEntries);
            path = parts[0].Replace("\"", "").Trim();
            key = parts[1].Replace("\"", "").Trim();
            value = parts[2].Replace("\"", "").Trim();
        }

        private void AddNewModelToCollection(string section, string path, string key, string value, string type)
        {
            var model = RegistryKeysToAdd.FirstOrDefault(x =>
            {
                return (x.RegistrySectionLong.Equals(section) &&
                    x.RegistryPathToKey.Equals(path) &&
                    x.RegistryKeyName != null && x.RegistryKeyName.Equals(key) &&
                    x.RegistryKeyValue != null && x.RegistryKeyValue.Equals(value) &&
                    x.RegistryKeyType != null && x.RegistryKeyType.Equals(type))
                    ||
                    (x.RegistrySectionLong.Equals(section) &&
                    x.RegistryPathToKey.Equals(path));
            });
            if (model == null)
            {
                model = new RegistryModel
                {
                    RegistrySectionLong = section,
                    RegistrySectionShort = ConvertSectionNameLongToShort(section),
                    RegistryPathToKey = path,
                    RegistryKeyName = key,
                    RegistryKeyValue = value,
                    RegistryKeyType = type
                };
                RegistryKeysToAdd.Add(model);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.RegistryKeyName) &&
                    !string.IsNullOrWhiteSpace(model.RegistryKeyValue) &&
                    !string.IsNullOrWhiteSpace(model.RegistryKeyType))
                {
                    model = new RegistryModel
                    {
                        RegistrySectionLong = section,
                        RegistrySectionShort = ConvertSectionNameLongToShort(section),
                        RegistryPathToKey = path,
                        RegistryKeyName = key,
                        RegistryKeyValue = value,
                        RegistryKeyType = type
                    };
                    RegistryKeysToAdd.Add(model);
                }
                else
                {
                    model.RegistrySectionShort = ConvertSectionNameLongToShort(section);
                    model.RegistryKeyName = key;
                    model.RegistryKeyValue = value;
                    model.RegistryKeyType = type;
                }
            }
        }

        private static void ExtractSectionPath(string temp, out string section, out string path)
        {
            var indexOfFirstDiagonal = temp.IndexOf("\\");
            section = temp.Substring(0, indexOfFirstDiagonal).Trim();
            path = temp.Substring(indexOfFirstDiagonal + 1);
        }
    }
}