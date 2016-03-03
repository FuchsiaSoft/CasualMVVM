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
    /// Provides the core interface for all ViewModels, including
    /// INotifyPropertyChanged and IDisposable.
    /// IDisposable is only used in this library to allow for
    /// disposal to invoke the exit Action, but can be overridden
    /// if necessary for applied uses.
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// READ ONLY reference to the Window that the ViewModel
        /// is associated with, if set by the WindowMediator.  This
        /// property may return null, and will only be set if the
        /// ShowWindow method is called.
        /// </summary>
        Window ActiveWindow { get; }

        /// <summary>
        /// Uses the WindowMediator to open a new blank Window, and
        /// set the content of the window to the XAML page associated
        /// with the ViewModel.
        /// </summary>
        void ShowWindow();

        /// <summary>
        /// Closes the Window associated with this ViewModel from the
        /// ViewModel layer.  If the ActiveWindow is not set then this
        /// method does nothing to preserve unit testability
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Sets the Action that the ViewModel should execute on ExecuteExitAction, or
        /// on deconstruction, or disposal.
        /// </summary>
        /// <param name="exitAction">The Action to execute</param>
        void SetExitAction(Action exitAction);

        /// <summary>
        /// Executes the Action specified when calling SetExitAction.  This method
        /// can be invoked manually, but will also be called on deconstruction or
        /// disposal.
        /// </summary>
        void ExecuteExitAction();
    }
}
