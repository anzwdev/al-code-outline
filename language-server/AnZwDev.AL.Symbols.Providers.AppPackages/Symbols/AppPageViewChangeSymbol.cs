using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPageViewChangeSymbol : AppSerializedSymbol<PageViewChangeSymbol>
    {

        [JsonPropertyName("Anchor")]
        public string? Anchor { get; set; }

        [JsonPropertyName("ChangeKind")]
        public int ChangeKind { get; set; }

        [JsonPropertyName("Views")]
        public AppPageViewSymbol[]? Views { get; set; }

        public override PageViewChangeSymbol CreateSymbol(string? ns)
        {
            return new PageViewChangeSymbol()
            {
                Anchor = Anchor,
                ChangeKind = AppEnumConverters.IntToPageViewChangeKind(ChangeKind),
                Views = Views.CreateSymbolsListOrNull<PageViewSymbol, AppPageViewSymbol>(ns)
            };
        }

    }
}