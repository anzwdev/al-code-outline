using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.ToolTips
{
    public class ToolTip
    {

        public required ObjectIdentifier SourceObjectIdentifier { get; init; }
        public required Label Value { get; init; }

    }
}
