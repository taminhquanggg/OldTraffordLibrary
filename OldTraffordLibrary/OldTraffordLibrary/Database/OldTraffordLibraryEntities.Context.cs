﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OldTraffordLibrary.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OldTraffordLibraryEntities : DbContext
    {
        public OldTraffordLibraryEntities()
            : base("name=OldTraffordLibraryEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_Book> tbl_Book { get; set; }
        public virtual DbSet<tbl_LoanVoucher> tbl_LoanVoucher { get; set; }
        public virtual DbSet<tbl_LoanVoucherDetail> tbl_LoanVoucherDetail { get; set; }
        public virtual DbSet<tbl_Reader> tbl_Reader { get; set; }
        public virtual DbSet<tbl_User> tbl_User { get; set; }
    }
}
