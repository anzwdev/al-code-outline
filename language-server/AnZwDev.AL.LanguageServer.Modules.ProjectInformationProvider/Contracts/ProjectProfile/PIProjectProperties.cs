using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.ProjectProfile
{
    internal class PIProjectProperties
    {

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public required string? Path { get; init; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public required string? Name { get; init; }

        [JsonProperty("runtimeVersion", NullValueHandling = NullValueHandling.Ignore)]
        public required string? RuntimeVersion { get; init; }

        [JsonProperty("firstIdRangeStart")]
        public required int FirstIdRangeStart { get; init; }

        [JsonProperty("lastIdRangeEnd")]
        public required int LastIdRangeEnd { get; init; }

    }
}
