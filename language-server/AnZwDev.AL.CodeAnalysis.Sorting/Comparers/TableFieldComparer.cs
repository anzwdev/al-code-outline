using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class TableFieldComparer<T> : IComparer<T> where T : FieldBaseSyntax
    {
        protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

        public TableFieldComparer()
        {
        }

        protected long GetFieldId(FieldSyntax? node)
        {
            if ((node != null) && (!string.IsNullOrWhiteSpace(node.No.ValueText)) && (long.TryParse(node.No.ValueText, out long value)))
                    return value;
            return 0;
        }

        public int Compare(T? x, T? y)
        {
            var fieldX = x as FieldSyntax;
            var fieldY = y as FieldSyntax;

            long xNo = GetFieldId(fieldX);
            long yNo = GetFieldId(fieldY);

            int value = xNo.CompareTo(yNo);
            if (value != 0)
                return value;

            return _stringComparer.Compare(x?.GetNameStringValue(), y?.GetNameStringValue());
        }
    }

}
