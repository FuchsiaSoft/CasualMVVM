using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vaper.Core.ViewModels
{
    /// <summary>
    /// Provides a base implementation of <see cref="IValidatingViewModel"/>, this class
    /// also derives from <see cref="SimpleViewModelBase/>
    /// </summary>
    public abstract class ValidatingViewModelBase : SimpleViewModelBase, IValidatingViewModel
    {
        protected ValidationContext _Context;

        private int _ValidationConcernCount;
        /// <summary>
        /// For documentation refer to <see cref="IValidatingViewModel.ValidationConcernCount"/>
        /// </summary>
        public int ValidationConcernCount
        {
            get { return _ValidationConcernCount; }
            private set
            {
                _ValidationConcernCount = value;
                RaisePropertyChanged("ValidationConcernCount");
            }
        }

        private ObservableCollection<ValidationResult> _LastValidationState;
        /// <summary>
        /// For documentation refer to <see cref="IValidatingViewModel.LastValidationState"/>
        /// </summary>
        public ObservableCollection<ValidationResult> LastValidationState
        {
            get { return _LastValidationState; }
            private set
            {
                _LastValidationState = value;
                RaisePropertyChanged("LastValidationState");
            }
        }


        private string _CurrentValidationConcern;
        /// <summary>
        /// For documentation refer to <see cref="IValidatingViewModel.CurrentValidationConcern"/>
        /// </summary>
        public string CurrentValidationConcern
        {
            get { return _CurrentValidationConcern; }
            private set
            {
                _CurrentValidationConcern = value;
                RaisePropertyChanged("CurrentValidationConcern");
            }
        }

        private bool _IsValidated;
        /// <summary>
        /// For documentation refer to <see cref="IValidatingViewModel.IsValidated"/>
        /// </summary>
        public bool IsValidated
        {
            get { return _IsValidated; }
            private set
            {
                _IsValidated = value;
                RaisePropertyChanged("IsValidated");
            }
        }

        private bool _HasValidationConcern;
        /// <summary>
        /// For documentation refer to <see cref="IValidatingViewModel.HasValidationConcern"/>
        /// </summary>
        public bool HasValidationConcern
        {
            get { return _HasValidationConcern; }
            private set
            {
                _HasValidationConcern = value;
                RaisePropertyChanged("HasValidationConcern");
            }
        }

        /// <summary>
        /// For documentation refer to <see cref="IValidatingViewModel.Validate(ICollection{ValidationResult})"/>
        /// </summary>
        /// <param name="validationResults"></param>
        /// <returns></returns>
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


    }
}
