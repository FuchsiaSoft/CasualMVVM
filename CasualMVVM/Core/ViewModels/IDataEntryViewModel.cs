using FuchsiaSoft.CasualMVVM.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace FuchsiaSoft.CasualMVVM.Core.ViewModels
{
    interface IDataEntryViewModel : IViewModel, IValidatingViewModel
    {
        string CurrentValidationConcern { get; set; }

        int ValidationConcernCount { get; set; }
        ObservableCollection<ValidationResult> LastValidationState { get; set; }
        DataEntryMode Mode { get; set; }
        ConditionalCommand SaveCommand { get; }

        bool CanSave(object parameter);

        void Save(object parameter);
    }
}
