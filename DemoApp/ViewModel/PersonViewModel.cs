using Vaper.Core.Commands;
using Vaper.Core.ViewModels;
using Vaper.WindowMediation;
using Vaper.WindowMediation.WindowCreation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
            Load();
        }

        private void Load()
        {
            using (DemoModelContainer db = new DemoModelContainer())
            {
                AvailableHairColours = new ObservableCollection<HairColour>
                    (db.HairColours);
            }
        }

        [Required(ErrorMessage ="Must specify a first name")]
        [Displayable("First Name:", DisplayType.SimpleTextBox, 0)]
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
        [Displayable("Last Name:", DisplayType.SimpleTextBox, 1)]
        public string LastName
        {
            get { return _Person.LastName; }
            set
            {
                _Person.LastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        [Required(ErrorMessage ="Age must be specified")]
        [Range(18,int.MaxValue, ErrorMessage ="Age must be over 18")]
        [Displayable("Age:", DisplayType.SimpleTextBox, 2)]
        public int? Age
        {
            get { return _Person.Age; }
            set
            {
                _Person.Age = value;
                RaisePropertyChanged("Age");
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
        [Displayable("Hair Colour:", DisplayType.ComboBox, 3, "SelectedHairColour", "Colour")]
        public ObservableCollection<HairColour> AvailableHairColours
        {
            get { return _AvailableHairColours; }
            set
            {
                _AvailableHairColours = value;
                RaisePropertyChanged("AvailableHairColours");
            }
        }


        [Displayable("New hair colour", DisplayType.Button, 4)]
        public RelayCommand AddNewHairColourCommand { get { return new RelayCommand(AddNewHairColour, CanAddNewHairColour); } }

        private bool CanAddNewHairColour(object obj)
        {
            return true;
        }

        private void AddNewHairColour(object obj)
        {
            HairColourViewModel childVM = new HairColourViewModel(new HairColour());
            childVM.SetExitAction(Load);
            childVM.ShowWindow(WindowType.NewAutoWindowRequest);
        }



        [Displayable("Checked Papers:", DisplayType.CheckBox, 5)]
        public bool CheckedPapers
        {
            get { return _Person.CheckedPapers; }
            set
            {
                _Person.CheckedPapers = value;
                RaisePropertyChanged("CheckedPapers");
            }
        }


        [Displayable("Comments", DisplayType.LargeTextBox, 7, enabledBy:"CheckedPapers")]
        public string Comments
        {
            get { return _Person.Comments; }
            set
            {
                _Person.Comments = value;
                RaisePropertyChanged("Comments");
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
                DbSet set = db.Set(SelectedHairColour.GetType());

                

                db.HairColours.Attach(_Person.HairColour);
                db.People.Add(_Person);
                db.SaveChanges();
            }
        }

    }
}
