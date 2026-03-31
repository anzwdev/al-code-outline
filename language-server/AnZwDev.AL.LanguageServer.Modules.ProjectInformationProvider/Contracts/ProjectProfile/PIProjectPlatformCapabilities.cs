using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.ProjectProfile
{
    internal class PIProjectPlatformCapabilities
    {

        [JsonProperty("interfaces")]
        public required bool Interfaces { get; init; }

        [JsonProperty("appAreasInheritance")]
        public required bool AppAreasInheritance { get; init; }

        [JsonProperty("namespaces")]
        public required bool Namespaces { get; init; }

        [JsonProperty("tableToolTips")]
        public required bool TableToolTips { get; init; }

    }

}
