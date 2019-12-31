using InstallerGUI.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InstallerGUI.Views
{
    /// <summary>
    /// Interaction logic for UserVariablesView.xaml
    /// </summary>
    public partial class UserVariablesView : UserControl
    {
        public UserVariablesView()
        {
            InitializeComponent();
        }

        private void RemoveRowButtonClick(object sender, RoutedEventArgs e)
        {
            (DataContext as IHandleCollectionItems).RemoveItem(DataGridUserVariables.SelectedItem);
        }

        private void VariableNameTextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!(((int)e.Key >= (int)Key.A && (int)e.Key <= (int)Key.Z) ||
                ((int)e.Key >= (int)Key.D0 && (int)e.Key <= (int)Key.D9) ||
                ((int)e.Key >= (int)Key.NumPad0 && (int)e.Key <= (int)Key.NumPad9) ||
                e.Key == Key.Space))
            {
                e.Handled = true;
            }
        }
    }
}
