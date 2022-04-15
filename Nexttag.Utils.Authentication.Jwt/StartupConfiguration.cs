using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Nexttag.Utils.Authentication.Jwt
{
    public class StartupConfiguration : IStartupConfiguration
    {
        public void AddAuthorizationAndAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var jwtTokenConfiguration = new JWTTokenConfiguration();
            new ConfigureFromConfigurationOptions<JWTTokenConfiguration>(configuration.GetSection("Token:JWT"))
                .Configure(jwtTokenConfiguration);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtAuthenticationScheme.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtAuthenticationScheme.AuthenticationScheme;
                })
                .AddJwtBearer((jwtOption) =>
                {
                    jwtOption.RequireHttpsMetadata = false;
                    jwtOption.SaveToken = true;

                    jwtOption.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = jwtTokenConfiguration.Issuer,
                        ValidAudience = jwtTokenConfiguration.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfiguration.Key)),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

        }
    }
}