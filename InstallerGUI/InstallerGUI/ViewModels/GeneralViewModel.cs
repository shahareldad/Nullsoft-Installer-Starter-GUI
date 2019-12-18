using InstallerGUI.Contracts;
using System;
using System.Text;

namespace InstallerGUI.ViewModels
{
    public class GeneralViewModel : BaseViewModel, IGetDataToNsi, IHoldGeneralInformation
    {
        private string _applcationName;
        private string _outputFilename;
        private string _productName;
        private string _companyName;
        private string _fileDescription;
        private string _legalCopyright;

        public string ApplcationName
        {
            get
            {
                return _applcationName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(_applcationName) || !_applcationName.Equals(value))
                {
                    _applcationName = value;
                    OnApplicationNameUpdated();
                }
            }
        }

        public string OutputFilename
        {
            get
            {
                return _outputFilename;
            }
            set
            {
                _outputFilename = value;
                OnPropertyChanged(nameof(OutputFilename));
            }
        }

        public string DestinationFolder { get; set; }

        public string FileVersion { get; set; }

        public string ProductVersion { get; set; }

        public string ProductName
        {
            get
            {
                return _productName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(_productName) || !_productName.Equals(value))
                {
                    _productName = value;
                    OnProductNameUpdated();
                }
            }
        }

        public string CompanyName
        {
            get
            {
                return _companyName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(_companyName) || !_companyName.Equals(value))
                {
                    _companyName = value;
                    OnCompanyNameUpdated();
                }
            }
        }

        public string FileDescription
        {
            get
            {
                return _fileDescription;
            }
            set
            {
                _fileDescription = value;
                OnPropertyChanged(nameof(FileDescription));
            }
        }

        public string LegalCopyright
        {
            get
            {
                return _legalCopyright;
            }
            set
            {
                _legalCopyright = value;
                OnPropertyChanged(nameof(LegalCopyright));
            }
        }

        public GeneralViewModel()
        {
            DestinationFolder = @"C:\Program Files\";
            FileVersion = "1.0.0.0";
            ProductVersion = "1.0.0.0";
        }

        private void OnApplicationNameUpdated()
        {
            if (string.IsNullOrWhiteSpace(_applcationName))
                return;

            if (string.IsNullOrWhiteSpace(OutputFilename))
            {
                var temp = string.Empty;
                foreach (var word in _applcationName.Split(' '))
                {
                    temp += char.ToUpperInvariant(word[0]) + word.Substring(1);
                }
                OutputFilename = string.Concat(temp, "Setup.exe");
            }
        }

        private void OnProductNameUpdated()
        {
            if (string.IsNullOrWhiteSpace(_productName))
                return;

            var temp = string.Empty;
            foreach (var word in _productName.Split(' '))
            {
                temp += char.ToUpperInvariant(word[0]) + word.Substring(1);
            }
            _productName = temp;
            OnPropertyChanged(nameof(ProductName));
            FileDescription = _productName + " installation file";
        }

        private void OnCompanyNameUpdated()
        {
            LegalCopyright = "Copyright " + _companyName;
        }

        public string GetDataToNsi()
        {
            var sb = new StringBuilder();
            sb.Append("; The name of the installer" + Environment.NewLine);
            sb.Append("Name \"" + ApplcationName + "\"" + Environment.NewLine);
            sb.Append(";Version Information" + Environment.NewLine);
            sb.Append("VIProductVersion \"" + ProductVersion + "\"" + Environment.NewLine);
            sb.Append("VIAddVersionKey \"ProductName\" \"" + ProductName + "\"" + Environment.NewLine);
            sb.Append("VIAddVersionKey \"CompanyName\" \"" + CompanyName + "\"" + Environment.NewLine);
            sb.Append("VIAddVersionKey \"FileVersion\" \"" + FileVersion + "\"" + Environment.NewLine);
            sb.Append("VIAddVersionKey \"FileDescription\" \"" + FileDescription + "\"" + Environment.NewLine);
            sb.Append("VIAddVersionKey \"LegalCopyright\" \"" + LegalCopyright + "\"" + Environment.NewLine);
            sb.Append("; The file to write" + Environment.NewLine);
            sb.Append("OutFile \"" + OutputFilename + "\"" + Environment.NewLine);
            sb.Append("; The default installation directory" + Environment.NewLine);
            sb.Append("InstallDir \"" + DestinationFolder + "\"" + Environment.NewLine);
            sb.Append("; Request application privileges for Windows Vista" + Environment.NewLine);
            sb.Append("RequestExecutionLevel admin" + Environment.NewLine + Environment.NewLine);

            return sb.ToString();
        }
    }
}