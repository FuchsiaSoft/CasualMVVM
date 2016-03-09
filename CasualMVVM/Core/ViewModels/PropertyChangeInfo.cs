using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMVVM.Core.ViewModels
{
    /// <summary>
    /// Container for detailing changes to an entity's
    /// property when the underlying database has seen
    /// a change since the original entity was retrieved,
    /// meaning that the new modified entity cannot be
    /// saved without conflicting with changes that have
    /// been made since.
    /// </summary>
    public sealed class PropertyChangeInfo
    {
        internal PropertyChangeInfo() { }

        /// <summary>
        /// The property that has seen a conflict
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// The original value that the property held
        /// when it was retrieved from the database
        /// </summary>
        public object OriginalValue { get; set; }

        /// <summary>
        /// The value that the property currently
        /// has in the database
        /// </summary>
        public object DatabaseValue { get; set; }

        /// <summary>
        /// The value that the property has since
        /// it was modified outside of the database
        /// </summary>
        public object ChangedValue { get; set; }
    }
}
