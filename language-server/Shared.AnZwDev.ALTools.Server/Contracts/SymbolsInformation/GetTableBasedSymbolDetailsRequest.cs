using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetTableBasedSymbolDetailsRequest : GetSymbolInformationDetailsRequest
    {
        public bool getExistingFields { get; set; }
        public bool getAvailableFields { get; set; }
    }
}
