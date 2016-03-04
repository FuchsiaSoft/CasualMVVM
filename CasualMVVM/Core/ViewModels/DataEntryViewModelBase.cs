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

        public bool Validate(out ICollection<ValidationResult> validationResults)
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
                return true;
            }

            return false;
        }
    }
}
