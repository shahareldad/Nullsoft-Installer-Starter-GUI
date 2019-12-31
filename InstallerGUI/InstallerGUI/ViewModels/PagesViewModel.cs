using InstallerGUI.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstallerGUI.ViewModels
{
    public class PagesViewModel : BaseViewModel, IHandleNsiData
    {
        public string ExtraPages { get; set; }

        public PagesViewModel()
        {
            ExtraPages = string.Empty;
        }

        public string GetInstallDataToNsi()
        {
            var sb = new StringBuilder();
            sb.Append("; Pages" + Environment.NewLine);
            sb.Append("Page instfiles" + Environment.NewLine);
            sb.Append("{0}" + Environment.NewLine);
            sb.Append("UninstPage uninstConfirm" + Environment.NewLine);
            sb.Append("UninstPage instfiles" + Environment.NewLine + Environment.NewLine);

            return string.Format(sb.ToString(), ExtraPages);
        }

        public string GetUninstallDataToNsi()
        {
            return string.Empty;
        }

        public void LoadDataFromNsi(IEnumerable<string> lines)
        {
        }
    }
}