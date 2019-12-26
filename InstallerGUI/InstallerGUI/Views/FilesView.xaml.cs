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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(DataGridSelectedFiles.SelectedItem is InstallationFile item)) return;

            if (!(sender is Button button)) return;

            var buttonContent = button.Content?.ToString();

            switch (buttonContent)
            {
                case "Program Files":
                    item.DestinationFolder = "$PROGRAMFILES\\";
                    break;
                case "Windows":
                    item.DestinationFolder = "$WINDIR\\";
                    break;
                case "System32":
                    item.DestinationFolder = "$SYSDIR\\";
                    break;
                case "Temporary":
                    item.DestinationFolder = "$TEMP\\";
                    break;
                case "Desktop":
                    item.DestinationFolder = "$DESKTOP\\";
                    break;
                case "Destination Folder":
                    item.DestinationFolder = "$INSTDIR\\";
                    break;
                default:
                    item.DestinationFolder = "$INSTDIR\\";
                    break;
            }
        }
    }
}