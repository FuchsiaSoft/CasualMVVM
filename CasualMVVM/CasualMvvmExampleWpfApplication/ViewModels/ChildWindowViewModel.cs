using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation;

namespace FuchsiaSoft.CasualMvvmExampleWpfApplication.ViewModels
{
    class ChildWindowViewModel : DataEntryViewModelBase
    {


        private string _FirstName;
        [Required(ErrorMessage ="Must specify a first name")]
        [Displayable("First name", DisplayType.SimpleTextBox, typeof(string), 1)]
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
        [Displayable("Last Name", DisplayType.SimpleTextBox, typeof(string), 2)]
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
        [Displayable("Notes", DisplayType.LargeTextBox, typeof(string), 4)]
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
        [Displayable("Age", DisplayType.SimpleTextBox, typeof(int?), 3)]
        public int? Age
        {
            get { return _Age; }
            set
            {
                _Age = value;
                RaisePropertyChanged("Age");
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
