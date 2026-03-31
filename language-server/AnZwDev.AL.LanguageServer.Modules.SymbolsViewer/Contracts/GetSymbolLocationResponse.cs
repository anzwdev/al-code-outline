using AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider.Contracts.Location;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Contracts
{
    internal class GetSymbolLocationResponse
    {

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public SPSymbolLocation? Location { get; set; }

    }
}
