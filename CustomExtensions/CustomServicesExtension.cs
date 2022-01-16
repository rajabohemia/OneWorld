using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneWorld.Configurations;
using OneWorld.Services;

namespace OneWorld.CustomExtensions
{
    public static class CustomServicesExtension
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Cookie Config
            var cookieConfig = new CookieConfiguration();
            configuration.GetSection("Cookie").Bind(cookieConfig);
            services.AddSingleton(cookieConfig);
            //Cookie Config Ends

            services.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();
            services.AddScoped<IAccountRepo, AccountRepo>();
        }
    }
}