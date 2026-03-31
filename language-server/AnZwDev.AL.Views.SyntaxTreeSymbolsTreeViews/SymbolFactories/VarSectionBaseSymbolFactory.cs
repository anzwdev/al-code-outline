using AnZwDev.AL.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories.PropertySetters;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolFactories
{
    internal static class VarSectionBaseSymbolFactory
    {

        public static SyntaxTreeSymbolsTreeViewNode CreateSymbol(VarSectionBaseSyntax node, SyntaxTreeSymbolsTreeViewNode? parentNode, ALSyntaxNodeKind kind)
        {
            var symbol = SyntaxNodeSymbolFactory.CreateSymbol(
                node, null, 
                kind,
                TreeViewNodeNameSetters.Kind);

            (var hasChildNodes, var span) = node.GetChildNodesFullSpan();
            if (hasChildNodes)
                symbol.ContentRange = node.SyntaxTree.GetLineRange(span);

            return symbol;
        }


    }
}
