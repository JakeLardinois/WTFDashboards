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
    
    public partial class DashboardDbEntities : DbContext
    {
        public DashboardDbEntities()
            : base("name=DashboardDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<WorkOrderMetric> WorkOrderMetrics { get; set; }
        public DbSet<InventoryCostMetric> InventoryCostMetrics { get; set; }
        public DbSet<MachineHourMetric> MachineHourMetrics { get; set; }
    }
}
