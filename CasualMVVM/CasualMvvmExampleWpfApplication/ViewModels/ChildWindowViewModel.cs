using Vaper.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Vaper.WindowMediation.WindowCreation;
using System.Windows.Media;
using VaperExampleWpfApplication.Model;
using System.Collections.ObjectModel;
using Vaper.Core.Commands;
using System.Windows;

namespace VaperExampleWpfApplication.ViewModels
{
    class ChildWindowViewModel : DataEntryViewModelBase
    {


        private string _FirstName;
        [Required(ErrorMessage ="Must specify a first name")]
        [Displayable("First name", DisplayType.SimpleTextBox, 1)]
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                RaisePropertyChanged("FirstName");
            }
        }


        private string _LastName;
        [Required(ErrorMessage ="Must specify a last name")]
        [Displayable("Last Name", DisplayType.SimpleTextBox, 2)]
        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                RaisePropertyChanged("LastName");
            }
        }


        private string _Notes;
        [Displayable("Notes", DisplayType.LargeTextBox, 4)]
        public string Notes
        {
            get { return _Notes; }
            set
            {
                _Notes = value;
                RaisePropertyChanged("Notes");
            }
        }


        private int? _Age;
        [Required(ErrorMessage ="Must specify an age")]
        [Range(18,100,ErrorMessage ="Age must be between 18 and 100")]
        [Displayable("Age", DisplayType.SimpleTextBox, 3)]
        public int? Age
        {
            get { return _Age; }
            set
            {
                _Age = value;
                RaisePropertyChanged("Age");
            }
        }



        private HairColour _SelectedHairColour;
        [Required(ErrorMessage ="Must specify a hair colour")]
        public HairColour SelectedHairColour
        {
            get { return _SelectedHairColour; }
            set
            {
                _SelectedHairColour = value;
                RaisePropertyChanged("SelectedHairColour");
            }
        }


        private ObservableCollection<HairColour> _HairColours = new ObservableCollection<HairColour>()
        {
            new HairColour() { ColourName = "Blonde" },
            new HairColour() { ColourName = "Black" },
            new HairColour() { ColourName = "Brown" },
            new HairColour() { ColourName = "White" }
        };
        [Displayable("Hair Colour", DisplayType.ListBox, 4, displayMemberPath:"ColourName", selectedItemPath:"SelectedHairColour")]
        public ObservableCollection<HairColour> HairColours
        {
            get { return _HairColours; }
            set
            {
                _HairColours = value;
                RaisePropertyChanged("HairColours");
            }
        }

        [Displayable("Add new hair colour",DisplayType.Button,5)]
        public RelayCommand AddNewHairColourCommand { get { return new RelayCommand(AddNewHairColour, CanAddNewHairColour); } }

        private bool CanAddNewHairColour(object obj)
        {
            return true;
        }

        private void AddNewHairColour(object obj)
        {
            HairViewModel newVM = new HairViewModel();
            newVM.ShowWindow(CasualMVVM.WindowMediation.WindowType.NewModalAutoWindowRequest);
        }



        private bool _CheckValue;
        [Displayable("Check box test", DisplayType.CheckBox, 6)]
        public bool CheckValue
        {
            get { return _CheckValue; }
            set
            {
                _CheckValue = value;
                RaisePropertyChanged("CheckValue");
            }
        }



        private DateTime? _ChosenDate;
        [Displayable("A date picker", DisplayType.DatePicker, 6)]
        [DateRange(2015,01,01,2015,12,31, ErrorMessage ="Date not in 2015")]
        public DateTime? ChosenDate
        {
            get { return _ChosenDate; }
            set
            {
                _ChosenDate = value;
                RaisePropertyChanged("ChosenDate");
            }
        }


        private string _LinkedText;
        [Displayable("Enabled by check", DisplayType.SimpleTextBox,  7, enabledBy:"CheckValue")]
        public string LinkedText
        {
            get { return _LinkedText; }
            set
            {
                _LinkedText = value;
                RaisePropertyChanged("LinkedText");
            }
        }


        protected override void SaveNew(object parameter)
        {
            CloseWindow();
            ExecuteExitAction();
        }

        protected override void SaveExisting(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
