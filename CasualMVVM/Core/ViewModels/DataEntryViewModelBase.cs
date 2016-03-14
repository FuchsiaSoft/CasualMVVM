using System.Collections.Generic;
using System.Linq;
using Vaper.Core.Commands;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.ObjectModel;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using Vaper.WindowMediation;
using Vaper.WindowMediation.WindowCreation;

namespace Vaper.Core.ViewModels
{
    /// <summary>
    /// Used to indicate to a <see cref="IDataEntryViewModel"/>
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

    /// <summary>
    /// Implements <see cref="IDataEntryViewModel"/> and provides a base ViewModel
    /// for dealing with re-usable ViewModels for adding and editing records.
    /// Inherits from <see cref="ValidatingViewModelBase"/>
    /// </summary>
    public abstract class DataEntryViewModelBase : ValidatingViewModelBase, IDataEntryViewModel
    {
        /// <summary>
        /// For documentation refer to <see cref="IDataEntryViewModel.Mode"/>
        /// </summary>
        public DataEntryMode Mode { get; set; } = DataEntryMode.New;

        /// <summary>
        /// For documentation refer to <see cref="IDataEntryViewModel.SaveCommand"/>
        /// </summary>
        public RelayCommand SaveCommand { get { return new RelayCommand(Save, CanSave); } }

        /// <summary>
        /// For documentation refer to <see cref="IDataEntryViewModel.CanSave(object)"/>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual bool CanSave(object parameter)
        {
            if (Validate(LastValidationState))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// For documentation refer to <see cref="IDataEntryViewModel.Save(object)"/>.
        /// In this implementation, the <see cref="Mode"/> property directly controls
        /// the flow of this method, and the abstract methods <see cref="SaveNew(object)"/>
        /// or <see cref="SaveExisting(object)"/> will be called accordingly.
        /// The return value of those methods will determine whether or not
        /// the flow of this method continues (closing the window), to allow
        /// for handling of exceptions etc.
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void Save(object parameter)
        {
            switch (Mode)
            {
                case DataEntryMode.New:
                    if (SaveNew(parameter))
                        CloseWindow(true);
                    break;

                case DataEntryMode.Edit:
                    if (SaveExisting(parameter))
                        CloseWindow(true);
                    break;
            }
        }

        /// <summary>
        /// Will create an auto generated search window for
        /// the provided Type and will use the Enumerable
        /// provided as the source of objects that can be searched against.
        /// The return value will be either the selected object,
        /// or null if none was selected.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="availableObjects"></param>
        /// <returns></returns>
        public virtual T Search<T>(IEnumerable<T> availableObjects)
            where T : class
        {
            SearchViewModel<T> searchViewModel = new SearchViewModel<T>(availableObjects);
            searchViewModel.ShowWindow();
            return searchViewModel.Result;
        }

        /// <summary>
        /// Must be implemented in derived class, and should be used for the logic to
        /// save new records.  If true is returned at the end of implementation then
        /// the <see cref="Save(object)"/> method will continue and close any
        /// associated window.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected abstract bool SaveNew(object parameter);

        /// <summary>
        /// Must be implemented in a derived class, and should be used for the logic
        /// to save existing records.  If true is returned at the end of the implementation
        /// then the <see cref="Save(object)"/> method will continue and close any
        /// associated windows.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected abstract bool SaveExisting(object parameter);

        /// <summary>
        /// For documentation refer to <see cref="IDataEntryViewModel.CancelCommand"/>
        /// </summary>
        public SimpleCommand CancelCommand { get { return new SimpleCommand(Cancel); } }

        /// <summary>
        /// Closes the using <see cref="IViewModel.CloseWindow"/>
        /// </summary>
        protected virtual void Cancel()
        {
            CloseWindow();
        }

    }

    
}
