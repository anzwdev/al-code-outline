using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPageActionSymbol : AppSerializedSymbol<PageActionSymbol>
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Kind")]
        public int Kind { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("Actions")]
        public AppPageActionSymbol[]? Actions { get; set; }

        [JsonPropertyName("TargetId")]
        public int TargetId { get; set; }

        [JsonPropertyName("TargetName")]
        public string? TargetName { get; set; }

        public override PageActionSymbol CreateSymbol(string? ns)
        {
            return new PageActionSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty,
                Kind = AppEnumConverters.IntToPageActionKind(Kind),
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                Actions = Actions.CreateSymbolsListOrNull<PageActionSymbol, AppPageActionSymbol>(ns),
                TargetId = TargetId,
                TargetName = TargetName ?? String.Empty               
            };
        }


    }
}
