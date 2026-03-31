using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetObjectMethodsResponse
    {

        [JsonProperty("methods", NullValueHandling = NullValueHandling.Ignore)]
        public required List<PIMethodListItem>? Methods { get; set; }

    }
}
