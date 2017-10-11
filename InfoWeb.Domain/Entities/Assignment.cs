using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace InfoWeb.Domain.Entities
{
    public class Assignment
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int Hours { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public User Assignator { get; set; }
               
        public Project Project { get; set; }

        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }

        public User Assignee { get; set; }

        [Range(1, int.MaxValue)]
        public int AssigneeId { get; set; }

        public AssignmentType AssignmentType { get; set; }

        [Range(1, int.MaxValue)]
        public int AssignmentTypeId{get;set;}

        public HourType HourType { get; set; }

        [Range(1, int.MaxValue)]
        public int HourTypeId { get; set; }
    }
}
