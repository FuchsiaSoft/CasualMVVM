using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vaper.Core.ViewModels
{
    interface IDataEntryViewModel<T, Context> : IDataEntryViewModel
        where T : class
        where Context : DbContext
    {
        bool ContinueOnConflict(IEnumerable<PropertyChangeInfo> conflicts);
    }
}
