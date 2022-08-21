using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Nexttag.Application.Base.Interfaces;

namespace Nexttag.Application.Base;

public static class ServiceCollectionExtensison
{
    public static void AddApplicationResolvers(this IServiceCollection serviceCollection)

    {
        var dataAccess = Assembly.GetCallingAssembly();

        dataAccess.DefinedTypes
            .Where(t => t.GetInterface(typeof(ICommand<,>).Name) != null)
            .ToList()
            .ForEach(t => AddTransientInjection(serviceCollection, typeof(ICommand<,>), t));

        serviceCollection.AddHttpContextAccessor();
        /*serviceCollection.AddScoped((c) =>
        {
            var resolver = new PharmarcyNetworkResolver(c.GetService<IHttpContextAccessor>());

            return resolver.Resolve();
        });*/

    }

    public static void AddHttpHelper(this IServiceCollection serviceCollection)

    {
        serviceCollection.AddSingleton<HttpClientHelper>();
    }

    public static void AddTransientInjection(IServiceCollection serviceCollection, Type serviceType, Type implementationType)
    {
        serviceCollection.AddTransient(implementationType);
        serviceCollection.AddTransient(implementationType.GetInterface(serviceType.Name), implementationType);
    }

}