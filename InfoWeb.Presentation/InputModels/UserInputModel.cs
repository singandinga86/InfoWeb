using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoWeb.Domain.Entities;

namespace InfoWeb.Presentation.InputModels
{
    public class UserInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }

    }
}
