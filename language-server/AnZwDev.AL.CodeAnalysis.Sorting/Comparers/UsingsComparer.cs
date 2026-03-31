using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class UsingsComparer : IComparer<SyntaxNodeSortInfo<UsingDirectiveSyntax>>
    {
        protected IComparer<string> _comparer;

        public UsingsComparer(string[]? prefixSortOrder)
        {
            _comparer = new FullyQualifiedNameComparer(prefixSortOrder);
        }

        public int Compare(SyntaxNodeSortInfo<UsingDirectiveSyntax>? x, SyntaxNodeSortInfo<UsingDirectiveSyntax>? y)
        {
            return _comparer.Compare(x?.Name, y?.Name);
        }
    }

}
