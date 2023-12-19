using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class QueryInformation : SymbolWithIdInformation
    {

        [JsonProperty("namespace", NullValueHandling = NullValueHandling.Ignore)]
        public string Namespace { get; set; }

        public QueryInformation()
        {
        }

        public QueryInformation(ALAppQuery query) : base(query)
        {
            Namespace = query.NamespaceName;
            if (query.Properties != null)
                this.Caption = query.Properties.GetValue("Caption");
        }

    }
}
