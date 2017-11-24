using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Clients.Models;

namespace Clients.DataAccess
{
    public partial class ClientsContext : DbContext
    {
        public virtual DbSet<Clientes> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.HasIndex(e => e.Identificacion)
                    .HasName("IX_CLIENTES")
                    .IsUnique();

                entity.Property(e => e.Telefono).HasDefaultValueSql("0");
            });
        }
    }
}