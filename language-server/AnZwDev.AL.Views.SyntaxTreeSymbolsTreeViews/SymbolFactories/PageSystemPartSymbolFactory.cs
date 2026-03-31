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
    internal static class PageSystemPartSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(PageSystemPartSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = ControlBaseSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.PageSystemPart);

            if (node.SystemPartType != null)
                symbol.FullName = symbol.FullName + ": " + node.SystemPartType.ToString();

            return symbol;
        }
    }
}
