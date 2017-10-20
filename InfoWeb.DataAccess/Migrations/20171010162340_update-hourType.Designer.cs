using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using InfoWeb.DataAccess;

namespace InfoWeb.DataAccess.Migrations
{
    [DbContext(typeof(InfoWebDatabaseContext))]
    [Migration("20171010162340_update-hourType")]
    partial class updatehourType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InfoWeb.Domain.Entities.Assignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssigmatorId");

                    b.Property<int?>("AssignatorId");

                    b.Property<int>("AssigneeId");

                    b.Property<int>("AssignmentTypeId");

                    b.Property<DateTime>("Date");

                    b.Property<int>("HourTypeId");

                    b.Property<int>("Hours");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("AssignatorId");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("AssignmentTypeId");

                    b.HasIndex("HourTypeId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.AssignmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("IX_ASSIGNMENTTYPE_NAME");

                    b.ToTable("AssignmentTypes");
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.HourType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("IX_HOUrTYPE_NAME");

                    b.ToTable("HourTypes");
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClientId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("TypeId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.ProjectsHoursTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("HourTypeId");

                    b.Property<int>("Hours");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("HourTypeId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectsHoursTypes");
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.ProjectType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("IX_PROJECTTYPE_NAME");

                    b.ToTable("ProjectTypes");
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("IX_ROLE_NAME");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("IX_USER_NAME");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.Assignment", b =>
                {
                    b.HasOne("InfoWeb.Domain.Entities.User", "Assignator")
                        .WithMany()
                        .HasForeignKey("AssignatorId");

                    b.HasOne("InfoWeb.Domain.Entities.User", "Assignee")
                        .WithMany()
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InfoWeb.Domain.Entities.AssignmentType", "AssignmentType")
                        .WithMany("Assignments")
                        .HasForeignKey("AssignmentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InfoWeb.Domain.Entities.HourType", "HourType")
                        .WithMany()
                        .HasForeignKey("HourTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InfoWeb.Domain.Entities.Project", "Project")
                        .WithMany("Assignments")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.Project", b =>
                {
                    b.HasOne("InfoWeb.Domain.Entities.Client", "Client")
                        .WithMany("Projects")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InfoWeb.Domain.Entities.ProjectType", "Type")
                        .WithMany("Projects")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.ProjectsHoursTypes", b =>
                {
                    b.HasOne("InfoWeb.Domain.Entities.HourType", "HourType")
                        .WithMany("ProjectsHoursTypes")
                        .HasForeignKey("HourTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InfoWeb.Domain.Entities.Project", "Project")
                        .WithMany("ProjectsHoursTypes")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InfoWeb.Domain.Entities.User", b =>
                {
                    b.HasOne("InfoWeb.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
