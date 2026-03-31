using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetNamespaceAndUsingsResponse
    {

        [JsonProperty("namespace", NullValueHandling = NullValueHandling.Ignore)]
        public string? Namespace { get; set; }

        [JsonProperty("usings", NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? Usings { get; set; }

    }
}
