using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetTableFieldsResponse
    {

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public required List<PITableFieldListItem>? Fields { get; init; }

    }
}
