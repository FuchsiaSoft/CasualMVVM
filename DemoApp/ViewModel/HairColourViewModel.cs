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
    class HairColourViewModel : DataEntryViewModelBase
    {
        private HairColour _HairColour;

        public HairColourViewModel(HairColour hairColour)
        {
            _HairColour = hairColour;
        }

        [Required(ErrorMessage ="Must specify a hair colour")]
        [Displayable("Hair Colour:", DisplayType.SimpleTextBox, 0)]
        public string ColourName
        {
            get { return _HairColour.Colour; }
            set
            {
                _HairColour.Colour = value;
                RaisePropertyChanged("ColourName");
            }
        }

        protected override void SaveExisting(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void SaveNew(object parameter)
        {


            using (DemoModelContainer db = new DemoModelContainer())
            {
                db.HairColours.Add(_HairColour);
                db.SaveChanges();
            }
        }
    }
}
