using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols;
using AnZwDev.AL.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetTableFieldsRequest
    {

        [JsonProperty("path")]
        public string? Path { get; set; }

        [JsonProperty("tableIdentifier")]
        public PIObjectIdentifier? TableIdentifier { get; set; }

        [JsonProperty("fieldClassFilter")]
        public FieldClass[]? FieldClassFilter { get; set; }

        [JsonProperty("includeToolTips")]
        public bool IncludeToolTips { get; set; }

        [JsonProperty("toolTipsSourceDependencies")]
        public string[]? ToolTipsSourceDependencies { get; set; }

    }
}
