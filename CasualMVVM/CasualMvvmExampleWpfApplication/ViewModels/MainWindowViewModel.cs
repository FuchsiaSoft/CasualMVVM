using Vaper.Core.Commands;
using Vaper.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VaperExampleWpfApplication.ViewModels
{
    class MainWindowViewModel : SimpleViewModelBase
    {
        private string _MyProperty;

        public string MyProperty
        {
            get { return _MyProperty; }
            set
            {
                _MyProperty = value;
                RaisePropertyChanged("MyProperty");
            }
        }


        public RelayCommand NewWindowCommand { get { return new RelayCommand(NewWindow, CanNewWindow); } }

        private bool CanNewWindow(object obj)
        {
            return true;
        }

        private void NewWindow(object obj)
        {
            ChildWindowViewModel viewModel = new ChildWindowViewModel();

            viewModel.SetExitAction(DoSomething);

            viewModel.ShowWindow(CasualMVVM.WindowMediation.WindowType.NewAutoWindowRequest);
        }

        private void DoSomething()
        {
            MyProperty = "Do Something executed successfully";
        }
    }
}
