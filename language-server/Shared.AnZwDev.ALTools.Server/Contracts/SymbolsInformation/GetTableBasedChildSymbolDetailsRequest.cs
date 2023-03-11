using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetTableBasedChildSymbolDetailsRequest : GetTableBasedSymbolDetailsRequest
    {
        public string objectName { get; set; }
    }
}
