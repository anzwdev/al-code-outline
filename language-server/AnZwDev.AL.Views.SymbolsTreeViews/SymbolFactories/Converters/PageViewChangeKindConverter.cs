using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters
{
    internal static class PageViewChangeKindConverter
    {

        public static ALSyntaxNodeKind ToALSyntaxNodeKind(this PageViewChangeKind kind)
        {
            return kind switch
            {
                PageViewChangeKind.Add => ALSyntaxNodeKind.ViewAddChange,
                PageViewChangeKind.AddFirst => ALSyntaxNodeKind.ViewAddChange,
                PageViewChangeKind.AddLast => ALSyntaxNodeKind.ViewAddChange,
                PageViewChangeKind.AddBefore => ALSyntaxNodeKind.ViewAddChange,
                PageViewChangeKind.AddAfter => ALSyntaxNodeKind.ViewAddChange,

                PageViewChangeKind.MoveFirst => ALSyntaxNodeKind.ViewMoveChange,
                PageViewChangeKind.MoveLast => ALSyntaxNodeKind.ViewMoveChange,
                PageViewChangeKind.MoveBefore => ALSyntaxNodeKind.ViewMoveChange,
                PageViewChangeKind.MoveAfter => ALSyntaxNodeKind.ViewMoveChange,

                PageViewChangeKind.Modify => ALSyntaxNodeKind.ViewModifyChange,

                _ => ALSyntaxNodeKind.ViewAddChange
            };
        }


    }
}
