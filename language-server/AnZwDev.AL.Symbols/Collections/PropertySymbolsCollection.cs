using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public partial class PropertySymbolsCollection
    {

        private readonly Dictionary<PropertyKind, PropertySymbol> _properties = new Dictionary<PropertyKind, PropertySymbol>();

        public PropertySymbolsCollection()
        {
        }

        public T GetValue<T>(PropertyKind key, T defaultValue)
        {
            if (_properties.ContainsKey(key))
            {
                var propertyValue = (PropertyValueSymbol<T>)_properties[key];
                if (propertyValue != null)
                    return propertyValue.Value;
            }
            return defaultValue;
        }

        public void SetValue<T>(PropertyKind key, T value)
        {
            _properties[key] = new PropertyValueSymbol<T>()
            {
                Kind = key,
                Value = value
            };
        }

        public void Set(PropertySymbol symbol)
        {
            _properties[symbol.Kind] = symbol;
        }

        public bool Contains(PropertyKind key)
        {
            return _properties.ContainsKey(key);
        }

    }
}
