using Dommel;

namespace Nexttag.Database.Configuration
{
    public class SnakeCaseTableNameResolver : ITableNameResolver
    {
        public string ResolveTableName(Type type)
        {
            var tableName = type.Name.ToCharArray().ToList();
            var isAllUpper = tableName.All(c => char.IsUpper(c));
            if (isAllUpper)
            {
                return type.Name.ToLower();
            }
            return string.Concat(tableName.Where(c => c != '_').Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c.ToString() : c.ToString())).ToLower();
        }
    }
}