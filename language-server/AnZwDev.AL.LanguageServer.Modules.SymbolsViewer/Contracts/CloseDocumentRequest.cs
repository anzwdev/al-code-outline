using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Contracts
{
    internal class CloseDocumentRequest
    {

        [JsonProperty("documentUid")]
        public int DocumentUid { get; set; }

    }
}
