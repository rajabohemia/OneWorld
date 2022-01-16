using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OneWorld.Helpers;

namespace OneWorld.CustomExtensions
{
    public static class CustomSwaggerExtension
    {
        public static void AddCustomSwaggerandApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.Conventions.Add(new VersionByNamespaceConvention());
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader());
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.SubstituteApiVersionInUrl = true;
                options.GroupNameFormat = "'v'VVV";
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(PathConstant.ApiVersion.v1, new OpenApiInfo()
                {
                    Contact = new OpenApiContact()
                    {
                        Email = "hackstr@outlook.com",
                        Name = "Rahul Sharma",
                        Url = new Uri("https://facebook.com/hackstr")
                    },
                    Description = "Version-1 API Testing",
                    Title = "Version-1",
                    Version = PathConstant.ApiVersion.v1
                });
            });
        }

        public static void UseCustomSwaggerandApiVersioning(this IApplicationBuilder app)
        {
            app.UseApiVersioning();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{PathConstant.ApiVersion.v1}/swagger.json", "API v1");
            });
        }
    }
}