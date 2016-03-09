using FuchsiaSoft.CasualMVVM.WindowMediation;
using FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation;
using System;
using System.ComponentModel;
using System.Windows;

namespace FuchsiaSoft.CasualMVVM.Core.ViewModels
{
    /// <summary>
    /// Provides the core interface for all ViewModels, including
    /// <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Reference to the <see cref="Window"/> that the ViewModel
        /// is associated with, if set by the <see cref="WindowMediator"/>.  This
        /// property may return null, and will only be set if the
        /// <see cref="ShowWindow"/> method is called.
        /// </summary>
        Window ActiveWindow { get; }

        /// <summary>
        /// Gets or sets the flag which controls whether the <see cref="IViewModel"/>
        /// will invoke its exit <see cref="Action"/> on <see cref="Window.Closed"/>
        /// </summary>
        bool InvokeOnWindowClose { get; }

        /// <summary>
        /// Uses the <see cref="WindowMediator"/> to open a new blank <see cref="Window"/>, and
        /// set the content of the window to the XAML page associated
        /// with the ViewModel.  If this method is used then the window
        /// will not be opened modal by default.  If you need a modal window
        /// refer to <see cref="ShowWindow(WindowType)"/>
        /// </summary>
        void ShowWindow();

        /// <summary>
        /// Uses the <see cref="WindowMediator"/> to open a new blank <see cref="Window"/>, using 
        /// the supplied <see cref="WindowType"/> to control whether or not the window is modal.
        /// The content of the window is set to the XAML page associated with
        /// the ViewModel.
        /// </summary>
        /// <param name="type"></param>
        void ShowWindow(WindowType type, IWindowSettings settings = null);

        /// <summary>
        /// Closes the <see cref="Window"/> associated with this ViewModel from the
        /// ViewModel layer.  If the <see cref="ActiveWindow"/> is not set then this
        /// method does nothing so as to preserve unit testability.  If this overload
        /// is used, then by default the exit action will be invoked.
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Same as <see cref="CloseWindow"/> except a bool is available to
        /// force the exit action to invoke on closing the Window.
        /// </summary>
        /// <param name="forceExitAction"></param>
        void CloseWindow(bool forceExitAction);

        /// <summary>
        /// Sets the <see cref="Action"/> that the ViewModel should execute on 
        /// <see cref="ExecuteExitAction"/>, or
        /// on deconstruction
        /// </summary>
        /// <param name="exitAction">The <see cref="Action"/> to execute</param>
        void SetExitAction(Action exitAction);

        /// <summary>
        /// Sets the <see cref="Action"/> that the ViewModel should execute on 
        /// <see cref="ExecuteExitAction"/>, or
        /// on deconstruction
        /// </summary>
        /// <param name="exitAction"></param>
        void SetExitAction(Action<object> exitAction);

        /// <summary>
        /// Executes the <see cref="Action"/> specified when calling <see cref="SetExitAction(Action)"/>.  This method
        /// can be invoked manually, but will also be called on deconstruction
        /// </summary>
        void ExecuteExitAction();

        /// <summary>
        /// Executes the <see cref="Action{T}"/> specified when calling <see cref="SetExitAction(Action{object})"/>
        /// with the specified parameter.
        /// </summary>
        /// <param name="parameter"></param>
        void ExecuteExitAction(object parameter);

        /// <summary>
        /// Sets the title of the Window (if using the standard <see cref="WindowService"/>
        /// provided, which binds to this property.
        /// </summary>
        string WindowTitle { get; set; }

        void SetActiveWindow(Window window, bool invokeOnClose);
    }
}
