using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal abstract class HashTableParser<T>
    {

        private Dictionary<string, T> _values;
        private T _defaultValue;

        public HashTableParser(T defaultValue)
        {
            _values = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
            _defaultValue = defaultValue;
        }
        
        protected void Add(string key, T value)
        {
            _values.Add(key, value);
        }

        public T Parse(string? text)
        {
            if ((!String.IsNullOrWhiteSpace(text)) && (_values.ContainsKey(text)))
                return _values[text];
            return _defaultValue;
        }

    }
}
