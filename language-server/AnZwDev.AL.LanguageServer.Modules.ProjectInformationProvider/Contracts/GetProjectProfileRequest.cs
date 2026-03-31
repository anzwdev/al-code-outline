using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider.Contracts
{
    public class GetProjectProfileRequest
    {

        [JsonProperty("path")]
        public string? Path { get; set; }

    }
}
