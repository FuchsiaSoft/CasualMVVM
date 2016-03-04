using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuchsiaSoft.CasualMVVM.Core.Commands;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FuchsiaSoft.CasualMVVM.Core.ViewModels
{
    public abstract class DataEntryViewModelBase : SimpleViewModelBase, IDataEntryViewModel
    {
        protected ValidationContext _Context;

        protected ICollection<ValidationResult> _LastValidationState;

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
            if (Validate(out _LastValidationState))
            {
                return true;
            }

            return false;
        }

        public abstract void Save(object parameter);

        public virtual bool Validate(out ICollection<ValidationResult> validationResults)
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
                HasValidationConcern = true;
                IsValidated = false;
                return true;
            }

            HasValidationConcern = false;
            IsValidated = true;
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
