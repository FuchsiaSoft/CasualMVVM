using Vaper.Core.Commands;
using Vaper.Core.ViewModels;
using Vaper.WindowMediation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DemoApp.ViewModel
{
    class MainWindowViewModel : SimpleViewModelBase
    {
        public MainWindowViewModel()
        {
            RefreshData();
        }

        private ObservableCollection<Person> _AvailablePeople;

        public ObservableCollection<Person> AvailablePeople
        {
            get { return _AvailablePeople; }
            set
            {
                _AvailablePeople = value;
                RaisePropertyChanged("AvailablePeople");
            }
        }

        private Person _SelectedPerson;

        public Person SelectedPerson
        {
            get { return _SelectedPerson; }
            set
            {
                _SelectedPerson = value;
                RaisePropertyChanged("SelectedPerson");
            }
        }

        public RelayCommand NewPersonCommand { get { return new RelayCommand(NewPerson, CanNewPerson); } }

        private bool CanNewPerson(object obj)
        {
            return true;
        }

        private void NewPerson(object obj)
        {
            GenericPersonViewModel childVM = new GenericPersonViewModel(new Person(), DataEntryMode.New);
            //PersonViewModel childVM = new PersonViewModel(new Person());
            childVM.SetExitAction(RefreshData);
            childVM.ShowWindow(WindowType.NewAutoWindowRequest);
        }


        public RelayCommand EditPersonCommand { get { return new RelayCommand(EditPerson, CanEditPerson); } }

        private bool CanEditPerson(object obj)
        {
            return SelectedPerson != null;
        }

        private void EditPerson(object obj)
        {
            GenericPersonViewModel childVM = new GenericPersonViewModel(SelectedPerson, DataEntryMode.Edit);
            childVM.SetExitAction(RefreshData);
            childVM.ShowWindow(WindowType.NewAutoWindowRequest);
        }


        private void RefreshData()
        {
            using (DemoModelContainer db = new DemoModelContainer())
            {
                AvailablePeople = new ObservableCollection<Person>
                    (db.People);
            }

            if (SelectedPerson != null)
            SelectedPerson = AvailablePeople
                .Where(p => p.Id == SelectedPerson.Id)
                .FirstOrDefault();
        }


    }
}
