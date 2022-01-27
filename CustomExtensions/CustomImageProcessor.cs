using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Web.DependencyInjection;

namespace OneWorld.CustomExtensions
{
    public static class CustomImageProcessor
    {
        public static void AddCustomImageSharp(this IServiceCollection services)
        {
            services.AddImageSharp(options =>
            {
                options.Configuration = Configuration.Default;
                options.MemoryStreamManager = new RecyclableMemoryStreamManager();
                options.BrowserMaxAge = TimeSpan.FromDays(7);
                options.CacheMaxAge = TimeSpan.FromDays(365);
                options.CachedNameLength = 8;
            });
        }

        public static void UseCustomImageSharp(this IApplicationBuilder app)
        {
            app.UseImageSharp();
        }
        
    }
}