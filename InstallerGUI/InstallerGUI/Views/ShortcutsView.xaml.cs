using InstallerGUI.Contracts;
using InstallerGUI.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace InstallerGUI.Views
{
    /// <summary>
    /// Interaction logic for ShortcutsView.xaml
    /// </summary>
    public partial class ShortcutsView : UserControl
    {
        public ShortcutsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(DataGridShortcuts.SelectedCells[0] is DataGridCellInfo cell)) return;

            if (!(sender is Button button)) return;

            var columnMemberPath = cell.Column.SortMemberPath;

            var item = default(ShortcutModel);
            if (cell.Item.ToString().Equals("{NewItemPlaceholder}"))
            {
                AddNewRowButtonClick(null, null);
                if (!(DataGridShortcuts.ItemsSource is ObservableCollection<ShortcutModel> collection)) return;

                item = collection[collection.Count - 1];
            }
            else
            {
                item = cell.Item as ShortcutModel;
                if (item == null) return;
            }

            var buttonContent = button.Content?.ToString();
            switch (buttonContent)
            {
                case "Program Files":
                    SetValue(item, columnMemberPath, "$PROGRAMFILES\\");
                    break;
                case "Windows":
                    SetValue(item, columnMemberPath, "$WINDIR\\");
                    break;
                case "System32":
                    SetValue(item, columnMemberPath, "$SYSDIR\\");
                    break;
                case "Temporary":
                    SetValue(item, columnMemberPath, "$TEMP\\");
                    break;
                case "Desktop":
                    SetValue(item, columnMemberPath, "$DESKTOP\\");
                    break;
                case "Destination Folder":
                    SetValue(item, columnMemberPath, "$INSTDIR\\");
                    break;
                default:
                    SetValue(item, columnMemberPath, "$INSTDIR\\");
                    break;
            }
        }

        private static void SetValue(ShortcutModel item, string columnMemberPath, string value)
        {
            var type = typeof(ShortcutModel);
            type.GetProperty(columnMemberPath).SetValue(item, value);
        }

        private void AddNewRowButtonClick(object sender, RoutedEventArgs e)
        {
            (DataContext as IHandleCollectionItems).AddEmptyItem();
        }

        private void RemoveRowButtonClick(object sender, RoutedEventArgs e)
        {
            (DataContext as IHandleCollectionItems).RemoveItem(DataGridShortcuts.SelectedItem);
        }
    }
}
