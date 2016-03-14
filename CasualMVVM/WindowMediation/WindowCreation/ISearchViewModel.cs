using Vaper.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vaper.WindowMediation.WindowCreation
{
    public interface ISearchViewModel : IViewModel
    {
        IEnumerable<Searchable> GetColumns();
    }
}
