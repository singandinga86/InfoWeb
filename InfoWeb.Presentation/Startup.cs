namespace InfoWeb.Presentation
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Serialization;
    using InfoWeb.Domain.Interfaces;
    using InfoWeb.DataAccess.Repositories;
    using InfoWeb.DataAccess;

        public class Startup
        {
            private IConfigurationRoot Configuration { get; set; }

            public Startup(IHostingEnvironment environment)
            {
                var configurationBuilder = new ConfigurationBuilder().
                                            SetBasePath(environment.ContentRootPath)
                                            .AddJsonFile("appsettings.json", false, true);
                Configuration = configurationBuilder.Build();
            }
            // This method gets called by the runtime. Use this method to add services to the container.
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            public void ConfigureServices(IServiceCollection services)
            {
                /*services.AddCors(options => {
                    options.AddPolicy("AllowAll", policyBuilder => {
                        policyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
                    });
                });*/
                services.AddMvc().AddJsonOptions(options => {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

                string connectionString = Configuration.GetConnectionString("DefaultConnection");

                services.AddDbContext<InfoWebDatabaseContext>(options =>
                {
                    options.UseSqlServer(connectionString,op => op.UseRowNumberForPaging());
                });
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IProjectRepository, ProjectRepository>();
                services.AddScoped<IAssignmentRepository, AssignmentRepository>();
                services.AddScoped<IAssignmentTypeRepository, AssignmentTypeRepository>();
                services.AddScoped<IClientRepository, ClientRepository>();
                services.AddScoped<IHourTypeRepository, HourTypeRepository>();
                services.AddScoped<IProjectHourTypeRepository, ProjectHourTypeRepository>();
                services.AddScoped<IProjectTypeRepository, ProjectTypeRepository>();
                services.AddScoped<IRoleRepository, RoleRepository>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IInfoWebQueryModel, InfoWebQueryModel>();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
            {
                loggerFactory.AddConsole();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

            //app.UseCors("AllowAll");
            app.UseDefaultFiles();
            app.UseStaticFiles();                
            app.UseMvc();
            }
        }
}
