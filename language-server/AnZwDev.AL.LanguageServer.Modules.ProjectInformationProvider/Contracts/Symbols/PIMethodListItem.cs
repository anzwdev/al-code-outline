using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols
{
    internal class PIMethodListItem
    {

        [JsonProperty("name")]
        public required string Name { get; init; }

        [JsonProperty("header")]
        public required string Header { get; init; }

    }
}
