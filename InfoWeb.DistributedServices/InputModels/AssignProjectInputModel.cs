using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoWeb.DistributedServices.InputModels
{
    public class AssignProjectInputModel
    {
        public int ProjectId { get; set; }
        public int AssigneeId { get; set; }
    }
}
