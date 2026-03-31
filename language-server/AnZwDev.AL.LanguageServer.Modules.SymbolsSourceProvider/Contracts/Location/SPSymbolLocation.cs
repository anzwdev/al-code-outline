using AnZwDev.AL.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider.Contracts.Location
{
    public class SPSymbolLocation
    {

        [JsonProperty("schema", NullValueHandling = NullValueHandling.Ignore)]
        public string? Schema { get; set; }

        [JsonProperty("containerPath", NullValueHandling = NullValueHandling.Ignore)]
        public string? ContainerPath { get; set; }

        [JsonProperty("sourcePath", NullValueHandling = NullValueHandling.Ignore)]
        public string? SourcePath { get; set; }

        [JsonProperty("range", NullValueHandling = NullValueHandling.Ignore)]
        public TextRange? Range { get; set; }

    }
}
