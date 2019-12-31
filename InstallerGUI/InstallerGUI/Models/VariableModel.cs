using InstallerGUI.Infrastructure;

namespace InstallerGUI.Models
{
    public class VariableModel : INPC
    {
        private string _variableName;
        private string _variableValue;
        private bool _userDefined;

        public bool UserDefined
        {
            get { return _userDefined; }
            set
            {
                _userDefined = value;
                OnPropertyChanged(nameof(UserDefined));
            }
        }

        public string VariableName
        {
            get { return _variableName; }
            set
            {
                _variableName = value;
                OnPropertyChanged(nameof(VariableName));
            }
        }

        public string VariableValue
        {
            get { return _variableValue; }
            set
            {
                _variableValue = value;
                OnPropertyChanged(nameof(VariableValue));
            }
        }
    }
}