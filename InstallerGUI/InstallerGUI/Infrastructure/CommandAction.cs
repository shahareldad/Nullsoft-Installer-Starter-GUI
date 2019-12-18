using System;
using System.Windows.Input;

namespace InstallerGUI.Infrastructure
{
    public class CommandAction : ICommand
    {
        private Action _action;
        private Func<bool> _canExecute;

        public CommandAction(Action action) : this(action, null)
        {
        }

        public CommandAction(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute();
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }

    public class CommandAction<T> : ICommand
    {
        private Action<T> _action;
        private Func<bool> _canExecute;

        public CommandAction(Action<T> action) : this(action, null)
        {
        }

        public CommandAction(Action<T> action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute();
            return true;
        }

        public void Execute(object parameter)
        {
            _action((T)parameter);
        }
    }
}