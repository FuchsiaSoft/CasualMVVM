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
    public interface IDataEntryViewModel : IViewModel, IValidatingViewModel
    {
        DataEntryMode Mode { get; set; }

        ConditionalCommand SaveCommand { get; }

        SimpleCommand CancelCommand { get; }

        bool CanSave(object parameter);

        void Save(object parameter);
    }
}
