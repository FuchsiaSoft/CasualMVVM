using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMvvmExampleWpfApplication.ViewModels
{
    class ChildWindowViewModel : SimpleViewModelBase
    {
        private string _MyProperty = "Test string goes here";

        public string MyProperty
        {
            get { return _MyProperty; }
            set
            {
                _MyProperty = value;
                RaisePropertyChanged("MyProperty");
            }
        }

    }
}
