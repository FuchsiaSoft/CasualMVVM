using Vaper.WindowMediation;
using Vaper.WindowMediation.WindowCreation;
using System;
using System.Windows;

namespace Vaper.Core.ViewModels
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
            private set
            {
                _ActiveWindow = value;
                if (InvokeOnWindowClose)
                {
                    _ActiveWindow.Closed += _ActiveWindow_Closed; 
                }
            }
        }

        private bool _IsBusy;

        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        protected virtual void MarkBusy()
        {
            IsBusy = true;
        }

        protected virtual void MarkFree()
        {
            IsBusy = false;
        }

        /// <summary>
        /// For documentaiton refer to <see cref="IViewModel.InvokeOnWindowClose"/>
        /// </summary>
        public bool InvokeOnWindowClose { get; private set; }

        protected virtual void _ActiveWindow_Closed(object sender, EventArgs e)
        {
            ExecuteExitAction();
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.WindowTitle"/>
        /// this property can be overridden in derived classes
        /// to provide custom logic for determining window
        /// title
        /// </summary>
        public virtual string WindowTitle { get; set; }

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
                _HasActionInvoked = true;
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
        public virtual void ShowWindow()
        {
            ShowWindow(WindowType.NewWindowRequest);
        }

        /// <summary>
        /// For documentation refer to <see cref="IViewModel.ShowWindow(WindowType)"/>
        /// </summary>
        /// <param name="type"></param>
        public virtual void ShowWindow(WindowType type, IWindowSettings settings = null)
        {
            WindowMediator.RaiseMessage(type, this, settings);
        }

        public void SetActiveWindow(Window window, bool invokeOnClose)
        {
            InvokeOnWindowClose = invokeOnClose;
            ActiveWindow = window;
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
