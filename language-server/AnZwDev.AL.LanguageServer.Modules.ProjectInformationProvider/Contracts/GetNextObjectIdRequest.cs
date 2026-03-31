using AnZwDev.AL.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetNextObjectIdRequest
    {

        [JsonProperty("path")]
        public string? Path { get; set; }

        [JsonProperty("kind")]
        public ObjectKind Kind { get; set; }

    }
}
