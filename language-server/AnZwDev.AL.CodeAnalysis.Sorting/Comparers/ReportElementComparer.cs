using AnZwDev.AL.CodeAnalysis.Sorting;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class ReportElementComparer : IComparer<SyntaxNodeSortInfo<ReportDataItemElementSyntax>>
    {
        protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

        public ReportElementComparer()
        {
        }

        public int Compare(SyntaxNodeSortInfo<ReportDataItemElementSyntax>? x, SyntaxNodeSortInfo<ReportDataItemElementSyntax>? y)
        {
            var xKind = x?.Kind ?? SyntaxKind.None;
            var yKind = y?.Kind ?? SyntaxKind.None;

            if (xKind == yKind && xKind == SyntaxKind.ReportColumn)
                return _stringComparer.Compare(x?.Name, y?.Name);
            if (xKind == SyntaxKind.ReportColumn)
                return -1;
            if (yKind == SyntaxKind.ReportColumn)
                return 1;

            var xIndex = x?.Index ?? 0;
            var yIndex = y?.Index ?? 0;

            return xIndex - yIndex;
        }
    }

}
