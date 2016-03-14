using Vaper.Core.ViewModels;
using Vaper.WindowMediation.WindowCreation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.ViewModel
{
    class GenericHairColourViewModel : DataEntryViewModelBase<HairColour, DemoModelContainer>
    {
        public GenericHairColourViewModel(HairColour hairColour, DataEntryMode mode) : base (hairColour, mode)
        {

        }


        public override void Load()
        {
            
        }

        [Required(ErrorMessage ="Must specify a hair colour")]
        [Displayable("Hair Colour", DisplayType.SimpleTextBox, 0)]
        public string HairColour
        {
            get { return _Entity.Colour; }
            set
            {
                _Entity.Colour = value;
                RaisePropertyChanged("HairColour");
            }
        }

    }
}
