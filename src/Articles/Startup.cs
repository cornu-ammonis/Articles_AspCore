﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Articles.Data;
using Articles.Models;
using Articles.Services;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Articles.Models.Core;
using Microsoft.AspNetCore.Mvc;

namespace Articles
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
               

                //commented for deployment because this throws an error if run on azure 
               builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            ConnectionString = Configuration.GetConnectionString("defaultConnection");
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            //require HTTPS
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<ILinkRepository, LinkRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

          //obsolete for 1.1  app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

          //obsolate for 1.1  app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();
           /* app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads")),
                RequestPath = new PathString("/StaticFiles")
            }); */


            app.UseIdentity();
          // app.ApplicationServices.GetRequiredService<ApplicationDbContext>().Seed();


            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {

                routes.MapRoute(name: "Author",
                   template: "{controller=Blog}/{action=PostsByAuthor}/{author}");

                routes.MapRoute(name: "Subscribe",
                    template: "{controller=Blog}/{action=Subscribe}/{authorname}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

               


            /*
                routes.MapRoute(
                    name: "tag",
                    template: "{controller=Blog}/{action=Tag}/{tag?}");*/
            }
            );
        }
    }
}
