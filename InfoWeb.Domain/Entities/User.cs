using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace InfoWeb.Domain.Entities
{
    public class User
    {
        public int Id { get; set;}

        [Required]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
