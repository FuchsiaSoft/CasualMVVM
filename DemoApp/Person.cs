//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Age { get; set; }
        public bool CheckedPapers { get; set; }
        public string Comments { get; set; }
        public int HairColour_Id { get; set; }
    
        public virtual HairColour HairColour { get; set; }
    }
}
