using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class IdentifierEqualsLiteralSyntaxExtensions
    {

        public static SyntaxNode GetCommaSeparatedListParent(this IdentifierEqualsLiteralSyntax node)
        {
            SyntaxNode parent = node.Parent;
            if (parent is CommaSeparatedIdentifierEqualsLiteralListSyntax)
                return parent.Parent;
            return null;
        }

    }
}
