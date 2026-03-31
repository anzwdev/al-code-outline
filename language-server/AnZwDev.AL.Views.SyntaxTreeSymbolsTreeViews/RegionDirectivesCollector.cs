using AnZwDev.AL.CodeAnalysis.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews
{
    internal static class RegionDirectivesCollector
    {

        public static RegionDirective Collect(SyntaxTree syntaxTree, SyntaxNode node)
        {
            RegionDirective firstRegionDirective = new RegionDirective();
            RegionDirective lastRegionDirective = firstRegionDirective;
            int level = 0;

            var syntaxTriviasCollection = node.DescendantTrivia();

            if (syntaxTriviasCollection != null)
                foreach (var triviaSyntax in syntaxTriviasCollection)
                {
                    var kind = triviaSyntax.Kind;
                    if ((kind == SyntaxKind.RegionDirectiveTrivia) || (kind == SyntaxKind.EndRegionDirectiveTrivia))
                    {
                        var isStartRegion = (kind == SyntaxKind.RegionDirectiveTrivia);
                        if (isStartRegion)
                            level++;
                        else
                            level--;

                        var name = (isStartRegion) ? triviaSyntax.ToString() : "";
                        var newRegionDirective = new RegionDirective(
                            isStartRegion, level, name,
                            syntaxTree.GetLineRange(triviaSyntax.FullSpan),
                            syntaxTree.GetLineRange(triviaSyntax.Span));

                        lastRegionDirective.Next = newRegionDirective;
                        lastRegionDirective = newRegionDirective;
                    }
                }

            return firstRegionDirective;
        }

    }
}
