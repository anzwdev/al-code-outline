using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.ServiceModel
{
    public class InstanceServiceProvider : IServiceProvider
    {

        private readonly IServiceProvider? _parentServiceProvider;
        private readonly Dictionary<Type, object> _services = new();

        public InstanceServiceProvider(IServiceProvider? parentServiceProvider = null)
        {
            _parentServiceProvider = parentServiceProvider;
        }

        public void AddSingleton<T>(T instance) where T : class
        {
            _services[typeof(T)] = instance;
        }

        public void AddSingleton<TKey, TInstance>() where TKey : class where TInstance : TKey, new()
        {
            AddSingleton<TKey>(new TInstance());
        }

        public object? GetService(Type serviceType)
        {
            if (_services.TryGetValue(serviceType, out var service))
                return service;

            if (_parentServiceProvider != null)
                return _parentServiceProvider.GetService(serviceType);

            return null;
        }
    }
}
