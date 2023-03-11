using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public static class ALAppExtensions
    {

        public static ALSymbolKind ToALSymbolKind(this ALAppPageControlKind controlKind)
        {
            switch (controlKind)
            {
                case ALAppPageControlKind.Area:
                    return ALSymbolKind.PageArea;
                case ALAppPageControlKind.Group:
                    return ALSymbolKind.PageGroup;
                case ALAppPageControlKind.CueGroup:
                    return ALSymbolKind.PageGroup;
                case ALAppPageControlKind.Repeater:
                    return ALSymbolKind.PageRepeater;
                case ALAppPageControlKind.Fixed:
                case ALAppPageControlKind.Grid:
                    return ALSymbolKind.PageGroup;
                case ALAppPageControlKind.Part:
                    return ALSymbolKind.PagePart;
                case ALAppPageControlKind.SystemPart:
                    return ALSymbolKind.PageSystemPart;
                case ALAppPageControlKind.Field:
                    return ALSymbolKind.PageField;
                case ALAppPageControlKind.Label:
                    return ALSymbolKind.PageLabel;
                case ALAppPageControlKind.UserControl:
                    return ALSymbolKind.PageUserControl;
                case ALAppPageControlKind.Chart:
                    return ALSymbolKind.PageChartPart;
            }
            return ALSymbolKind.Undefined;
        }

        public static ALSymbolKind ToALSymbolKind(this ALAppPageActionKind actionKind)
        {
            switch (actionKind)
            {
                case ALAppPageActionKind.Group:
                    return ALSymbolKind.PageActionGroup;
                case ALAppPageActionKind.Area:
                    return ALSymbolKind.PageActionArea;
                case ALAppPageActionKind.Action:
                    return ALSymbolKind.PageAction;
                case ALAppPageActionKind.Separator:
                    return ALSymbolKind.PageActionSeparator;
            }
            return ALSymbolKind.Undefined;
        }

    }
}
