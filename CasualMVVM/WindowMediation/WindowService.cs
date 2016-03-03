using System.Windows;

namespace FuchsiaSoft.CasualMVVM.WindowMediation
{
    public class WindowService : IWindowService
    {
        /// <summary>
        /// For documentation refer to <see cref="IWindowService"/>
        /// This method can be overridden in a derived class to allow
        /// for custom Windows rather than standard WPF blank windows.
        /// </summary>
        /// <param name="viewModel"></param>
        public virtual void ShowWindow(object viewModel, WindowType type)
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
        }
    }
}
