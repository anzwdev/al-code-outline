using AnZwDev.AL.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.Symbols;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetObjectsListResponse
    {

        [JsonProperty("objects", NullValueHandling = NullValueHandling.Ignore)]
        public List<PIObjectListItem> Objects { get; } = new List<PIObjectListItem>();

    }
}
