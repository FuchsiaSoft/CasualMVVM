using System.Collections.Generic;
using System.Linq;
using FuchsiaSoft.CasualMVVM.Core.Commands;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.ObjectModel;

namespace FuchsiaSoft.CasualMVVM.Core.ViewModels
{
    /// <summary>
    /// Used to indicate to a <see cref="DataEntryViewModelBase"/>
    /// whether the window is used for creating new records, or
    /// editing existing records.  Assists in facilitating
    /// ViewModel re-use for both instances.
    /// </summary>
    public enum DataEntryMode
    {
        /// <summary>
        /// ViewModel is for a new records
        /// </summary>
        New,
        /// <summary>
        /// ViewModel is for existing records
        /// </summary>
        Edit
    }

    public abstract class DataEntryViewModelBase : SimpleViewModelBase, IDataEntryViewModel
    {
        protected ValidationContext _Context;

        public DataEntryMode Mode { get; set; } = DataEntryMode.New;

        private int _ValidationConcernCount;

        public int ValidationConcernCount
        {
            get { return _ValidationConcernCount; }
            set
            {
                _ValidationConcernCount = value;
                RaisePropertyChanged("ValidationConcernCount");
            }
        }


        private ObservableCollection<ValidationResult> _LastValidationState;

        public ObservableCollection<ValidationResult> LastValidationState
        {
            get { return _LastValidationState; }
            set
            {
                _LastValidationState = value;
                RaisePropertyChanged("LastValidationState");
            }
        }

        private string _CurrentValidationConcern;

        public string CurrentValidationConcern
        {
            get { return _CurrentValidationConcern; }
            set
            {
                _CurrentValidationConcern = value;
                RaisePropertyChanged("CurrentValidationConcern");
            }
        }

        private bool _HasValidationConcern;

        public bool HasValidationConcern
        {
            get { return _HasValidationConcern; }
            set
            {
                _HasValidationConcern = value;
                RaisePropertyChanged("HasValidationConcern");
            }
        }


        private bool _IsValidated;

        public bool IsValidated
        {
            get { return _IsValidated; }
            set
            {
                _IsValidated = value;
                RaisePropertyChanged("IsValidated");
            }
        }

        public ConditionalCommand SaveCommand { get { return new ConditionalCommand(Save, CanSave); } }

        public virtual bool CanSave(object parameter)
        {
            if (Validate(_LastValidationState))
            {
                return true;
            }

            return false;
        }

        public virtual void Save(object parameter)
        {
            switch (Mode)
            {
                case DataEntryMode.New:
                    SaveNew(parameter);
                    CloseWindow(true);
                    break;

                case DataEntryMode.Edit:
                    SaveExisting(parameter);
                    CloseWindow(true);
                    break;

                default:
                    break;
            }
        }

        protected abstract void SaveNew(object parameter);

        protected abstract void SaveExisting(object parameter);

        public virtual bool Validate(ICollection<ValidationResult> validationResults)
        {
            if (_Context == null) _Context = new ValidationContext(this);

            validationResults = new List<ValidationResult>();

            IEnumerable<PropertyInfo> properties = this.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                _Context.MemberName = property.Name;

                Validator.TryValidateProperty
                    (property.GetValue(this), _Context, validationResults);
            }

            if (validationResults.Count == 0)
            {
                HasValidationConcern = false;
                CurrentValidationConcern = null;
                ValidationConcernCount = 0;
                IsValidated = true;
                return true;
            }

            HasValidationConcern = true;
            CurrentValidationConcern =
                validationResults.First().ErrorMessage;
            ValidationConcernCount = validationResults.Count - 1;
            IsValidated = false;
            return false;
        }

        public virtual ValidationResult ValidateProperty(string propertyName)
        {
            return ValidateProperty(this.GetType().GetProperty(propertyName));
        }

        public virtual ValidationResult ValidateProperty(PropertyInfo property)
        {
            ValidationContext context = new ValidationContext(this);

            ICollection<ValidationResult> results = new List<ValidationResult>();
                Validator.TryValidateProperty(property, context, results);

            return results.FirstOrDefault();
        }
    }
}
