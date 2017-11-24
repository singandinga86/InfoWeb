using Clients.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Clients.DataAccess
{
    public class ClientRepository : IClientRepository
    {
        private ClientsContext context;

        public ClientRepository(ClientsContext context)
        {
            this.context = context;
        }
        public int Create(Clientes client)
        {
            return this.ManageClient(client, 'i');
        }

        public int Delete(long id)
        {
            var client = new Clientes() { Id = id };
            return this.ManageClient(client, 'b');
        }

        public Clientes GetClient(long id)
        {
            return this.context.Clientes.Where(c => c.Id == id).FirstOrDefault();
        }

        public IEnumerable<Clientes> GetClients(string searchCriteria)
        {
            if (string.IsNullOrEmpty(searchCriteria))
            {
                searchCriteria = "''";
            }
            else
            {
                searchCriteria = "'" + searchCriteria + "'";
            }

            IEnumerable<Clientes> result = context.Clientes.FromSql("SELECT * FROM FTN_CLIENTES_PRUEBA_MANTENIMIENTO(" + searchCriteria + ")").ToList();
            return result;
        }

        public int Update(Clientes client)
        {
            return this.ManageClient(client, 'a');
        }

        private int ManageClient(Clientes client, char operation)
        {
            int result = 0;
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "STPR_CLIENTES_PRUEBA_MANTENIMIENTO";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                this.ConfigureCommandParameters(command, client, operation);

                command.Parameters.Add(new SqlParameter("@result", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });

                if (command.Connection.State != System.Data.ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                command.ExecuteNonQuery();

                result = (int)command.Parameters["@result"].Value;
            }
            return result;
        }

        private void ConfigureCommandParameters(DbCommand command, Clientes client, char operation)
        {
            
            command.Parameters.Add(new SqlParameter("@operation", operation));
           
           switch(operation)
            {
                case 'i':
                    command.Parameters.Add(new SqlParameter("@id", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@nombre_completo", client.NombreCompleto));
                    command.Parameters.Add(new SqlParameter("@identificacion", client.Identificacion.ToString()));
                    command.Parameters.Add(new SqlParameter("@telefono", client.Telefono.ToString()));
                    break;
                case 'a':
                    command.Parameters.Add(new SqlParameter("@id", client.Id));
                    command.Parameters.Add(new SqlParameter("@nombre_completo", client.NombreCompleto.ToString()));
                    command.Parameters.Add(new SqlParameter("@identificacion", client.Identificacion.ToString()));
                    command.Parameters.Add(new SqlParameter("@telefono", client.Telefono.ToString()));
                    break;
                case 'b':
                    command.Parameters.Add(new SqlParameter("@id", client.Id));
                    command.Parameters.Add(new SqlParameter("@nombre_completo", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@identificacion", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@telefono", DBNull.Value));
                    break;  
            }
        }
   
    }
}
