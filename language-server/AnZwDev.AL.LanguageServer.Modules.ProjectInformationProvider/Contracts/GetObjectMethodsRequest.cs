using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetObjectMethodsRequest
    {

        [JsonProperty("path")]
        public string? Path { get; set; }

        [JsonProperty("identifier")]
        public PIObjectIdentifier? Identifier { get; set; }

        [JsonProperty("includePrivate")]
        public bool IncludePrivate { get; set; }

    }
}
