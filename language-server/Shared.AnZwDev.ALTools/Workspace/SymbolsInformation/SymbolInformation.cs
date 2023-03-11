using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class SymbolInformation
    {

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("caption")]
        public string Caption { get; set; }

        public SymbolInformation()
        {
        }

        public SymbolInformation(ALAppElementWithName symbol)
        {
            this.Name = symbol.Name;            
        }

    }
}
