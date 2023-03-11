using AnZwDev.ALTools.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AnZwDev.ALTools.Core
{
    public class SyntaxNodeNameComparer : IComparer<string>, IComparer
    {

        private AlphanumComparatorFast _stringComparer = new AlphanumComparatorFast();

        public int Compare(string x, string y)
        {
            bool xEmpty = String.IsNullOrWhiteSpace(x);
            bool yEmpty = String.IsNullOrWhiteSpace(y);
            if (xEmpty != yEmpty)
                return xEmpty ? 1 : -1;
            if (xEmpty)
                return 0;
            return _stringComparer.Compare(x, y);
        }

        public int Compare(object x, object y)
        {
            return this.Compare(x as string, y as string);
        }

    }
}
