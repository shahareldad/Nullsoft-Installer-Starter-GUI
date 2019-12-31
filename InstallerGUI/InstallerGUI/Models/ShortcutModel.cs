using InstallerGUI.Infrastructure;

namespace InstallerGUI.Models
{
    public class ShortcutModel : INPC
    {
        private string _linkValue;

        private string _targetValue;

        private string _parameters;

        private string _iconFile;

        private string _iconIndexNumber;

        private string _startOptions;

        private string _keyboardShortcut;

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string KeyboardShortcut
        {
            get { return _keyboardShortcut; }
            set
            {
                _keyboardShortcut = value;
                OnPropertyChanged(nameof(KeyboardShortcut));
            }
        }

        public string StartOptions
        {
            get { return _startOptions; }
            set
            {
                _startOptions = value;
                OnPropertyChanged(nameof(StartOptions));
            }
        }

        public string IconIndexNumber
        {
            get { return _iconIndexNumber; }
            set
            {
                _iconIndexNumber = value;
                OnPropertyChanged(nameof(IconIndexNumber));
            }
        }

        public string IconFile
        {
            get { return _iconFile; }
            set
            {
                _iconFile = value;
                OnPropertyChanged(nameof(IconFile));
            }
        }

        public string Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
                OnPropertyChanged(nameof(Parameters));
            }
        }

        public string TargetValue
        {
            get { return _targetValue; }
            set
            {
                _targetValue = value;
                OnPropertyChanged(nameof(TargetValue));
            }
        }

        public string LinkValue
        {
            get { return _linkValue; }
            set
            {
                _linkValue = value;
                OnPropertyChanged(nameof(LinkValue));
            }
        }
    }
}