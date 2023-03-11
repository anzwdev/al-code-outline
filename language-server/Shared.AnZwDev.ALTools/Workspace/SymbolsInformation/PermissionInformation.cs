using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class PermissionInformation
    {

        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
        
        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Source { get; set; }

        public PermissionInformation()
        {
        }

    }
}
