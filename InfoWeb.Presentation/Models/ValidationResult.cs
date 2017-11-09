using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoWeb.Presentation.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {

        }

        public ValidationResult(String message)
        {
            Messages.Add(message);
        }
        public List<string> Messages { get; set; } = new List<string>();
        public bool IsValid { get {
                return Messages.Count == 0;
            }
            private set { }
        }
    }
}
