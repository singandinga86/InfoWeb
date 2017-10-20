using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using System.Linq;

namespace InfoWeb.Domain.Interfaces
{
    public interface IInfoWebQueryModel
    {
        IQueryable<User> Users { get; }

        IQueryable<Role> Roles { get; }

        IQueryable<ProjectType> ProjectType { get; }

        IQueryable<ProjectsHoursTypes> ProjectHoursTypes { get; }

        IQueryable<Project> Projects { get; }

        IQueryable<HourType> HourTypes { get; }

        IQueryable<Client> Clients { get; }

        IQueryable<AssignmentType> AssignmentTypes { get; }

        IQueryable<Assignment> Assignments { get; }
    }
}
