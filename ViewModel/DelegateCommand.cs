using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GitUtilSimulate.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action action;
        private Action<object> execute;
        private Predicate<object> canExecute;
        private EventHandler CanExecuteChangedInternal;

        public DelegateCommand(Action<object> execute)
           : this(execute, DefaultCanExecute)
        {
        }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentException("canExecute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //action();
            execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public DelegateCommand(Action action)
        {
            this.action = action;
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
