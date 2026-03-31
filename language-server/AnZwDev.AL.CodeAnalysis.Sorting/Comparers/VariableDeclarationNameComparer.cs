using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class VariableDeclarationNameComparer : IComparer<VariableDeclarationNameSyntax>
    {
        protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

        public int Compare(VariableDeclarationNameSyntax? x, VariableDeclarationNameSyntax? y)
        {
            var xName = x?.Name?.Unquoted();
            var yName = y?.Name?.Unquoted();
            return _stringComparer.Compare(xName, yName);
        }
    }

}
