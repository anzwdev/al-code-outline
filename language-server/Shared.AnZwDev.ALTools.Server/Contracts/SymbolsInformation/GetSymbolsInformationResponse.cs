using AnZwDev.ALTools.Workspace.SymbolsInformation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetSymbolsInformationResponse<T> where T : SymbolInformation
    {

        public List<T> symbols { get; set; }

    }
}
