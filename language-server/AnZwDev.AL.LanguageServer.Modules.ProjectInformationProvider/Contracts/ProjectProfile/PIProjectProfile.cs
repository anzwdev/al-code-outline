using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.ProjectProfile
{
    internal class PIProjectProfile
    {

        [JsonProperty("affixes", NullValueHandling = NullValueHandling.Ignore)]
        public required PIAffixesSettings? Affixes { get; init; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public required PIProjectProperties? Properties { get; init; }

        [JsonProperty("platformCapabilities", NullValueHandling = NullValueHandling.Ignore)]
        public required PIProjectPlatformCapabilities? PlatformCapabilities { get; init; }

    }
}
