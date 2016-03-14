using Vaper.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace Vaper.Core.ViewModels
{
    public interface IDataEntryViewModel : IViewModel, IValidatingViewModel
    {
        DataEntryMode Mode { get; set; }

        RelayCommand SaveCommand { get; }

        SimpleCommand CancelCommand { get; }

        bool CanSave(object parameter);

        void Save(object parameter);
    }
}
