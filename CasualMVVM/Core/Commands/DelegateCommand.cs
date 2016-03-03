using System;
using System.Windows.Input;

namespace FuchsiaSoft.CasualMVVM.Core.Commands
{
    /// <summary>
    /// Delegate command provides a simple way of satisfying the <see cref="ICommand"/>
    /// interface and is designed to be used for operations that are
    /// always executable (i.e. where you would just return true for
    /// the method).
    /// </summary>
    public class DelegateCommand : ICommand
    {

        /// <summary>
        /// The message that will be returned in an <see cref="ArgumentNullException"/> if the
        /// action provided on construction is null.
        /// </summary>
        private const string NULL_MESSAGE =
            "The action supplied to DelegateCommand constructor cannot be null. " +
            "Review your code and make sure that the DelegateCommand is being " +
            "constructed with a valid Action";

        /// <summary>
        /// The action that will be invoked on <see cref="Execute(object)"/>
        /// </summary>
        private Action _Action;

        /// <summary>
        /// Initialises a <see cref="DelegateCommand"/> (<see cref="ICommand"/> implementation) which will
        /// always be executable.  Useful for simple operations, like a CloseWindow or
        /// Refresh button.  The action passed through cannot be null.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> that will be executed when the
        /// <see cref="Execute(object)"/> method is called</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown
        /// if consutructor is called with a null value for action parameter</exception>
        public DelegateCommand(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", NULL_MESSAGE);
            }

            _Action = action;
        }

        /// <summary>
        /// The event for CanExecuteChanged which is irrelevant for this
        /// implementation of <see cref="ICommand"/> as a <see cref="DelegateCommand"/> is always
        /// executable.
        /// </summary>
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore

        /// <summary>
        /// Determines whether an <see cref="ICommand"/> can execute, which will translate
        /// as enabled/disabled to a button in WPF if the command is bound.
        /// For <see cref="DelegateCommand"/> this will always return true and therefore
        /// the button would always be enabled.  For an implementation that
        /// allows defining whether the command is able to execute
        /// see <see cref="RelayCommand"/>
        /// </summary>
        /// <param name="parameter">The parmeter passed as the commandparameter,
        /// not used in this implementation.</param>
        /// <returns>Always returns true</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Invokes the <see cref="Action"/> that was supplied on construction
        /// of the <see cref="DelegateCommand"/>.
        /// </summary>
        /// <param name="parameter">The parameter passed as the commandparameter,
        /// not used in this implementation</param>
        public void Execute(object parameter)
        {
            _Action.Invoke();
        }
    }
}
