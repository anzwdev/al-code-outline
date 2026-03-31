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
    internal static class PageAreaSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(PageAreaSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            return ControlGroupBaseSymbolFactory.CreateSymbol(node, parentNode, ALSyntaxNodeKind.PageArea);
        }
    }
}
