using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPageControlChangeSymbol : AppSerializedSymbol<PageControlChangeSymbol>
    {

        [JsonPropertyName("Anchor")]
        public string? Anchor { get; set; }

        [JsonPropertyName("ChangeKind")]
        public int ChangeKind { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("Controls")]
        public AppPageControlSymbol[]? Controls { get; set; }

        public override PageControlChangeSymbol CreateSymbol(string? ns)
        {
            return new PageControlChangeSymbol()
            {
                Anchor = Anchor,
                ChangeKind = AppEnumConverters.IntToPageControlChangeKind(ChangeKind),
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                Controls = Controls.CreateSymbolsListOrNull<PageControlSymbol, AppPageControlSymbol>(ns)
            };
        }

    }
}
