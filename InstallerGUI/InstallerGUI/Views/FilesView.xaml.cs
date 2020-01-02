using InstallerGUI.Contracts;
using InstallerGUI.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace InstallerGUI.Views
{
    /// <summary>
    /// Interaction logic for FilesView.xaml
    /// </summary>
    public partial class FilesView : UserControl
    {
        public FilesView()
        {
            InitializeComponent();
        }

        private void OpenFileSelection_Clicked(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true
            };
            var result = dialog.ShowDialog();

            var fileHandler = DataContext as IHandleFiles;
            fileHandler.HandleFiles(dialog.FileNames);
        }

        private void SelectVariableClicked(object sender, RoutedEventArgs e)
        {
            var selectedCell = DataGridSelectedFiles.SelectedCells?[0];

            var columnName = selectedCell?.Column.SortMemberPath;
            var item = selectedCell?.Item as InstallationFile;
            var variable = VariablesComboBox.SelectedItem as VariableModel;

            var type = typeof(InstallationFile);
            var propertyInfo = type.GetProperty(columnName);

            var finalValue = variable.UserDefined ? "${" + variable.VariableName + "}\\" : "$" + variable.VariableName + "\\";
            propertyInfo.SetValue(item, finalValue);
        }
    }
}