using AnZwDev.AL.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetObjectsListRequest
    {

        [JsonProperty("path")]
        public string? Path { get; set; }

        [JsonProperty("filter")]
        public GetObjectListFilter? Filter { get; set; }

    }
}
