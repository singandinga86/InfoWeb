using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace InfoWeb.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public User User { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public String Message { get; set; }

        public bool Seen { get; set; }

        public User Sender { get; set; }

        public int SenderId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
