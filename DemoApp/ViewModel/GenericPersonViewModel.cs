using FuchsiaSoft.CasualMVVM.Core.Commands;
using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using FuchsiaSoft.CasualMVVM.WindowMediation;
using FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoApp.ViewModel
{
    class GenericPersonViewModel : DataEntryViewModelBase<Person, DemoModelContainer>
    {
        public GenericPersonViewModel(Person person, DataEntryMode mode) : base(person, mode)
        {
            //TODO: implement any custom logic for consutrction
            //here.  Base class will already populate the
            //protected _Entity field and call the Load()
            //method below
        }

        public override void Load()
        {
            AvailableHairColours = new ObservableCollection<HairColour>
                (_DbContext.HairColours);

            SelectedHairColour = AvailableHairColours
                .FirstOrDefault(h => h == _Entity.HairColour);

            AvailablePeople = new ObservableCollection<Person>
                (_DbContext.People);
        }

        [Required(ErrorMessage = "Must specify a first name")]
        [Displayable("First Name:", DisplayType.SimpleTextBox, 0)]
        public string FirstName
        {
            get { return _Entity.FirstName; }
            set
            {
                _Entity.FirstName = value;
                RaisePropertyChanged("FirstName");
            }
        }



        [Required(ErrorMessage = "Must specify a last name")]
        [Displayable("Last Name:", DisplayType.SimpleTextBox, 1)]
        public string LastName
        {
            get { return _Entity.LastName; }
            set
            {
                _Entity.LastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        [Required(ErrorMessage = "Age must be specified")]
        [Range(18, int.MaxValue, ErrorMessage = "Age must be over 18")]
        [Displayable("Age:", DisplayType.SimpleTextBox, 2)]
        public int? Age
        {
            get { return _Entity.Age; }
            set
            {
                _Entity.Age = value;
                RaisePropertyChanged("Age");
            }
        }


        [Required(ErrorMessage = "Must specify a hair colour")]
        public HairColour SelectedHairColour
        {
            get { return _Entity.HairColour; }
            set
            {
                _Entity.HairColour = value;
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
        public ConditionalCommand AddNewHairColourCommand { get { return new ConditionalCommand(AddNewHairColour, CanAddNewHairColour); } }

        private bool CanAddNewHairColour(object obj)
        {
            return true;
        }

        private void AddNewHairColour(object obj)
        {
            GenericHairColourViewModel childVM = new GenericHairColourViewModel(new HairColour(), DataEntryMode.New);
            childVM.SetExitAction(Load);
            childVM.ShowWindow(WindowType.NewAutoWindowRequest);

            //HairColourViewModel childVM = new HairColourViewModel(new HairColour());
            //childVM.SetExitAction(Load);
            //childVM.ShowWindow(WindowType.NewAutoWindowRequest);
        }


        [Displayable("Checked Papers:", DisplayType.CheckBox, 5)]
        public bool CheckedPapers
        {
            get { return _Entity.CheckedPapers; }
            set
            {
                _Entity.CheckedPapers = value;
                RaisePropertyChanged("CheckedPapers");
            }
        }


        private Person _RelatedPerson;
        [Displayable("Related person:",DisplayType.SearchableField,6,displayMemberPath:"FirstName", SearchCommandPath ="SearchPersonCommand")]
        public Person RelatedPerson
        {
            get { return _RelatedPerson; }
            set
            {
                _RelatedPerson = value;
                RaisePropertyChanged("RelatedPerson");
            }
        }

        private ObservableCollection<Person> _AvailablePeople;

        public ObservableCollection<Person> AvailablePeople
        {
            get { return _AvailablePeople; }
            set
            {
                _AvailablePeople = value;
                RaisePropertyChanged("AvailablePeople");
            }
        }


        public ICommand SearchPersonCommand { get { return new SimpleCommand(SearchPerson); } }

        private void SearchPerson()
        {
            Search(AvailablePeople, _RelatedPerson);
        }

        [Displayable("Comments", DisplayType.LargeTextBox, 7, enabledBy: "CheckedPapers")]
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
