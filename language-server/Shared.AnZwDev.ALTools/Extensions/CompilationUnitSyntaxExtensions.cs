using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
#if BC
    public static class CompilationUnitSyntaxExtensions
    {

        public static string GetNamespaceName(this CompilationUnitSyntax compilationUnit)
        {
            return compilationUnit.NamespaceDeclaration?.Name?.ToString().Trim();
        }

    }

#endif
}
