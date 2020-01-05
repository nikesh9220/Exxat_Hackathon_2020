.NET Core Runtime
	- the "dotnet.exe"
		- Hosts the .NET Core app as "In-Process" for execution
ASP.NET Core Runtime	
	- ASP.NET WebForms
	- ASP.NET MVC
	- Api
 
The Project file for InProc Hosting

  <PropertyGroup>
   <!--The Target Framework-->
    <TargetFramework>netcoreapp2.2</TargetFramework>
  <!--An Inprocess Hosting-->		
    <AspNetCoreHostingModel>InProcess</AspNetCoreHostingModel>
  </PropertyGroup>
 
  
Program.cs to provide initial application dependency configuration

The WebHosting Environment is created (Self-Host, IIS Express) WebHost uses the 'STartup' class to create default dependencies and object builder to
run and execute the application
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

The Startup.cs the Startup class
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

            app.UseMvc();
        }
    }


    Using  EntityFramework Core aka EFCore for Data Access
    1. Microsoft.EntityFrameworkCore
    2. Microsoft.EntityFrameworkCore.Relational
    3. Microsoft.EntityFrameworkCore.SqlServer
    4.  Microsoft.EntityFrameworkCore.Tools

The Database First Approach
CLI Command
dotnet ef scaffold "Connection-String" Provider -o Models -table <LIST of Tables to be scaffolded>
Connection-String, the database connection string
Provider, the database provider 

The Code-First Approach
1. Generate Migrations
dotnet ef migrations add Migration-name -c DBContext Class path

Example
dotnet ef migrations add FirstMigration -c AspNetCoreApps.Models.ExStudentDbContext


2. Update database
dotnet ef  database update -c DBContext Class path

example

>dotnet ef database update -c AspNetCoreApps.Models.ExStudentDbContext

The DbContext Class
1. Class that used to map with Database over the connection string
2. Class to Perform Transactions using  SaveChanges() / SaveChangesAsync() method
3. Class used to map with Db Tables using Model/Entity classes with DbSet<T> class
4. DbSet<T> class represents the Cursor to store all records from Table of name T that map with class name T




In ASP.NET Core the DI Container have lifetime for objects as
1. Transient --> Object will be created for each request and used only for a specific controller scope. 
    When the controller is unloaded the object will also be disposed.
services.AddTransient<T,U>();

2. Scoped --> Object will be created for each new request and will be meintained for a session
services.AddScoped<T,U>();

3. Singleton --> Object will be created throught the lifecycle of the application.
services.AddSingleton<T>();

* Important
// regiter the ExStudentDbContext in the Container
            services.AddDbContext<ExStudentDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("AppConnString")));

ApiController in ASP.NET Core
1. HttpMethod Attributes
    HttpGetAttribute, HttpPostAttribute, HttpPutAttribute, HttpDeleteAttribute
2. IActionResult Contract
    Used for Response from MVC Controllers as viewResulr, RedirectToActionResult,
            FileResult, etc.
    Used for Response from Api Controller
        - OkResult, Ok(), OkObjectResult, the Ok(CLR Object to be Serialized)
        - BadRequest(), NotFound(), NoContent(), 