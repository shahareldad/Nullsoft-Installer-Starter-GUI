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

        private void SelectVariableClicked(object sender, RoutedEventArgs e)
        {
            var selectedCell = DataGridShortcuts.SelectedCells?[0];

            var columnName = selectedCell?.Column.SortMemberPath;
            var item = selectedCell?.Item as ShortcutModel;
            var variable = VariablesComboBox.SelectedItem as VariableModel;

            var type = typeof(ShortcutModel);
            var propertyInfo = type.GetProperty(columnName);

            var finalValue = variable.UserDefined ? "${" + variable.VariableName + "}\\" : "$" + variable.VariableName + "\\";
            propertyInfo.SetValue(item, finalValue);
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