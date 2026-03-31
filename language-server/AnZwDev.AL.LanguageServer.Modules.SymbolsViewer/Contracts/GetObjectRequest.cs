using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Contracts
{
    internal class GetObjectRequest
    {

        [JsonProperty("documentUid")]
        public int DocumentUid { get; set; }

        [JsonProperty("objectUid")]
        public int ObjectUid { get; set; }

    }
}
