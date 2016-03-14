using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaper.Core.Commands;

namespace Vaper.Core.ViewModels
{
    /// <summary>
    /// Provides the interface for a ViewModel which has the
    /// ability to handle validation of properties using
    /// reflection to examine data annotations.  The recommended use for
    /// this ViewModel is to have a <see cref="RelayCommand"/>'s <see cref="RelayCommand.CanExecute(object)"/>
    /// tied to the <see cref="Validate(ICollection{ValidationResult})"/> method, 
    /// which will make the bound button only clickable if validation is passed
    /// </summary>
    public interface IValidatingViewModel : IViewModel
    {
        /// <summary>
        /// Gets the current validation issue
        /// </summary>
        string CurrentValidationConcern { get; }

        /// <summary>
        /// Gets the total number of validation issues that are found
        /// in the ViewModel
        /// </summary>
        int ValidationConcernCount { get; }

        /// <summary>
        /// Gets a list of ValidationResults that together represent the
        /// state of validation for all properties in the ViewModel.
        /// This can be enumerated to check all validation issues.
        /// </summary>
        ObservableCollection<ValidationResult> LastValidationState { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> indicating whether or not the ViewModel
        /// passes validation
        /// </summary>
        bool IsValidated { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> indicating whether or not the VewModel
        /// fails validaiton.  This is the inverse of <see cref="IsValidated"/>
        /// and is mainly here as a convenience for binding
        /// </summary>
        bool HasValidationConcern { get; }

        /// <summary>
        /// Checks the attributes of all properties in the ViewModel, and
        /// validates any <see cref="ValidationAttribute"/>s found.  If
        /// any fail validation then this returns false and the collection
        /// specified in the parameter will be populated with the results.
        /// </summary>
        /// <param name="validationResults"></param>
        /// <returns></returns>
        bool Validate(ICollection<ValidationResult> validationResults);
    }
}
