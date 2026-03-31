using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Extensions
{
    public static class NamespaceDeclarationSyntaxExtensions
    {

        public static string? GetNamespaceName(this NamespaceDeclarationSyntax? namespaceDeclarationSyntax)
        {
            return namespaceDeclarationSyntax?.Name?.ToString().Trim();
        }

    }
}
