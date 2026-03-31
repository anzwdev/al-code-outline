using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPageViewSymbol : AppSerializedSymbol<PageViewSymbol>
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("ControlChanges")]
        public AppPageControlChangeSymbol[]? ControlChanges { get; set; }

        public override PageViewSymbol CreateSymbol(string? ns)
        {
            return new PageViewSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty,
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                ControlChanges = ControlChanges.CreateSymbolsListOrNull<PageControlChangeSymbol, AppPageControlChangeSymbol>(ns)
            };
        }

    }
}
