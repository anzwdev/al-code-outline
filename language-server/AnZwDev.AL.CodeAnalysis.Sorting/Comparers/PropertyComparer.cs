using AnZwDev.AL.CodeAnalysis.Sorting;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class PropertyComparer : IComparer<SyntaxNodeSortInfo<PropertySyntaxOrEmpty>>
    {
        protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

        public PropertyComparer()
        {
        }

        public int Compare(SyntaxNodeSortInfo<PropertySyntaxOrEmpty>? x, SyntaxNodeSortInfo<PropertySyntaxOrEmpty>? y)
        {
            int val = _stringComparer.Compare(x?.Name, y?.Name);
            if (val != 0)
                return val;
            var xIndex = x?.Index ?? 0;
            var yIndex = y?.Index ?? 0;

            return xIndex - yIndex;
        }
    }

}
