using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppEnumExtensionTypeSymbol : AppObjectWithIdSymbol<EnumExtensionTypeSymbol>
    {

        [JsonPropertyName("TargetObject")]
        public string? TargetObject { get; set; }

        [JsonPropertyName("Values")]
        public AppEnumValueSymbol[]? Values { get; set; }


        public override EnumExtensionTypeSymbol CreateSymbol(string? ns)
        {
            var properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties);

            return new EnumExtensionTypeSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), properties)
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Values = Values.CreateSymbolsListOrNull<EnumValueSymbol, AppEnumValueSymbol>(ns),
                Usings = null,
                ExtendedObjectReference = ALSymbolExpressionParser.ParseObjectReference(ObjectKind.EnumExtensionType, TargetObject, null)
            };
        }


    }
}
