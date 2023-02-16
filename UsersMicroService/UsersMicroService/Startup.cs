using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersMicroService.Mappers;
using UsersMicroService.Services;
using UserTicketSystemCore.Interfaces;
using UserTicketSystemCore.Services.Abstractions;
using UserTicketSystemData;
using UserTicketSystemData.Repositories;

namespace UsersMicroService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserTicketSystemContext>();
            // register automapper
            services.AddAutoMapper(typeof(CustomMapper));
            // Register IUserRepository and IUserHierarchyRepository with their implementations
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserHierarchyRepository, UserHierarchyRepository>();

            // Register UserService as a scoped dependency
            services.AddScoped<IUserService, UserService>();

            //register swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Users API", Version = "v1" });
            });
            services.AddSwaggerGenNewtonsoftSupport();


            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API V1");
            });
        }
    }
}
