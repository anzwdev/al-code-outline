using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.ToolTips
{
    public class FieldToolTips
    {

        public required TableFieldSymbol Field { get; init; }
        public List<ToolTip> ToolTips { get; } = new List<ToolTip>();

    }
}
