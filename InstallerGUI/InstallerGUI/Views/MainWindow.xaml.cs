using InstallerGUI.Contracts;
using Microsoft.Win32;
using System.Windows;

namespace InstallerGUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            (DataContext as IRequestOpenFileExplorer).GetFilenameToSaveEvent += MainWindow_GetFilenameToSaveEvent;
            (DataContext as IRequestOpenFileExplorer).GetFilenameToLoadEvent += MainWindow_GetFilenameToLoadEvent;
        }

        private string MainWindow_GetFilenameToLoadEvent()
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                AddExtension = true,
                DefaultExt = "nsi",
                Filter = "NSIS files (*.nsi)|*.nsi"
            };
            var result = dialog.ShowDialog();
            if (result.HasValue)
                return dialog.FileName;
            return string.Empty;
        }

        private string MainWindow_GetFilenameToSaveEvent()
        {
            var dialog = new SaveFileDialog
            {
                CheckFileExists = false,
                AddExtension = true,
                DefaultExt = "nsi"
            };
            var result = dialog.ShowDialog();
            if (result.HasValue)
                return dialog.FileName;
            return string.Empty;
        }
    }
}