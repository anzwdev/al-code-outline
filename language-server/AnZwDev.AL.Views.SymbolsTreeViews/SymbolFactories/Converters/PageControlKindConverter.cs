using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters
{
    internal static class PageControlKindConverter
    {

        public static ALSyntaxNodeKind ToALSyntaxNodeKind(this PageControlKind kind)
        {
            return kind switch
            {
                PageControlKind.Area => ALSyntaxNodeKind.PageArea,
                PageControlKind.Group => ALSyntaxNodeKind.PageGroup,
                PageControlKind.CueGroup => ALSyntaxNodeKind.PageGroup,
                PageControlKind.Repeater => ALSyntaxNodeKind.PageRepeater,
                PageControlKind.Fixed => ALSyntaxNodeKind.PageGroup,
                PageControlKind.Grid => ALSyntaxNodeKind.PageGroup,
                PageControlKind.Part => ALSyntaxNodeKind.PagePart,
                PageControlKind.SystemPart => ALSyntaxNodeKind.PageSystemPart,
                PageControlKind.Field => ALSyntaxNodeKind.PageField,
                PageControlKind.Label => ALSyntaxNodeKind.PageLabel,
                PageControlKind.UserControl => ALSyntaxNodeKind.PageUserControl,
                PageControlKind.ChartPart => ALSyntaxNodeKind.PageChartPart,
                _ => ALSyntaxNodeKind.PageGroup
            };
        }


    }
}
