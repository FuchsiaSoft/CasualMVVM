﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoApplication.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DemoModelContainer : DbContext
    {
        public DemoModelContainer()
            : base("name=DemoModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<CaseType> CaseTypes { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TelephoneContact> TelephoneContacts { get; set; }
    }
}