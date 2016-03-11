using FuchsiaSoft.CasualMVVM.WindowMediation.WindowCreation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    [MetadataType(typeof(PersonMetaData))]
    public partial class Person { }

    class PersonMetaData
    {
        [Searchable(Header ="First Name", DisplayPath = "FirstName")]
        public string FirstName { get; set; }

        [Searchable(Header ="Last Name", DisplayPath = "LastName")]
        public string LastName { get; set; }

        [Searchable(Header = "Colour", DisplayPath ="HairColour.Colour")]
        public HairColour HairColour { get; set; }
    }
}
