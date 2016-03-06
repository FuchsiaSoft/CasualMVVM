using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMVVM.Core.ViewModels
{
    interface IValidatingViewModel : IViewModel
    {
        bool Validate(ICollection<ValidationResult> validationResults);
    }
}
