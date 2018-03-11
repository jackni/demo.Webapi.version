using Demo.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.DataAccess
{
    public class DemoContext: DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        // this enable DI with .net core service provider
        public DemoContext(DbContextOptions<DemoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //setup database schema
            modelBuilder.HasDefaultSchema("demo");

            modelBuilder.Entity<Todo>()
                .ToTable("Todo")
                .HasKey(e => e.Id);
            modelBuilder.Entity<Todo>().Property(e => e.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Todo>().Property(e => e.Description).HasMaxLength(255);
            modelBuilder.Entity<Todo>().Property(e => e.CreatedDateTime).IsRequired();
            modelBuilder.Entity<Todo>().Property(e => e.RowVersion)
                .IsRowVersion()
                .ValueGeneratedOnAdd();
        }
    }
}
