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
    internal static class PageGroupSymbolFactory
    {
        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(PageGroupSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode)
        {
            var kind = (node.ControlKeyword.Kind == SyntaxKind.PageRepeaterKeyword) ? ALSyntaxNodeKind.PageRepeater : ALSyntaxNodeKind.PageGroup;
            return ControlGroupBaseSymbolFactory.CreateSymbol(node, parentNode, kind);
        }
    }
}
