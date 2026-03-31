using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Contracts
{
    internal class GetSymbolLocationRequest
    {

        [JsonProperty("documentUid")]
        public int DocumentUid { get; set; }

        [JsonProperty("objectUid")]
        public int ObjectUid { get; set; }

        [JsonProperty("directAppFileAccess")]
        public bool DirectAppFileAccess { get; set; }

    }
}
