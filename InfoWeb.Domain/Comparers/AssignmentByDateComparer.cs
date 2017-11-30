using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.Domain.Comparers
{
    public class AssignmentByDateComparer : IEqualityComparer<Assignment>
    {
        public bool Equals(Assignment x, Assignment y)
        {
            return x.Date == y.Date;
        }

        public int GetHashCode(Assignment obj)
        {
            return obj.Date.GetHashCode();
        }
    }
}
