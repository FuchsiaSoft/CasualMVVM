using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FuchsiaSoft.CasualMVVM.Core.Commands
{
    /// <summary>
    /// RelayCommand provides a comprehensive implementation of the ICommand
    /// interface and is designed for operations where the ability to execute
    /// the action is variable and determined by the application state.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The message that will be returned in an ArgumentNullException if the
        /// action provided on construction is null.
        /// </summary>
        private const string NULL_ACTION_MESSAGE =
            "The action supplied to RelayCommand constructor cannot be null. " +
            "Review your code and make sure that the RelayCommand is being " +
            "constructed with a valid Action";

        /// <summary>
        /// The message that will be returned in an ArgumentNullException if the
        /// Predicate provided on construction is null (assuming the constructor
        /// overload requiring a predicate was the one called)
        /// </summary>
        private const string NULL_PREDICATE_MESSAGE =
            "The predicate supplied to RelayCommand constructor cannot be null. " +
            "Review your code and make sure that the RelayCommand is being " +
            "constructed with a valid Predicate.  If you are seeking to " +
            "have a RelayCommand that will always be executable, either " +
            "use the constructor overload that does not require a Predicate " + 
            "parameter, or look at DelegateCommand";

        /// <summary>
        /// The actioAction that will be invoked on Execute
        /// </summary>
        private Action<object> _Action;

        /// <summary>
        /// The predicate that determines whether or not the action
        /// is executable
        /// </summary>
        private Predicate<object> _Predicate;

        /// <summary>
        /// Initialises a RelayCommand (ICommand implementation) for which the
        /// Execute action's ability to execute is dependent on the supplied predicate.
        /// The supplied Predicate should determine from the application state whether
        /// or not the Action is safe/allowed to invoke at the current time.
        /// </summary>
        /// <param name="action">The action that should be invoked on
        /// command execution.</param>
        /// <param name="predicate">The Predicate that determines whether or
        /// not the action is allowed to be invoked.</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown
        /// if either parameters for this constructor are null.  If no predicate is required
        /// then use the other constructor overload or look at DelegateCommand</exception>
        public RelayCommand(Action<object> action, Predicate<object> predicate)
        {
            CheckAction(action);
            CheckPredicate(predicate);

            _Action = action;
            _Predicate = predicate;
        }

        /// <summary>
        /// Initialises a RelayCommand (ICommand implementation) for which the Execute
        /// action's ability to execute is not dependent on the application state.  If
        /// this constructor is used then an internal Predicate will be used for evaluation
        /// that always returns true.
        /// </summary>
        /// <param name="action">The Action that should be invoked on command
        /// execution.</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown
        /// if the supplied Action is null.</exception>
        public RelayCommand(Action<object> action)
        {
            CheckAction(action);

            _Action = action;
            _Predicate = ReturnTrue;
        }

        /// <summary>
        /// The event that handles changing state for the Predicate
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Determines whether an ICommand can execute.  For this implementation
        /// the return value of this method is determined by the Predicate supplied
        /// on construction, or if none was supplied then it will always return true.
        /// </summary>
        /// <param name="parameter">The parmeter passed as the command parameter</param>
        /// <returns>True if the command can execute.</returns>
        public bool CanExecute(object parameter)
        {
            return _Predicate.Invoke(parameter);
        }

        /// <summary>
        /// Invokes the Action that was supplied on construction.
        /// </summary>
        /// <param name="parameter">The parameter passed as the commandparameter.</param>
        public void Execute(object parameter)
        {
            _Action.Invoke(parameter);
        }

        /// <summary>
        /// Checks whether the supplied Action is null.  In own method to prevent duplication.
        /// </summary>
        /// <param name="action"></param>
        private static void CheckAction(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action", NULL_ACTION_MESSAGE);
            }
        }

        /// <summary>
        /// Checks whether the supplied Predicate is null, in own method to prevent duplication.
        /// </summary>
        /// <param name="predicate"></param>
        private static void CheckPredicate(Predicate<object> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", NULL_PREDICATE_MESSAGE);
            }
        }

        /// <summary>
        /// An private method which is used in absence of a supplied
        /// predicate and will always return true.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool ReturnTrue(object parameter)
        {
            return true;
        }
    }
}
