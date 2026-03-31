using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class PageFieldSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(PageFieldSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = ControlBaseSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.PageField);

            if (node.Expression != null)
                symbol.Source = node.Expression.ToString();

            return symbol;
        }
    }
}
