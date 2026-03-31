using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Comparers
{
    public class StringArrayComparer : IComparer<string[]>, IComparer
    {

        private NullableStringComparer _stringComparer = new NullableStringComparer();

        public int Compare(string[]? x, string[]? y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            var xLength = x.Length;
            var yLength = y.Length;

            int count = Math.Min(xLength, yLength);
            for (int i = 0; i < count; i++)
            {
                var val = _stringComparer.Compare(x[i], y[i]);
                if (val != 0)
                    return val;
            }
            return xLength.CompareTo(yLength);
        }

        public int Compare(object? x, object? y)
        {
            return this.Compare(x as string[], y as string[]);
        }
    }
}
