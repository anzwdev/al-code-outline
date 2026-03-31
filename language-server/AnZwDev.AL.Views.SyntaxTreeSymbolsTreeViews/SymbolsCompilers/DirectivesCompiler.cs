using AnZwDev.AL.Syntax;
using AnZwDev.AL.CodeAnalysis.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews.SymbolsCompilers
{
    internal static class DirectivesCompiler
    {

        public static SyntaxTreeSymbolsTreeViewNode CreateRegionDirectiveSymbol(int idx, RegionDirectiveTriviaSyntax node)
        {
            var syntaxTree = node.SyntaxTree;

            return new SyntaxTreeSymbolsTreeViewNode()
            {
                Kind = ALSyntaxNodeKind.Region,
                Name = "#region",
                FullName = node.ToString(),
                Range = syntaxTree.GetLineRange(node.FullSpan),
                SelectionRange = syntaxTree.GetLineRange(node.Span)
            };
        }

        public static void UpdateEndRegionSymbol(EndRegionDirectiveTriviaSyntax node, SyntaxTreeSymbolsTreeViewNode symbol)
        {
            if (symbol.Range != null)
                symbol.Range.Expand(node.SyntaxTree.GetLineRange(node.FullSpan));
        }

    }
}
