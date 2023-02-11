using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nexttag.Authentication;

public interface IStartupConfiguration
{
    void AddAuthorizationAndAuthentication(IServiceCollection services, IConfiguration configuration);
}