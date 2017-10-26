using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DataAccess
{
    public class InfoWebDatabaseContextFactory : IDbContextFactory<InfoWebDatabaseContext>
    {
        public InfoWebDatabaseContext Create(DbContextFactoryOptions options)
        {
            var Configuration = new ConfigurationBuilder()
                                .SetBasePath(options.ContentRootPath)
                                .AddJsonFile("appsettings.json", false)
                                .Build();

            string connectionString = Configuration.GetConnectionString("database");

            var optionsBuilder = new DbContextOptionsBuilder<InfoWebDatabaseContext>();

            optionsBuilder.UseSqlServer(connectionString, op => op.UseRowNumberForPaging());

            return new InfoWebDatabaseContext(optionsBuilder.Options);

        }
    }
}
