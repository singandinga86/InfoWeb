using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoWeb.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace InfoWeb.Presentation.InputModels
{
    public class UserInputModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirmation { get; set; }

    }
}
