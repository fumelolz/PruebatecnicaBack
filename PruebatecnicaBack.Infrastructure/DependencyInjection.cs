using api_puntos.Interceptors;
using PruebatecnicaBack.Application.Common.Interfaces.Authentication;
using PruebatecnicaBack.Application.Common.Interfaces.Storage;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Application.Common.Services;
using PruebatecnicaBack.Infrastructure.Authentication;
using PruebatecnicaBack.Infrastructure.Persistence;
using PruebatecnicaBack.Infrastructure.Persistence.Repositories;
using PruebatecnicaBack.Infrastructure.Services;
using PruebatecnicaBack.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PruebatecnicaBack.Application.Common.Interfaces.scrapping;
using PruebatecnicaBack.Infrastructure.Services.Scraper;

namespace PruebatecnicaBack.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<DateTrackingInterceptor>();
            services.AddAuth(configuration);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IZoneRepository, ZoneRepository>();
            services.AddTransient<IFileStorage, LocalFileStorage>();
            services.AddScoped<IScraperService, ScraperService>();
            services.AddHttpContextAccessor();
            services.AddDbContext<ApplicationDbContext>((sp, options) => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")).AddInterceptors(sp.GetRequiredService<DateTrackingInterceptor>()));
            
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
        {
            var JwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, JwtSettings);

            services.AddSingleton(Options.Create(JwtSettings));
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JwtSettings.Issuer,
                        ValidAudience = JwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Secret)),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var path = context.HttpContext.Request.Path;
                            if (path.StartsWithSegments("/api/v1/auth/refreshtoken") &&
                                context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                                context.NoResult();
                                return Task.CompletedTask;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            return services;
        }
    }
}
