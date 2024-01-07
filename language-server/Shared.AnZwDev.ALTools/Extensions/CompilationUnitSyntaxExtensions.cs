using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
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

        public static bool HasNamespaces(CompilationUnitSyntax compilationUnit)
        {
            return
                ((compilationUnit.Usings != null) && (compilationUnit.Usings.Count > 0)) ||
                (!String.IsNullOrWhiteSpace(compilationUnit.GetNamespaceName()));
        }

        public static LinePosition GetUsingsStartLinePosition(this CompilationUnitSyntax compilationUnit, TextLineCollection textLines)
        {
            if ((compilationUnit.Usings != null) && (compilationUnit.Usings.Count > 0))
                return textLines.GetLinePosition(compilationUnit.Usings[0].Span.Start);

            if (compilationUnit.NamespaceDeclaration != null)
                return textLines.GetLinePosition(compilationUnit.NamespaceDeclaration.FullSpan.End);

            return new LinePosition(0, 0);
        }

        public static bool UsesNamespaces(this CompilationUnitSyntax compilationUnit)
        {
            return
                (!String.IsNullOrWhiteSpace(compilationUnit.NamespaceDeclaration?.Name?.ToString())) ||
                ((compilationUnit.Usings != null) && (compilationUnit.Usings.Count > 0));
        }

    }

#endif
}
