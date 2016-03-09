using System.Collections.Generic;
using System.Linq;
using FuchsiaSoft.CasualMVVM.Core.Commands;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.ObjectModel;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;

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

        public SimpleCommand CancelCommand { get { return new SimpleCommand(Cancel); } }

        private void Cancel()
        {
            CloseWindow();
        }

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

    public abstract class DataEntryViewModelBase<T, Context> : DataEntryViewModelBase
        where T : class
        where Context : DbContext
    {

        protected DataEntryViewModelBase (T entity, DataEntryMode mode)
        {
            _Entity = entity;
            Mode = mode;

            ValidateState();

            Load();
        }

        private const string NOT_ENTITY_MESSAGE =
            "The type specified for the generic DataEntryViewModelBase<T, Context> is " +
            "not an entity type known to the DbContext Type specified.  Make sure that the " +
            "context and object type you are using are both from the same entity model.";

        protected T _Entity;

        private IEnumerable<Type> _EntityTypes = GetEntityTypes();

        private static IEnumerable<Type> GetEntityTypes()
        {
            using (Context db = (Activator.CreateInstance<Context>()))
            {
                return db.GetEntityTypes();
            }
        }

        /// <summary>
        /// Performs a quick check to make sure that the provided entity T
        /// matches a known Entity Type in supplied Context Type
        /// </summary>
        private void ValidateState()
        {
            if (!_EntityTypes.Any(t=> t == typeof(T))) 
            {
                throw new NotSupportedException(NOT_ENTITY_MESSAGE);
            }
        }

        public abstract void Load();

        protected override void SaveExisting(object parameter)
        {
            using (Context db = Activator.CreateInstance<Context>())
            {
                AttachRelatedProperties(db);

                db.Set(_Entity.GetType()).Attach(_Entity);

                MarkAsModified(db, _Entity);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Saves the <see cref="T"/> to the database as a new record.  This
        /// method will also attempt to attach any related entities if they
        /// already exist in the database, and create them if not.
        /// </summary>
        /// <param name="parameter"></param>
        protected override void SaveNew(object parameter)
        {
            using (Context db = Activator.CreateInstance<Context>())
            {
                AttachRelatedProperties(db);

                db.Set(_Entity.GetType()).Add(_Entity);

                db.SaveChanges();
            }
        }


        private void AttachRelatedProperties(Context db)
        {
            foreach (PropertyInfo property in _Entity.GetType().GetProperties())
            {
                if (IsEntityType(property))
                {
                    object entity = property.GetValue(_Entity);
                    DbSet set = db.Set(property.PropertyType);

                    set.Attach(entity);

                    MarkAsModified(db, entity);

                    if (db.Entry(entity).GetDatabaseValues() == null)
                    {
                        set.Add(entity);
                    }
                }
            }
        }


        /// <summary>
        /// Marks the specified entity as modified within the specified context
        /// just an internal helper method to get rid of a repeating, and ugly block
        /// of code.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        private void MarkAsModified(Context db, object entity)
        {
            ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager
                    .ChangeObjectState(entity, EntityState.Modified);
        }


        /// <summary>
        /// Checks if the provided property is a type included
        /// in the Entity Model, and if so returns true
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool IsEntityType(PropertyInfo property)
        {
            if (_EntityTypes.Any(t=>t == property.PropertyType))
            {
                return true;
            }

            return false;
        }
    }
}
