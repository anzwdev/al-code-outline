using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.System.Comparers
{
    public class NullableStringComparer : IComparer<string>, IComparer
    {

        private AlphanumComparatorFast _stringComparer = new AlphanumComparatorFast();

        public int Compare(string? x, string? y)
        {
            bool xEmpty = string.IsNullOrWhiteSpace(x);
            bool yEmpty = string.IsNullOrWhiteSpace(y);
            if (xEmpty != yEmpty)
                return xEmpty ? 1 : -1;
            if (xEmpty)
                return 0;
            return _stringComparer.Compare(x, y);
        }

        public int Compare(object? x, object? y)
        {
            return Compare(x as string, y as string);
        }

    }
}
