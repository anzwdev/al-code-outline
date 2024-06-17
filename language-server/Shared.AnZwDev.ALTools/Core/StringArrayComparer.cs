using AnZwDev.ALTools.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Core
{
    public class StringArrayComparer : IComparer<string[]>, IComparer
    {

        private NullableStringComparer _stringComparer = new NullableStringComparer();

        public int Compare(string[] x, string[] y)
        {
            int count = Math.Min(x.Length, y.Length);
            for (int i = 0; i < count; i++)
            {
                var val = _stringComparer.Compare(x[i], y[i]);
                if (val != 0)
                    return val;
            }
            return x.Length - y.Length;
        }

        public int Compare(object x, object y)
        {
            return this.Compare(x as string[], y as string[]);
        }
    }
}
