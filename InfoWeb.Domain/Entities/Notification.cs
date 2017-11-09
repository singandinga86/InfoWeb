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
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        public String Message { get; set; }

        public bool Seen { get; set; }

        public User Sender { get; set; }

        [Range(1, int.MaxValue)]
        public int? SenderId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
    }
}
