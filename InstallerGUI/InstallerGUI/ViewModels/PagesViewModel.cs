using InstallerGUI.Contracts;
using System;
using System.Text;

namespace InstallerGUI.ViewModels
{
    public class PagesViewModel : BaseViewModel, IGetDataToNsi
    {
        public string ExtraPages { get; set; }

        public PagesViewModel()
        {
            ExtraPages = string.Empty;
        }

        public string GetDataToNsi()
        {
            var sb = new StringBuilder();
            sb.Append("; Pages" + Environment.NewLine);
            sb.Append("Page instfiles" + Environment.NewLine);
            sb.Append("{0}" + Environment.NewLine);
            sb.Append("UninstPage uninstConfirm" + Environment.NewLine);
            sb.Append("UninstPage instfiles" + Environment.NewLine + Environment.NewLine);

            return string.Format(sb.ToString(), ExtraPages);
        }
    }
}