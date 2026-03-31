using AnZwDev.AL.CodeAnalysis.Sorting;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class ReportColumnComparer : IComparer<SyntaxNodeSortInfo<ReportColumnSyntax>>
    {
        protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

        public ReportColumnComparer()
        {
        }

        public int Compare(SyntaxNodeSortInfo<ReportColumnSyntax>? x, SyntaxNodeSortInfo<ReportColumnSyntax>? y)
        {
            return _stringComparer.Compare(x?.Name, y?.Name);
        }
    }

}
