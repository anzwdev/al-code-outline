using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AnZwDev.AL.Symbols.Metadata;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppInternalsVisibleToModuleSymbol
    {

        [JsonPropertyName("AppId")]
        public string? AppId { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Publisher")]
        public string? Publisher { get; set; }


        public InternalsVisibleToModule CreateSymbol()
        {
            return new InternalsVisibleToModule()
            {
                AppId = AppId ?? String.Empty,
                Publisher = Publisher,
                Name = Name,
            };
        }


    }
}
