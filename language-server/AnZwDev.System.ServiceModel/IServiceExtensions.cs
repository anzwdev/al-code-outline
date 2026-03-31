using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.ServiceModel
{
    public static class IServiceExtensions
    {

        public static T? GetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.GetService(typeof(T)) as T;
        }

        public static T GetService<T>(this IServiceProvider serviceProvider, T defaultValue) where T : class
        {
            var value = serviceProvider.GetService<T>();
            if (value == null)
                return defaultValue;
            return value;
        }

    }
}
