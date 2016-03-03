﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMVVM.WindowMediation
{
    /// <summary>
    /// Possible message types that can be sent by the mediator
    /// </summary>
    public enum WindowType
    {
        /// <summary>
        /// Request a new window for the viewmodel
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
        /// Gets or sets the MessageType
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
    /// there is a WindowListener listening for the event.  If not the
    /// event will just be swallowed.  This allows for unit testing viewmodels
    /// in isolation of UI
    /// </summary>
    public static class WindowMediator
    {
        /// <summary>
        /// The event that is raised when a viewmodel has requested a new window,
        /// this will be accompanied by an EventArgs that can be cast as an instance
        /// of WindowMediatorEventArgs to access the viewmodel that made the request
        /// </summary>
        internal static event EventHandler WindowRequested;

        /// <summary>
        /// Raises the event (WindowRequested) requesting a new window be opened for the viewmodel.
        /// </summary>
        /// <param name="type">The type of request to raise</param>
        /// <param name="oldViewModel">The originating viewmodel (parent)</param>
        /// <param name="newViewModel">The viewmodel that needs a window opening for it (child)</param>
        internal static void RaiseMessage(WindowType type, object oldViewModel, object newViewModel)
        {
            EventHandler handler = WindowRequested;

            if (handler != null)
            {
                WindowMediatorEventArgs args = new WindowMediatorEventArgs()
                {
                    WindowType = type,
                    ViewModel = newViewModel
                };

                handler(oldViewModel, args);
            }
        }
    }
}