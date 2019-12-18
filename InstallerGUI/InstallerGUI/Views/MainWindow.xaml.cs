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
        }

        private string MainWindow_GetFilenameToSaveEvent()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result.HasValue)
                return dialog.FileName;
            return string.Empty;
        }
    }
}