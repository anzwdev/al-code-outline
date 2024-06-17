using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AnZwDev.ALTools.Core
{
    public class FullyQualifiedNameComparer : IComparer<string>, IComparer
    {

        private StringArrayComparer _namePartsComparer = new StringArrayComparer();
        public string[] PrefixSortOrder { get; set; } = null;

        public FullyQualifiedNameComparer()
        {
        }

        public FullyQualifiedNameComparer(string[] prefixSortOrder)
        {
            PrefixSortOrder = prefixSortOrder;
        }

        public int Compare(string x, string y)
        {
            bool xEmpty = String.IsNullOrWhiteSpace(x);
            bool yEmpty = String.IsNullOrWhiteSpace(y);
            if (xEmpty != yEmpty)
                return xEmpty ? 1 : -1;
            if (xEmpty)
                return 0;
            
            var xPrefixOrder = GetPrefixSortOrder(x);
            var yPrefixOrder = GetPrefixSortOrder(y);
            if (xPrefixOrder != yPrefixOrder)
                return xPrefixOrder - yPrefixOrder;

            return _namePartsComparer.Compare(x.Split('.'), y.Split('.'));
        }

        public int Compare(object x, object y)
        {
            return this.Compare(x as string, y as string);
        }

        protected int GetPrefixSortOrder(string prefix)
        {
            if ((String.IsNullOrWhiteSpace(prefix)) || (PrefixSortOrder == null) || (PrefixSortOrder.Length == 0))
                return -1;

            var prefixWithSeparator = prefix + ".";

            for (int i = 0; i < PrefixSortOrder.Length; i++)
            {
                if ((prefix.StartsWith(PrefixSortOrder[i], StringComparison.OrdinalIgnoreCase)) || (prefixWithSeparator.Equals(PrefixSortOrder[i], StringComparison.OrdinalIgnoreCase)))
                    return i;
            }

            return PrefixSortOrder.Length;
        }

    }
}
