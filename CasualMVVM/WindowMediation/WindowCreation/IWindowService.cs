using FuchsiaSoft.CasualMVVM.Core.ViewModels;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    /// <summary>
    /// Provides the interface for opening new Windows
    /// ala MVVM.  Windows will all be opened blank, and
    /// their content property will be set as a xaml page
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Opens a blank <see cref="System.Windows.Window"/> and sets the content
        /// for it to the requisite xaml Page
        /// (found from DataTemplate set in App.xaml)
        /// </summary>
        /// <param name="ViewModel">The ViewModel to open
        /// the window for</param>
        /// <param name="type">The <see cref="WindowType"/> to open</param>
        void ShowWindow(IViewModel ViewModel, WindowType type, IWindowSettings settings);

        /// <summary>
        /// Creates a search window
        /// </summary>
        /// <param name="viewModel"></param>
        void ShowSearchWindow(IViewModel viewModel);
    }
}
