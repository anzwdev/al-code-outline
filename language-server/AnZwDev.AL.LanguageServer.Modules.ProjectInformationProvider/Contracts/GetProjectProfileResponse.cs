using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts.ProjectProfile;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    internal class GetProjectProfileResponse
    {

        [JsonProperty("profile", NullValueHandling = NullValueHandling.Ignore)]
        public required PIProjectProfile? Profile { get; init; }

    }
}
