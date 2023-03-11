using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class IEnumerableExtensions
    {

        public static IEnumerable<T> MergeWith<T>(this IEnumerable<T> mainEnumerable, params IEnumerable<T>[] additionalEnumerables)
        {
            if (mainEnumerable != null)
                foreach (T item in mainEnumerable)
                    yield return item;

            for (int i = 0; i < additionalEnumerables.Length; i++)
                if (additionalEnumerables[i] != null)
                    foreach (T item in additionalEnumerables[i])
                        yield return item;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable, bool nullIfEmpty)
        {
            bool isEmpty = true;
            HashSet<T> hashSet = new HashSet<T>();
            foreach (T item in enumerable)
            {
                hashSet.Add(item);
                isEmpty = false;
            }
            if (isEmpty)
                return null;
            return hashSet;
        }

        public static HashSet<string> ToLowerCaseHashSet(this IEnumerable<string> enumerable)
        {
            HashSet<string> hashSet = new HashSet<string>();
            foreach (string item in enumerable)
                hashSet.Add(item.ToLower());
            return hashSet;
        }



    }
}
