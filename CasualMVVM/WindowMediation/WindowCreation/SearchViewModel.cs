using FuchsiaSoft.CasualMVVM.Core;
using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    internal class SearchViewModel<T> : SimpleViewModelBase, ISearchViewModel<T>
        where T : class
    {
        internal SearchViewModel(IEnumerable<T> objects)
        {
            AvailableObjects = new ObservableCollection<T>(objects);
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

        public ObservableCollection<T> AvailableObjects { get; set; }


        //ShowWindow is special for this ViewModel... we want SimpleViewModelBase
        //so that we can have ExitAction defined etc., but this ViewModel is tied to
        //a specific implementation of general purpose, auto generated search windows.
        //Since this library doesn't have a central App.xaml to associate ViewModel and
        //DataTemplate it needs to be done by hand here.


        public override void ShowWindow()
        {
            
            //TODO: make it show the right Window etc.
            throw new NotImplementedException();
        }

        public override void ShowWindow(WindowType type, IWindowSettings settings = null)
        {

            base.ShowWindow(type, settings);
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
            List<Searchable> attributes = new List<Searchable>();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                Searchable attribute = property.GetCustomAttribute<Searchable>(true);
                if (attribute != null)
                {
                    attributes.Add(attribute);
                }
            }

            return attributes;
        }
    }
}
