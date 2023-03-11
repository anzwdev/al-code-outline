using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class TableInformation : BaseObjectInformation
    {

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public List<TableFieldInformaton> Fields { get; set; } = null;

        [JsonProperty("primaryKeys", NullValueHandling = NullValueHandling.Ignore)]
        public List<TableFieldInformaton> PrimaryKeys { get; set; } = null;


        public TableInformation()
        {
        }

        public TableInformation(ALAppTable table): base(table)
        {
        }


    }
}
