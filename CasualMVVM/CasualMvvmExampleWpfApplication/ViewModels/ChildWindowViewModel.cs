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
        private string _FieldOne;
        [Required(ErrorMessage ="Must specify field one")]
        public string FieldOne
        {
            get { return _FieldOne; }
            set
            {
                _FieldOne = value;
                RaisePropertyChanged("FieldOne");
            }
        }

        private string _FieldTwo;
        [Required(ErrorMessage ="Must specify field two")]
        [Displayable("Big text field:", DisplayType.LargeTextBox,typeof(string), 2)]
        public string FieldTwo
        {
            get { return _FieldTwo; }
            set
            {
                _FieldTwo = value;
                RaisePropertyChanged("FieldTwo");
            }
        }

        private int _NumberField;
        [Range(5,1000,ErrorMessage ="Value must be between 5 and 1000")]
        [Displayable("Number Field:", DisplayType.SimpleTextBox, typeof(int), 1)]
        public int NumberField
        {
            get { return _NumberField; }
            set
            {
                _NumberField = value;
                RaisePropertyChanged("NumberField");
            }
        }

        private string _MyProperty;
        [Displayable("New Property:",DisplayType.LargeTextBox,typeof(string),4)]
        public string MyProperty
        {
            get { return _MyProperty; }
            set
            {
                _MyProperty = value;
                RaisePropertyChanged("MyProperty");
            }
        }

        private string _KitsonsProperty;
        [Displayable("Kitson:",DisplayType.SimpleTextBox,typeof(string),0)]
        public string KitsonsProperty
        {
            get { return _KitsonsProperty; }
            set
            {
                _KitsonsProperty = value;
                RaisePropertyChanged("KitsonsProperty");
            }
        }


        private string _NewProperty;
        [Displayable("Label text in here", DisplayType.LargeTextBox, typeof(string), 7)]
        public string NewProperty
        {
            get { return _NewProperty; }
            set
            {
                _NewProperty = value;
                RaisePropertyChanged("NewProperty");
            }
        }



        public override void Save(object parameter)
        {
            CloseWindow();
            ExecuteExitAction();
        }

        public override bool CanSave(object parameter)
        {
            bool returnValue = base.CanSave(parameter);

            if (returnValue == false)
            {
                CurrentValidationConcern = _LastValidationState.First().ErrorMessage;
            }
            else
            {
                CurrentValidationConcern = "No validation errors";
            }

            return returnValue;
        }
    }
}
