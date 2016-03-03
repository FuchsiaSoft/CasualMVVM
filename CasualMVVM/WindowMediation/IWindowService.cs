namespace FuchsiaSoft.CasualMVVM.WindowMediation
{
    /// <summary>
    /// Provides the interface for opening new Windows
    /// ala MVVM.  Windows will all be opened blank, and
    /// their content property will be set as a xaml page
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Opens a blank window and sets the content
        /// for it to the requisite xaml Page
        /// (found from DataTemplate set in App.xaml)
        /// </summary>
        /// <param name="ViewModel">The ViewModel to open
        /// the window for</param>
        /// <param name="type">The window type to open</param>
        void ShowWindow(object ViewModel, WindowType type);
    }
}
