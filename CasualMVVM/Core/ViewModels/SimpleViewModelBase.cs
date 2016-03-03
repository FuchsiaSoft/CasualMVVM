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
    /// IViewModel, INotifyPropertyChanged and IDisposable.
    /// </summary>
    public abstract class SimpleViewModelBase : ObservableObject, IViewModel
    {
        /// <summary>
        /// The action that will be executed when ExecuteExitAction is called,
        /// or the ViewModel is deconstructed or disposed.
        /// </summary>
        private Action _ExitAction = null;

        /// <summary>
        /// Flag for whether the exit Action has already been invoked to
        /// prevent invoking it multiple times.
        /// </summary>
        private bool _HasActionInvoked = false;

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
            if (!_HasActionInvoked)
            {
                _ExitAction.Invoke();
            }
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.SetExitAction(Action)"/>
        /// </summary>
        public void SetExitAction(Action exitAction)
        {
            _ExitAction = exitAction;
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.ShowWindow"/>
        /// </summary>
        public virtual void ShowWindow()
        {
            ShowWindow(WindowType.NewWindowRequest);
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.ShowWindow(WindowType)"/>
        /// </summary>
        /// <param name="type"></param>
        public virtual void ShowWindow(WindowType type)
        {
            WindowMediator.RaiseMessage(type, this);
        }

        /// <summary>
        /// When the ViewModel is disposed the exit Action is invoked
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
