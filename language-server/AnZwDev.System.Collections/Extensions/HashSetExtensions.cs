using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Collections.Extensions
{
    public static class HashSetExtensions
    {

        public static HashSet<string> Create(params string[] values)
        {
            return new HashSet<string>(values);
        }

        public static HashSet<T> AddOrCreate<T>(this HashSet<T>? hashSet, T value)
        {
            if (hashSet == null)
                hashSet = new HashSet<T>();
            if (!hashSet.Contains(value))
                hashSet.Add(value);
            return hashSet;
        }

        public static void AddRange(this HashSet<string> hashSet, IEnumerable<string> values)
        {
            foreach (var value in values)
                hashSet.Add(value);
        }

        public static bool IsNullOrEmpty(this HashSet<string>? hashSet)
        {
            return (hashSet == null) || (hashSet.Count == 0);
        }

    }
}
