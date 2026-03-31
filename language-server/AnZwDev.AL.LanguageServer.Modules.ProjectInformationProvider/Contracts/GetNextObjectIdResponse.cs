using AnZwDev.AL.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetNextObjectIdResponse
    {

        [JsonProperty("kind")]
        public required ObjectKind Kind { get; init; }

        [JsonProperty("id")]
        public required int Id { get; init; }

    }
}
