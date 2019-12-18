using System;
using System.Windows;

namespace InstallerGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            StartupUri = new Uri("/InstallerGUI;component/Views/MainWindow.xaml", UriKind.Relative);
        }
    }
}