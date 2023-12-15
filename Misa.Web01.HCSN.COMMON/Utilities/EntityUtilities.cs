
using MEDITRACK.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.COMMON.Utilities
{
    public static class EntityUtilities
    {
        public static string GetTableName<T>()
        {
            string tableName = typeof(T).Name;
            var tableAttributes = typeof(T).GetTypeInfo().GetCustomAttributes<TableAttribute>();
            if (tableAttributes.Count() > 0)
            {
                tableName = tableAttributes.First().Name;
            }
            return tableName;
        }
    public static string GetPreFix<T>()
    {
        string prefix = "";

        var tableAttributes = typeof(T).GetTypeInfo().GetCustomAttributes<PrefixAttribute>();
        if (tableAttributes.Count() > 0)
        {
                prefix = tableAttributes.First().Name;
         }

        return prefix;
    }
    }
}
