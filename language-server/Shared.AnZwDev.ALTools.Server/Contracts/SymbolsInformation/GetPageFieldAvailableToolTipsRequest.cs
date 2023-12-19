using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetPageFieldAvailableToolTipsRequest
    {
        public string path { get; set; }
        public string objectType { get; set; }
        public SymbolReference objectReference { get; set; }
        public SymbolReference sourceTableReference { get; set; }
        public string fieldExpression { get; set; }

    }
}
