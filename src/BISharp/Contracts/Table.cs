using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BISharp.Contracts
{
    public class Tables
    {
        public List<Table> value { get; set; }
    }
    public class Table
    {
        public string name { get; set; }
        public List<Column> columns { get; set; }

        public static Table FromType(Type t)
        {
            var table = new Table();
            table.name = t.Name;
            table.columns = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Select(p => new Column { name = p.Name, dataType = toPowerBiType(p.PropertyType) })
                .ToList();

            return table;
        }

        private static string toPowerBiType(Type propertyType)
        {
            if (propertyType == typeof(int) || propertyType == typeof(long))
            {
                return "Int64";
            }
            if (propertyType == typeof(double) || propertyType == typeof(float))
            {
                return "Double";
            }
            if (propertyType == typeof(bool))
            {
                return "bool";
            }
            if (propertyType == typeof(DateTime))
            {
                return "DateTime";
            }

            return "string";
        }
    }

    public class Column
    {
        public string name { get; set; }
        public string dataType { get; set; }
    }

}
