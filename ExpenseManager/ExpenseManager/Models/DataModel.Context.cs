﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpenseManager.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ExpenseManagerEntities : DbContext
    {
        public ExpenseManagerEntities()
            : base("name=ExpenseManagerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ExpenseRecord> ExpenseRecord { get; set; }
        public virtual DbSet<PaymentsSchedule> PaymentsSchedule { get; set; }
        public virtual DbSet<Threshold> Threshold { get; set; }
        public virtual DbSet<Type> Type { get; set; }
        public virtual DbSet<Saving> Saving { get; set; }
    }
}
