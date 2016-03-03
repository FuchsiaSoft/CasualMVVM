using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using System.Windows;

namespace FuchsiaSoft.CasualMVVM.WindowMediation
{
    /// <summary>
    /// For documentation of the contract refer to <see cref="IWindowService"/>
    /// ... It is possible to derive an instance of this class to provide
    /// a custom <see cref="IWindowService"/> that can be passed to
    /// the <see cref="WindowListener"/> on application startup to facilitate
    /// your own logic or control which Window to use as referred to in
    /// documentation for <see cref="ShowWindow(object, WindowType)"/>
    /// </summary>
    public class WindowService : IWindowService
    {
        /// <summary>
        /// For documentation refer to <see cref="IWindowService"/>
        /// This method can be overridden in a derived class to allow
        /// for custom Windows rather than standard WPF blank windows.
        /// </summary>
        /// <param name="viewModel"></param>
        public virtual void ShowWindow(IViewModel viewModel, WindowType type)
        {
            Window window = new Window();
            window.Content = viewModel;

            switch (type)
            {
                case WindowType.NewWindowRequest:
                    window.Show();
                    break;

                case WindowType.NewModalWindowRequest:
                    window.ShowDialog();
                    break;

                default:
                    break;
            }

            viewModel.ActiveWindow = window;
        }
    }
}
