﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DashboardDataGatherer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SytelineDbEntities : DbContext
    {
        public SytelineDbEntities()
            : base("name=SytelineDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<itemprice> itemprices { get; set; }
        public DbSet<job> jobs { get; set; }
        public DbSet<jobtran> jobtrans { get; set; }
    }
}
