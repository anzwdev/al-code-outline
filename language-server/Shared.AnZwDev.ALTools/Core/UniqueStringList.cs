using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace AnZwDev.ALTools.Core
{
    public class UniqueStringList
    {

        public static bool IsNullOrEmpty(UniqueStringList list)
        {
            return ((list == null) || (list.Count == 0));
        }


        private List<string> _values = new List<string>();
        private HashSet<string> _uniqueValues = new HashSet<string>();

        public string this[int index]
        {
            get { return _values[index]; }
        }

        public int Count
        {
            get { return _values.Count; }
        }

        public UniqueStringList()
        {
        }

        public void Add(string value)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                string key = GetKey(value);
                if (!_uniqueValues.Contains(key))
                {
                    _values.Add(value);
                    _uniqueValues.Add(key);
                }
            }
        }

        public bool Contains(string value)
        {
            string key = GetKey(value);
            return _uniqueValues.Contains(key);
        }

        private string GetKey(string value)
        {
            return value.ToLower();
        }

        public bool Equals(UniqueStringList list)
        {
            if ((list == null) || (this.Count != list.Count))
                return false;

            for (int i = 0; i < list.Count; i++)
                if (!Contains(list[i]))
                    return false;

            return true;
        }

    }
}
