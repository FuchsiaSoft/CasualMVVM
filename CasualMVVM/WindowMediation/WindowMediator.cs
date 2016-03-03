using System;

namespace FuchsiaSoft.CasualMVVM.WindowMediation
{
    /// <summary>
    /// Possible Window types that can be sent by the mediator
    /// </summary>
    public enum WindowType
    {
        /// <summary>
        /// Request a new <see cref="System.Windows.Window"/> for the viewmodel
        /// </summary>
        NewWindowRequest,
        /// <summary>
        /// Request a new modal window for the viewmodel
        /// </summary>
        NewModalWindowRequest
    }

    /// <summary>
    /// Derived from EventArgs to be passed through when Mediator
    /// raises events so that the correct viewmodel can be traced
    /// </summary>
    internal class WindowMediatorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the <see cref="WindowType"/>
        /// </summary>
        public WindowType WindowType { get; set; }
        /// <summary>
        /// Gets or sets the ViewModel that needs a window requesting for it
        /// </summary>
        public object ViewModel { get; set; }

    }

    /// <summary>
    /// Provides a central point for raising events requesting new windows
    /// for viewmodels.  This event can be raised to spawn new windows if
    /// there is a <see cref="WindowListener"/> listening for the event.  If not the
    /// event will just be swallowed.  This allows for unit testing viewmodels
    /// in isolation of UI
    /// </summary>
    public static class WindowMediator
    {
        /// <summary>
        /// The event that is raised when a viewmodel has requested a new window,
        /// this will be accompanied by an <see cref="EventArgs"/> that can be cast as an instance
        /// of <see cref="WindowMediatorEventArgs"/> to access the viewmodel that made the request
        /// </summary>
        internal static event EventHandler WindowRequested;

        /// <summary>
        /// Raises the event (<see cref="WindowRequested"/>) requesting a new window be opened for the viewmodel.
        /// </summary>
        /// <param name="type">The type of request to raise</param>
        /// <param name="newViewModel">The viewmodel that needs a window opening for it</param>
        internal static void RaiseMessage(WindowType type, object viewModel)
        {
            EventHandler handler = WindowRequested;

            if (handler != null)
            {
                WindowMediatorEventArgs args = new WindowMediatorEventArgs()
                {
                    WindowType = type,
                    ViewModel = viewModel
                };

                handler(viewModel, args);
            }
        }
    }
}
