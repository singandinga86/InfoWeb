using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoWeb.Presentation.InputModels
{
    public class AssignProjectGroupInputModel
    {
        public Project project { get; set; }
        public HourType hourType { get; set; }
        public User user { get; set; }
        public List<User> usersSelected { get; set; }
    }
}
