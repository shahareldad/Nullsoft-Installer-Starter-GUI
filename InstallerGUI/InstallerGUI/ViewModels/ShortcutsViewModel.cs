using InstallerGUI.Contracts;
using InstallerGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace InstallerGUI.ViewModels
{
    public class ShortcutsViewModel : BaseViewModel, IHandleNsiData, IHandleCollectionItems
    {
        public ObservableCollection<ShortcutModel> Shortcuts { get; set; }

        public ShortcutsViewModel()
        {
            Shortcuts = new ObservableCollection<ShortcutModel>();
        }

        public string GetInstallDataToNsi()
        {
            if (!Shortcuts.Any()) return string.Empty;

            var sb = new StringBuilder();

            sb.Append(";create shortcuts" + Environment.NewLine);

            foreach (var item in Shortcuts)
            {
                sb.Append(" CreateShortCut \"");
                sb.Append(item.LinkValue);
                sb.Append("\" \"");
                sb.Append(item.TargetValue);
                sb.Append("\" \"");
                sb.Append(item.Parameters);
                sb.Append("\"");
                //sb.Append("\" \"");
                if (!string.IsNullOrWhiteSpace(item.IconFile))
                {
                    sb.Append(" \"");
                    sb.Append(item.IconFile);
                    sb.Append("\" ");
                    if (!string.IsNullOrWhiteSpace(item.IconIndexNumber))
                    {
                        sb.Append(item.IconIndexNumber);
                        //sb.Append(" ");
                    };
                };
                //if (!string.IsNullOrWhiteSpace(item.StartOptions))
                //{
                //    sb.Append(item.StartOptions);
                //    sb.Append(" ");
                //};
                //if (!string.IsNullOrWhiteSpace(item.KeyboardShortcut))
                //{
                //    sb.Append(item.KeyboardShortcut);
                //    sb.Append(" \"");
                //};
                //if (!string.IsNullOrWhiteSpace(item.Description))
                //{
                //    sb.Append(item.Description);
                //    sb.Append("\"");
                //};
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public void LoadDataFromNsi(IEnumerable<string> lines)
        {
            Shortcuts.Clear();

            var filteredLines = lines.Where(x => x.ToLower().Contains("createshortcut "));

            var currentOutputPath = string.Empty;
            foreach (var line in filteredLines)
            {
                var temp = line.ToLower().Replace("createshortcut ", "").Trim();
                var splitted = temp.Split(' ').Select(x => x.Replace("\"", "").Trim());
                var link = splitted.ElementAt(0);
                var target = splitted.ElementAt(1);
                var parameters = splitted.ElementAt(2);
                string iconFile = string.Empty, iconIndex = string.Empty, startOptions = string.Empty,
                    keyboardShortcut = string.Empty, description = string.Empty;
                if (splitted.Count() > 3)
                    iconFile = splitted.ElementAt(3);
                if (splitted.Count() > 4)
                    iconIndex = splitted.ElementAt(4);
                if (splitted.Count() > 5)
                    startOptions = splitted.ElementAt(5);
                if (splitted.Count() > 6)
                    keyboardShortcut = splitted.ElementAt(6);
                if (splitted.Count() > 7)
                    description = splitted.ElementAt(7);
                Shortcuts.Add(new ShortcutModel
                {
                    LinkValue = link,
                    TargetValue = target,
                    Parameters = parameters,
                    IconFile = iconFile,
                    IconIndexNumber = iconIndex,
                    StartOptions = startOptions,
                    KeyboardShortcut = keyboardShortcut,
                    Description = description
                });
            }
        }

        public void AddEmptyItem()
        {
            Shortcuts.Add(new ShortcutModel());
        }

        public void RemoveItem(object model)
        {
            Shortcuts.Remove(model as ShortcutModel);
        }

        public string GetUninstallDataToNsi()
        {
            if (!Shortcuts.Any()) return string.Empty;

            var sb = new StringBuilder();

            sb.Append(" ;Delete shortcuts" + Environment.NewLine);

            foreach (var item in Shortcuts)
            {
                sb.Append(" Delete  \"");
                sb.Append(item.LinkValue);
                sb.Append("\"");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}