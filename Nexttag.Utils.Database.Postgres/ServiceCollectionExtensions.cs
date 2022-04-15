using System.Data;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dommel;
using Microsoft.Extensions.DependencyInjection;
using Nexttag.Utils.Database.Configuration;
using Npgsql;

namespace Nexttag.Utils.Database.Postgres
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPosgressDataBase(this IServiceCollection serviceCollection, string connectionString)
        {
            if (FluentMapper.EntityMaps.Count == 0 && FluentMapper.TypeConventions.IsEmpty)
            {
                FluentMapper.Initialize(config =>
                {
                    DefaultTypeMap.MatchNamesWithUnderscores = true;
                    config.ForDommel();
                    DommelMapper.SetTableNameResolver(new SnakeCaseTableNameResolver());
                    DommelMapper.SetColumnNameResolver(new SnakeCaseColumnNameResolver());
                });
            }

            serviceCollection.AddScoped<IDbConnection>((option) =>
            {
                return new NpgsqlConnection(connectionString);
            });

            serviceCollection.AddSingleton(typeof(IConnectionFactory), s => new PostgreSQLConnectionFactory(connectionString));
            serviceCollection.AddSingleton(typeof(IDbParameterBuilder), s => new PostegreSQLBuilderParameters());
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddScoped(s =>
            {
                var connectionFactory = s.GetService<IConnectionFactory>();
                var dbParameterBuilder = s.GetService<IDbParameterBuilder>();
                return new DatabaseContext(connectionFactory.GetConnection(), dbParameterBuilder);
            });
        }
    }
}