using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation
{
    /// <summary>
    /// Defines a property as being Searchable for the purposes of auto generated
    /// search windows
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class Searchable : Attribute
    {
        public Searchable() { }

        public Searchable(string displayPath)
        {
            DisplayPath = displayPath;
        }

        public Searchable(string displayPath, string header)
        {
            DisplayPath = displayPath;
            Header = header;
        }

        /// <summary>
        /// Gets or sets a string which should match the name of the
        /// property to be searched on if this attribute is used on
        /// a property which is not a value type
        /// </summary>
        public string DisplayPath { get; set; }

        /// <summary>
        /// The order that the columns will be shown in the search window
        /// </summary>
        public int DisplayOrder { get; set; }

        private string _Header;
        /// <summary>
        /// Gets or sets a string which will be used as the column header
        /// for searchable windows.  If this value is null it will return the
        /// same as DisplayPath
        /// </summary>
        public string Header
        {
            get
            {
                return _Header == null ? DisplayPath : _Header;
            }
            set
            {
                _Header = value;
            }
        }
    }
}
