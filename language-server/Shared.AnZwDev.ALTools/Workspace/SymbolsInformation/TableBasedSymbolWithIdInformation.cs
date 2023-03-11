using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableBasedSymbolWithIdInformation : TableBasedSymbolInformation
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        public TableBasedSymbolWithIdInformation()
        {
        }

        public TableBasedSymbolWithIdInformation(ALAppElementWithNameId symbol, string source) : base(symbol, source)
        {
            this.Id = symbol.Id;
        }

    }
}
