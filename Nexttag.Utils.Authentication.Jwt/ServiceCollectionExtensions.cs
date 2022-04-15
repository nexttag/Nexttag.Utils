using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Nexttag.Utils.Authentication.Jwt
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtSecurity(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            serviceCollection.AddSingleton<IAuthenticationScheme, JwtAuthenticationScheme>();
            serviceCollection.AddSingleton<IStartupConfiguration, StartupConfiguration>();
            serviceCollection.AddScoped(c =>
            {
                var httpContext = c.GetService<IHttpContextAccessor>();
                return new JwtPayload(httpContext.HttpContext.User?.Claims ?? new List<Claim>(0));
            });
            serviceCollection.AddScoped(c =>
            {
                JwtPayload jwtPayload = c.GetService<JwtPayload>();
                return new TokenInfo
                {
                    Id = jwtPayload.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? string.Empty,
                    Identification = jwtPayload.Claims.FirstOrDefault(c => c.Type == "identification")?.Value ?? string.Empty,
                    Application = jwtPayload.Claims.FirstOrDefault(c => c.Type == "application")?.Value ?? string.Empty,
                    Name = jwtPayload.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty,
                    Roles = jwtPayload.Claims.Where(c => c.Type == "roles" || c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray(),
                    Permissions = jwtPayload.Claims.Where(c => c.Type == "permissions").Select(c=> c.Value).ToArray()
                };
            });
            serviceCollection.AddScoped((provider) =>
            {
                var jwtTokenConfiguration = new JWTTokenConfiguration();
                new ConfigureFromConfigurationOptions<JWTTokenConfiguration>(configuration.GetSection("Token:JWT"))
                    .Configure(jwtTokenConfiguration);
                return new JwtSecurityTokenResolver(jwtTokenConfiguration);
            });
        }
    }
}