using AnZwDev.AL.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class PagePartSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(PagePartSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = ControlBaseSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.PagePart);

            if (node.PartName != null)
                symbol.FullName = symbol.FullName + ": " + node.PartName.ToString();

            return symbol;
        }
    }
}
