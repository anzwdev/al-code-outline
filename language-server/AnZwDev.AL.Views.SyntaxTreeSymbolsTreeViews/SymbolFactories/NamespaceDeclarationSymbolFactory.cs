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
    internal static class NamespaceDeclarationSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(NamespaceDeclarationSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, null, 
                ALSyntaxNodeKind.Namespace,
                TreeViewNodeNameSetters.Kind);

            var namespaceName = node.GetNamespaceName();
            if (!String.IsNullOrWhiteSpace(namespaceName))
                symbol.FullName = symbol.Name + " " + namespaceName;

            return symbol;
        }
    }
}
