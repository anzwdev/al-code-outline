using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class SymbolWithIdInformation : SymbolInformation
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        public SymbolWithIdInformation()
        {
        }

        public SymbolWithIdInformation(ALAppElementWithNameId symbol)
        {
            this.Id = symbol.Id;
            this.Name = symbol.Name;
        }

    }
}
