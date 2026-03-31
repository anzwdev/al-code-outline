using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPageActionChangeSymbol : AppSerializedSymbol<PageActionChangeSymbol>
    {

        [JsonPropertyName("Anchor")]
        public string? Anchor { get; set; }

        [JsonPropertyName("ChangeKind")]
        public int ChangeKind { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("Actions")]
        public AppPageActionSymbol[]? Actions { get; set; }

        public override PageActionChangeSymbol CreateSymbol(string? ns)
        {
            return new PageActionChangeSymbol()
            {
                Anchor = Anchor,
                ChangeKind = AppEnumConverters.IntToPageActionChangeKind(ChangeKind),
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                Actions = Actions.CreateSymbolsListOrNull<PageActionSymbol, AppPageActionSymbol>(ns)
            };
        }

    }
}
