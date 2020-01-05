using Hackathon.CustomMiddleware;
using Hackathon.Models;
using Hackathon.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace AspNetCoreApps
{
    public class Startup
    {
        /// <summary>
        /// IConfiguration, the contract, that provides application configuration information to
        /// the Startup class. All these configurations are written in appsettings.json
        /// e.g. ConnectionStrings
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// The method for defining required services (congfiguration and Dependencies) in the container
        /// IServiceCollection is used to register services in the container and the lifetime of service types
        /// is managed by the 'ServiceDescriptor' class
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // regiter the _DbContext in the Container
            services.AddDbContext<_DbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("AppConnString")));

            // register the service classes

            //services.AddScoped<IService<University, int>, UniversityService>();
            services.AddScoped<IService<StudentDetails, int>, StudentDetailservice>();
            services.AddScoped<IPreferenceService<UserPreferences,int>,PreferenceService>();

            services.AddCors(options => options.AddPolicy("corspolicy", policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        ///  This method represent the current Http Request and all additional objects to be provided
        ///  the Http request e.g. Security, Exception., etc
        ///  IApplicationBuilder: Builds all "middleware"  for current Http Request
        ///  IHostingEnvironment: provided Hosting
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // register the custom middleware
            app.UseCustomErrorHandlerMiddleware();
            app.UseMvc();
            app.UseCors("corspolicy");
        }
    }
}
