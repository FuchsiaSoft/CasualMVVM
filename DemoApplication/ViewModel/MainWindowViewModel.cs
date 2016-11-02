using DemoApplication.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaper.Core.Commands;
using Vaper.Core.ViewModels;
using Vaper.WindowMediation;
using Vaper.WindowMediation.WindowCreation;

namespace DemoApplication.ViewModel
{
    class MainWindowViewModel : SimpleViewModelBase
    {
        public MainWindowViewModel()
        {
            Refresh();
        }

        private Case _SelectedCase;

        public Case SelectedCase
        {
            get { return _SelectedCase; }
            set
            {
                _SelectedCase = value;
                RaisePropertyChanged("SelectedCase");
            }
        }

        private ObservableCollection<Case> _AvailableCases;

        public ObservableCollection<Case> AvailableCases
        {
            get { return _AvailableCases; }
            set
            {
                _AvailableCases = value;
                RaisePropertyChanged("AvailableCases");
            }
        }

        public RelayCommand EditCaseCommand { get { return new RelayCommand(EditCase, CanEditCase); } }

        private bool CanEditCase(object obj)
        {
            return SelectedCase != null;
        }

        private void EditCase(object obj)
        {
            throw new NotImplementedException();
        }

        public RelayCommand NewCaseCommand { get { return new RelayCommand(NewCase, CanNewCase); } }

        private bool CanNewCase(object obj)
        {
            return true;
        }

        private void NewCase(object obj)
        {
            NewCaseViewModel viewModel = new NewCaseViewModel(new Case(), DataEntryMode.New);
            viewModel.ShowWindow(WindowType.NewAutoWindowRequest);
            viewModel.SetExitAction(Refresh);
        }

        private void Refresh()
        {
            using (DemoModelContainer db = new DemoModelContainer())
            {
                AvailableCases = new ObservableCollection<Case>(db.Cases.ToList());
            }
        }
    }
}
