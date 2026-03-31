using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetNamespaceAndUsingsRequest
    {

        [JsonProperty("path")]
        public string? Path { get; set; }

        [JsonProperty("objectIdentifier")]
        public PIObjectIdentifier? ObjectIdentifier { get; set; }

        [JsonProperty("referencedObjectsIdentifiers")]
        public PIObjectIdentifier[]? ReferencedObjectsIdentifiers { get; set; }

        [JsonProperty("force")]
        public bool Force { get; set; }

        [JsonProperty("rootNamespace")]
        public string? RootNamespace { get; set; }

    }
}
