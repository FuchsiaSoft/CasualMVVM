using FuchsiaSoft.CasualMVVM.Core.Commands;
using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using FuchsiaSoft.CasualMVVM.WindowMediation;
using FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.ViewModel
{
    class PersonViewModel : DataEntryViewModelBase
    {
        private Person _Person;
        public PersonViewModel(Person person)
        {
            _Person = person;

            RefreshHairColours();
        }

        private void RefreshHairColours()
        {
            using (DemoModelContainer db = new DemoModelContainer())
            {
                AvailableHairColours = new ObservableCollection<HairColour>
                    (db.HairColours);

                if (SelectedHairColour != null)
                SelectedHairColour = AvailableHairColours
                    .Where(h => h.Id == SelectedHairColour.Id)
                    .FirstOrDefault();
            }
        }

        [Required(ErrorMessage ="Must specify a first name")]
        [Displayable("First Name", DisplayType.SimpleTextBox, typeof(string), 0)]
        public string FirstName
        {
            get { return _Person.FirstName; }
            set
            {
                _Person.FirstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        [Required(ErrorMessage ="Must specify a last name")]
        [Displayable("Last Name", DisplayType.SimpleTextBox, typeof(string), 1)]
        public string LastName
        {
            get { return _Person.LastName; }
            set
            {
                _Person.LastName = value;
                RaisePropertyChanged("LastName");
            }
        }


        [Required(ErrorMessage ="Must specify an age")]
        [Range(18,int.MaxValue,ErrorMessage ="Age must be over 18")]
        [Displayable("Age", DisplayType.SimpleTextBox, typeof(int?), 2)]
        public int? Age
        {
            get { return _Person.Age; }
            set
            {
                _Person.Age = value;
                RaisePropertyChanged("Age");
            }
        }



        [Displayable("Checked Papers", DisplayType.CheckBox, typeof(bool), 3)]
        public bool CheckedPapers
        {
            get { return _Person.CheckedPapers; }
            set
            {
                _Person.CheckedPapers = value;
                RaisePropertyChanged("CheckedPapers");
            }
        }


        [Displayable("Comments", DisplayType.LargeTextBox, typeof(string), 4, enabledBy:"CheckedPapers")]
        public string Comments
        {
            get { return _Person.Comments; }
            set
            {
                _Person.Comments = value;
                RaisePropertyChanged("Comments");
            }
        }



        [Required(ErrorMessage ="Must specify a hair colour")]
        public HairColour SelectedHairColour
        {
            get { return _Person.HairColour; }
            set
            {
                _Person.HairColour = value;
                RaisePropertyChanged("SelectedHairColour");
            }
        }


        private ObservableCollection<HairColour> _AvailableHairColours;
        [Displayable("Hair Colour", DisplayType.ListBox, typeof(ObservableCollection<HairColour>), 5, selectedItemPath:"SelectedHairColour", displayMemberPath:"Colour")]
        public ObservableCollection<HairColour> AvailableHairColours
        {
            get { return _AvailableHairColours; }
            set
            {
                _AvailableHairColours = value;
                RaisePropertyChanged("AvailableHairColours");
            }
        }

        [Displayable("Add new hair colour",DisplayType.Button,typeof(string),5)]
        public ConditionalCommand AddNewHairColourCommand { get { return new ConditionalCommand(AddNewHairColour, CanAddNewHairColour); } }

        private bool CanAddNewHairColour(object obj)
        {
            return true;
        }

        private void AddNewHairColour(object obj)
        {
            HairColourViewModel childVM = new HairColourViewModel(new HairColour());
            childVM.SetExitAction(RefreshHairColours);
            childVM.ShowWindow(WindowType.NewModalAutoWindowRequest);
        }




        protected override void SaveExisting(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void SaveNew(object parameter)
        {
            using (DemoModelContainer db = new DemoModelContainer())
            {
                db.People.Add(_Person);
                db.SaveChanges();
            }
        }
    }
}
