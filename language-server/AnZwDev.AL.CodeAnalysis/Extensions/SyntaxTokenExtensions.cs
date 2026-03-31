using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Extensions
{
    public static class SyntaxTokenExtensions
    {

        public static bool IsEmpty(this SyntaxToken token)
        {
            return (token.Kind == SyntaxKind.None);
        }

    }
}
