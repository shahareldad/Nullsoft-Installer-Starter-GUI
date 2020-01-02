using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using InstallerGUI.Contracts;
using InstallerGUI.Infrastructure;
using InstallerGUI.Models;

namespace InstallerGUI.ViewModels
{
    public class UserVariablesViewModel : BaseViewModel, IHandleNsiData, IHandleCollectionItems, IHoldVariables
    {
        private IEnumerable<VariableModel> _defaultShortcuts = new List<VariableModel>
        {
            new VariableModel { VariableValue = "Destination_Folder", VariableName = "INSTDIR", UserDefined = false },
            new VariableModel { VariableValue = "Temporary", VariableName = "TEMP", UserDefined = false },
            new VariableModel { VariableValue = "Desktop", VariableName = "DESKTOP", UserDefined = false },
            new VariableModel { VariableValue = "Program Files", VariableName = "PROGRAMFILES", UserDefined = false },
            new VariableModel { VariableValue = "Windows", VariableName = "WINDIR", UserDefined = false },
            new VariableModel { VariableValue = "System32", VariableName = "SYSDIR", UserDefined = false }
        };

        public IEnumerable<VariableModel> AllVariables
        {
            get
            {
                return Enumerable.Concat(_defaultShortcuts, Variables);
            }
        }

        public ObservableCollection<VariableModel> Variables { get; set; }

        public ICommand AddNewVariableCommand { get; set; }

        public string VariableName { get; set; }

        public string VariableValue { get; set; }

        public bool HasMUI
        {
            get
            {
                return Variables.Any(x => x.VariableName.Contains("MUI"));
            }
        }

        public UserVariablesViewModel()
        {
            Variables = new ObservableCollection<VariableModel>();
            AddNewVariableCommand = new CommandAction(AddNewVariableCommandAction);
        }

        private void AddNewVariableCommandAction()
        {
            var model = new VariableModel
            {
                VariableName = VariableName.Replace(" ", "_"),
                VariableValue = VariableValue,
                UserDefined = true
            };
            Variables.Add(model);
        }

        public void AddEmptyItem()
        {
            
        }

        public string GetInstallDataToNsi()
        {
            if (!Variables.Any()) return string.Empty;

            var sb = new StringBuilder();

            sb.Append(";variables" + Environment.NewLine);

            foreach (var item in Variables.OrderBy(x => !x.UserDefined))
            {
                sb.Append("!define ");
                if (item.UserDefined)
                {
                    sb.Append(item.VariableName);
                    sb.Append(" \"");
                    sb.Append(item.VariableValue);
                    sb.Append("\" ");
                }
                else
                {
                    sb.Append(item.VariableName);
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public string GetUninstallDataToNsi()
        {
            return string.Empty;
        }

        public void LoadDataFromNsi(IEnumerable<string> lines)
        {
            Variables.Clear();

            var filteredLines = lines.Where(x => x.ToLower().Contains("!define "));

            var defineLength = "!define ".Length;
            foreach (var line in filteredLines)
            {
                var temp = line.Remove(0, defineLength).Trim();
                if (line.ToLower().Contains("\""))
                {
                    var name = temp.Substring(0, temp.IndexOf(' '));
                    var value = temp.Replace(name, "").Replace("\"", "").Trim();
                    Variables.Add(new VariableModel
                    {
                        VariableName = name,
                        VariableValue = value,
                        UserDefined = true
                    });
                }
                else
                {
                    Variables.Add(new VariableModel
                    {
                        VariableName = temp,
                        VariableValue = null,
                        UserDefined = false
                    });
                }
            }
        }

        public void RemoveItem(object model)
        {
            Variables.Remove(model as VariableModel);
        }
    }
}
