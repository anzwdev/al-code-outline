using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetSymbolsInformationRequest
    {

        public string path { get; set; }
        public bool includeNonAccessible { get; set; }

    }
}
