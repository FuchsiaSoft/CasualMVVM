using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vaper.Core.ViewModels
{
    public interface IValidatingViewModel : IViewModel
    {
        string CurrentValidationConcern { get; }

        int ValidationConcernCount { get; }
        ObservableCollection<ValidationResult> LastValidationState { get; }

        bool IsValidated { get; }

        bool HasValidationConcern { get; }

        bool Validate(ICollection<ValidationResult> validationResults);
    }
}
