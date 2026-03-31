using AnZwDev.AL.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols
{
    internal class PIObjectListItem
    {
        [JsonProperty("kind")]
        public required ObjectKind Kind { get; init; }

        [JsonProperty("id")]
        public required int Id { get; init; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public required string? Name { get; init; }

        [JsonProperty("namespace", NullValueHandling = NullValueHandling.Ignore)]
        public required string? Namespace { get; init; }

        [JsonProperty("uid")]
        public required int Uid { get; init; }

        [JsonProperty("inherentPermissions", NullValueHandling = NullValueHandling.Ignore)]
        public required string? InherentPermissions { get; init; }

        [JsonProperty("fullInherentPermissions")]
        public required bool FullInherentPermissions { get; init; }

    }

}
