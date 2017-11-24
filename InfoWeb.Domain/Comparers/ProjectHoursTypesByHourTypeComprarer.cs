using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.Domain.Comparers
{
    public class ProjectHoursTypesByHourTypeComprarer : IEqualityComparer<ProjectsHoursTypes>
    {
        public bool Equals(ProjectsHoursTypes x, ProjectsHoursTypes y)
        {
            if(x.HourType == null || y.HourType == null)
            {
                throw new InvalidProgramException();
            }

            return x.HourType.Id == y.HourType.Id;
        }

        public int GetHashCode(ProjectsHoursTypes obj)
        {
            return obj.HourType.Id.GetHashCode();
        }
    }
}
