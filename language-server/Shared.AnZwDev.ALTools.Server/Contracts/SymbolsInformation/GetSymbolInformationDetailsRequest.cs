using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetSymbolInformationDetailsRequest : GetSymbolsInformationRequest
    {

        public string name { get; set; }

    }
}
