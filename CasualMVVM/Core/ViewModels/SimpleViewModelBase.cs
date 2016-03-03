using FuchsiaSoft.CasualMVVM.WindowMediation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FuchsiaSoft.CasualMVVM.Core.ViewModels
{
    /// <summary>
    /// Provides a simple base ViewModel to derive from, implementing
    /// <see cref="IViewModel"/>, <see cref="INotifyPropertyChanged"/> 
    /// and <see cref="IDisposable"/>.
    /// </summary>
    public abstract class SimpleViewModelBase : ObservableObject, IViewModel
    {
        /// <summary>
        /// The <see cref="Action"/> that will be executed when 
        /// <see cref="ExecuteExitAction"/>is called,
        /// or the ViewModel is deconstructed or disposed.
        /// </summary>
        protected Action<object> _ExitAction = null;

        /// <summary>
        /// Flag for whether the exit Action has already been invoked to
        /// prevent invoking it multiple times.
        /// </summary>
        protected bool _HasActionInvoked = false;

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.ActiveWindow"/>
        /// </summary>
        public Window ActiveWindow { get; private set; }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.CloseWindow"/>
        /// </summary>
        public void CloseWindow()
        {
            if (ActiveWindow != null)
            {
                ActiveWindow.Close();
            }
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.ExecuteExitAction"/>
        /// </summary>
        public void ExecuteExitAction()
        {
            ExecuteExitAction(null);
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.ExecuteExitAction(object)"/>
        /// </summary>
        /// <param name="parameter"></param>
        public void ExecuteExitAction(object parameter)
        {
            if (!_HasActionInvoked)
            {
                _ExitAction.Invoke(parameter);
            }
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.SetExitAction(Action)"/>
        /// </summary>
        public void SetExitAction(Action exitAction)
        {
            _ExitAction = new Action<object>(o => exitAction());
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.SetExitAction(Action{object})"/>
        /// </summary>
        /// <param name="exitAction"></param>
        public void SetExitAction(Action<object> exitAction)
        {
            _ExitAction = exitAction;
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.ShowWindow"/>
        /// </summary>
        public void ShowWindow()
        {
            ShowWindow(WindowType.NewWindowRequest);
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.ShowWindow(WindowType)"/>
        /// </summary>
        /// <param name="type"></param>
        public void ShowWindow(WindowType type)
        {
            WindowMediator.RaiseMessage(type, this);
        }

        /// <summary>
        /// When the ViewModel is disposed the exit <see cref="Action"/> is invoked
        /// (if it hasn't been invoked already once).  This method can be
        /// overridden if needed in your own ViewModels for bespoke
        /// behaviour or if your ViewModel needs to release
        /// resources on disposal.
        /// </summary>
        public virtual void Dispose()
        {
            ExecuteExitAction();
        }

        /// <summary>
        /// Deconstructor just invokes exit action if it hasn't already.
        /// </summary>
        ~SimpleViewModelBase()
        {
            ExecuteExitAction();
        }
    }
}
