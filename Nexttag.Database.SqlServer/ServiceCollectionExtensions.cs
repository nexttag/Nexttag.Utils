using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dommel;
using Nexttag.Database.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Nexttag.Database.SqlServer;

    public static class ServiceCollectionExtensions
    {
        public static void AddSqlServerDataBase(this IServiceCollection serviceCollection, string connectionString)
        {
            if (FluentMapper.EntityMaps.IsEmpty && FluentMapper.TypeConventions.IsEmpty)
            {
                FluentMapper.Initialize(config =>
                {
                    DefaultTypeMap.MatchNamesWithUnderscores = true;
                    config.ForDommel();
                    DommelMapper.SetTableNameResolver(new SnakeCaseTableNameResolver());
                    DommelMapper.SetColumnNameResolver(new SnakeCaseColumnNameResolver());
                
                });
            }
            
            serviceCollection.AddScoped<IDbConnection>((option) => { return new SqlConnection(connectionString); });
            serviceCollection.AddSingleton(typeof(IConnectionFactory), s => new SqlServerConnectionFactory(connectionString));
            serviceCollection.AddSingleton(typeof(IDbParameterBuilder), s => new SqlServerBuilderParameters());
         
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddScoped(s =>
            {
                var connectionFactory = s.GetService<IConnectionFactory>();
                var dbParameterBuilder = s.GetService<IDbParameterBuilder>();
                return new DatabaseContext(connectionFactory.GetConnection(), dbParameterBuilder);
            });
        }
    }
