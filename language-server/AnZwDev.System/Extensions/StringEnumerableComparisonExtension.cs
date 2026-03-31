using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Extensions
{
    public static class StringEnumerableComparisonExtension
    {

        public static bool SetEquals(this IEnumerable<string>? list1, IEnumerable<string>? list2, IEqualityComparer<string>? equalityComparer = null)
        {
            if (list1 == null)
                return (list2 == null);

            if (list2 == null)
                return false;

            if (equalityComparer == null)
                equalityComparer = StringComparer.Ordinal;

            var set1 = new HashSet<string>(list1, equalityComparer);
            return set1.SetEquals(list2);
        }

    }
}
