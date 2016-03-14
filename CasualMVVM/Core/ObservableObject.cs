using System.ComponentModel;

namespace Vaper.Core
{
    /// <summary>
    /// Provides an abstract base implementation of <see cref="INotifyPropertyChanged"/>,
    /// this is used to facilitate ViewModels, and can be derived to create
    /// any other object that is required to be observable and raise binding
    /// events.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// The event that is raised for data binding required by WPF.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the specified instance property.
        /// </summary>
        /// <param name="propertyName">The name of the property to
        /// raise the event for</param>
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
