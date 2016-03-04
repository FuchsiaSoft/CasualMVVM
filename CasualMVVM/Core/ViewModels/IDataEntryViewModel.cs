using FuchsiaSoft.CasualMVVM.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FuchsiaSoft.CasualMVVM.Core.ViewModels
{
    interface IDataEntryViewModel : IViewModel
    {
        bool Validate(out ICollection<ValidationResult> validationResults);

        ConditionalCommand SaveCommand { get; }

        bool CanSave(object parameter);

        void Save(object parameter);

    }
}
