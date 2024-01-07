using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.SymbolsInformation
{
    public class SymbolReference
    {

        public string[] usings { get; set; }
        public string namespaceName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string nameWithNamespaceOrId { get; set; }

    }
}
