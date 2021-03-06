﻿using InfoWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DataAccess.Repositories
{
    public class ProjectRepository : GenericRepository<Project,int>, 
                                     IProjectRepository
    {
        public ProjectRepository(InfoWebDatabaseContext context): base(context)
        {
         
        }

        public IQueryable<Project> Projects => context.Projects;

        public IEnumerable<Project> GetAll()
        {
            return entitySet.Include(p => p.Client)
                  .Include(p => p.ProjectsHoursTypes)
                  .Include(p => p.Client)
                  .Include(p => p.Type)
                  .OrderBy(p => p.Name)
                  .ToList();
        }

        public override Project GetById(int id)
        {
            var project = entitySet.Where(p => p.Id == id)
                  .Include(p => p.Client)
                  .Include(p => p.Type)
                  .FirstOrDefault();
            if(project != null)
            {
                project.ProjectsHoursTypes = context.ProjectsHoursTypes
                       .Where(pht => pht.ProjectId == project.Id)
                       .Include(pht => pht.HourType)
                       .ToList();
            }

            return project;
        }

        public IEnumerable<Project> GetRange(int skip, int take)
        {
            return base.GetRange(entitySet
                  .Include(p => p.Client)
                  .Include(p => p.ProjectsHoursTypes)
                  .Include(p => p.Type), skip, take);
        }

        public IEnumerable<Project> GetProjectsAssignedBy(int userId, int skip = 0, int take = 0)
        {
            var query = (from p in entitySet
                        join a in this.context.Assignments
                        on p.Id equals a.Project.Id
                        where a.Assignator.Id == userId
                        select p).Include(p => p.Client)
                        .Include(p => p.ProjectsHoursTypes)
                        .Include(p => p.Type)
                        .AsNoTracking().Distinct();
            return base.GetRange(query, skip, take);
                        
        }

        public IEnumerable<Project> GetProjectsAssignedTo(int userId, int skip = 0, int take = 0)
        {
            var query = (from a in this.context.Assignments
                         join p in entitySet
                         on a.Project.Id equals p.Id
                         where a.Assignee.Id == userId
                         select p)
                         .Include(p => p.Client)
                         .Include(p => p.ProjectsHoursTypes)
                         .Include(p => p.Type)
                         .OrderBy(p => p.Name)
                         .GroupBy(p => p)                        
                         .Select(p => p.First());
            return base.GetRange(query, skip, take);
        }

        public IEnumerable<Project> GetUnassignedProjects(int skip = 0, int take = 0)
        {
            var query = (from p in context.Projects
                        where !context.Assignments.Any(a => a.ProjectId == p.Id)
                        select p).Include(p => p.Client)
                        .Include(p => p.ProjectsHoursTypes)
                        .Include(p => p.Type);

            return base.GetRange(query,skip, take);
        }

        //public IEnumerable<Project> SearchProject(string search, int skip = 0, int take = 0)
        //{
        //    var query = context.Projects.
        //        Where(p => p.Name.Contains(search) || p.Client.Name.Contains(search) || p.Type.Name.Contains(search) || search == "")
        //        .Include(p => p.Client)
        //        .Include(p => p.Type);

        //    return base.GetRange(query, skip, take);
        //}

        public bool CanItemBeRemoved(int id)
        {
            return context.Assignments.Where(a => a.ProjectId == id).FirstOrDefault() == null;
        }
    }
}
