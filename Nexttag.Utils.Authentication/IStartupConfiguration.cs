using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nexttag.Utils.Authentication
{
    public interface IStartupConfiguration
    {
        void AddAuthorizationAndAuthentication(IServiceCollection services, IConfiguration configuration);
    }
}