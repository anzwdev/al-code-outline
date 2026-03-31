using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppEnumTypeSymbol : AppObjectWithIdSymbol<EnumTypeSymbol>
    {

        [JsonPropertyName("Values")]
        public AppEnumValueSymbol[]? Values { get; set; }

        [JsonPropertyName("ImplementedInterfaces")]
        public List<string>? ImplementedInterfaces { get; set; }


        public override EnumTypeSymbol CreateSymbol(string? ns)
        {
            return new EnumTypeSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Values = Values.CreateSymbolsListOrNull<EnumValueSymbol, AppEnumValueSymbol>(ns),
                ImplementedInterfaces = ALSymbolExpressionParser.ParseObjectReferenceListOrNull(ObjectKind.Interface, ImplementedInterfaces, null),
                Usings = null
            };
        }

    }
}
