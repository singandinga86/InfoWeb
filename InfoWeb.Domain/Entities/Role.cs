using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace InfoWeb.Domain.Entities
{
    public class Role
    {
        public Role()
        {

        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
