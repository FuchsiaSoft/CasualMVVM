using FuchsiaSoft.CasualMVVM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    public interface ISearchViewModel : IViewModel
    {
        IEnumerable<Searchable> GetColumns();
    }
}
