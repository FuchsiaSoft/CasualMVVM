using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMvvmExampleWpfApplication.ViewModels
{
    class HairViewModel : DataEntryViewModelBase
    {

        private string _ColourName;
        [Required(ErrorMessage ="Must specify a hair colour")]
        [Displayable("Hair colour", DisplayType.SimpleTextBox, 0)]
        public string ColourName
        {
            get { return _ColourName; }
            set
            {
                _ColourName = value;
                RaisePropertyChanged("ColourName");
            }
        }

        protected override void SaveExisting(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void SaveNew(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
