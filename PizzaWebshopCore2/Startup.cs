﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaWebshopCore2.Data;
using PizzaWebshopCore2.Models;
using PizzaWebshopCore2.Services;

namespace PizzaWebshopCore2
{

    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            if (_environment.IsProduction() || _environment.IsStaging())
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Pizzeria;Trusted_Connection=True;"));
            else
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("DefaultConnection"));


            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseInMemoryDatabase("DefaultConnection"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddTransient<UserManager<ApplicationUser>>();
            services.AddTransient<RoleManager<IdentityRole>>();
            services.AddTransient<TransformationService>();
            services.AddTransient<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddTransient<IDatabaseService, DatabaseService>();
            services.AddSession();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            app.UseSession();




            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            if (_environment.IsProduction() || _environment.IsStaging())
                context.Database.Migrate();
           
            DbInitializer.Initialize(userManager, context, roleManager);
        }
    }
}
