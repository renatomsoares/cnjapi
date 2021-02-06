using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Application.AutoMapper;
using Application.Filters;
using Application.Filters.Logger;
using Infra.Contexts;
using Infra.UnitOfWork;
using Services;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Api
{
    public class Startup
    {
        private readonly string _environmentName;
        private IHostingEnvironment _environment;
        private IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var basePath = Directory.GetCurrentDirectory();
            var applicationPath = Directory.GetCurrentDirectory() + "//";

            var builder = new ConfigurationBuilder()
              .SetBasePath(basePath)
              .AddJsonFile("connectStrings.json", false, true)
              .AddJsonFile(applicationPath + $"appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();

            Configuration = builder.Build();

            _environmentName = env.EnvironmentName;
            _environment = env;
            ConfigureMap.Configure(); //autoMapper
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            //Using DbContext (EntityFrameWork)
            services.AddDbContext<CNJDBContext>(
                options => options.UseSqlServer(Configuration["ConnectionStrings:CNJDB_Lawsuits"], builder => builder.UseRowNumberForPaging())
            );

            //Using mvc pattern
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(MidExceptionFilter));
            });

            services.AddMvc().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });


            
            //Using Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = $"CNJ API - Environment: {_environmentName}",
                    Version = "v1",
                    Description = "National Council of Justice.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Renato Mesquita Soares",
                        Email = "rmsnatal@gmail.com",
                        //Url = "https://www.linkedin.com/in/renatomsoares/"
                    }
                });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.SchemaFilter<IgnorePropertiesSchemaFilter>();
                c.IgnoreObsoleteActions();
                c.DescribeAllEnumsAsStrings();
            });
            


            /* 
            => Dependency Injection
            - Transient
            - Scoped
            - Singleton
            */
            services.AddTransient<IUnitOfWork, UnitOfWork<CNJDBContext>>();
            services.AddSingleton(Configuration);
            services.AddTransient<LawsuitService>();
           
            services.AddMvc();

            services.AddHttpClient("NucleoPGE", client =>
                       {
                           client.DefaultRequestHeaders.Accept.Clear();
                           client.DefaultRequestHeaders.Accept.Add(
                               new MediaTypeWithQualityHeaderValue("application/json"));

                           string baseURL = Configuration.GetSection("NucleoPGE:BaseURL").Value;
                           client.BaseAddress = new System.Uri(baseURL);
                       });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UsePathBase("/api");
            }

            // Cors
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.EnableValidator();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CNJ API");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}