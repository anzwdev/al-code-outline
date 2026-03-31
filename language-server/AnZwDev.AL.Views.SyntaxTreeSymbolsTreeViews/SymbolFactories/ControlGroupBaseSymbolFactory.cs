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
    internal static class ControlGroupBaseSymbolFactory
    {

        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(ControlGroupBaseSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode, ALSyntaxNodeKind kind)
        {
            var symbol = ControlBaseSymbolFactory.CreateSymbol(node, parentNode, kind);
            if ((node.OpenBraceToken.Kind != SyntaxKind.None) && (node.CloseBraceToken.Kind != SyntaxKind.None))
                symbol.ContentRange = node.SyntaxTree.GetLineRange(node.OpenBraceToken.Span.Union(node.CloseBraceToken.Span));

            return symbol;
        }

    }
}
