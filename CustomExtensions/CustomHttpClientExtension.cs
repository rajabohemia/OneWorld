using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneWorld.Helpers;

namespace OneWorld.CustomExtensions
{
    public static class CustomHttpClientExtension
    {
        public static void AddCustomHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var AuthHeader = "Authorization";
            var BaseUrl = configuration["Baseurl"];
            
            services.AddHeaderPropagation(options =>
            {
                options.Headers.Add(AuthHeader, context =>
                {
                    var token = context.HttpContext.Request.Cookies[PathConstant.cookies.JWTAuthToken];
                    return !string.IsNullOrWhiteSpace(token) ? $"Bearer {token}" : string.Empty;
                });
            });

            services.AddHttpClient(PathConstant.ApiVersion.v1,
                    options => { options.BaseAddress = new Uri($"{BaseUrl}/{PathConstant.ApiVersion.v1}/"); })
                .AddHeaderPropagation(options => options.Headers.Add(AuthHeader));
        }

        public static void UseCustomHttpClient(this IApplicationBuilder app)
        {
            app.UseHeaderPropagation();
        }
        
    }
}