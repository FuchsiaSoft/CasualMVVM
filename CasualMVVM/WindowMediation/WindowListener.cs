﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMVVM.WindowMediation
{
    /// <summary>
    /// Provides a central listening point for the event raised by WindowMediator
    /// requesting new windows for viewmodels.  This class will not pick up
    /// events unless StartListening() is called, this should generally be done on 
    /// application startup in the View layer of a MVVM application
    /// </summary>
    public static class WindowListener
    {
        private const string NULL_SERVICE_MESSAGE =
            "A null value was passed when a valid IWindowService implementation " +
            "was required.  A base implementation is provided with " +
            "FuchsiaSoft.CasualMVVM.WindowMediation.WindowService which will " +
            "open blank, standard WPF windows.  If you require a specific Window of " +
            "your own design, implement a class for " +
            "FuchsiaSoft.CasualMVVM.WindowMediation.IWindowService and use the ShowWindow " +
            "method to initialise and show your chosen Window.  You may also derive from " +
            "FuchsiaSoft.CasualMVVM.WindowMediation.WindowService, and the ShowWindow method " +
            "can be overridden";

        /// <summary>
        /// The IWindowService implementation that will do the opening of the windows.
        /// </summary>
        private static IWindowService _Service;

        /// <summary>
        /// Makes the listener start listening for window requests event from
        /// the WindowMediator.  You must supply a valid IWindowService to this method,
        /// a standard implementation (using blank WPF windows) can be found at
        /// FuchsiaSoft.CasualMVVM.WindowMediation.WindowService
        /// </summary>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown
        /// if the supplied IWindowService is null.  It must be a valid implementation
        /// of IWindowService</exception>
        public static void StartListening(IWindowService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service", NULL_SERVICE_MESSAGE);
            }

            WindowMediator.WindowRequested += WindowMessenger_WindowRequested;
        }

        /// <summary>
        /// Makes the listener start listening for window requests event from
        /// the WindowMediator.  If this constructor is called rather than the one
        /// requiring a IWindowService, 
        /// </summary>
        public static void StartListening()
        {
            StartListening(new WindowService());
        }

        /// <summary>
        /// When an event is spotted, the window will be opened using the
        /// IWindowService implementation, which checks for associated datatemplate
        /// in the App.xaml of the View... it will then create a blank Window
        /// and set the content to the associated Page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void WindowMessenger_WindowRequested(object sender, EventArgs e)
        {
            WindowMediatorEventArgs args = (WindowMediatorEventArgs)e;
            _Service.ShowWindow(args.ViewModel, args.WindowType);
        }
    }
}