using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OneWorld.Configurations;

namespace OneWorld.CustomExtensions
{
    public static class CustomAuthenticationExtension
    {
        public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            var jwtConfig = new JWTConfiguration();
            Configuration.GetSection("JWT").Bind(jwtConfig);
            services.AddSingleton(jwtConfig); //Singleton JWTCOnfig Added here

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidAudience = jwtConfig.Aud,
                    ValidIssuer = jwtConfig.Iss,
                    IssuerSigningKey = jwtConfig.SymmetricSecurityKey
                };
            }).AddGoogle(options =>
            {
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.ClientId = Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            }).AddFacebook(options =>
            {
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });
        }
    }
}