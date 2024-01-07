using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetSymbolInformationDetailsRequest : GetSymbolsInformationRequest
    {

        public SymbolReference symbolReference { get; set; }

    }
}
