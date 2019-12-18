using System.Windows;
using System.Windows.Controls;

namespace InstallerGUI.Views
{
    /// <summary>
    /// Interaction logic for GeneralView.xaml
    /// </summary>
    public partial class GeneralView : UserControl
    {
        public GeneralView()
        {
            InitializeComponent();
        }

        private void OpenFolderBrowsing_Clicked(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = OpenFolderTextBox.Text;
                var result = dialog.ShowDialog();
                OpenFolderTextBox.Text = dialog.SelectedPath;
            }
        }
    }
}