using System.Reflection;
using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Hosting;

namespace Nexttag.Database.Postgres
{
    public static class MigrationExtensions
    {
        public static bool RunMigration(this IHost hostbuilder, string connectionString)
        {
            var upgraderConfiguration = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetCallingAssembly())
                .LogToConsole()
                .BuildConfiguration();

            var upgrader = new UpgradeEngine(upgraderConfiguration);

            Console.WriteLine($"Upgrader: { upgraderConfiguration }");
            upgrader.TryConnect(out var messageError);
            Console.WriteLine($"ConnectionString: {connectionString}");
            Console.WriteLine($"UpgraderConnection: { messageError }");

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

            return result.Successful;

        }
    }
}