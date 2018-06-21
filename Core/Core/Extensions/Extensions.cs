using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Core.Extensions
{
    /// <summary>
    /// Расширения
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Получение отображаемого имени enum
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            if (enumValue == null) return string.Empty;

            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DescriptionAttribute>()
                .Description;
        }

        public static decimal ToDecimal(this string value)
        {
            NumberStyles ns = NumberStyles.AllowDecimalPoint;
            IFormatProvider fp = value.Contains(",")
                ? new CultureInfo("ru-RU")
                : new CultureInfo("en-US");

            decimal result;
            if (decimal.TryParse(value, ns, fp, out result))
            {
                return result;
            }
            return 0m;
        }
        
        /// <summary>
        /// List to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iList"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            var dataTable = new DataTable();
            var propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (var i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                var propertyDescriptor = propertyDescriptorCollection[i];
                var type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            var values = new object[propertyDescriptorCollection.Count];
            foreach (var iListItem in iList)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
