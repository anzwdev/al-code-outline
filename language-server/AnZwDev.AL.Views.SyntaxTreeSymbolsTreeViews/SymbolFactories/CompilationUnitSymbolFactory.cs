using AnZwDev.AL.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class CompilationUnitSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(CompilationUnitSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var namespaceName = node.NamespaceDeclaration?.Name?.ToString().Trim();
            var usings = node.Usings.GetUsingsNamespacesNames();
            return SyntaxNodeSymbolFactory.CreateSymbol(
                node, null,
                ALSyntaxNodeKind.CompilationUnit, 
                TreeViewNodeNameSetters.Kind,
                namespaceName, usings);
        }
    }
}
