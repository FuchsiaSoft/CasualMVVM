using FuchsiaSoft.CasualMVVM.Core.Commands;
using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMvvmExampleWpfApplication.ViewModels
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


        public ConditionalCommand NewWindowCommand { get { return new ConditionalCommand(NewWindow, CanNewWindow); } }

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
