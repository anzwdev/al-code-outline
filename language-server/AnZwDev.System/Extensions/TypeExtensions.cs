using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Extensions
{
    public static class TypeExtensions
    {

        public static string? TryGetPropertyValueAsString(this Type nodeType, object obj, string propertyName)
        {
            return nodeType.TryGetPropertyValue(obj, propertyName)?.ToString();
        }

        public static T? TryGetPropertyValue<T>(this Type type, object obj, string propertyName) where T : class
        {
            return (T?)TryGetPropertyValue(type, obj, propertyName);
        }

        public static T TryGetStructPropertyValue<T>(this Type type, object obj, string propertyName) where T : struct
        {
            var value = TryGetPropertyValue(type, obj, propertyName);
            if (value != null)
                return (T)value;
            else
                return default(T);
        }

        public static object? TryGetPropertyValue(this Type type, object obj, string propertyName)
        {
            return type.GetProperty(propertyName)?.GetValue(obj);
        }

    }
}
