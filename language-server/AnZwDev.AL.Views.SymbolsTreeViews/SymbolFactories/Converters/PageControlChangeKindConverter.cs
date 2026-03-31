using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters
{
    internal static class PageControlChangeKindConverter
    {

        public static ALSyntaxNodeKind ToALSyntaxNodeKind(this PageControlChangeKind kind)
        {
            return kind switch
            {
                PageControlChangeKind.Add => ALSyntaxNodeKind.ControlAddChange,
                PageControlChangeKind.AddFirst => ALSyntaxNodeKind.ControlAddChange,
                PageControlChangeKind.AddLast => ALSyntaxNodeKind.ControlAddChange,
                PageControlChangeKind.AddBefore => ALSyntaxNodeKind.ControlAddChange,
                PageControlChangeKind.AddAfter => ALSyntaxNodeKind.ControlAddChange,

                PageControlChangeKind.MoveFirst => ALSyntaxNodeKind.ControlMoveChange,
                PageControlChangeKind.MoveLast => ALSyntaxNodeKind.ControlMoveChange,
                PageControlChangeKind.MoveBefore => ALSyntaxNodeKind.ControlMoveChange,
                PageControlChangeKind.MoveAfter => ALSyntaxNodeKind.ControlMoveChange,

                PageControlChangeKind.Modify => ALSyntaxNodeKind.ControlModifyChange,

                _ => ALSyntaxNodeKind.ControlAddChange
            };
        }


    }
}
