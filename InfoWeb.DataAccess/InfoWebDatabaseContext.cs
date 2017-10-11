using InfoWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.DataAccess
{
    public class InfoWebDatabaseContext: DbContext
    {
        public InfoWebDatabaseContext():base()
        {

        }

        public InfoWebDatabaseContext(DbContextOptions<InfoWebDatabaseContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<HourType> HourTypes { get; set; }
        public DbSet<ProjectsHoursTypes> ProjectsHoursTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentType> AssignmentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(user => user.Name).HasName("IX_USER_NAME").IsUnique();
            modelBuilder.Entity<Role>().HasIndex(role => role.Name).HasName("IX_ROLE_NAME").IsUnique();
            modelBuilder.Entity<ProjectType>().HasIndex(type => type.Name).HasName("IX_PROJECTTYPE_NAME").IsUnique();
            modelBuilder.Entity<HourType>().HasIndex(type => type.Name).HasName("IX_HOURTYPE_NAME").IsUnique();
            modelBuilder.Entity<AssignmentType>().HasIndex(type => type.Name).HasName("IX_ASSIGNMENTTYPE_NAME").IsUnique();
        }
    }
}
