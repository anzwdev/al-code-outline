using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider.Contracts
{
    internal class GetAppFileSymbolSourceRequest
    {

        [JsonProperty("appFilePath", NullValueHandling = NullValueHandling.Ignore)]
        public string? AppFilePath { get; set; }

        [JsonProperty("sourceFilePath", NullValueHandling = NullValueHandling.Ignore)]
        public string? SourceFilePath { get; set; }

    }
}
