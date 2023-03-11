using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetPageDetailsRequest : GetTableBasedSymbolDetailsRequest
    {

        public bool getToolTips { get; set; }
        public string[] toolTipsSourceDependencies { get; set; }

    }
}
