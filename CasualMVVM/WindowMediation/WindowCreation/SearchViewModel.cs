using Vaper.Core;
using Vaper.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Vaper.Core.Commands;
using System.ComponentModel.DataAnnotations;

namespace Vaper.WindowMediation.WindowCreation
{
    internal class SearchViewModel<T> : SimpleViewModelBase, ISearchViewModel
        where T : class
    {
        internal SearchViewModel(IEnumerable<T> objects)
        {
            if (objects == null)
            {
                AvailableObjects = new ObservableCollection<T>();
            }
            else
            {
                AvailableObjects = new ObservableCollection<T>(objects);
            }

            FilteredObjects = new ObservableCollection<T>(AvailableObjects);
        }

        private T _SelectedObject;

        public T SelectedObject
        {
            get { return _SelectedObject; }
            set
            {
                _SelectedObject = value;
                RaisePropertyChanged("SelectedObject");
            }
        }

        private string _FilterText;

        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                RaisePropertyChanged("FilterText");
            }
        }


        private ObservableCollection<T> _AvailableObjects;

        public ObservableCollection<T> AvailableObjects
        {
            get { return _AvailableObjects; }
            set
            {
                _AvailableObjects = value;
                RaisePropertyChanged("AvailableObjects");
            }
        }



        //ShowWindow is special for this ViewModel... we want SimpleViewModelBase
        //so that we can have ExitAction defined etc., but this ViewModel is tied to
        //a specific implementation of general purpose, auto generated search windows.
        //Since this library doesn't have a central App.xaml to associate ViewModel and
        //DataTemplate it needs to be done by hand here.


        /// <summary>
        /// Shows the search window in dialog mode.
        /// </summary>
        public override void ShowWindow()
        {
            WindowMediator.RaiseSearchMessage(this);
        }

        /// <summary>
        /// Will always throw a <see cref="NotSupportedException"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="settings"></param>
        /// <exception cref="NotSupportedException">
        /// Will always throw this exception as there are
        /// no customisation options for search windows</exception>
        public override void ShowWindow(WindowType type, IWindowSettings settings = null)
        {
            throw new NotSupportedException();            
        }

        /// <summary>
        /// Gets a list of <see cref="Searchable"/> attributes for the
        /// objects contained within <see cref="T"/> which can be used
        /// to determine the column bindings and header values for an 
        /// auto generated window.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Searchable> GetColumns()
        {
            //TODO: will this work with just object.properties
            //rather than being generic?  would get rid of some headaches
            //elsewhere


            List<Searchable> attributes = new List<Searchable>();

            //TODO: this isn't picking up metadata attributes defined in
            //a different class

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                Searchable attribute = property.GetCustomAttribute<Searchable>(true);
                if (attribute != null)
                {
                    attributes.Add(attribute);
                }
            }


            //also check for metadata attributes defined in a separate class
            IEnumerable<MetadataTypeAttribute> metaAttributes = 
                typeof(T).GetCustomAttributes<MetadataTypeAttribute>(true);

            foreach (MetadataTypeAttribute meta in metaAttributes)
            {
                Type type = meta.MetadataClassType;

                foreach (PropertyInfo property in type.GetProperties())
                {
                    Searchable attribute = property.GetCustomAttribute<Searchable>(true);
                    if (attribute != null)
                    {
                        attributes.Add(attribute);
                    }
                }
            }

            return attributes;
        }

        public T Result { get; set; }

        public RelayCommand SelectCommand { get { return new RelayCommand(Select, CanSelect); } }

        private bool CanSelect(object obj)
        {
            return SelectedObject != null;
        }

        /// <summary>
        /// The found <see cref="T"/>, this will be
        /// populated when the window is closed, and may
        /// return null if the user cancelled the window.
        /// </summary>
        /// <param name="obj"></param>
        private void Select(object obj)
        {
            Result = SelectedObject;
        }

        public RelayCommand CancelCommand { get { return new RelayCommand(Cancel, CanCancel); } }

        private bool CanCancel(object obj)
        {
            return true;
        }

        private void Cancel(object obj)
        {
            Result = null;
            CloseWindow();
        }

        private ObservableCollection<T> _FilteredObjects;

        public ObservableCollection<T> FilteredObjects
        {
            get { return _FilteredObjects; }
            set
            {
                _FilteredObjects = value;
                RaisePropertyChanged("FilteredObjects");
            }
        }


        public SimpleCommand SearchCommand { get { return new SimpleCommand(Search); } }

        private async void Search()
        {
            //MarkBusy();

            //await Task.Run(() =>
            //{
            //    //TODO: not quite sure how to achieve what
            //    //i'm after here with LINQ, so long handed
            //    //method for now!

            //    foreach (T item in AvailableObjects)
            //    {
            //        if (FilterText == null)
            //        {
            //            FilteredObjects = new ObservableCollection<T>
            //                (AvailableObjects);
            //            return;
            //        }

            //        foreach (PropertyInfo property in typeof(T).GetProperties())
            //        {
            //            if (property.GetValue(item) == null) break;

            //            if (property.GetValue(item)
            //                .ToString().ToUpper()
            //                .Contains(FilterText))
            //            {
            //                FilteredObjects.Add(item);
            //                break;
            //            }
            //        }
            //    }
            //});

            //MarkFree();
        }
    }
}
