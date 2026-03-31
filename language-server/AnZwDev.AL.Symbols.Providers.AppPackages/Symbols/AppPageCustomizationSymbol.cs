using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPageCustomizationSymbol : AppObjectWithIdSymbol<PageCustomizationSymbol>
    {

        [JsonPropertyName("TargetObject")]
        public string? TargetObject { get; set; }

        [JsonPropertyName("ControlChanges")]
        public AppPageControlChangeSymbol[]? ControlChanges { get; set; }

        public override PageCustomizationSymbol CreateSymbol(string? ns)
        {
            var properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties);

            return new PageCustomizationSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), properties)
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                ControlChanges = ControlChanges.CreateSymbolsListOrNull<PageControlChangeSymbol, AppPageControlChangeSymbol>(ns),
                Usings = null,
                ExtendedObjectReference = ALSymbolExpressionParser.ParseObjectReference(ObjectKind.Page, TargetObject, null)
            };
        }

    }
}
