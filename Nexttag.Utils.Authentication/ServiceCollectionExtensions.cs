using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Nexttag.Utils.Authentication
{
    public static class ServiceCollectionExtensions
    {

        public static void AddAuthorizationAndAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();

            var startupConfigurations = serviceProvider.GetServices<IStartupConfiguration>();
            var authorizationSchemes = serviceProvider.GetServices<IAuthenticationScheme>();

            foreach (var startupConfiguration in startupConfigurations)
            {
                startupConfiguration.AddAuthorizationAndAuthentication(services, configuration);
            }

            services
                .AddAuthorizationCore(auth =>
                {

                    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                        authorizationSchemes.Select(x => x.Scheme).ToArray()
                    );
                    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                    var bearerBuild = defaultAuthorizationPolicyBuilder.Build();
                    auth.DefaultPolicy = bearerBuild;
                    auth.AddPolicy("Bearer", bearerBuild);

                });
        }
    }
}