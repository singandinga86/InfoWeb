using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clients.Models
{
    public partial class Clientes
    {
        public bool ArePhoneAndIdentifierValidQuantities()
        {
            return this.Identificacion > 0 && this.Identificacion < 9999999999 &&
                   this.Telefono > 0 && this.Telefono < 9999999999;
        }
    }
}
