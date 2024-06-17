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

    public class SortUsingsSyntaxRewriter : ALSyntaxRewriter
    {

        #region Usings comparer

        protected class UsingsComparer : IComparer<SyntaxNodeSortInfo<UsingDirectiveSyntax>>
        {
            protected IComparer<string> _comparer;

            public UsingsComparer(string[] prefixSortOrder)
            {
                _comparer = new FullyQualifiedNameComparer(prefixSortOrder);
            }

            public int Compare(SyntaxNodeSortInfo<UsingDirectiveSyntax> x, SyntaxNodeSortInfo<UsingDirectiveSyntax> y)
            {
                return _comparer.Compare(x.Name, y.Name);
            }
        }

        #endregion

        public bool SortSingleNodeRegions { get; set; } = false;
        public String[] PrefixesSortOrder { get; set; }

        public SortUsingsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            if ((node.Usings != null) && (node.Usings.Count > 1))
                //Disabled diagnostics check because it might contain warning that using is not used
                //which would block the command
                //&& (!node.Usings.Any(p => p.ContainsDiagnostics)))
            {
                var newUsings = SyntaxNodesGroupsTree<UsingDirectiveSyntax>.SortSyntaxListWithSortInfo(
                    node.Usings, new UsingsComparer(PrefixesSortOrder), SortSingleNodeRegions, out bool sorted);
                if (sorted)
                {
                    NoOfChanges++;
                    node = node.WithUsings(newUsings);
                }
            }

            return base.VisitCompilationUnit(node);
        }

    }

#endif

}
