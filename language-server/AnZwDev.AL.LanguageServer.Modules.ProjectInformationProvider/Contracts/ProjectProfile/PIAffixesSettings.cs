using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.ProjectProfile
{
    internal class PIAffixesSettings
    {

        [JsonProperty("mandatoryPrefixes")]
        public List<string>? MandatoryPrefixes { get; set; }

        [JsonProperty("mandatorySuffixes")]
        public List<string>? MandatorySuffixes { get; set; }

        [JsonProperty("mandatoryAffixes")]
        public List<string>? MandatoryAffixes { get; set; }

    }

}
