using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cw5.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Cw5.Middlewares;
using Cw5.Models;
using Cw5.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using static System.Net.WebRequestMethods;

namespace Cw5
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IStudentDbService, SqlServerStudentDbService>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer=true,
                        ValidateAudience=true,
                        ValidateLifetime=true,
                        ValidIssuer="Gakko",
                        ValidAudience="Students",
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
                    };
                });

            // services.AddAuthentication("AuthenticationBasic")
            //     .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("AuthenticationBasic", null);
            
            services.AddControllers();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1",new OpenApiInfo{Title = "Student App API", Version = "v1"});
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentDbService dbService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Student App API");
            });
            
            // app.UseMiddleware<LoggingMiddleware>();

            // app.Use(async (context, next) =>
            // {
            //     if (!context.Request.Headers.ContainsKey("Index"))
            //     {
            //         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //         await context.Response.WriteAsync("Nie podano indeksu");
            //         return;
            //     }
            //
            //     string index = context.Request.Headers["Index"].ToString();
            //     
            //     if (!dbService.CheckIndexNumber(index))
            //     {
            //         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //         await context.Response.WriteAsync("Student o podanym indeksie nie istnieje");
            //         return;
            //     }
            //     
            //     await next();
            // });
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
            });
        }
    }
}
