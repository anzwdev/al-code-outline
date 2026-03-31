using AnZwDev.AL.Symbols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetObjectListFilter
    {

        [JsonProperty("kind")]
        public ObjectKind Kind { get; set; }

        [JsonProperty("appIdFilter")]
        public string[]? AppIdFilter { get; set; }

        [JsonProperty("skipDependencies")]
        public bool SkipDependencies { get; set; }

        [JsonProperty("excludeFullInherentPermissions")]
        public bool ExcludeFullInherentPermissions { get; set; }

    }
}
