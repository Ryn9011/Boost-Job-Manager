using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JobTracker.API.data;
using JobTracker.API.Data;
using JobTracker.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JobTracker.API
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
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddDbContext<DataContext>(x => x.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddTransient<Seed>();
            services.AddCors();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddScoped<MessageRepository>();
            services.AddAutoMapper();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddAuthorization(options => 
            {
                options.AddPolicy("MustBeAdmin", p => p.RequireAuthenticatedUser().RequireRole("admin"));
            });
            
        }

        // public void ConfigureDevelopmentServices(IServiceCollection services)
        // {
        //     var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
        //     services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
        //     services.AddMvc();
        //     services.AddCors();
        //     services.AddAutoMapper();
        //     services.AddScoped<IAuthRepository, AuthRepository>();
        //     services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //         .AddJwtBearer(options => {
        //             options.TokenValidationParameters = new TokenValidationParameters
        //             {
        //                 ValidateIssuerSigningKey = true,
        //                 IssuerSigningKey = new SymmetricSecurityKey(key),
        //                 ValidateIssuer = false,
        //                 ValidateAudience = false
        //             };
        //         });
        //     services.AddAuthorization(options => 
        //     {
        //         options.AddPolicy("MustBeAdmin", p => p.RequireAuthenticatedUser().RequireRole("admin"));
        //     });
            
        // }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            seeder.SeedUsers();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
            app.UseMvc(routes => {
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Fallback", action = "Index"}
                );
            });
        }
    }
}
 