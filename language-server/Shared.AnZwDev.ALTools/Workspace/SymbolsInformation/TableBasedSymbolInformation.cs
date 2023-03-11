using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableBasedSymbolInformation : SymbolInformation
    {

        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("existingTableFields")]
        public List<TableFieldInformaton> ExistingTableFields { get; set; }
        [JsonProperty("availableTableFields")]
        public List<TableFieldInformaton> AvailableTableFields { get; set; }

        public TableBasedSymbolInformation()
        {
        }
        public TableBasedSymbolInformation(ALAppElementWithName element, string source)
        {
            this.ExistingTableFields = null;
            this.AvailableTableFields = null;
            this.Name = element.Name;
            this.Source = source;
        }

        public void Sort()
        {
            SymbolInformationComparer comparer = new SymbolInformationComparer();
            if (AvailableTableFields != null)
                AvailableTableFields.Sort(comparer);
            if (ExistingTableFields != null)
                ExistingTableFields.Sort(comparer);
        }

    }
}
