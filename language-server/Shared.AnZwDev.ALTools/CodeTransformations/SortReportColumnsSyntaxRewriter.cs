using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SortReportColumnsSyntaxRewriter: ALSyntaxRewriter
    {

        #region Report element comparer

        protected class ReportElementComparer : IComparer<SyntaxNodeSortInfo<ReportDataItemElementSyntax>>
        {
            protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

            public ReportElementComparer()
            {
            }

            public int Compare(SyntaxNodeSortInfo<ReportDataItemElementSyntax> x, SyntaxNodeSortInfo<ReportDataItemElementSyntax> y)
            {
                if ((x.Kind == y.Kind) && (x.Kind == ConvertedSyntaxKind.ReportColumn))
                    return _stringComparer.Compare(x.Name, y.Name);
                if (x.Kind == ConvertedSyntaxKind.ReportColumn)
                    return -1;
                if (y.Kind == ConvertedSyntaxKind.ReportColumn)
                    return 1;
                return (x.Index - y.Index);
            }
        }

        #endregion

        #region Report column comparer

        protected class ReportColumnComparer : IComparer<SyntaxNodeSortInfo<ReportColumnSyntax>>
        {
            protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

            public ReportColumnComparer()
            {
            }

            public int Compare(SyntaxNodeSortInfo<ReportColumnSyntax> x, SyntaxNodeSortInfo<ReportColumnSyntax> y)
            {
                return _stringComparer.Compare(x.Name, y.Name);
            }
        }

        #endregion


        public SortReportColumnsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitReportDataItem(ReportDataItemSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics) && (node.Elements != null) && (node.Elements.Count > 1))
            {
                SyntaxList<ReportDataItemElementSyntax> elements =
                    SyntaxNodesGroupsTree<ReportDataItemElementSyntax>.SortSyntaxListWithSortInfo(
                        node.Elements, new ReportElementComparer(), out bool sorted);
                if (sorted)
                    this.NoOfChanges++;
                node = node.WithElements(elements);
            }
            return base.VisitReportDataItem(node);
        }

#if BC

        public override SyntaxNode VisitReportExtensionDataSetAddColumn(ReportExtensionDataSetAddColumnSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics) && (node.Columns != null) && (node.Columns.Count > 1))
            {
                SyntaxList<ReportColumnSyntax> columns =
                    SyntaxNodesGroupsTree<ReportColumnSyntax>.SortSyntaxListWithSortInfo(
                        node.Columns, new ReportColumnComparer(), out bool sorted);
                if (sorted)
                    this.NoOfChanges++;
                node = node.WithColumns(columns);
            }
            return base.VisitReportExtensionDataSetAddColumn(node);
        }

#endif

    }
}
