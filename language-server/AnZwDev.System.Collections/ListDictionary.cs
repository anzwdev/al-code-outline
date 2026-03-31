using System;
using System.Collections.Generic;

namespace AnZwDev.System.Collections
{
    public class ListDictionary<Key, Value> where Key : notnull where Value : notnull
    {

        private Dictionary<Key, List<Value>> _items;

        public ListDictionary()
        {
            _items = new Dictionary<Key, List<Value>>();
        }

        public ListDictionary(IEqualityComparer<Key> keyComparer)
        {
            _items = new Dictionary<Key, List<Value>>(keyComparer);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void Add(Key key, IEnumerable<Value> values)
        {
            var list = GetOrCreateValuesList(key);
            list.AddRange(values);
        }

        public void Add(Key key, Value value)
        {
            var list = GetOrCreateValuesList(key);
            list.Add(value);
        }

        public bool ContainsKey(Key key)
        {
            return _items.ContainsKey(key);
        }

        public bool Contains(Key key, Value value)
        {
            return ((_items.ContainsKey(key)) && (_items[key].Contains(value)));
        }

        public bool Remove(Key key)
        {
            if (_items.ContainsKey(key))
                return _items.Remove(key);
            return false;
        }

        public bool Remove(Key key, Value value)
        {
            if (_items.ContainsKey(key))
            {
                var list = _items[key];
                var result = list.Remove(value);
                if (list.Count == 0)
                    _items.Remove(key);
                return result;
            }
            return false;
        }

        public Value? GetFirstOrDefault(Key key)
        {
            if (_items.ContainsKey(key))
            {
                var list = _items[key];
                if (list.Count > 0)
                    return list[0];
            }
            return default(Value);
        }

        public List<Value>? GetAll(Key key)
        {
            if (_items.ContainsKey(key))
                return _items[key];
            return null;
        }

        private List<Value> GetOrCreateValuesList(Key key)
        {
            if (_items.ContainsKey(key))
                return _items[key];
            var list = new List<Value>();
            _items.Add(key, list);
            return list;
        }

    }
}
