using Microsoft.EntityFrameworkCore;
using SelfReferencingSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfReferencingSample
{
    // https://medium.com/@dmitry.pavlov/tree-structure-in-ef-core-how-to-configure-a-self-referencing-table-and-use-it-53effad60bf
    //
    public class AppContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli

            // https://stackoverflow.com/questions/59926120/entity-framework-core-self-reference-and-one-to-many-relation-to-another-table
            // https://stackoverflow.com/questions/46082682/entity-framework-itself-referention-parent-child
            options.UseSqlite("Data Source=app.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.HasKey(x => x.Id);
                entity.Property(x => x.FullName);
                entity.HasOne(x => x.Parent)
                    .WithMany(x => x.Children)
                    .HasForeignKey(x => x.ParentId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });
        }
    }
}
