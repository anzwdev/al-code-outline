using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters
{
    internal static class PageActionChangeKindConverter
    {

        public static ALSyntaxNodeKind ToALSyntaxNodeKind(this PageActionChangeKind kind)
        {
            return kind switch
            {
                PageActionChangeKind.Add => ALSyntaxNodeKind.ActionAddChange,
                PageActionChangeKind.AddFirst => ALSyntaxNodeKind.ActionAddChange,
                PageActionChangeKind.AddLast => ALSyntaxNodeKind.ActionAddChange,
                PageActionChangeKind.AddBefore => ALSyntaxNodeKind.ActionAddChange,
                PageActionChangeKind.AddAfter => ALSyntaxNodeKind.ActionAddChange,

                PageActionChangeKind.MoveFirst => ALSyntaxNodeKind.ActionMoveChange,
                PageActionChangeKind.MoveLast => ALSyntaxNodeKind.ActionMoveChange,
                PageActionChangeKind.MoveBefore => ALSyntaxNodeKind.ActionMoveChange,
                PageActionChangeKind.MoveAfter => ALSyntaxNodeKind.ActionMoveChange,

                PageActionChangeKind.Modify => ALSyntaxNodeKind.ActionModifyChange,

                _ => ALSyntaxNodeKind.ActionAddChange
            };
        }


    }
}
