using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALLanguageInformation
{
    public class CaseInformationCollection
    {

        private Dictionary<string, string> _values;

        public CaseInformationCollection()
        {
            _values = new Dictionary<string, string>();
        }

        public void Add(string value)
        {
            _values.Add(value.ToLower(), value);
        }

        public void AddRange(params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
                Add(values[i]);
        }

        public bool TryFixCase(ref string value)
        {
            if (value != null)
            {
                string key = value.ToLower();
                if (_values.ContainsKey(key))
                {
                    value = _values[key];
                    return true;
                }
            }
            return false;
        }

        public string FixCase(string value)
        {
            TryFixCase(ref value);
            return value;
        }

    }
}
