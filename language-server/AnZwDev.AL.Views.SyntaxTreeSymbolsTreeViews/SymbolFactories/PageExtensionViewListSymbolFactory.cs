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
    internal static class PageExtensionViewListSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode? CreateSymbol(PageExtensionViewListSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            if (node.Changes.Count == 0)
                return null;

            return SyntaxNodeSymbolFactory.CreateSymbol(
                node, null, 
                ALSyntaxNodeKind.PageExtensionViewList,
                TreeViewNodeNameSetters.Kind,
                node.OpenBraceToken, node.CloseBraceToken);
        }
    }
}
