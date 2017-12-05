using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InfoWeb.Domain.Entities;

namespace InfoWeb.Domain.Interfaces
{
    public interface IProjectRepository: IGenericRepository<Project, int>, INomenclatorRepository<int>
    {
        IQueryable<Project> Projects { get; }
        IEnumerable<Project> GetProjectsAssignedBy(int userId, int skip = 0, int take = 0);
        IEnumerable<Project> GetProjectsAssignedTo(int userId, int skip = 0, int take = 0);

        IEnumerable<Project> GetUnassignedProjects(int skip = 0, int take = 0);
        //IEnumerable<Project> SearchProject(string search, int skip = 0, int take = 0);
    }
}
