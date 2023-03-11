using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetNextObjectIdRequest : GetSymbolsInformationRequest
    {
        public string objectType { get; set; }
    }
}
