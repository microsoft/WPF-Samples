using System;
using System.Windows.Input;

namespace WpfTreeView
{
    public class RelayCommand : ICommand
    {
        public static readonly RelayCommand None = new RelayCommand((_) => { }, (_) => false);

        /// <summary>
        /// Action that gets executed when the command is triggered.
        /// </summary>
        private readonly Action<object> _Execute;

        /// <summary>
        /// Predicate that is evaluated to determine if the command 
        /// </summary>
        private readonly Predicate<object> _CanExecute;

        private event EventHandler _CanExecuteChanged;

        /// <summary>
        /// Creates a relay command 
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _Execute = execute;
            _CanExecute = canExecute;
        }

        /// <summary>
        /// Evaluates if the command can execute.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _CanExecute == null ? true : _CanExecute.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _CanExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _CanExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _Execute?.Invoke(parameter);
        }

        /// <summary>
        /// Notifies listeners that the can execute predicate has changed.
        /// </summary>
        /// <param name="sender"></param>
        public void NotifyCanExecuteChanged(object sender)
        {
            _CanExecuteChanged?.Invoke(sender, EventArgs.Empty);
        }
    }
}
