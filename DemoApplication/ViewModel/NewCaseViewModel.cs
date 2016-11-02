using DemoApplication.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaper.Core.ViewModels;
using Vaper.WindowMediation.WindowCreation;

namespace DemoApplication.ViewModel
{
    class NewCaseViewModel : DataEntryViewModelBase<Case, DemoModelContainer>
    {
        public NewCaseViewModel(Case entity, DataEntryMode mode) : base (entity, mode)
        {

        }

        public override void Load()
        {
            
        }

        [Displayable("Case name", DisplayType.SimpleTextBox, 0)]
        [Required(ErrorMessage ="Must specify a case name")]
        public string Name
        {
            get { return _Entity.Name; }
            set
            {
                _Entity.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        [Displayable("Opened date", DisplayType.DatePicker, 1)]
        public DateTime OpenedDate
        {
            get { return _Entity.OpenedDate; }
            set
            {
                _Entity.OpenedDate = value;
                RaisePropertyChanged("OpenedDate");
            }
        }

        [Displayable("Comments", DisplayType.LargeTextBox, 2)]
        public string Comments
        {
            get { return _Entity.Comments; }
            set
            {
                _Entity.Comments = value;
                RaisePropertyChanged("Comments");
            }
        }



    }
}
