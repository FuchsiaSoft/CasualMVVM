using Vaper.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using Vaper.WindowMediation.WindowCreation;
using System.Windows.Input;

namespace Vaper.Core.ViewModels
{
    /// <summary>
    /// Provides an interface for data entry ViewModels, which have reflection
    /// based validaiton and can be used for both adding and editing data
    /// re-using the same ViewModel.  inherits from <see cref="IViewModel"/>
    /// and <see cref="IValidatingViewModel"/>.  Classes implementing this interface
    /// are able to use the <see cref="Displayable"/> attribute for
    /// run-time generated data entry windows.
    /// </summary>
    public interface IDataEntryViewModel : IViewModel, IValidatingViewModel
    {
        /// <summary>
        /// Gets or sets the "Mode" of the ViewModel using the <see cref="DataEntryMode"/> enum.
        /// The value of this property should control the behaviour of the <see cref="Save"/> method.
        /// </summary>
        DataEntryMode Mode { get; set; }

        /// <summary>
        /// Gets a <see cref="RelayCommand"/> for which the <see cref="RelayCommand.CanExecute(object)"/>
        /// predicate is tied to the result of <see cref="CanSave(object)"/>
        /// It can be bound to a Save button in a view, or if using auto generated
        /// windows this will be bound automatically
        /// </summary>
        RelayCommand SaveCommand { get; }

        /// <summary>
        /// Gets a <see cref="SimpleCommand"/> which will close the active window, can
        /// be bound to a Cancel button in a view, or if using auto generated windows
        /// this will be bound automatically.
        /// </summary>
        SimpleCommand CancelCommand { get; }

        /// <summary>
        /// Indicates whether or not the <see cref="Save(object)"/> method
        /// can execute for the <see cref="SaveCommand"/>.  This method should get
        /// its value directly from the parent's <see cref="IValidatingViewModel.Validate(ICollection{ValidationResult})"/>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool CanSave(object parameter);

        /// <summary>
        /// Executes the save operation.
        /// </summary>
        /// <param name="parameter"></param>
        void Save(object parameter);
    }
}
