﻿using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Sales;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(x => x.Description)
                .HasDefaultValue("No description");

            modelBuilder.Entity<Sale>()
                .Property(x => x.Date)
                .HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(modelBuilder);
        }
    }
}
