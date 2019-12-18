using InstallerGUI.Contracts;
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
    }
}