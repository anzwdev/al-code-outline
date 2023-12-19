using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.Core;
using System.Linq;
using System.Globalization;

namespace AnZwDev.ALTools.CodeTransformations.Namespaces
{

#if BC

    internal class SortUsingSyntaxRewriter : ALSyntaxRewriter
    {

        #region Usings comparer

        protected class UsingComparer : IComparer<SyntaxNodeSortInfo<UsingDirectiveSyntax>>
        {
            protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

            public UsingComparer()
            {
            }

            public int Compare(SyntaxNodeSortInfo<UsingDirectiveSyntax> x, SyntaxNodeSortInfo<UsingDirectiveSyntax> y)
            {
                int val = _stringComparer.Compare(x.Name, y.Name);
                if (val != 0)
                    return val;
                return x.Index - y.Index;
            }
        }

        #endregion

        public bool SortSingleNodeRegions { get; set; } = false;

        public SortUsingSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            if ((node.Usings != null) && (node.Usings.Count > 1) && (!node.Usings.Any(p => p.ContainsDiagnostics)))
            {
                var newUsings = SyntaxNodesGroupsTree<UsingDirectiveSyntax>.SortSyntaxListWithSortInfo(
                    node.Usings, new UsingComparer(), SortSingleNodeRegions, out bool sorted);
                if (sorted)
                {
                    NoOfChanges++;
                    node = node.WithUsings(newUsings);
                }
            }

            return base.VisitCompilationUnit(node);
        }

        public SyntaxList<UsingDirectiveSyntax> SortUsingsList(SyntaxList<UsingDirectiveSyntax> usingsSyntaxList, out bool sorted)
        {
            return SyntaxNodesGroupsTree<UsingDirectiveSyntax>.SortSyntaxListWithSortInfo(
                usingsSyntaxList, new UsingComparer(), SortSingleNodeRegions, out sorted);
        }

    }

#endif

}
