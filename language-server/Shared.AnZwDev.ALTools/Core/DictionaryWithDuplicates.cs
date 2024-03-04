using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnZwDev.ALTools.Core
{
    public class DictionaryWithDuplicates<Key, Value>
    {

        private Dictionary<Key, Value> _singleElements = new Dictionary<Key, Value>();
        private Dictionary<Key, List<Value>> _multipleElements = new Dictionary<Key, List<Value>>();

        public DictionaryWithDuplicates()
        {
            _singleElements = new Dictionary<Key, Value>();
            _multipleElements = new Dictionary<Key, List<Value>>();
        }

        public DictionaryWithDuplicates(IEqualityComparer<Key> keyComparer)
        {
            _singleElements = new Dictionary<Key, Value>(keyComparer);
            _multipleElements = new Dictionary<Key, List<Value>>(keyComparer);
        }

        public void Clear()
        {
            _singleElements.Clear();
            _multipleElements.Clear();
        }

        public void Add(Key key, Value value)
        {
            if (_singleElements.ContainsKey(key))
            {
                List<Value> list;
                if (_multipleElements.ContainsKey(key))
                    list = _multipleElements[key];
                else
                {
                    list = new List<Value>();
                    list.Add(_singleElements[key]);
                    _multipleElements.Add(key, list);
                    _singleElements.Remove(key);
                }
                list.Add(value);
            }
            else
                _singleElements.Add(key, value);
        }

        public bool ContainsKey(Key key)
        {
            return _singleElements.ContainsKey(key) || _multipleElements.ContainsKey(key);
        }

        public bool ContainsSingleElementKey(Key key)
        {
            return _singleElements.ContainsKey(key);
        }

        public bool ContainsMultipleElementsKey(Key key)
        {
            return _multipleElements.ContainsKey(key);
        }

        public bool ContainsValue(Key key, Value value)
        {
            if (_singleElements.ContainsKey(key))
                return _singleElements[key].Equals(value);
            if (_multipleElements.ContainsKey(key))
                return _multipleElements[key].Contains(value);
            return false;
        }

        public bool Remove(Key key)
        {
            if (_singleElements.ContainsKey(key))
            {
                _singleElements.Remove(key);
                return true;
            }
            if (_multipleElements.ContainsKey(key))
            {
                _multipleElements.Remove(key);
                return true;
            }
            return false;
        }

        public bool Remove(Key key, Value value)
        {
            if (_singleElements.ContainsKey(key))
            {
                if (_singleElements[key].Equals(value))
                {
                    _singleElements.Remove(key);
                    return true;
                }
                return false;
            }

            if (_multipleElements.ContainsKey(key))
            {
                List<Value> list = _multipleElements[key];
                if (list.Contains(value))
                {
                    list.Remove(value);
                    if (list.Count == 0)
                        _multipleElements.Remove(key);
                    return true;
                }
            }

            return false;
        }

        public Value GetFirstValue(Key key)
        {
            if (_singleElements.ContainsKey(key))
                return _singleElements[key];
            if (_multipleElements.ContainsKey(key))
                return _multipleElements[key][0];
            return default(Value);
        }

        public Value GetSingleValue(Key key)
        {
            if (_singleElements.ContainsKey(key))
                return _singleElements[key];
            return default(Value);
        }

        public List<Value> GetMultipleValues(Key key)
        {
            if (_multipleElements.ContainsKey(key))
                return _multipleElements[key];
            return null;
        }

    }
}
