using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.InformationProviders.ToolTips
{
    public class TableToolTips
    {

        public ObjectIdentifier Identifier { get; set; }
        public Dictionary<string, FieldToolTips> Fields { get; } = new Dictionary<string, FieldToolTips>(StringComparer.OrdinalIgnoreCase);

        public void AddToolTip(string fieldName, ToolTip toolTip)
        {
        }

    }
}

