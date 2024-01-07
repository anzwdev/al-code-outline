using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class InterfaceInformation : SymbolInformation
    {

        [JsonProperty("namespace", NullValueHandling = NullValueHandling.Ignore)]
        public string Namespace { get; set; }

        public InterfaceInformation()
        {
        }

        public InterfaceInformation(ALAppInterface symbol) : base(symbol)
        {
            Namespace = symbol.NamespaceName;
        }

    }
}
