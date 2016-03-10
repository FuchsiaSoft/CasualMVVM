using FuchsiaSoft.CasualMVVM.Core;
using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using FuchsiaSoft.CasualMVVM.Core.Commands;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    internal class SearchViewModel : SimpleViewModelBase, ISearchViewModel
    {
        internal SearchViewModel(IEnumerable<object> objects, object obj)
        {
            //TODO: put some logic in here to make sure that the objects are
            //of the same type, maybe should bring back generics, as long as
            //its cast by its interface and the GetColumns doesn't return 
            //the generic type then it should be fine?
            _OriginalObject = obj;

            if (objects == null)
            {
                AvailableObjects = new ObservableCollection<object>();
            }
            else
            {
                AvailableObjects = new ObservableCollection<object>(objects);
            }
        }

        private object _OriginalObject;

        private object _SelectedObject;

        public object SelectedObject
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


        public ObservableCollection<object> AvailableObjects { get; set; }


        //ShowWindow is special for this ViewModel... we want SimpleViewModelBase
        //so that we can have ExitAction defined etc., but this ViewModel is tied to
        //a specific implementation of general purpose, auto generated search windows.
        //Since this library doesn't have a central App.xaml to associate ViewModel and
        //DataTemplate it needs to be done by hand here.


        public override void ShowWindow()
        {
            WindowMediator.RaiseSearchMessage(this);
        }

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

            foreach (PropertyInfo property in AvailableObjects.First().GetType().GetProperties())
            {
                Searchable attribute = property.GetCustomAttribute<Searchable>(true);
                if (attribute != null)
                {
                    attributes.Add(attribute);
                }
            }

            return attributes;
        }

        public ConditionalCommand SelectCommand { get { return new ConditionalCommand(Select, CanSelect); } }

        private bool CanSelect(object obj)
        {
            return SelectedObject != null;
        }

        private void Select(object obj)
        {
            _OriginalObject = SelectedObject;
        }

        public ConditionalCommand CancelCommand { get { return new ConditionalCommand(Cancel, CanCancel); } }

        private bool CanCancel(object obj)
        {
            return true;
        }

        private void Cancel(object obj)
        {
            CloseWindow();
        }


    }
}
