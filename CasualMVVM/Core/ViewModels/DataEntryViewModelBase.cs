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
using FuchsiaSoft.CasualMVVM.WindowMediation;
using FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation;

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

    public abstract class DataEntryViewModelBase : ValidatingViewModelBase, IDataEntryViewModel
    {
        public DataEntryMode Mode { get; set; } = DataEntryMode.New;

        public ConditionalCommand SaveCommand { get { return new ConditionalCommand(Save, CanSave); } }

        public virtual bool CanSave(object parameter)
        {
            if (Validate(LastValidationState))
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

        public virtual void Search(IEnumerable<object> availableObjects, object selectedObject)
        {
            ISearchViewModel searchViewModel = new SearchViewModel(availableObjects, selectedObject);
            searchViewModel.ShowWindow();
        }

        protected abstract void SaveNew(object parameter);

        protected abstract void SaveExisting(object parameter);

        public SimpleCommand CancelCommand { get { return new SimpleCommand(Cancel); } }

        protected virtual void Cancel()
        {
            CloseWindow();
        }

    }

    
}
