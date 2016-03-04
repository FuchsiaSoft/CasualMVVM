using FuchsiaSoft.CasualMVVM.WindowMediation;
using FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation;
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
    /// </summary>
    public abstract class SimpleViewModelBase : ObservableObject, IViewModel
    {
        /// <summary>
        /// The <see cref="Action"/> that will be executed when 
        /// <see cref="ExecuteExitAction"/>is called,
        /// or the ViewModel is deconstructed
        /// </summary>
        protected Action<object> _ExitAction = null;

        /// <summary>
        /// Flag for whether the exit Action has already been invoked to
        /// prevent invoking it multiple times.
        /// </summary>
        protected bool _HasActionInvoked = false;

        private Window _ActiveWindow;
        /// <summary>
        /// For documentation refer to <see cref="IViewModel.ActiveWindow"/>
        /// </summary>
        public Window ActiveWindow
        {
            get { return _ActiveWindow; }
            set
            {
                _ActiveWindow = value;
                if (InvokeOnWindowClose)
                {
                    _ActiveWindow.Closed += _ActiveWindow_Closed; 
                }
            }
        }

        /// <summary>
        /// For documentaiton refer to <see cref="IViewModel.InvokeOnWindowClose"/>
        /// </summary>
        public bool InvokeOnWindowClose { get; set; }

        protected virtual void _ActiveWindow_Closed(object sender, EventArgs e)
        {
            ExecuteExitAction();
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.WindowTitle"/>
        /// </summary>
        public string WindowTitle { get; set; }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.CloseWindow"/>
        /// </summary>
        public void CloseWindow()
        {
            CloseWindow(true);
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.CloseWindow(bool)"/>
        /// </summary>
        /// <param name="forceExitAction"></param>
        public void CloseWindow(bool forceExitAction)
        {
            if (ActiveWindow != null)
            {
                ActiveWindow.Close();
            }

            if (forceExitAction)
            {
                ExecuteExitAction();
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
            if (!_HasActionInvoked && _ExitAction != null)
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
        public void ShowWindow(WindowType type, IWindowSettings settings = null)
        {
            WindowMediator.RaiseMessage(type, this, settings);
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
