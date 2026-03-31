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
    internal static class PageChartPartSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(PageChartPartSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var symbol = ControlBaseSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.PageChartPart);

            if (node.ChartPartType != null)
                symbol.FullName = symbol.FullName + ": " + node.ChartPartType.ToString();

            return symbol;
        }
    }
}
