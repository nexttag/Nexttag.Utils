using Dommel;
using System.Linq;
using System.Reflection;

namespace Nexttag.Utils.Database.Configuration
{
    public class SnakeCaseColumnNameResolver : IColumnNameResolver
    {
        public string ResolveColumnName(PropertyInfo property)
        {
            var columnName = property.Name.ToCharArray().ToList();
            var isAllUpper = columnName.All(c => char.IsUpper(c));
            if (isAllUpper)
            {
                return property.Name;
            }
            return string.Concat(columnName.Where(c => c != '_').Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c.ToString() : c.ToString()));
        }
    }
}