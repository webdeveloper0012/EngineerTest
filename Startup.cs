using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EngineerTest.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IO;
using EngineerTest.Services;
using EngineerTest.Services.Scheduling;

namespace EngineerTest
{
    public class Startup
    {
        /// <summary>
        /// Set up configuration
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configur service 
        /// </summary>
        /// <param name="services"> services</param>
        public void ConfigureServices(IServiceCollection services)
        {



            services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration["Logging:ConnectionStrings:Default"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Home/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Home/AccessDenied";

                options.SlidingExpiration = true;
            });


            services.AddAuthentication().AddFacebook(facebookOptions =>
            {

                facebookOptions.AppId = Configuration["Logging:Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Logging:Authentication:Facebook:AppSecret"];
            });// Add application services.
            //services.AddTransient<IEmailSender, EmailSender>();

            services.AddAuthentication().AddGoogle(GoogleOptions =>
            {

                GoogleOptions.ClientId = Configuration["Logging:Authentication:Google:ClientId"];
                GoogleOptions.ClientSecret = Configuration["Logging:Authentication:Google:ClientSecret"];


            });
            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Logging:Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = Configuration["Logging:Authentication:Microsoft:ClientSecret"];
            });

            services.AddMvc();

            // Add scheduled tasks & scheduler
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(Configuration["Logging:ConnectionStrings:Default"]);
            ApplicationDbContext _context = new ApplicationDbContext(optionsBuilder.Options);
            services.AddSingleton<IScheduledTask>(s => new MovieFetchTask(_context));
            
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configur app and environment
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">hosting environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {





            app.UseMvc();
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
   

}
