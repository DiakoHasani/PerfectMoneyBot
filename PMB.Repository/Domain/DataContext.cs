using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Repository.Domain
{
    public class DataContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("data source=.;initial catalog=PerfectMoneyDb;integrated security=true;");
            }
        }

        public virtual DbSet<TblPriceHistory> TblPriceHistories { get; set; }
        public virtual DbSet<TblError> TblErrors { get; set; }
    }
}
