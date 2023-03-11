using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class IListExtensions
    {

        public static bool IsOrdered<T>(this IList<T> list, IComparer<T> comparer)
        {
            if ((list == null) || (list.Count <= 1))
                return true;
            for (int i=0; i<(list.Count -1); i++)
            {
                if (comparer.Compare(list[i], list[i + 1]) > 0)
                    return false;
            }
            return true;
        }

        public static void AddUniqueRange<T>(this IList<T> list, IEnumerable<T> valuesEnumerable)
        {
            if (valuesEnumerable != null)
                foreach (T value in valuesEnumerable)
                {
                    if (!list.Contains(value))
                        list.Add(value);
                }
        }

    }
}
