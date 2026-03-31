using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols
{
    internal class PILabel
    {

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public required string? Value { get; init; }

        [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
        public required string? Comment { get; init; }

    }
}
