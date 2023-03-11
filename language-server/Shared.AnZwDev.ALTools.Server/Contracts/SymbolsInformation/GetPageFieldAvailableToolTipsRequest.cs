using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetPageFieldAvailableToolTipsRequest
    {
        public string path { get; set; }
        public string objectType { get; set; }
        public string objectName { get; set; }
        public string sourceTable { get; set; }
        public string fieldExpression { get; set; }

    }
}
