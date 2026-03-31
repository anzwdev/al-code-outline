using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class IdentifierNameComparer : IComparer<IdentifierNameSyntax>
    {

        protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

        public int Compare(IdentifierNameSyntax? x, IdentifierNameSyntax? y)
        {
            var xName = x?.Identifier.ValueText;
            var yName = y?.Identifier.ValueText;
            return _stringComparer.Compare(xName, yName);
        }
    }
}
