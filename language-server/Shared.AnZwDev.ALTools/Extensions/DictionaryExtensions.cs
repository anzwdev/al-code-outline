using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class DictionaryExtensions
    {

        public static string GetStringValue(this Dictionary<string, string> dictionary, string key, string defaultValue = null)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            return defaultValue;
        }

        public static bool GetBoolValue(this Dictionary<string, string> dictionary, string key, bool defaultValue = false)
        {
            string value = dictionary.GetStringValue(key);
            if (String.IsNullOrWhiteSpace(value))
                return defaultValue;
            return ((value.ToLower() == "true") || (value == "1"));
        }

        public static List<string> GetStringListValue(this Dictionary<string, string> dictionary, string keyPrefix)
        {
            List<string> valuesList = new List<string>();
            foreach (string key in dictionary.Keys)
            {
                if ((key.StartsWith(keyPrefix)) && (!String.IsNullOrWhiteSpace(dictionary[key])))
                    valuesList.Add(dictionary[key]);
            }
            if (valuesList.Count == 0)
                return null;
            return valuesList;
        }

        public static ET FindOrCreate<KT, ET>(this Dictionary<KT, ET> dictionary, KT key) where ET : new()
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            ET value = new ET();
            dictionary.Add(key, value);
            return value;
        }

    }
}
