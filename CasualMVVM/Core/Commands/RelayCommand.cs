using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Vaper.Core.Commands
{
    /// <summary>
    /// RelayCommand provides an implementation of the <see cref="ICommand"/>
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
            "parameter, or look at SimpleCommand";

        /// <summary>
        /// The <see cref="Action{T}"/> that will be invoked on <see cref="Execute(object)"/>
        /// </summary>
        private Action<object> _Action;

        /// <summary>
        /// The <see cref="Predicate{T}"/> that determines whether or not the action
        /// is executable
        /// </summary>
        private Predicate<object> _Predicate;

        /// <summary>
        /// Initialises a <see cref="RelayCommand"/> (<see cref="ICommand"/> implementation) for which the
        /// <see cref="Execute(object)"/> <see cref="Action"/>'s ability to 
        /// execute is dependent on the supplied <see cref="Predicate{T}"/>.
        /// The supplied <see cref="Predicate{T}"/> should determine from the application state whether
        /// or not the <see cref="Action"/> is safe/allowed to invoke at the current time.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> that should be invoked on
        /// command execution.</param>
        /// <param name="predicate">The <see cref="Predicate{T}"/> that determines whether or
        /// not the action is allowed to be invoked.</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown
        /// if either parameters for this constructor are null.  If no predicate is required
        /// then use the other constructor overload or look at <see cref="SimpleCommand"/></exception>
        public RelayCommand(Action<object> action, Predicate<object> predicate)
        {
            CheckAction(action);
            CheckPredicate(predicate);

            _Action = action;
            _Predicate = predicate;
        }

        /// <summary>
        /// Initialises a <see cref="RelayCommand"/> (<see cref="ICommand"/> 
        /// implementation) for which the <see cref="Execute(object)"/>
        /// <see cref="Action"/>'s ability to execute is not dependent on the application state.  If
        /// this constructor is used then an internal <see cref="Predicate{T}"/> will be used for evaluation
        /// that always returns true.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> that should be invoked on command
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
        /// The event that handles changing state for the <see cref="Predicate{T}"/>
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
        /// Determines whether an <see cref="ICommand"/> can execute.  For this implementation
        /// the return value of this method is determined by the <see cref="Predicate{T}"/> supplied
        /// on construction, or if none was supplied then it will always return true.
        /// </summary>
        /// <param name="parameter">The parmeter passed as the command parameter</param>
        /// <returns>True if the command can execute.</returns>
        public bool CanExecute(object parameter)
        {
            return _Predicate.Invoke(parameter);
        }

        /// <summary>
        /// Invokes the <see cref="Action"/> that was supplied on construction.
        /// </summary>
        /// <param name="parameter">The parameter passed as the commandparameter.</param>
        public void Execute(object parameter)
        {
            _Action.Invoke(parameter);
        }

        /// <summary>
        /// Checks whether the supplied <see cref="Action"/> is null.  
        /// In own method to prevent duplication.
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
        /// Checks whether the supplied <see cref="Predicate{T}"/> is null, 
        /// in own method to prevent duplication.
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
        /// <see cref="Predicate{T}"/> and will always return true.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool ReturnTrue(object parameter)
        {
            return true;
        }
    }
}
