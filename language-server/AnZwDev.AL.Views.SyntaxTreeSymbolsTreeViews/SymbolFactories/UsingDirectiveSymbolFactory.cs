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
    internal static class UsingDirectiveSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(UsingDirectiveSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, null, 
                ALSyntaxNodeKind.UsingDirective,
                TreeViewNodeNameSetters.Kind);

            var namespaceName = node.Name?.ToString();
            if (!String.IsNullOrWhiteSpace(namespaceName))
                symbol.FullName = symbol.Name + " " + namespaceName;

            return symbol;
        }
    }
}
