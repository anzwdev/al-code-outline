using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider.Contracts
{
    internal class GetAppFileSymbolSourceResponse
    {

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string? Source { get; set; }

    }
}
