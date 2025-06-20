﻿using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using visiom.Core;

namespace visiom.Api.Configuration
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly Logger<Startup> _logger;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOpenApiDocument(c =>
                c.PostProcess = document =>
                {
                    document.Info = new NSwag.OpenApiInfo
                    {
                        Title = "visiom SearchEngine",
                        Version = "v1",
                        Description = "Visiom is a modern Search Engine build with React & .NET.",
                        Contact = new NSwag.OpenApiContact
                        {
                            Name = "Visiom",
                            Email = ""
                        }
                    };
                }
            );

            services.AddCors(c =>
            {
                c.AddDefaultPolicy(new CorsPolicyBuilder().WithOrigins(_configuration.GetSection("CorsUrls").Get<string[]>())
                    .AllowAnyMethod()
                .AllowAnyHeader()
                    .AllowCredentials()
                    .Build());
            });

            services.AddControllers();

            services.ConfigureCoreServices();
            services.ConfigureCore();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
