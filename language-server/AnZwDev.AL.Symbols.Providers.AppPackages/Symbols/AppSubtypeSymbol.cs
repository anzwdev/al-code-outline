using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppSubtypeSymbol
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("ModuleId")]
        public string? ModuleId { get; set; }

        public SubtypeSymbol CreateSymbol()
        {
            return new SubtypeSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty,
                ModuleId = ModuleId
            };
        }


    }
}
