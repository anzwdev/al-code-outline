using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Contracts
{
    internal class GetObjectHeadersTreeRequest
    {

        [JsonProperty("documentUid")]
        public int DocumentUid { get; set; }

    }
}
