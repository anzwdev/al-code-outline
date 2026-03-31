using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Extensions
{
    public static class IdentifierEqualsLiteralSyntaxExtensions
    {

        public static SyntaxNode? GetCommaSeparatedListParent(this IdentifierEqualsLiteralSyntax node)
        {
            SyntaxNode parent = node.Parent;
            if (parent is CommaSeparatedIdentifierEqualsLiteralListSyntax)
                return parent.Parent;
            return null;
        }

    }
}
