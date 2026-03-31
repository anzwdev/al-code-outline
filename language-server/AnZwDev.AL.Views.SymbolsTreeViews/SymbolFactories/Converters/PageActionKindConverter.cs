using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters
{
    internal static class PageActionKindConverter
    {

        public static ALSyntaxNodeKind ToALSyntaxNodeKind(this PageActionKind kind)
        {
            return kind switch
            {
                PageActionKind.Area => ALSyntaxNodeKind.PageActionArea,
                PageActionKind.Group => ALSyntaxNodeKind.PageActionGroup,
                PageActionKind.Action => ALSyntaxNodeKind.PageAction,
                PageActionKind.Separator => ALSyntaxNodeKind.PageActionSeparator,
                PageActionKind.ActionRef => ALSyntaxNodeKind.PageAction,
                PageActionKind.CustomAction => ALSyntaxNodeKind.PageAction,
                PageActionKind.SystemAction => ALSyntaxNodeKind.PageAction,
                PageActionKind.FileUploadAction => ALSyntaxNodeKind.PageAction,
                _ => ALSyntaxNodeKind.PageAction
            };
        }


    }
}
