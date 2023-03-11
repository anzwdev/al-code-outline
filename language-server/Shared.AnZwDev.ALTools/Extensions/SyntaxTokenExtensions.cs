using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class SyntaxTokenExtensions
    {

        public static bool IsEmpty(this SyntaxToken token)
        {
            return (token.Kind.ConvertToLocalType() == ALSymbols.Internal.ConvertedSyntaxKind.None);
        }

        public static bool IsEmptyOrBefore(this SyntaxToken token, int position)
        {
            return (token.IsEmpty()) || (token.Span.End <= position);
        }

        public static bool IsEmptyOrAfter(this SyntaxToken token, int position)
        {
            return (token.IsEmpty()) || (token.Span.Start >= position);
        }

    }
}

