using AnZwDev.ALTools.Workspace.SymbolsInformation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetInterfaceMethodsListResponse
    {

        public List<MethodInformation> symbols { get; set; }

    }
}
