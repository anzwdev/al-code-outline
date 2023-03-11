using AnZwDev.ALTools.Workspace.SymbolsInformation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetSymbolInformationDetailsResponse<T> where T : SymbolInformation
    {

        public T symbol { get; set; }

    }
}
