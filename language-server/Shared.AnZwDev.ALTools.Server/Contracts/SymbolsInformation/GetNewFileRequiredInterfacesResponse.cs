using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class GetNewFileRequiredInterfacesResponse
    {

        public bool usesNamespaces { get; set; } = false;
        public string namespaceName { get; set; } = String.Empty;
        public List<string> usings { get; set; } = new List<string>();

    }
}
